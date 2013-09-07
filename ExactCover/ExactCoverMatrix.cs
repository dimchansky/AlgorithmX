using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrderedColumnsRow = System.Collections.Generic.IEnumerable<int>;

namespace ExactCover
{
    using ExactCoverSolution = IEnumerable<OrderedColumnsRow>;

    public sealed class ExactCoverMatrix
    {
        private readonly ColumnObject _head = new ColumnObject(-1);
        private readonly ColumnObject[] _indexedColumns;

        public ExactCoverMatrix(int columns)
        {           
            _indexedColumns = Enumerable
                .Range(0, columns)
                .Select(i => new ColumnObject(i))
                .ToArray();

            _indexedColumns.Aggregate(_head, (lastColumn, currentColumn) => lastColumn.InsertRight(currentColumn).Column);
        }

        public void AddRows(IEnumerable<OrderedColumnsRow> rows)
        {
            foreach (var orderedColumnsRow in rows)
            {
                new CheckedOrderedColumns(orderedColumnsRow)
                    .Select(columnIndex =>
                    {
                        var column = _indexedColumns[columnIndex];
                        column.Size += 1;
                        return column.InsertUp(new DataObject{Column = column});
                    })
                    .Aggregate((DataObject)null, (lastObject, currentObject) => lastObject != null ? lastObject.InsertRight(currentObject) : currentObject);
            }
        }

        public IEnumerable<ExactCoverSolution> Solve()
        {
            return Search(0, new Stack<DataObject>());
        }

        private IEnumerable<ExactCoverSolution> Search(int k, Stack<DataObject> partialSolution)
        {
            if (_head.Right == _head)
            {
                yield return partialSolution.Select(o => o.GetList(d => d.Right).Select(d => d.Column.Index).OrderBy(i => i)).ToArray();
            }

            var c = _head.Right.Column;
            var s = c.Size;
            foreach (var j in _head.GetList(o => o.Right).Skip(2).Select(o => o.Column))
            {
                if (j.Size >= s) continue;
                c = j;
                s = j.Size;
            }

            CoverColumn(c);

            foreach (var r in c.GetList(o => o.Down).Skip(1))
            {
                partialSolution.Push(r);

                foreach (var j in r.GetList(o => o.Right).Skip(1))
                {
                    CoverColumn(j.Column);
                }

                foreach (var solution in Search(k+1, partialSolution))
                {
                    yield return solution;
                }

                partialSolution.Pop();

                foreach (var j in r.GetList(o => o.Left).Skip(1))
                {
                    UnCoverColumn(j.Column);
                }
            }

            UnCoverColumn(c);
        }

        private static void CoverColumn(ColumnObject c)
        {
            c.Right.Left = c.Left;
            c.Left.Right = c.Right;

            foreach (var i in c.GetList(o => o.Down).Skip(1))
            {
                foreach (var j in i.GetList(o => o.Right).Skip(1))
                {
                    j.Down.Up = j.Up;
                    j.Up.Down = j.Down;
                    j.Column.Size -= 1;
                }
            }
        }

        private static void UnCoverColumn(ColumnObject c)
        {
            foreach (var i in c.GetList(o => o.Up).Skip(1))
            {
                foreach (var j in i.GetList(o => o.Left).Skip(1))
                {
                    j.Column.Size += 1;
                    j.Down.Up = j;
                    j.Up.Down = j;
                }
            }

            c.Right.Left = c;
            c.Left.Right = c;
        }

        #region Helpers

        private struct CheckedOrderedColumns : OrderedColumnsRow
        {
            private readonly OrderedColumnsRow _orderedColumns;

            public CheckedOrderedColumns(OrderedColumnsRow orderedColumns)
            {
                _orderedColumns = orderedColumns;
            }

            private OrderedColumnsRow CheckedEnumerable()
            {
                using (var e = _orderedColumns.GetEnumerator())
                {
                    if (!e.MoveNext()) yield break;

                    var lastElement = e.Current;
                    yield return lastElement;

                    while (e.MoveNext())
                    {
                        var current = e.Current;
                        if (current <= lastElement)
                        {
                            throw new ArgumentException("Column indexes aren't growing strictly.");
                        }

                        yield return current;
                        lastElement = current;
                    }
                }
            }

            public IEnumerator<int> GetEnumerator()
            {
                return CheckedEnumerable().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        #endregion
    }
}