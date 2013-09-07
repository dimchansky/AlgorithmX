namespace ExactCover
{
    public sealed class ColumnObject : DataObject
    {
        private readonly int _index;

        public int Index { get { return _index; } }
        public int Size { get; set; }

        public ColumnObject(int index)
        {
            _index = index;
            Column = this;
        }
    }
}