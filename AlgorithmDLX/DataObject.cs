using System.Collections.Generic;

namespace AlgorithmDLX
{
    internal class DataObject
    {
        public DataObject Left { get; set; }
        public DataObject Right { get; set; }
        public DataObject Up { get; set; }
        public DataObject Down { get; set; }

        public ColumnObject Column { get; protected set; }

        public DataObject(ColumnObject column)
        {
            Left = this;
            Right = this;
            Up = this;
            Down = this;

            Column = column;
        }

        public void InsertLeft(DataObject o)
        {
            var left = Left;
            Left = o;
            o.Right = this;
            o.Left = left;
            left.Right = o;
        }


        public void InsertUp(DataObject o)
        {
            var up = Up;
            Up = o;
            o.Down = this;
            o.Up = up;
            up.Down = o;
        }

        public IEnumerable<int> GetOrderedColumnsRow()
        {
            var rowStart = this;
            var rowStartIdx = rowStart.Column.Index;

            for (var i = rowStart.Left; i != this && i.Column.Index < rowStartIdx; i = i.Left)
            {
                rowStart = i;
                rowStartIdx = i.Column.Index;
            }

            yield return rowStart.Column.Index;

            for (var i = rowStart.Right; i != rowStart; i = i.Right)
            {
                yield return i.Column.Index;
            }
        }
    }
}
