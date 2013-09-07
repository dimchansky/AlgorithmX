using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ExactCover.Test
{
    [TestFixture]
    public class ExactCoverMatrixTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentException_Is_Thrown_If_Columns_Are_Not_Ordered()
        {
            // arrange
            var m = new ExactCoverMatrix(4);

            // act
            m.AddRows(new[]
            {
                new []{3,2},
            });

            // assert
            Assert.Fail();
        }

        [Test]
        public void Solve_Returns_Correct_Solution_For_Test_Matrix_1()
        {
            // arrange
            var m = new ExactCoverMatrix(7);
            m.AddRows(new[]
            {
                new[] {2, 4, 5},
                new[] {0, 3, 6},
                new[] {1, 2, 5},
                new[] {0, 3},
                new[] {1, 6},
                new[] {3, 4, 6}
            });

            // act
            var solutions = m.Solve();

            // assert

            AssertSoolutionsAreEqual(
                new[]
                {
                    new[]
                    {
                        new[] {2, 4, 5},
                        new[] {0, 3},
                        new[] {1, 6}
                    }
                },
                solutions);
        }

        [Test]
        public void Solve_Returns_Correct_Solution_For_Test_Matrix_1_With_Additional_Empty_Rows()
        {
            // arrange
            var m = new ExactCoverMatrix(7);
            m.AddRows(new[]
            {
                new int[0],
                new[] {2, 4, 5},
                new[] {0, 3, 6},
                new[] {1, 2, 5},
                new[] {0, 3},
                new[] {1, 6},
                new[] {3, 4, 6}
            });

            // act
            var solutions = m.Solve();

            // assert

            AssertSoolutionsAreEqual(
                new[]
                {
                    new[]
                    {
                        new[] {2, 4, 5},
                        new[] {0, 3},
                        new[] {1, 6}
                    }
                },
                solutions);
        }

        [Test]
        public void Solve_Returns_Correct_Solution_For_Test_Matrix_2()
        {
            // arrange
            var m = new ExactCoverMatrix(4);
            m.AddRows(new[]
            {
                new[] {0, 1, 2},
                new[] {0, 2},
                new[] {1},
                new[] {3}
            });

            // act
            var solutions = m.Solve();

            // assert
            AssertSoolutionsAreEqual(
                new[]
                {
                    new[]
                    {
                        new[] {1},
                        new[] {0, 2},
                        new[] {3}
                    },
                    new[]
                    {
                        new[] {0, 1, 2},
                        new[] {3}
                    }
                },
                solutions);
        }

        [Test]
        public void Solve_Returns_No_Solutions_For_Test_Matrix_3()
        {
            // arrange
            var m = new ExactCoverMatrix(4);
            m.AddRows(new[]
            {
                new[] {0, 1},
                new[] {0, 2},
                new[] {1, 2},
            });

            // act
            var solutions = m.Solve();

            // assert
            Assert.AreEqual(0, solutions.Count());
        }

        #region Helpers

        private static void AssertSoolutionsAreEqual(IEnumerable<IEnumerable<IEnumerable<int>>> expected,
            IEnumerable<IEnumerable<IEnumerable<int>>> result)
        {
            CollectionAssert.AreEquivalent(StringifyRows(expected), StringifyRows(result));
        }

        private static IEnumerable<IEnumerable<string>> StringifyRows(
            IEnumerable<IEnumerable<IEnumerable<int>>> solutions)
        {
            return solutions.Select(solution => solution.Select(row => string.Join(",", row)).OrderBy(s => s));
        }

        #endregion
    }
}