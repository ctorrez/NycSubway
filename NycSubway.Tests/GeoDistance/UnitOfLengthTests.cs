using Microsoft.VisualStudio.TestTools.UnitTesting;
using NycSubway.Core.Services.GeoDistance;

namespace NycSubway.Tests.GeoDistance
{
    [TestClass]
    public class UnitOfLengthTests
    {
        [TestMethod]
        public void ConvertFromMiles_ForMiles_ReturnsSameValue()
        {
            var result = UnitOfLength.Miles.ConvertFromMiles(10.5);
            Assert.AreEqual(10.5, result, 0.0001);
        }

        [TestMethod]
        public void ConvertFromMiles_ForKilometers_MultipliesByCorrectFactor()
        {
            var result = UnitOfLength.Kilometers.ConvertFromMiles(1.0);
            Assert.AreEqual(1.609344, result, 0.000001);
        }

        [TestMethod]
        public void ConvertFromMiles_ForNauticalMiles_MultipliesByCorrectFactor()
        {
            var result = UnitOfLength.NauticalMiles.ConvertFromMiles(1.0);
            Assert.AreEqual(0.8684, result, 0.0001);
        }
    }
}
