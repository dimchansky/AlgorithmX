using System;
using System.Collections.Generic;
using System.Linq;
using OrderedColumnsRow = System.Collections.Generic.IEnumerable<int>;

namespace AlgorithmDLX
{
    using ExactCover = IEnumerable<OrderedColumnsRow>;

    public sealed class AlgorithmXMatrix
    {
        private readonly ColumnObject _head = new ColumnObject(-1);
        private readonly ColumnObject[] _indexedColumns;

        public AlgorithmXMatrix(int columns)
        {
            _indexedColumns = new ColumnObject[columns];

            for (var i = 0; i < _indexedColumns.Length; i++)
            {
                var column = new ColumnObject(i);
                _head.InsertLeft(column);
                _indexedColumns[i] = column;                
            }
        }

        public void AddRows(IEnumerable<OrderedColumnsRow> rows)
        {
            foreach (var orderedColumnsRow in rows)
            {
                var lastElement = -1;
                DataObject firstColumn = null;
                foreach (var columnIndex in orderedColumnsRow)
                {
                    if (columnIndex <= lastElement)
                    {
                        throw new ArgumentException("Column indexes aren't growing strictly.");
                    }
                    lastElement = columnIndex;

                    var column = _indexedColumns[columnIndex];

                    var dataObject = new DataObject(column);
                    column.InsertUp(dataObject);
                    column.Size += 1;

                    if (firstColumn == null)
                    {
                        firstColumn = dataObject;
                    }
                    else
                    {
                        firstColumn.InsertLeft(dataObject);
                    }
                }
            }
        }

        public IEnumerable<ExactCover> GetAllExactCovers()
        {
            return Search(new Stack<DataObject>());
        }

        private IEnumerable<ExactCover> Search(Stack<DataObject> partialSolution)
        {
            var headRight = _head.Right;
            if (headRight == _head)
            {
                yield return partialSolution.Select(o => o.GetOrderedColumnsRow());
            }

            var c = headRight.Column;
            var s = c.Size;

            for (var j = headRight.Right; j != _head; j = j.Right)
            {
                var jc = j.Column;

                var jsize = jc.Size;
                if (jsize >= s) continue;
                
                c = jc;
                s = jsize;
            }

            CoverColumn(c);

            for (var r = c.Down; r != c; r = r.Down)
            {
                partialSolution.Push(r);

                for (var j = r.Right; j != r; j = j.Right)
                {
                    CoverColumn(j.Column);
                }

                foreach (var solution in Search(partialSolution))
                {
                    yield return solution;
                }

                partialSolution.Pop();

                for (var j = r.Left; j != r; j = j.Left)
                {
                    UnCoverColumn(j.Column);
                }
            }

            UnCoverColumn(c);
        }

        private static void CoverColumn(DataObject c)
        {
            c.Right.Left = c.Left;
            c.Left.Right = c.Right;

            for (var i = c.Down; i != c; i = i.Down)
            {
                for (var j = i.Right; j != i; j = j.Right)
                {
                    j.Down.Up = j.Up;
                    j.Up.Down = j.Down;
                    j.Column.Size -= 1;
                }
            }
        }

        private static void UnCoverColumn(DataObject c)
        {
            for (var i = c.Up; i != c; i = i.Up)
            {
                for (var j = i.Left; j != i; j = j.Left)
                {
                    j.Column.Size += 1;
                    j.Down.Up = j;
                    j.Up.Down = j;
                }
            }

            c.Right.Left = c;
            c.Left.Right = c;
        }
    }
}