namespace System
{
    using System.Text;

    [Serializable]
    internal class Tuple<T1, T2, T3, T4>
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;

        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this._item1 = item1;
            this._item2 = item2;
            this._item3 = item3;
            this._item4 = item4;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            builder.Append(this._item1);
            builder.Append(", ");
            builder.Append(this._item2);
            builder.Append(", ");
            builder.Append(this._item3);
            builder.Append(", ");
            builder.Append(this._item4);
            return builder.ToString();
        }

        public T1 Item1
        {
            get
            {
                return this._item1;
            }
        }

        public T2 Item2
        {
            get
            {
                return this._item2;
            }
        }

        public T3 Item3
        {
            get
            {
                return this._item3;
            }
        }

        public T4 Item4
        {
            get
            {
                return this._item4;
            }
        }
    }
}

