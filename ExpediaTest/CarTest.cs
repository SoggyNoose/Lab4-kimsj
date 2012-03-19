using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;

namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}

        [Test()]
        public void TestThatCarGetsLocationFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();

            String firstLocation = "Terre Haute";
            String otherLocation = "Spaaaaace!";

            using(mocks.Record())
            {
                mockDatabase.getCarLocation(15);
                LastCall.Return(firstLocation);

                mockDatabase.getCarLocation(311);
                LastCall.Return(otherLocation);
            }

            var target = new Car(10);
            target.Database = mockDatabase;

            String result;

            result = target.getCarLocation(311);
            Assert.AreEqual(result, otherLocation);

            result = target.getCarLocation(15);
            Assert.AreEqual(result, firstLocation);
        }

        [Test()]
        public void TestCarGetsMileageFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();

            Int32 Mileage = 112995832;

            mockDatabase.Miles = Mileage;

            var target = new Car(10);
            target.Database = mockDatabase;

            Int32 result;
            result = target.Mileage;
            Assert.AreEqual(result, Mileage);
        }

        [Test()]
        public void TestThatIKnowHowToUseObjectMother()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();

            int Mileage = 31415926;

            mockDatabase.Miles = Mileage;

            var target = ObjectMother.BMW();
            target.Database = mockDatabase;

            int result = target.Mileage;
            Assert.AreEqual(result, Mileage);
        }
	}
}
