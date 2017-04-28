namespace MsgPack.Serialization.EmittingSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using MsgPack.Serialization.Reflection;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    internal abstract class SerializerEmitter : IDisposable
    {
        private readonly TextWriter _trace = (IsTraceEnabled ? new StringWriter(CultureInfo.InvariantCulture) : TextWriter.Null);
        protected static readonly Type[] UnpackFromCoreParameterTypes = new Type[] { typeof(Unpacker) };

        protected SerializerEmitter()
        {
        }

        public abstract MessagePackSerializer<T> CreateInstance<T>(SerializationContext context);
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._trace.Dispose();
            }
        }

        public void FlushTrace()
        {
            StringWriter writer = this._trace as StringWriter;
            if (writer != null)
            {
                StringBuilder stringBuilder = writer.GetStringBuilder();
                TraceSource emit = Tracer.Emit;
                if ((emit != null) && (0 < stringBuilder.Length))
                {
                    emit.TraceData(TraceEventType.Verbose, 0x65, stringBuilder);
                }
                stringBuilder.Clear();
            }
        }

        public abstract TracingILGenerator GetPackToMethodILGenerator();
        public abstract TracingILGenerator GetUnpackFromMethodILGenerator();
        public abstract TracingILGenerator GetUnpackToMethodILGenerator();
        public abstract Action<TracingILGenerator, int> RegisterSerializer(Type targetType);

        protected static bool IsTraceEnabled
        {
            get
            {
                return ((Tracer.Emit.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose);
            }
        }

        protected TextWriter Trace
        {
            get
            {
                return this._trace;
            }
        }
    }
}

