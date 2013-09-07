using System;
using System.Collections.Generic;

namespace ExactCover
{
    public class DataObject
    {
        public DataObject Left { get; set; }
        public DataObject Right { get; set; }
        public DataObject Up { get; set; }
        public DataObject Down { get; set; }

        public ColumnObject Column { get; set; }

        public DataObject()
        {
            Left = this;
            Right = this;
            Up = this;
            Down = this;
        }

        public DataObject InsertRight(DataObject o)
        {
            var right = Right;
            Right = o; 
            o.Left = this;
            o.Right = right; 
            right.Left = o;

            return o;
        }

        public DataObject InsertUp(DataObject o)
        {
            var up = Up;
            Up = o;
            o.Down = this;
            o.Up = up;
            up.Down = o;

            return o;
        }

        public IEnumerable<DataObject> GetList(Func<DataObject, DataObject> next)
        {
            var tmp = this;
            do
            {
                yield return tmp;

                tmp = next(tmp);
            }
            while (tmp != this);
        }
    }
}
