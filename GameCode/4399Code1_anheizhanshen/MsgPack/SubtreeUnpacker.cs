namespace MsgPack
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    internal sealed class SubtreeUnpacker : Unpacker
    {
        private readonly Stack<bool> _isMap;
        private readonly Stack<long> _itemsCount;
        private readonly SubtreeUnpacker _parent;
        private readonly ItemsUnpacker _root;
        private readonly Stack<long> _unpacked;

        public SubtreeUnpacker(ItemsUnpacker parent) : this(parent, null)
        {
        }

        private SubtreeUnpacker(ItemsUnpacker root, SubtreeUnpacker parent)
        {
            Contract.Assert(root != null);
            Contract.Assert(root.IsArrayHeader || root.IsMapHeader);
            this._root = root;
            this._parent = parent;
            this._unpacked = new Stack<long>(2);
            this._itemsCount = new Stack<long>(2);
            this._isMap = new Stack<bool>(2);
            if (root.ItemsCount > 0L)
            {
                this._itemsCount.Push(root.ItemsCount * (root.IsMapHeader ? ((long) 2) : ((long) 1)));
                this._unpacked.Push(0L);
                this._isMap.Push(root.IsMapHeader);
            }
        }

        private void DiscardCompletedStacks()
        {
            if (this._itemsCount.Count == 0)
            {
                Contract.Assert(this._unpacked.Count == 0);
            }
            else
            {
                while (this._unpacked.Peek() == this._itemsCount.Peek())
                {
                    this._itemsCount.Pop();
                    this._unpacked.Pop();
                    this._isMap.Pop();
                    if (this._itemsCount.Count == 0)
                    {
                        Contract.Assert(this._unpacked.Count == 0);
                        break;
                    }
                    this._unpacked.Push(this._unpacked.Pop() + 1L);
                }
            }
        }

        protected sealed override void Dispose(bool disposing)
        {
            if (disposing)
            {
                while (this.ReadCore())
                {
                }
                if (this._parent != null)
                {
                    this._parent.EndReadSubtree();
                }
                else
                {
                    this._root.EndReadSubtree();
                }
            }
            base.Dispose(disposing);
        }

        protected internal sealed override void EndReadSubtree()
        {
            base.EndReadSubtree();
            this._unpacked.Pop();
            this._unpacked.Push(this._itemsCount.Peek());
            this.DiscardCompletedStacks();
        }

        public override bool ReadArrayLength(out long result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0L;
                return false;
            }
            if (!this._root.ReadSubtreeArrayLength(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadBinary(out byte[] result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = null;
                return false;
            }
            if (!this._root.ReadSubtreeBinary(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadBoolean(out bool result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = false;
                return false;
            }
            if (!this._root.ReadSubtreeBoolean(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadByte(out byte result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeByte(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        protected sealed override bool ReadCore()
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                return false;
            }
            if (!this._root.ReadSubtreeItem())
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadDouble(out double result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0.0;
                return false;
            }
            if (!this._root.ReadSubtreeDouble(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadInt16(out short result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeInt16(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadInt32(out int result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeInt32(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadInt64(out long result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0L;
                return false;
            }
            if (!this._root.ReadSubtreeInt64(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadMapLength(out long result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0L;
                return false;
            }
            if (!this._root.ReadSubtreeMapLength(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadNullableBoolean(out bool? result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeNullableBoolean(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadNullableByte(out byte? result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeNullableByte(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadNullableDouble(out double? result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeNullableDouble(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadNullableInt16(out short? result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeNullableInt16(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadNullableInt32(out int? result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeNullableInt32(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadNullableInt64(out long? result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeNullableInt64(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadNullableSByte(out sbyte? result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeNullableSByte(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadNullableSingle(out float? result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeNullableSingle(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadNullableUInt16(out ushort? result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeNullableUInt16(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadNullableUInt32(out uint? result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeNullableUInt32(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadNullableUInt64(out ulong? result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeNullableUInt64(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadObject(out MessagePackObject result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = new MessagePackObject();
                return false;
            }
            if (!this._root.ReadSubtreeObject(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadSByte(out sbyte result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeSByte(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadSingle(out float result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0f;
                return false;
            }
            if (!this._root.ReadSubtreeSingle(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadString(out string result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = null;
                return false;
            }
            if (!this._root.ReadSubtreeString(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        protected sealed override Unpacker ReadSubtreeCore()
        {
            if (this._unpacked.Count == 0)
            {
                throw new InvalidOperationException("This unpacker is located in the tail.");
            }
            if (!(this._root.IsArrayHeader || this._root.IsMapHeader))
            {
                throw new InvalidOperationException("This unpacker is not located in the head of collection.");
            }
            return new SubtreeUnpacker(this._root, this);
        }

        public override bool ReadUInt16(out ushort result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeUInt16(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadUInt32(out uint result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0;
                return false;
            }
            if (!this._root.ReadSubtreeUInt32(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        public override bool ReadUInt64(out ulong result)
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                result = 0L;
                return false;
            }
            if (!this._root.ReadSubtreeUInt64(out result))
            {
                return false;
            }
            if (this._root.IsArrayHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount);
                this._unpacked.Push(0L);
                this._isMap.Push(false);
            }
            else if (this._root.IsMapHeader)
            {
                this._itemsCount.Push(this._root.ItemsCount * 2L);
                this._unpacked.Push(0L);
                this._isMap.Push(true);
            }
            else
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return true;
        }

        protected sealed override long? SkipCore()
        {
            this.DiscardCompletedStacks();
            if (this._itemsCount.Count == 0)
            {
                return 0L;
            }
            long? nullable = this._root.SkipSubtreeItem();
            if (nullable.HasValue)
            {
                this._unpacked.Push(this._unpacked.Pop() + 1L);
            }
            return nullable;
        }

        public sealed override MessagePackObject? Data
        {
            get
            {
                return this._root.Data;
            }
            protected set
            {
                this._root.InternalSetData(value);
            }
        }

        public sealed override bool IsArrayHeader
        {
            get
            {
                return this._root.IsArrayHeader;
            }
        }

        public sealed override bool IsMapHeader
        {
            get
            {
                return this._root.IsMapHeader;
            }
        }

        public sealed override long ItemsCount
        {
            get
            {
                return ((this._itemsCount.Count == 0) ? 0L : (this._itemsCount.Peek() / (this._isMap.Peek() ? ((long) 2) : ((long) 1))));
            }
        }

        internal override long? UnderlyingStreamPosition
        {
            get
            {
                return this._root.UnderlyingStreamPosition;
            }
        }
    }
}

