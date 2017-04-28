namespace System
{
    using System.Text;

    [Serializable]
    internal class Tuple<T1, T2>
    {
        private readonly T1 _item1;
        private readonly T2 _item2;

        public Tuple(T1 item1, T2 item2)
        {
            this._item1 = item1;
            this._item2 = item2;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            builder.Append(this._item1);
            builder.Append(", ");
            builder.Append(this._item2);
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
    }
}

