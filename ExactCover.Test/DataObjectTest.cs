using NUnit.Framework;

namespace ExactCover.Test
{
    [TestFixture]
    public class DataObjectTest
    {
        [Test]
        public void InsertRight_Correctly_Appends_One_Object_To_The_Right_Of_One()
        {
            // arrange
            var d1 = new DataObject();
            var d2 = new DataObject();

            // act
            var result = d1.InsertRight(d2);

            // assert
            Assert.AreSame(d2, result);
            Assert.AreSame(d1.Right, d2);
            Assert.AreSame(d2.Right, d1);
            Assert.AreSame(d1.Left, d2);
            Assert.AreSame(d2.Left, d1);
        }

        [Test]
        public void InsertRight_Correctly_Appends_One_Object_To_The_Right_Of_Two()
        {
            // arrange        
            var d1 = new DataObject();
            var d2 = new DataObject();
            d1.InsertRight(d2);

            var d3 = new DataObject();

            // act
            var result = d2.InsertRight(d3);

            // assert
            Assert.AreSame(d3, result);
            Assert.AreSame(d1.Right, d2);
            Assert.AreSame(d2.Right, d3);
            Assert.AreSame(d3.Right, d1);
            Assert.AreSame(d1.Left, d3);
            Assert.AreSame(d3.Left, d2);
            Assert.AreSame(d2.Left, d1);
        }

        [Test]
        public void InsertRight_Correctly_Insserts_One_Object_Between_Two()
        {
            // arrange        
            var d1 = new DataObject();
            var d3 = new DataObject();
            d1.InsertRight(d3);

            var d2 = new DataObject();

            // act
            var result = d1.InsertRight(d2);

            // assert
            Assert.AreSame(d2, result);
            Assert.AreSame(d1.Right, d2);
            Assert.AreSame(d2.Right, d3);
            Assert.AreSame(d3.Right, d1);
            Assert.AreSame(d1.Left, d3);
            Assert.AreSame(d3.Left, d2);
            Assert.AreSame(d2.Left, d1);
        }

        [Test]
        public void InsertUp_Correctly_Appends_One_Object_Above_The_One()
        {
            // arrange        
            var d1 = new DataObject();
            var d2 = new DataObject();

            // act
            var result = d1.InsertUp(d2);

            // assert
            Assert.AreSame(d2, result);
            Assert.AreSame(d1.Up, d2);
            Assert.AreSame(d2.Up, d1);
            Assert.AreSame(d1.Down, d2);
            Assert.AreSame(d2.Down, d1);
        }

        [Test]
        public void InsertUp_Correctly_Appends_One_Object_Above_Two()
        {
            // arrange        
            var d1 = new DataObject();
            var d2 = new DataObject();
            d1.InsertUp(d2);

            var d3 = new DataObject();

            // act
            var result = d2.InsertUp(d3);

            // assert
            Assert.AreSame(d3, result);
            Assert.AreSame(d1.Up, d2);
            Assert.AreSame(d2.Up, d3);
            Assert.AreSame(d3.Up, d1);
            Assert.AreSame(d1.Down, d3);
            Assert.AreSame(d3.Down, d2);
            Assert.AreSame(d2.Down, d1);
        }
    }
}
