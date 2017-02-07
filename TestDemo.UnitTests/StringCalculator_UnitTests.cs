using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//referenced from https://www.youtube.com/watch?v=Oy-Ny1Op6PY
namespace TestDemo.UnitTests
{
    public class StringCalculator_UnitTests 
    {
        private Mock<IStore> _mockStore;

        private StringCalculator GetCalculator()
        {
            _mockStore = new Mock<IStore>();
            var calc = new StringCalculator(_mockStore.Object);
            return calc;
        }
        [Test]
        public void Add_EmptyString_Returns0()
        {
            //Arrange
            StringCalculator calc = GetCalculator();// <-- new refactored code : old code -->new StringCalculator();
            int expectedResult = 0;

            //Act
            int result = calc.Add("");

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("1", 1)]
        [TestCase("2", 2)]
        [TestCase("3", 3)]
        [TestCase("6", 6)]
        [TestCase("9", 9)]
        public void Add_SingleNumbers_ReturnsTheNumber(string input, int expectedResult)
        {
            StringCalculator calc = GetCalculator();// <-- new refactored code : old code -->new StringCalculator();
            //var expectedResult = 3; Removed this variable after inputting "string input, int expectedResult parameters
            int result = calc.Add(input);
            //int result = calc.Add("3");
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("2,3", 5)]
        [TestCase("101,20", 121)]
        [TestCase("3, 8, 10", 21)]
        [TestCase("1,2,3,4,5,6,7", 28)]
        public void Add_MultipleNumbers_ReturnSumOfAllNumbers(string input, int expectedResult)
        {

            //public void Add_MultipleNumbers_SomeOfAllNumbers()
            //{
            //    //arrange
            //    StringCalculator calc = new StringCalculator();
            //    //act
            //    int result = calc.Add("1,2,5");
            //    int expectedResult = 8;
            //    //assert
            //    Assert.AreEqual(expectedResult, result);
            //}
            //REFACTORED CODE BELOW
            //Arrange
            StringCalculator calc = GetCalculator();// <-- new refactored code : old code -->new StringCalculator();
            //Act
            int result = calc.Add(input);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("-1, 5", 4)]
        [TestCase("-1", -1)]
        [TestCase("-1, -5, -12", -18)]
        public void MinusNumbers_Scenario_AreSummedCorrectly(string input, int expectedResult)
        {
            StringCalculator calc = GetCalculator();
            var result = calc.Add(input);
            Assert.AreEqual(expectedResult, result);
        }

        //[Test]
        //public void Add_ResultIsAPrimeNumber_ResultsAreSaved()
        //{
        //    //Mock<IStore> mockstore = new Mock<IStore>(); lower line is refactoredNo longer used sinc
        //    //StringCalculator calc = new StringCalculator(mockstore.Object); this line refactored to line below
        //    StringCalculator calc = GetCalculator();
        //    var result = calc.Add("3,4");
        //    _mockStore.Verify(m => m.Save(It.IsAny<int>()), Times.Once); //mockstore refactored to _mockStore
        //}     
        // COD ABOVE REFACTORED TO CODE BELOW:
        [TestCase("2")]
        [TestCase("5,6")]
        [TestCase("3,4")]
        [TestCase("10,10,3")]
        [TestCase("5,5,5,5,5,5,5,5,5,5,3")] 
        public void Add_ResultIsAPrimeNumber_ResultsAreSaved(string input)
        {
            StringCalculator calc = GetCalculator();
            var result = calc.Add(input);
            _mockStore.Verify(m => m.Save(It.IsAny<int>()), Times.Once);
        }

        [TestCase("4")]
        [TestCase("5,5")]
        [TestCase("5,4")]
        [TestCase("10,10,5")]
        [TestCase("5,5,5,5,5,5,5,5,5,5,1")]
        public void Add_ResultIs_NOT_APrimeNumber_InputsAndResultAre_NOT_Saved(string input)
        {
            StringCalculator calc = GetCalculator();
            var result = calc.Add(input);
            _mockStore.Verify(m => m.Save(It.IsAny<int>()), Times.Never);
        }



    }

    public interface IStore
    {
        void Save(int result);
    }

    public class StringCalculator 
    {
        private readonly IStore _store;

        public StringCalculator(IStore store)
        {
            _store = store;
        }
        //    public int Add(string input)
        //    {
        //        if (string.IsNullOrEmpty(input))
        //            return 0;

        //        //return 0; return int.Parse(input) broke "return 0;" code to the left. To correct, see code if statement
        //        return int.Parse(input);
        //    }
        //
        //REFACTORED CODE BELOW:
        public int Add(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }

            var numbers = input.Split(',');
            var total = 0;
            foreach (var number in numbers)
            {
                total += int.Parse(number);
            }

            if (_store != null)
            {
                if (IsPrime(total))
                {
                    _store.Save(total);
                }
            }
            return total;
        }

        private bool IsPrime(int number)
        {
            if (number == 2)
                return true;
            if (number % 2 == 0)
                return false;
            for (int i = 3; i <= (int)(Math.Sqrt(number)); i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}


