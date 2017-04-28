namespace MsgPack.Serialization.Reflection
{
    using MsgPack;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    internal sealed class TracingILGenerator : IDisposable
    {
        private static readonly ConstructorInfo _ArgumentException_ctor_String_String_Exception = typeof(ArgumentException).GetConstructor(new Type[] { typeof(string), typeof(string), typeof(Exception) });
        private static readonly PropertyInfo _cultureInfo_CurrentCulture = typeof(CultureInfo).GetProperty("CurrentCulture");
        private static readonly PropertyInfo _cultureInfo_InvariantCulture = typeof(CultureInfo).GetProperty("InvariantCulture");
        private readonly Stack<Label> _endOfExceptionBlocks;
        private readonly Label _endOfMethod;
        private string _indentChars;
        private int _indentLevel;
        private readonly bool _isDebuggable;
        private bool _isEnded;
        private bool _isInDynamicMethod;
        private readonly Dictionary<Label, string> _labels;
        private int _lineNumber;
        private readonly Dictionary<LocalBuilder, string> _localDeclarations;
        private static readonly MethodInfo _Object_GetType = typeof(object).GetMethod("GetType");
        private readonly TextWriter _realTrace;
        private static readonly Type[] _standardExceptionConstructorParamterTypesWithInnerException = new Type[] { typeof(string), typeof(Exception) };
        private static readonly MethodInfo _string_Format = typeof(string).GetMethod("Format", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(IFormatProvider), typeof(string), typeof(object[]) }, null);
        private readonly TextWriter _trace;
        private readonly StringBuilder _traceBuffer;
        private static readonly MethodInfo _type_GetTypeFromHandle = typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) });
        private readonly ILGenerator _underlying;

        public TracingILGenerator(DynamicMethod dynamicMethod, TextWriter traceWriter) : this((dynamicMethod != null) ? dynamicMethod.GetILGenerator() : null, true, traceWriter, false)
        {
            Contract.Assert(dynamicMethod != null);
        }

        public TracingILGenerator(MethodBuilder methodBuilder, TextWriter traceWriter) : this((methodBuilder != null) ? methodBuilder.GetILGenerator() : null, false, traceWriter, false)
        {
            Contract.Assert(methodBuilder != null);
        }

        public TracingILGenerator(MethodBuilder methodBuilder, TextWriter traceWriter, bool isDebuggable) : this((methodBuilder != null) ? methodBuilder.GetILGenerator() : null, false, traceWriter, isDebuggable)
        {
            Contract.Assert(methodBuilder != null);
        }

        private TracingILGenerator(ILGenerator underlying, bool isInDynamicMethod, TextWriter traceWriter, bool isDebuggable)
        {
            this._localDeclarations = new Dictionary<LocalBuilder, string>();
            this._labels = new Dictionary<Label, string>();
            this._endOfExceptionBlocks = new Stack<Label>();
            this._indentChars = "  ";
            this._isEnded = false;
            this._underlying = underlying;
            this._realTrace = traceWriter ?? TextWriter.Null;
            this._traceBuffer = (traceWriter != null) ? new StringBuilder() : null;
            this._trace = (traceWriter != null) ? new StringWriter(this._traceBuffer, CultureInfo.InvariantCulture) : TextWriter.Null;
            this._isInDynamicMethod = isInDynamicMethod;
            this._endOfMethod = (underlying == null) ? new Label() : underlying.DefineLabel();
            this._isDebuggable = isDebuggable;
        }

        public void BeginCatchBlock(Type exceptionType)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(exceptionType != null);
            Contract.Assert(this.IsInExceptionBlock);
            this.Unindent();
            this.TraceStart();
            this.TraceWrite(".catch");
            this.TraceType(exceptionType);
            this.TraceWriteLine();
            this.Indent();
            this._underlying.BeginCatchBlock(exceptionType);
        }

        public void BeginExceptFilterBlock()
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(this.IsInExceptionBlock);
            this.Unindent();
            this.TraceStart();
            this.TraceWriteLine(".filter");
            this.Indent();
            this._underlying.BeginExceptFilterBlock();
        }

        public Label BeginExceptionBlock()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceWriteLine(".try");
            this.Indent();
            Label item = this._underlying.BeginExceptionBlock();
            this._endOfExceptionBlocks.Push(item);
            this._labels[item] = "END_TRY_" + this._labels.Count.ToString(CultureInfo.InvariantCulture);
            return item;
        }

        public void BeginFaultBlock()
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(this.IsInExceptionBlock);
            this.Unindent();
            this.TraceStart();
            this.TraceWriteLine(".fault");
            this.Indent();
            this._underlying.BeginFaultBlock();
        }

        public void BeginFinallyBlock()
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(this.IsInExceptionBlock);
            this.Unindent();
            this.TraceStart();
            this.TraceWriteLine(".finally");
            this.Indent();
            this._underlying.BeginFinallyBlock();
        }

        public LocalBuilder DeclareLocal(Type localType)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(localType != null);
            return this.DeclareLocalCore(localType, null);
        }

        public LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(localType != null);
            return this.DeclareLocalCore(localType, null, pinned);
        }

        public LocalBuilder DeclareLocal(Type localType, string name)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(localType != null);
            Contract.Assert(name != null);
            return this.DeclareLocalCore(localType, name);
        }

        public LocalBuilder DeclareLocal(Type localType, string name, bool pinned)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(localType != null);
            Contract.Assert(name != null);
            return this.DeclareLocalCore(localType, name, pinned);
        }

        private LocalBuilder DeclareLocalCore(Type localType, string name)
        {
            LocalBuilder key = this._underlying.DeclareLocal(localType);
            this._localDeclarations.Add(key, name);
            if (!this._isInDynamicMethod && this._isDebuggable)
            {
                try
                {
                    key.SetLocalSymInfo(name);
                }
                catch (NotSupportedException)
                {
                    this._isInDynamicMethod = true;
                }
            }
            return key;
        }

        private LocalBuilder DeclareLocalCore(Type localType, string name, bool pinned)
        {
            LocalBuilder key = this._underlying.DeclareLocal(localType, pinned);
            this._localDeclarations.Add(key, name);
            if (!this._isInDynamicMethod && this._isDebuggable)
            {
                try
                {
                    key.SetLocalSymInfo(name);
                }
                catch (NotSupportedException)
                {
                    this._isInDynamicMethod = true;
                }
            }
            return key;
        }

        public Label DefineLabel()
        {
            Contract.Assert(!this.IsEnded);
            return this.DefineLabel("LABEL_" + this._labels.Count.ToString(CultureInfo.InvariantCulture));
        }

        public Label DefineLabel(string name)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(name != null);
            Label key = this._underlying.DefineLabel();
            this._labels.Add(key, name);
            return key;
        }

        public void Dispose()
        {
            this._trace.Dispose();
        }

        public void EmitAdd()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Add);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Add);
        }

        public void EmitAdd_Ovf()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Add_Ovf);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Add_Ovf);
        }

        public void EmitAdd_Ovf_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Add_Ovf_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Add_Ovf_Un);
        }

        public void EmitAnd()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.And);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.And);
        }

        public void EmitAnyCall(MethodInfo target)
        {
            Contract.Assert(target != null);
            if (target.IsStatic || target.DeclaringType.IsValueType)
            {
                this.EmitCall(target);
            }
            else
            {
                this.EmitCallvirt(target);
            }
        }

        public void EmitAnyLdarg(int argumentIndex)
        {
            Contract.Assert((0 <= argumentIndex) && (argumentIndex <= 0xffff));
            switch (argumentIndex)
            {
                case 0:
                    this.EmitLdarg_0();
                    break;

                case 1:
                    this.EmitLdarg_1();
                    break;

                case 2:
                    this.EmitLdarg_2();
                    break;

                case 3:
                    this.EmitLdarg_3();
                    break;

                default:
                    if ((-128 <= argumentIndex) && (argumentIndex <= 0x7f))
                    {
                        this.EmitLdarg_S((byte) ((sbyte) argumentIndex));
                    }
                    else
                    {
                        this.EmitLdarg(argumentIndex);
                    }
                    break;
            }
        }

        public void EmitAnyLdarga(int argumentIndex)
        {
            Contract.Assert((0 <= argumentIndex) && (argumentIndex <= 0xffff));
            if ((-128 <= argumentIndex) && (argumentIndex <= 0x7f))
            {
                this.EmitLdarga_S((byte) ((sbyte) argumentIndex));
            }
            else
            {
                this.EmitLdarga(argumentIndex);
            }
        }

        public void EmitAnyLdc_I4(int value)
        {
            switch (value)
            {
                case -1:
                    this.EmitLdc_I4_M1();
                    break;

                case 0:
                    this.EmitLdc_I4_0();
                    break;

                case 1:
                    this.EmitLdc_I4_1();
                    break;

                case 2:
                    this.EmitLdc_I4_2();
                    break;

                case 3:
                    this.EmitLdc_I4_3();
                    break;

                case 4:
                    this.EmitLdc_I4_4();
                    break;

                case 5:
                    this.EmitLdc_I4_5();
                    break;

                case 6:
                    this.EmitLdc_I4_6();
                    break;

                case 7:
                    this.EmitLdc_I4_7();
                    break;

                default:
                    if ((-128 <= value) && (value <= 0x7f))
                    {
                        this.EmitLdc_I4_S((byte) ((sbyte) value));
                    }
                    else
                    {
                        this.EmitLdc_I4(value);
                    }
                    break;
            }
        }

        public void EmitAnyLdelem(Type elementType, Action<TracingILGenerator> arrayLoadingEmitter, long index)
        {
            Contract.Assert(elementType != null);
            Contract.Assert(0L <= index);
            Contract.Assert(arrayLoadingEmitter != null);
            arrayLoadingEmitter(this);
            this.EmitLiteralInteger(index);
            if (elementType.IsGenericParameter)
            {
                this.EmitLdelem(elementType);
            }
            else if (!elementType.IsValueType)
            {
                this.EmitLdelem_Ref();
            }
            else
            {
                switch (Type.GetTypeCode(elementType))
                {
                    case TypeCode.Boolean:
                    case TypeCode.SByte:
                        this.EmitLdelem_I1();
                        return;

                    case TypeCode.Char:
                    case TypeCode.UInt16:
                        this.EmitLdelem_U2();
                        return;

                    case TypeCode.Byte:
                        this.EmitLdelem_U1();
                        return;

                    case TypeCode.Int16:
                        this.EmitLdelem_I2();
                        return;

                    case TypeCode.Int32:
                        this.EmitLdelem_I4();
                        return;

                    case TypeCode.UInt32:
                        this.EmitLdelem_U4();
                        return;

                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        this.EmitLdelem_I8();
                        return;

                    case TypeCode.Single:
                        this.EmitLdelem_R4();
                        return;

                    case TypeCode.Double:
                        this.EmitLdelem_R8();
                        return;
                }
                this.EmitLdelema(elementType);
                this.EmitLdobj(elementType);
            }
        }

        public void EmitAnyLdloc(int localIndex)
        {
            Contract.Assert((0 <= localIndex) && (localIndex <= 0xffff));
            switch (localIndex)
            {
                case 0:
                    this.EmitLdloc_0();
                    break;

                case 1:
                    this.EmitLdloc_1();
                    break;

                case 2:
                    this.EmitLdloc_2();
                    break;

                case 3:
                    this.EmitLdloc_3();
                    break;

                default:
                    if ((-128 <= localIndex) && (localIndex <= 0x7f))
                    {
                        this.EmitLdloc_S((byte) ((sbyte) localIndex));
                    }
                    else
                    {
                        this.EmitLdloc(localIndex);
                    }
                    break;
            }
        }

        public void EmitAnyLdloc(LocalBuilder local)
        {
            Contract.Assert(local != null);
            this.EmitAnyLdloc(local.LocalIndex);
        }

        public void EmitAnyLdloca(int localIndex)
        {
            Contract.Assert((0 <= localIndex) && (localIndex <= 0xffff));
            if ((-128 <= localIndex) && (localIndex <= 0x7f))
            {
                this.EmitLdloca_S((byte) ((sbyte) localIndex));
            }
            else
            {
                this.EmitLdloca(localIndex);
            }
        }

        public void EmitAnyLdloca(LocalBuilder local)
        {
            Contract.Assert(local != null);
            this.EmitAnyLdloca(local.LocalIndex);
        }

        public void EmitAnyStelem(Type elementType, Action<TracingILGenerator> arrayLoadingEmitter, long index, Action<TracingILGenerator> elementLoadingEmitter)
        {
            Contract.Assert(elementType != null);
            Contract.Assert(0L <= index);
            Contract.Assert(arrayLoadingEmitter != null);
            Contract.Assert(elementLoadingEmitter != null);
            arrayLoadingEmitter(this);
            this.EmitLiteralInteger(index);
            elementLoadingEmitter(this);
            if (elementType.IsGenericParameter)
            {
                this.EmitStelem(elementType);
            }
            else if (!elementType.IsValueType)
            {
                this.EmitStelem_Ref();
            }
            else
            {
                switch (Type.GetTypeCode(elementType))
                {
                    case TypeCode.Boolean:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                        this.EmitStelem_I1();
                        return;

                    case TypeCode.Char:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                        this.EmitStelem_I2();
                        return;

                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                        this.EmitStelem_I4();
                        return;

                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        this.EmitStelem_I8();
                        return;

                    case TypeCode.Single:
                        this.EmitStelem_R4();
                        return;

                    case TypeCode.Double:
                        this.EmitStelem_R8();
                        return;
                }
                this.EmitLdelema(elementType);
                elementLoadingEmitter(this);
                this.EmitStobj(elementType);
            }
        }

        public void EmitAnyStloc(int localIndex)
        {
            Contract.Assert((0 <= localIndex) && (localIndex <= 0xffff));
            switch (localIndex)
            {
                case 0:
                    this.EmitStloc_0();
                    break;

                case 1:
                    this.EmitStloc_1();
                    break;

                case 2:
                    this.EmitStloc_2();
                    break;

                case 3:
                    this.EmitStloc_3();
                    break;

                default:
                    if ((-128 <= localIndex) && (localIndex <= 0x7f))
                    {
                        this.EmitStloc_S((byte) ((sbyte) localIndex));
                    }
                    else
                    {
                        this.EmitStloc(localIndex);
                    }
                    break;
            }
        }

        public void EmitAnyStloc(LocalBuilder local)
        {
            Contract.Assert(local != null);
            this.EmitAnyStloc(local.LocalIndex);
        }

        public void EmitArglist()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Arglist);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Arglist);
        }

        public void EmitBeq(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Beq);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Beq, target);
        }

        public void EmitBeq_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Beq_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Beq_S, target);
        }

        public void EmitBge(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Bge);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Bge, target);
        }

        public void EmitBge_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Bge_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Bge_S, target);
        }

        public void EmitBge_Un(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Bge_Un);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Bge_Un, target);
        }

        public void EmitBge_Un_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Bge_Un_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Bge_Un_S, target);
        }

        public void EmitBgt(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Bgt);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Bgt, target);
        }

        public void EmitBgt_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Bgt_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Bgt_S, target);
        }

        public void EmitBgt_Un(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Bgt_Un);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Bgt_Un, target);
        }

        public void EmitBgt_Un_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Bgt_Un_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Bgt_Un_S, target);
        }

        public void EmitBle(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ble);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ble, target);
        }

        public void EmitBle_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ble_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ble_S, target);
        }

        public void EmitBle_Un(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ble_Un);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ble_Un, target);
        }

        public void EmitBle_Un_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ble_Un_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ble_Un_S, target);
        }

        public void EmitBlt(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Blt);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Blt, target);
        }

        public void EmitBlt_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Blt_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Blt_S, target);
        }

        public void EmitBlt_Un(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Blt_Un);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Blt_Un, target);
        }

        public void EmitBlt_Un_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Blt_Un_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Blt_Un_S, target);
        }

        public void EmitBne_Un(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Bne_Un);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Bne_Un, target);
        }

        public void EmitBne_Un_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Bne_Un_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Bne_Un_S, target);
        }

        public void EmitBox(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Box);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Box, type);
        }

        public void EmitBr(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Br);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Br, target);
        }

        public void EmitBr_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Br_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Br_S, target);
        }

        public void EmitBreak()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Break);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Break);
        }

        public void EmitBrfalse(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Brfalse);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Brfalse, target);
        }

        public void EmitBrfalse_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Brfalse_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Brfalse_S, target);
        }

        public void EmitBrtrue(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Brtrue);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Brtrue, target);
        }

        public void EmitBrtrue_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Brtrue_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Brtrue_S, target);
        }

        public void EmitCall(MethodInfo target)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(target != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Call);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Call, target);
        }

        public void EmitCallConstructor(ConstructorInfo constructor)
        {
            Contract.Assert(constructor != null);
            Contract.Assert(!constructor.IsStatic);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Call);
            this.TraceWrite(" ");
            this.TraceOperand(constructor);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Call, constructor);
        }

        public void EmitCalli(CallingConvention unmanagedCallingConvention, Type returnType, Type[] parameterTypes)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(returnType != null);
            Contract.Assert(parameterTypes != null);
            Contract.Assert(Contract.ForAll<Type>(parameterTypes, item => item != null));
            this.TraceStart();
            this.TraceWrite("calli ");
            this.TraceSignature(null, new CallingConvention?(unmanagedCallingConvention), returnType, parameterTypes, Type.EmptyTypes);
            this._underlying.EmitCalli(OpCodes.Calli, unmanagedCallingConvention, returnType, parameterTypes);
        }

        public void EmitCalli(CallingConventions managedCallingConventions, Type returnType, Type[] requiredParameterTypes, Type[] optionalParameterTypes)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(returnType != null);
            Contract.Assert(requiredParameterTypes != null);
            Contract.Assert(Contract.ForAll<Type>(requiredParameterTypes, item => item != null));
            Contract.Assert(optionalParameterTypes != null);
            Contract.Assert(((optionalParameterTypes == null) || (optionalParameterTypes.Length == 0)) || ((managedCallingConventions & CallingConventions.VarArgs) == CallingConventions.VarArgs));
            Contract.Assert((optionalParameterTypes == null) || Contract.ForAll<Type>(optionalParameterTypes, item => item != null));
            this.TraceStart();
            this.TraceWrite("calli ");
            this.TraceSignature(new CallingConventions?(managedCallingConventions), null, returnType, requiredParameterTypes, optionalParameterTypes);
            this._underlying.EmitCalli(OpCodes.Calli, managedCallingConventions, returnType, requiredParameterTypes, optionalParameterTypes);
        }

        public void EmitCallvirt(MethodInfo target)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(target != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Callvirt);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Callvirt, target);
        }

        public void EmitCastclass(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Castclass);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Castclass, type);
        }

        public void EmitCeq()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ceq);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ceq);
        }

        public void EmitCgt()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Cgt);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Cgt);
        }

        public void EmitCgt_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Cgt_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Cgt_Un);
        }

        public void EmitCkfinite()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ckfinite);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ckfinite);
        }

        public void EmitClt()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Clt);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Clt);
        }

        public void EmitClt_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Clt_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Clt_Un);
        }

        public void EmitConstrainedCallvirt(Type constrainedTo, MethodInfo target)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(constrainedTo != null);
            Contract.Assert(target != null);
            this.TraceStart();
            this.TraceWrite("constraind.");
            this.TraceType(constrainedTo);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Constrained, constrainedTo);
            this.EmitCallvirt(target);
        }

        public void EmitConv_I()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_I);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_I);
        }

        public void EmitConv_I1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_I1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_I1);
        }

        public void EmitConv_I2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_I2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_I2);
        }

        public void EmitConv_I4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_I4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_I4);
        }

        public void EmitConv_I8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_I8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_I8);
        }

        public void EmitConv_Ovf_I()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_I);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_I);
        }

        public void EmitConv_Ovf_I_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_I_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_I_Un);
        }

        public void EmitConv_Ovf_I1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_I1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_I1);
        }

        public void EmitConv_Ovf_I1_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_I1_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_I1_Un);
        }

        public void EmitConv_Ovf_I2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_I2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_I2);
        }

        public void EmitConv_Ovf_I2_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_I2_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_I2_Un);
        }

        public void EmitConv_Ovf_I4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_I4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_I4);
        }

        public void EmitConv_Ovf_I4_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_I4_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_I4_Un);
        }

        public void EmitConv_Ovf_I8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_I8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_I8);
        }

        public void EmitConv_Ovf_I8_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_I8_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_I8_Un);
        }

        public void EmitConv_Ovf_U()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_U);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_U);
        }

        public void EmitConv_Ovf_U_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_U_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_U_Un);
        }

        public void EmitConv_Ovf_U1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_U1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_U1);
        }

        public void EmitConv_Ovf_U1_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_U1_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_U1_Un);
        }

        public void EmitConv_Ovf_U2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_U2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_U2);
        }

        public void EmitConv_Ovf_U2_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_U2_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_U2_Un);
        }

        public void EmitConv_Ovf_U4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_U4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_U4);
        }

        public void EmitConv_Ovf_U4_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_U4_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_U4_Un);
        }

        public void EmitConv_Ovf_U8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_U8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_U8);
        }

        public void EmitConv_Ovf_U8_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_Ovf_U8_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_Ovf_U8_Un);
        }

        public void EmitConv_R_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_R_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_R_Un);
        }

        public void EmitConv_R4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_R4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_R4);
        }

        public void EmitConv_R8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_R8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_R8);
        }

        public void EmitConv_U()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_U);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_U);
        }

        public void EmitConv_U1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_U1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_U1);
        }

        public void EmitConv_U2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_U2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_U2);
        }

        public void EmitConv_U4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_U4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_U4);
        }

        public void EmitConv_U8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Conv_U8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Conv_U8);
        }

        public void EmitCpblk()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Cpblk);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Cpblk);
        }

        public void EmitCpobj(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Cpobj);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Cpobj, type);
        }

        public void EmitCurrentCulture()
        {
            this.EmitGetProperty(_cultureInfo_CurrentCulture);
        }

        public void EmitDiv()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Div);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Div);
        }

        public void EmitDiv_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Div_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Div_Un);
        }

        public void EmitDup()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Dup);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Dup);
        }

        public void EmitEndfilter()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Endfilter);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Endfilter);
        }

        public void EmitEndfinally()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Endfinally);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Endfinally);
        }

        public void EmitExceptionBlock(Action<TracingILGenerator, Label> tryBlockEmitter, Action<TracingILGenerator, Label> finallyBlockEmitter, params Tuple<Type, Action<TracingILGenerator, Label, Type>>[] catchBlockEmitters)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(tryBlockEmitter != null);
            Contract.Assert(finallyBlockEmitter != null);
            Contract.Assert(catchBlockEmitters != null);
            Contract.Assert(Contract.ForAll<Tuple<Type, Action<TracingILGenerator, Label, Type>>>(catchBlockEmitters, item => ((item != null) && (item.Item1 != null)) && (item.Item2 != null)));
            this.EmitExceptionBlockCore(tryBlockEmitter, null, catchBlockEmitters, finallyBlockEmitter);
        }

        public void EmitExceptionBlock(Action<TracingILGenerator, Label> tryBlockEmitter, Tuple<Type, Action<TracingILGenerator, Label, Type>> firstCatchBlock, params Tuple<Type, Action<TracingILGenerator, Label, Type>>[] remainingCatchBlockEmitters)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(tryBlockEmitter != null);
            Contract.Assert(firstCatchBlock != null);
            Contract.Assert((firstCatchBlock.Item1 != null) && (firstCatchBlock.Item2 != null));
            Contract.Assert(remainingCatchBlockEmitters != null);
            Contract.Assert(Contract.ForAll<Tuple<Type, Action<TracingILGenerator, Label, Type>>>(remainingCatchBlockEmitters, item => ((item != null) && (item.Item1 != null)) && (item.Item2 != null)));
            this.EmitExceptionBlockCore(tryBlockEmitter, firstCatchBlock, remainingCatchBlockEmitters, null);
        }

        private void EmitExceptionBlockCore(Action<TracingILGenerator, Label> tryBlockEmitter, Tuple<Type, Action<TracingILGenerator, Label, Type>> firstCatchBlockEmitter, Tuple<Type, Action<TracingILGenerator, Label, Type>>[] remainingCatchBlockEmitters, Action<TracingILGenerator, Label> finallyBlockEmitter)
        {
            Label label = this.BeginExceptionBlock();
            tryBlockEmitter(this, label);
            if (firstCatchBlockEmitter != null)
            {
                this.BeginCatchBlock(firstCatchBlockEmitter.Item1);
                firstCatchBlockEmitter.Item2(this, label, firstCatchBlockEmitter.Item1);
            }
            foreach (Tuple<Type, Action<TracingILGenerator, Label, Type>> tuple in remainingCatchBlockEmitters)
            {
                this.BeginCatchBlock(tuple.Item1);
                firstCatchBlockEmitter.Item2(this, label, tuple.Item1);
            }
            if (finallyBlockEmitter != null)
            {
                this.BeginFinallyBlock();
                finallyBlockEmitter(this, label);
            }
            this.EndExceptionBlock();
        }

        public void EmitGetProperty(PropertyInfo property)
        {
            Contract.Assert(property != null);
            Contract.Assert(property.CanRead);
            this.EmitAnyCall(property.GetGetMethod(true));
        }

        public void EmitGetType()
        {
            this.EmitCall(_Object_GetType);
        }

        public void EmitInitblk()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Initblk);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Initblk);
        }

        public void EmitInitobj(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Initobj);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Initobj, type);
        }

        public void EmitInvariantCulture()
        {
            this.EmitGetProperty(_cultureInfo_InvariantCulture);
        }

        public void EmitIsinst(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Isinst);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Isinst, type);
        }

        public void EmitJmp(MethodInfo target)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(target != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Jmp);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Jmp, target);
        }

        public void EmitLdarg(int index)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert((0 <= index) && (index <= 0xffff));
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldarg);
            this.TraceWrite(" ");
            this.TraceOperand(index);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldarg, index);
        }

        public void EmitLdarg_0()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldarg_0);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldarg_0);
        }

        public void EmitLdarg_1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldarg_1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldarg_1);
        }

        public void EmitLdarg_2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldarg_2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldarg_2);
        }

        public void EmitLdarg_3()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldarg_3);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldarg_3);
        }

        public void EmitLdarg_S(byte value)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldarg_S);
            this.TraceWrite(" ");
            this.TraceOperand((int) value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldarg_S, value);
        }

        public void EmitLdarga(int index)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert((0 <= index) && (index <= 0xffff));
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldarga);
            this.TraceWrite(" ");
            this.TraceOperand(index);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldarga, index);
        }

        public void EmitLdarga_S(byte value)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldarga_S);
            this.TraceWrite(" ");
            this.TraceOperand((int) value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldarga_S, value);
        }

        public void EmitLdargThis()
        {
            this.EmitLdarg_0();
        }

        public void EmitLdc_I4(int value)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4);
            this.TraceWrite(" ");
            this.TraceOperand(value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4, value);
        }

        public void EmitLdc_I4_0()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4_0);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4_0);
        }

        public void EmitLdc_I4_1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4_1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4_1);
        }

        public void EmitLdc_I4_2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4_2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4_2);
        }

        public void EmitLdc_I4_3()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4_3);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4_3);
        }

        public void EmitLdc_I4_4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4_4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4_4);
        }

        public void EmitLdc_I4_5()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4_5);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4_5);
        }

        public void EmitLdc_I4_6()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4_6);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4_6);
        }

        public void EmitLdc_I4_7()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4_7);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4_7);
        }

        public void EmitLdc_I4_8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4_8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4_8);
        }

        public void EmitLdc_I4_M1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4_M1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4_M1);
        }

        public void EmitLdc_I4_S(byte value)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I4_S);
            this.TraceWrite(" ");
            this.TraceOperand((int) value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I4_S, value);
        }

        public void EmitLdc_I8(long value)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_I8);
            this.TraceWrite(" ");
            this.TraceOperand(value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_I8, value);
        }

        public void EmitLdc_R4(byte value)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_R4);
            this.TraceWrite(" ");
            this.TraceOperand((int) value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_R4, value);
        }

        public void EmitLdc_R8(double value)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldc_R8);
            this.TraceWrite(" ");
            this.TraceOperand(value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldc_R8, value);
        }

        public void EmitLdelem(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem, type);
        }

        public void EmitLdelem_I()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem_I);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem_I);
        }

        public void EmitLdelem_I1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem_I1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem_I1);
        }

        public void EmitLdelem_I2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem_I2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem_I2);
        }

        public void EmitLdelem_I4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem_I4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem_I4);
        }

        public void EmitLdelem_I8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem_I8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem_I8);
        }

        public void EmitLdelem_R4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem_R4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem_R4);
        }

        public void EmitLdelem_R8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem_R8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem_R8);
        }

        public void EmitLdelem_Ref()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem_Ref);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem_Ref);
        }

        public void EmitLdelem_U1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem_U1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem_U1);
        }

        public void EmitLdelem_U2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem_U2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem_U2);
        }

        public void EmitLdelem_U4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelem_U4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelem_U4);
        }

        public void EmitLdelema(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldelema);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldelema, type);
        }

        public void EmitLdfld(FieldInfo field)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(field != null);
            if (!(field is FieldBuilder) && field.GetRequiredCustomModifiers().Any<Type>(item => typeof(IsVolatile).Equals(item)))
            {
                this.TraceStart();
                this.TraceOpCode(OpCodes.Volatile);
                this.TraceWriteLine();
            }
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldfld);
            this.TraceWrite(" ");
            this.TraceOperand(field);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldfld, field);
        }

        public void EmitLdflda(FieldInfo field)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(field != null);
            if (!(field is FieldBuilder) && field.GetRequiredCustomModifiers().Any<Type>(item => typeof(IsVolatile).Equals(item)))
            {
                this.TraceStart();
                this.TraceOpCode(OpCodes.Volatile);
                this.TraceWriteLine();
            }
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldflda);
            this.TraceWrite(" ");
            this.TraceOperand(field);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldflda, field);
        }

        public void EmitLdftn(MethodInfo method)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(method != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldftn);
            this.TraceWrite(" ");
            this.TraceOperand(method);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldftn, method);
        }

        public void EmitLdind_I()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldind_I);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldind_I);
        }

        public void EmitLdind_I1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldind_I1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldind_I1);
        }

        public void EmitLdind_I2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldind_I2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldind_I2);
        }

        public void EmitLdind_I4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldind_I4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldind_I4);
        }

        public void EmitLdind_I8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldind_I8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldind_I8);
        }

        public void EmitLdind_R4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldind_R4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldind_R4);
        }

        public void EmitLdind_R8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldind_R8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldind_R8);
        }

        public void EmitLdind_Ref()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldind_Ref);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldind_Ref);
        }

        public void EmitLdind_U1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldind_U1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldind_U1);
        }

        public void EmitLdind_U2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldind_U2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldind_U2);
        }

        public void EmitLdind_U4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldind_U4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldind_U4);
        }

        public void EmitLdlen()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldlen);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldlen);
        }

        public void EmitLdloc(int index)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert((0 <= index) && (index <= 0xffff));
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldloc);
            this.TraceWrite(" ");
            this.TraceOperand(index);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldloc, index);
        }

        public void EmitLdloc_0()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldloc_0);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldloc_0);
        }

        public void EmitLdloc_1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldloc_1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldloc_1);
        }

        public void EmitLdloc_2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldloc_2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldloc_2);
        }

        public void EmitLdloc_3()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldloc_3);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldloc_3);
        }

        public void EmitLdloc_S(byte value)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldloc_S);
            this.TraceWrite(" ");
            this.TraceOperand((int) value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldloc_S, value);
        }

        public void EmitLdloca(int index)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert((0 <= index) && (index <= 0xffff));
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldloca);
            this.TraceWrite(" ");
            this.TraceOperand(index);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldloca, index);
        }

        public void EmitLdloca_S(byte value)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldloca_S);
            this.TraceWrite(" ");
            this.TraceOperand((int) value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldloca_S, value);
        }

        public void EmitLdnull()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldnull);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldnull);
        }

        public void EmitLdobj(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldobj);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldobj, type);
        }

        public void EmitLdsfld(FieldInfo field)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(field != null);
            if (!(field is FieldBuilder) && field.GetRequiredCustomModifiers().Any<Type>(item => typeof(IsVolatile).Equals(item)))
            {
                this.TraceStart();
                this.TraceOpCode(OpCodes.Volatile);
                this.TraceWriteLine();
            }
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldsfld);
            this.TraceWrite(" ");
            this.TraceOperand(field);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldsfld, field);
        }

        public void EmitLdsflda(FieldInfo field)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(field != null);
            if (!(field is FieldBuilder) && field.GetRequiredCustomModifiers().Any<Type>(item => typeof(IsVolatile).Equals(item)))
            {
                this.TraceStart();
                this.TraceOpCode(OpCodes.Volatile);
                this.TraceWriteLine();
            }
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldsflda);
            this.TraceWrite(" ");
            this.TraceOperand(field);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldsflda, field);
        }

        public void EmitLdstr(string value)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(value != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldstr);
            this.TraceWrite(" ");
            this.TraceOperand(value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldstr, value);
        }

        public void EmitLdtoken(FieldInfo target)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(target != null);
            if (!(target is FieldBuilder) && target.GetRequiredCustomModifiers().Any<Type>(item => typeof(IsVolatile).Equals(item)))
            {
                this.TraceStart();
                this.TraceOpCode(OpCodes.Volatile);
                this.TraceWriteLine();
            }
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldtoken);
            this.TraceWrite(" ");
            this.TraceOperandToken(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldtoken, target);
        }

        public void EmitLdtoken(MethodInfo target)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(target != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldtoken);
            this.TraceWrite(" ");
            this.TraceOperandToken(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldtoken, target);
        }

        public void EmitLdtoken(Type target)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(target != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldtoken);
            this.TraceWrite(" ");
            this.TraceOperandToken(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldtoken, target);
        }

        public void EmitLdvirtftn(MethodInfo method)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(method != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ldvirtftn);
            this.TraceWrite(" ");
            this.TraceOperand(method);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Ldvirtftn, method);
        }

        public void EmitLeave(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Leave);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Leave, target);
        }

        public void EmitLeave_S(Label target)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Leave_S);
            this.TraceWrite(" ");
            this.TraceOperand(target);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Leave_S, target);
        }

        public void EmitLiteralInteger(long value)
        {
            long num = value;
            if ((num <= 8L) && (num >= -1L))
            {
                switch (((int) (num - -1L)))
                {
                    case 0:
                        this.EmitLdc_I4_M1();
                        return;

                    case 1:
                        this.EmitLdc_I4_0();
                        return;

                    case 2:
                        this.EmitLdc_I4_1();
                        return;

                    case 3:
                        this.EmitLdc_I4_2();
                        return;

                    case 4:
                        this.EmitLdc_I4_3();
                        return;

                    case 5:
                        this.EmitLdc_I4_4();
                        return;

                    case 6:
                        this.EmitLdc_I4_5();
                        return;

                    case 7:
                        this.EmitLdc_I4_6();
                        return;

                    case 8:
                        this.EmitLdc_I4_7();
                        return;

                    case 9:
                        this.EmitLdc_I4_8();
                        return;
                }
            }
            if ((-128L <= value) && (value <= 0x7fL))
            {
                this.EmitLdc_I4_S((byte) value);
            }
            else if ((-2147483648L <= value) && (value <= 0x7fffffffL))
            {
                this.EmitLdc_I4((int) value);
            }
            else
            {
                this.EmitLdc_I8(value);
            }
        }

        public void EmitLocalloc()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Localloc);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Localloc);
        }

        public void EmitMkrefany(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Mkrefany);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Mkrefany, type);
        }

        public void EmitMul()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Mul);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Mul);
        }

        public void EmitMul_Ovf()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Mul_Ovf);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Mul_Ovf);
        }

        public void EmitMul_Ovf_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Mul_Ovf_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Mul_Ovf_Un);
        }

        public void EmitNeg()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Neg);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Neg);
        }

        public void EmitNewarr(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Newarr);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Newarr, type);
        }

        public void EmitNewarr(Type elementType, long length)
        {
            Contract.Assert(elementType != null);
            Contract.Assert(0L <= length);
            this.EmitNewarrCore(elementType, length);
        }

        public void EmitNewarr(int arrayLocalIndex, Type elementType, params Action<TracingILGenerator>[] elementLoadingEmitters)
        {
            Action<TracingILGenerator> arrayLoadingEmitter = null;
            Contract.Assert(0 <= arrayLocalIndex);
            Contract.Assert(elementType != null);
            Contract.Assert(elementLoadingEmitters != null);
            Contract.Assert(Contract.ForAll<Action<TracingILGenerator>>(elementLoadingEmitters, item => item != null));
            this.EmitNewarrCore(elementType, (long) elementLoadingEmitters.Length);
            this.EmitAnyStloc(arrayLocalIndex);
            for (long i = 0L; i < elementLoadingEmitters.Length; i += 1L)
            {
                if (arrayLoadingEmitter == null)
                {
                    arrayLoadingEmitter = il => il.EmitAnyLdloc(arrayLocalIndex);
                }
                this.EmitAnyStelem(elementType, arrayLoadingEmitter, i, elementLoadingEmitters[(int) ((IntPtr) i)]);
            }
        }

        public void EmitNewarr(Action<TracingILGenerator> arrayLoadingEmitter, Action<TracingILGenerator> arrayStoringEmitter, Type elementType, params Action<TracingILGenerator>[] elementLoadingEmitters)
        {
            Contract.Assert(arrayLoadingEmitter != null);
            Contract.Assert(arrayStoringEmitter != null);
            Contract.Assert(elementType != null);
            Contract.Assert(elementLoadingEmitters != null);
            Contract.Assert(Contract.ForAll<Action<TracingILGenerator>>(elementLoadingEmitters, item => item != null));
            this.EmitNewarrCore(elementType, (long) elementLoadingEmitters.Length);
            arrayStoringEmitter(this);
            for (long i = 0L; i < elementLoadingEmitters.Length; i += 1L)
            {
                arrayLoadingEmitter(this);
                this.EmitAnyStelem(elementType, null, i, elementLoadingEmitters[(int) ((IntPtr) i)]);
            }
        }

        private void EmitNewarrCore(Type elementType, long length)
        {
            this.EmitLiteralInteger(length);
            this.EmitNewarr(elementType);
        }

        public void EmitNewobj(ConstructorInfo constructor)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(constructor != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Newobj);
            this.TraceWrite(" ");
            this.TraceOperand(constructor);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Newobj, constructor);
        }

        public void EmitNop()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Nop);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Nop);
        }

        public void EmitNot()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Not);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Not);
        }

        public void EmitOr()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Or);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Or);
        }

        public void EmitPop()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Pop);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Pop);
        }

        public void EmitReadOnlyLdelema(Type elementType)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(elementType != null);
            this.TraceStart();
            this.TraceWriteLine("readonly.");
            this._underlying.Emit(OpCodes.Readonly);
            this.EmitLdelema(elementType);
        }

        public void EmitRefanytype()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Refanytype);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Refanytype);
        }

        public void EmitRefanyval(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Refanyval);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Refanyval, type);
        }

        public void EmitRem()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Rem);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Rem);
        }

        public void EmitRem_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Rem_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Rem_Un);
        }

        public void EmitRet()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Ret);
            this._underlying.Emit(OpCodes.Ret);
            this.FlushTrace();
            this._isEnded = true;
        }

        public void EmitRethrow()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Rethrow);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Rethrow);
        }

        public void EmitSetProperty(PropertyInfo property)
        {
            Contract.Assert(property != null);
            Contract.Assert(property.CanWrite);
            this.EmitAnyCall(property.GetSetMethod(true));
        }

        public void EmitShl()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Shl);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Shl);
        }

        public void EmitShr()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Shr);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Shr);
        }

        public void EmitShr_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Shr_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Shr_Un);
        }

        public void EmitSizeof(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Sizeof);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Sizeof, type);
        }

        public void EmitStarg(int index)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert((0 <= index) && (index <= 0xffff));
            this.TraceStart();
            this.TraceOpCode(OpCodes.Starg);
            this.TraceWrite(" ");
            this.TraceOperand(index);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Starg, index);
        }

        public void EmitStarg_S(byte value)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Starg_S);
            this.TraceWrite(" ");
            this.TraceOperand((int) value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Starg_S, value);
        }

        public void EmitStelem(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stelem);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stelem, type);
        }

        public void EmitStelem_I()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stelem_I);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stelem_I);
        }

        public void EmitStelem_I1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stelem_I1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stelem_I1);
        }

        public void EmitStelem_I2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stelem_I2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stelem_I2);
        }

        public void EmitStelem_I4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stelem_I4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stelem_I4);
        }

        public void EmitStelem_I8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stelem_I8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stelem_I8);
        }

        public void EmitStelem_R4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stelem_R4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stelem_R4);
        }

        public void EmitStelem_R8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stelem_R8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stelem_R8);
        }

        public void EmitStelem_Ref()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stelem_Ref);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stelem_Ref);
        }

        public void EmitStfld(FieldInfo field)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(field != null);
            if (!(field is FieldBuilder) && field.GetRequiredCustomModifiers().Any<Type>(item => typeof(IsVolatile).Equals(item)))
            {
                this.TraceStart();
                this.TraceOpCode(OpCodes.Volatile);
                this.TraceWriteLine();
            }
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stfld);
            this.TraceWrite(" ");
            this.TraceOperand(field);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stfld, field);
        }

        public void EmitStind_I()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stind_I);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stind_I);
        }

        public void EmitStind_I1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stind_I1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stind_I1);
        }

        public void EmitStind_I2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stind_I2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stind_I2);
        }

        public void EmitStind_I4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stind_I4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stind_I4);
        }

        public void EmitStind_I8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stind_I8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stind_I8);
        }

        public void EmitStind_R4()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stind_R4);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stind_R4);
        }

        public void EmitStind_R8()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stind_R8);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stind_R8);
        }

        public void EmitStind_Ref()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stind_Ref);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stind_Ref);
        }

        public void EmitStloc(int index)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert((0 <= index) && (index <= 0xffff));
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stloc);
            this.TraceWrite(" ");
            this.TraceOperand(index);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stloc, index);
        }

        public void EmitStloc_0()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stloc_0);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stloc_0);
        }

        public void EmitStloc_1()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stloc_1);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stloc_1);
        }

        public void EmitStloc_2()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stloc_2);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stloc_2);
        }

        public void EmitStloc_3()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stloc_3);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stloc_3);
        }

        public void EmitStloc_S(byte value)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stloc_S);
            this.TraceWrite(" ");
            this.TraceOperand((int) value);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stloc_S, value);
        }

        public void EmitStobj(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stobj);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stobj, type);
        }

        public void EmitStringFormat(int temporaryLocalArrayIndex, string formatLiteral, params Action<TracingILGenerator>[] argumentLoadingEmitters)
        {
            Contract.Assert(formatLiteral != null);
            Contract.Assert(0 < formatLiteral.Length);
            Contract.Assert(argumentLoadingEmitters != null);
            Contract.Assert(Contract.ForAll<Action<TracingILGenerator>>(argumentLoadingEmitters, item => item != null));
            this.EmitCurrentCulture();
            this.EmitLdstr(formatLiteral);
            this.EmitStringFormatArgumentAndCall(temporaryLocalArrayIndex, argumentLoadingEmitters);
        }

        public void EmitStringFormat(int temporaryLocalArrayIndex, Type resource, string resourceKey, params Action<TracingILGenerator>[] argumentLoadingEmitters)
        {
            Contract.Assert(resource != null);
            Contract.Assert(resourceKey != null);
            Contract.Assert(!string.IsNullOrEmpty(resourceKey));
            Contract.Assert(argumentLoadingEmitters != null);
            Contract.Assert(Contract.ForAll<Action<TracingILGenerator>>(argumentLoadingEmitters, item => item != null));
            this.EmitCurrentCulture();
            this.EmitGetProperty(resource.GetProperty(resourceKey, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static));
            this.EmitStringFormatArgumentAndCall(temporaryLocalArrayIndex, argumentLoadingEmitters);
        }

        private void EmitStringFormatArgumentAndCall(int temporaryLocalArrayIndex, Action<TracingILGenerator>[] argumentEmitters)
        {
            this.EmitNewarr(temporaryLocalArrayIndex, typeof(object), argumentEmitters);
            this.EmitAnyLdloc(temporaryLocalArrayIndex);
            this.EmitCall(_string_Format);
        }

        public void EmitStringFormatInvariant(int temporaryLocalArrayIndex, string formatLiteral, params Action<TracingILGenerator>[] argumentLoadingEmitters)
        {
            Contract.Assert(formatLiteral != null);
            Contract.Assert(0 < formatLiteral.Length);
            Contract.Assert(argumentLoadingEmitters != null);
            Contract.Assert(Contract.ForAll<Action<TracingILGenerator>>(argumentLoadingEmitters, item => item != null));
            this.EmitInvariantCulture();
            this.EmitLdstr(formatLiteral);
            this.EmitStringFormatArgumentAndCall(temporaryLocalArrayIndex, argumentLoadingEmitters);
        }

        public void EmitStringFormatInvariant(int temporaryLocalArrayIndex, Type resource, string resourceKey, params Action<TracingILGenerator>[] argumentLoadingEmitters)
        {
            Contract.Assert(resource != null);
            Contract.Assert(resourceKey != null);
            Contract.Assert(!string.IsNullOrEmpty(resourceKey));
            Contract.Assert(argumentLoadingEmitters != null);
            Contract.Assert(Contract.ForAll<Action<TracingILGenerator>>(argumentLoadingEmitters, item => item != null));
            this.EmitInvariantCulture();
            this.EmitGetProperty(resource.GetProperty(resourceKey, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static));
            this.EmitStringFormatArgumentAndCall(temporaryLocalArrayIndex, argumentLoadingEmitters);
        }

        public void EmitStsfld(FieldInfo field)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(field != null);
            if (!(field is FieldBuilder) && field.GetRequiredCustomModifiers().Any<Type>(item => typeof(IsVolatile).Equals(item)))
            {
                this.TraceStart();
                this.TraceOpCode(OpCodes.Volatile);
                this.TraceWriteLine();
            }
            this.TraceStart();
            this.TraceOpCode(OpCodes.Stsfld);
            this.TraceWrite(" ");
            this.TraceOperand(field);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Stsfld, field);
        }

        public void EmitSub()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Sub);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Sub);
        }

        public void EmitSub_Ovf()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Sub_Ovf);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Sub_Ovf);
        }

        public void EmitSub_Ovf_Un()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Sub_Ovf_Un);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Sub_Ovf_Un);
        }

        public void EmitSwitch(params Label[] targets)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(targets != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Switch);
            this.TraceWrite(" ");
            this.TraceOperand(targets);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Switch, targets);
        }

        public void EmitTailCall(MethodInfo target)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(target != null);
            this.TraceStart();
            this.TraceWriteLine("tail.");
            this._underlying.Emit(OpCodes.Tailcall);
            this.EmitCall(target);
            this.EmitRet();
        }

        public void EmitTailCalli(CallingConvention unmanagedCallingConvention, Type returnType, Type[] parameterTypes)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(returnType != null);
            Contract.Assert(parameterTypes != null);
            Contract.Assert(Contract.ForAll<Type>(parameterTypes, item => item != null));
            this.TraceStart();
            this.TraceWriteLine("tail.");
            this._underlying.Emit(OpCodes.Tailcall);
            this.EmitCalli(unmanagedCallingConvention, returnType, parameterTypes);
            this.EmitRet();
        }

        public void EmitTailCalli(CallingConventions managedCallingConventions, Type returnType, Type[] requiredParameterTypes, Type[] optionalParameterTypes)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(returnType != null);
            Contract.Assert(requiredParameterTypes != null);
            Contract.Assert(Contract.ForAll<Type>(requiredParameterTypes, item => item != null));
            Contract.Assert(optionalParameterTypes != null);
            Contract.Assert(((optionalParameterTypes == null) || (optionalParameterTypes.Length == 0)) || ((managedCallingConventions & CallingConventions.VarArgs) == CallingConventions.VarArgs));
            Contract.Assert((optionalParameterTypes == null) || Contract.ForAll<Type>(optionalParameterTypes, item => item != null));
            this.TraceStart();
            this.TraceWriteLine("tail.");
            this._underlying.Emit(OpCodes.Tailcall);
            this.EmitCalli(managedCallingConventions, returnType, requiredParameterTypes, optionalParameterTypes);
            this.EmitRet();
        }

        public void EmitTailCallVirt(MethodInfo target)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(target != null);
            this.TraceStart();
            this.TraceWriteLine("tail.");
            this._underlying.Emit(OpCodes.Tailcall);
            this.EmitCallvirt(target);
            this.EmitRet();
        }

        public void EmitThrow()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Throw);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Throw);
        }

        public void EmitThrowNewArgumentExceptionWithInnerException()
        {
            this.EmitNewobj(_ArgumentException_ctor_String_String_Exception);
            this.EmitThrow();
        }

        public void EmitThrowNewExceptionWithInnerException(Type exceptionType)
        {
            Contract.Assert(exceptionType != null);
            ConstructorInfo constructor = exceptionType.GetConstructor(_standardExceptionConstructorParamterTypesWithInnerException);
            if (constructor == null)
            {
                throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Exception type '{0}' does not have standard constructor '.ctor(String, Exception)'.", new object[] { exceptionType }));
            }
            this.EmitNewobj(constructor);
            this.EmitThrow();
        }

        public void EmitTypeOf(Type type)
        {
            Contract.Assert(type != null);
            this.EmitLdtoken(type);
            this.EmitCall(_type_GetTypeFromHandle);
        }

        public void EmitUnaligned(byte alignment)
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceWrite("unaligned.");
            this.TraceOperand((int) alignment);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Unaligned, alignment);
        }

        public void EmitUnbox(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Unbox);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Unbox, type);
        }

        public void EmitUnbox_Any(Type type)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(type != null);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Unbox_Any);
            this.TraceWrite(" ");
            this.TraceOperand(type);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Unbox_Any, type);
        }

        public void EmitXor()
        {
            Contract.Assert(!this.IsEnded);
            this.TraceStart();
            this.TraceOpCode(OpCodes.Xor);
            this.TraceWriteLine();
            this._underlying.Emit(OpCodes.Xor);
        }

        public void EndExceptionBlock()
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(this.IsInExceptionBlock);
            this.Unindent();
            this._underlying.EndExceptionBlock();
            this._endOfExceptionBlocks.Pop();
        }

        public void FlushTrace()
        {
            if ((this._traceBuffer != null) && (this._traceBuffer.Length > 0))
            {
                this.TraceLocals();
                this._trace.Flush();
                this._realTrace.Write(this._traceBuffer);
                this._traceBuffer.Clear();
            }
        }

        private void Indent()
        {
            this._indentLevel++;
        }

        public void MarkLabel(Label label)
        {
            this.TraceStart();
            this.TraceWriteLine(this._labels[label]);
            this._underlying.MarkLabel(label);
        }

        private void TraceField(FieldInfo field)
        {
            Contract.Assert(field != null);
            WriteType(this._trace, field.FieldType);
            this._trace.Write(" ");
            FieldBuilder builder = field as FieldBuilder;
            if (builder == null)
            {
                Type[] requiredCustomModifiers = field.GetRequiredCustomModifiers();
                if (0 < requiredCustomModifiers.Length)
                {
                    this._trace.Write("modreq(");
                    foreach (Type type in requiredCustomModifiers)
                    {
                        WriteType(this._trace, type);
                    }
                    this._trace.Write(") ");
                }
                Type[] optionalCustomModifiers = field.GetOptionalCustomModifiers();
                if (0 < optionalCustomModifiers.Length)
                {
                    this._trace.Write("modopt(");
                    foreach (Type type2 in optionalCustomModifiers)
                    {
                        WriteType(this._trace, type2);
                    }
                    this._trace.Write(") ");
                }
            }
            if (this._isInDynamicMethod || (builder == null))
            {
                WriteType(this._trace, field.DeclaringType);
            }
            this._trace.Write("::");
            this._trace.Write(field.Name);
        }

        private void TraceLocals()
        {
            Contract.Assert(this._realTrace != null);
            this._realTrace.WriteLine(".locals init (");
            foreach (KeyValuePair<LocalBuilder, string> pair in this._localDeclarations)
            {
                this.WriteIndent(this._realTrace, 1);
                this._realTrace.Write("[");
                this._realTrace.Write(pair.Key.LocalIndex);
                this._realTrace.Write("] ");
                WriteType(this._realTrace, pair.Key.LocalType);
                if (pair.Key.IsPinned)
                {
                    this._realTrace.Write("(pinned)");
                }
                if (pair.Value != null)
                {
                    this._realTrace.Write(" ");
                    this._realTrace.Write(pair.Value);
                }
                this._realTrace.WriteLine();
            }
            this._realTrace.WriteLine(")");
        }

        private void TraceMethod(MethodBase method)
        {
            Contract.Assert(method != null);
            bool flag = method is MethodBuilder;
            if (!method.IsStatic)
            {
                this._trace.Write("instance ");
            }
            CallingConvention? unamangedCallingConvention = null;
            if (!flag)
            {
                DllImportAttribute customAttribute = Attribute.GetCustomAttribute(method, typeof(DllImportAttribute)) as DllImportAttribute;
                if (customAttribute != null)
                {
                    unamangedCallingConvention = new CallingConvention?(customAttribute.CallingConvention);
                }
            }
            WriteCallingConventions(this._trace, new CallingConventions?(method.CallingConvention), unamangedCallingConvention);
            MethodInfo info = method as MethodInfo;
            if (info != null)
            {
                WriteType(this._trace, info.ReturnType);
                this._trace.Write(" ");
            }
            if (method.DeclaringType == null)
            {
                this._trace.Write("[.module");
                this._trace.Write(info.Module.Name);
                this._trace.Write("]::");
            }
            else if (!(!this._isInDynamicMethod && flag))
            {
                WriteType(this._trace, method.DeclaringType);
                this._trace.Write("::");
            }
            this._trace.Write(method.Name);
            this._trace.Write("(");
            if (!flag)
            {
                ParameterInfo[] parameters = method.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (i == 0)
                    {
                        this._trace.Write(" ");
                    }
                    else
                    {
                        this._trace.Write(", ");
                    }
                    if (parameters[i].IsOut)
                    {
                        this._trace.Write("out ");
                    }
                    else if (parameters[i].ParameterType.IsByRef)
                    {
                        this._trace.Write("ref ");
                    }
                    WriteType(this._trace, parameters[i].ParameterType.IsByRef ? parameters[i].ParameterType.GetElementType() : parameters[i].ParameterType);
                    this._trace.Write(" ");
                    this._trace.Write(parameters[i].Name);
                }
                if (0 < parameters.Length)
                {
                    this._trace.Write(" ");
                }
            }
            this._trace.Write(")");
        }

        private void TraceOpCode(OpCode opCode)
        {
            this._trace.Write(opCode.Name);
        }

        private void TraceOperand(double value)
        {
            this._trace.Write(value);
        }

        private void TraceOperand(int value)
        {
            this._trace.Write(value);
        }

        private void TraceOperand(long value)
        {
            this._trace.Write(value);
        }

        private void TraceOperand(Label value)
        {
            this._trace.Write(this._labels[value]);
        }

        private void TraceOperand(string value)
        {
            this._trace.Write(string.Format(CultureInfo.InvariantCulture, "\"{0:L}\"", new object[] { value }));
        }

        private void TraceOperand(Label[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (0 < i)
                {
                    this._trace.Write(", ");
                }
                this.TraceOperand(values[i]);
            }
        }

        private void TraceOperand(FieldInfo value)
        {
            this.TraceField(value);
        }

        private void TraceOperand(MethodBase value)
        {
            this.TraceMethod(value);
        }

        private void TraceOperand(Type value)
        {
            this.TraceType(value);
        }

        private void TraceOperandToken(FieldInfo target)
        {
            this._trace.Write("field ");
            this.TraceField(target);
            this.TraceOperandTokenValue(target.MetadataToken);
        }

        private void TraceOperandToken(MethodBase target)
        {
            this._trace.Write("method ");
            this.TraceMethod(target);
            this.TraceOperandTokenValue(target.MetadataToken);
        }

        private void TraceOperandToken(Type target)
        {
            this.TraceType(target);
            this.TraceOperandTokenValue(target.MetadataToken);
        }

        private void TraceOperandTokenValue(int value)
        {
            this._trace.Write("<");
            this._trace.Write(value.ToString("x8", CultureInfo.InvariantCulture));
            this._trace.Write(">");
        }

        private void TraceSignature(CallingConventions? managedCallingConventions, CallingConvention? unmanagedCallingConvention, Type returnType, Type[] requiredParameterTypes, Type[] optionalParameterTypes)
        {
            int num;
            WriteCallingConventions(this._trace, managedCallingConventions, unmanagedCallingConvention);
            WriteType(this._trace, returnType);
            this._trace.Write(" (");
            for (num = 0; num < requiredParameterTypes.Length; num++)
            {
                if (0 < num)
                {
                    this._trace.Write(", ");
                }
                else
                {
                    this._trace.Write(" ");
                }
                WriteType(this._trace, requiredParameterTypes[num]);
            }
            if (0 < optionalParameterTypes.Length)
            {
                if (0 < requiredParameterTypes.Length)
                {
                    this.TraceWrite(",");
                }
                this.TraceWrite(" [");
                for (num = 0; num < optionalParameterTypes.Length; num++)
                {
                    if (0 < num)
                    {
                        this._trace.Write(", ");
                    }
                    else
                    {
                        this._trace.Write(" ");
                    }
                    WriteType(this._trace, optionalParameterTypes[num]);
                }
                this.TraceWrite(" ]");
            }
            if (0 < (requiredParameterTypes.Length + optionalParameterTypes.Length))
            {
                this._trace.Write(" ");
            }
            this._trace.WriteLine(")");
        }

        private void TraceStart()
        {
            this.WriteLineNumber();
            this.WriteIndent();
        }

        private void TraceType(Type type)
        {
            WriteType(this._trace, type);
        }

        public void TraceWrite(string value)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(this._trace != null);
            this._trace.Write(value);
        }

        public void TraceWrite(string format, object arg0)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(this._trace != null);
            this._trace.Write(format, arg0);
        }

        public void TraceWrite(string format, params object[] args)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(this._trace != null);
            this._trace.Write(format, args);
        }

        public void TraceWriteLine()
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(this._trace != null);
            this._trace.WriteLine();
        }

        public void TraceWriteLine(string value)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(this._trace != null);
            this._trace.WriteLine(value);
        }

        public void TraceWriteLine(string format, object arg0)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(this._trace != null);
            this._trace.WriteLine(format, arg0);
        }

        public void TraceWriteLine(string format, params object[] args)
        {
            Contract.Assert(!this.IsEnded);
            Contract.Assert(this._trace != null);
            this._trace.WriteLine(format, args);
        }

        private void Unindent()
        {
            Contract.Assert(0 < this._indentLevel);
            this._indentLevel--;
        }

        private static void WriteCallingConventions(TextWriter writer, CallingConventions? managedCallingConverntions, CallingConvention? unamangedCallingConvention)
        {
            Contract.Assert(writer != null);
            bool flag = false;
            if (managedCallingConverntions.HasValue && ((((CallingConventions) managedCallingConverntions) & CallingConventions.ExplicitThis) == CallingConventions.ExplicitThis))
            {
                writer.Write("explicit");
                flag = true;
            }
            if (!unamangedCallingConvention.HasValue)
            {
                if (managedCallingConverntions.HasValue && ((((CallingConventions) managedCallingConverntions) & CallingConventions.VarArgs) == CallingConventions.VarArgs))
                {
                    if (flag)
                    {
                        writer.Write(" ");
                    }
                    writer.Write("varargs");
                }
            }
            else
            {
                switch (unamangedCallingConvention.Value)
                {
                    case CallingConvention.Winapi:
                    case CallingConvention.StdCall:
                        if (flag)
                        {
                            writer.Write(" ");
                        }
                        writer.Write("unmanaged stdcall");
                        return;
                }
                if (flag)
                {
                    writer.Write(" ");
                }
                writer.Write("unmanaged ");
                writer.Write(unamangedCallingConvention.Value.ToString().ToLowerInvariant());
            }
        }

        private void WriteIndent()
        {
            this.WriteIndent(this._trace, this._indentLevel);
        }

        private void WriteIndent(TextWriter writer, int indentLevel)
        {
            Contract.Assert(writer != null);
            Contract.Assert(0 <= indentLevel);
            for (int i = 0; i < indentLevel; i++)
            {
                writer.Write(this._indentChars);
            }
        }

        private void WriteLineNumber()
        {
            Contract.Assert(this._trace != null);
            this._trace.Write("L_{0:d4}\t", this._lineNumber);
            this._lineNumber++;
        }

        private static void WriteType(TextWriter writer, Type type)
        {
            Contract.Assert(writer != null);
            if ((type == null) || (type == typeof(void)))
            {
                writer.Write("void");
            }
            else if (type.IsGenericParameter)
            {
                writer.Write("{0}{1}", (type.DeclaringMethod == null) ? "!" : "!!", type.Name);
            }
            else
            {
                writer.Write("[{0}]{1}", type.Assembly.GetName().Name, type.FullName);
            }
        }

        public Label? EndOfCurrentExceptionBlock
        {
            get
            {
                if (this._endOfExceptionBlocks.Count == 0)
                {
                    return null;
                }
                return new Label?(this._endOfExceptionBlocks.Peek());
            }
        }

        public Label EndOfMethod
        {
            get
            {
                return this._endOfMethod;
            }
        }

        public string IndentCharacters
        {
            get
            {
                return this._indentChars;
            }
            set
            {
                this._indentChars = value ?? "  ";
            }
        }

        public int IndentLevel
        {
            get
            {
                return this._indentLevel;
            }
        }

        public bool IsEnded
        {
            get
            {
                return this._isEnded;
            }
        }

        public bool IsInDynamicMethod
        {
            get
            {
                return this._isInDynamicMethod;
            }
        }

        public bool IsInExceptionBlock
        {
            get
            {
                return (0 < this._endOfExceptionBlocks.Count);
            }
        }

        public int LineNumber
        {
            get
            {
                return this._lineNumber;
            }
        }
    }
}

