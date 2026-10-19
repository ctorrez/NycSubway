using Microsoft.VisualStudio.TestTools.UnitTesting;
using NycSubway.Core.Services.GeoDistance;
using NycSubway.Core.Services.Station;
using System;

namespace NycSubway.Tests.GeoDistance
{
    [TestClass]
    public class GeoCalculatorTests
    {
        private IGeoCalculator _calculator = null!;

        [TestInitialize]
        public void Setup()
        {
            _calculator = new HaversineGeoCalculator();
        }

        [TestMethod]
        public void CalculateDistance_WhenPointsAreIdentical_ReturnsZeroWithoutNaN()
        {
            // Arrange
            var point = new GeoPoint(40.7527, -73.9772);

            // Act
            var distance = _calculator.CalculateDistance(point, point, UnitOfLength.Miles);

            // Assert
            Assert.AreEqual(0.0, distance, "Distance between identical points must be zero.");
            Assert.IsFalse(double.IsNaN(distance), "Distance must not be NaN.");
        }

        [TestMethod]
        public void CalculateDistance_BetweenGrandCentralAndTimesSquare_ReturnsExpectedDistance()
        {
            // Arrange - Real NYC Subway stations
            // Grand Central - 42 St: ~40.7527 N, -73.9772 W
            // Times Sq - 42 St: ~40.7557 N, -73.9870 W
            var grandCentral = new GeoPoint(40.7527, -73.9772);
            var timesSquare = new GeoPoint(40.7557, -73.9870);

            // Act
            var distanceInMiles = _calculator.CalculateDistance(grandCentral, timesSquare, UnitOfLength.Miles);

            // Assert - Expected distance across 42nd St is approx 0.55 miles (~880m)
            Assert.AreEqual(0.55, distanceInMiles, 0.05, "Distance between Grand Central and Times Sq should be approximately 0.55 miles.");
        }

        [TestMethod]
        public void CalculateDistance_WhenUnitIsKilometers_ConvertsCorrectly()
        {
            // Arrange
            var point1 = new GeoPoint(40.7527, -73.9772);
            var point2 = new GeoPoint(40.7557, -73.9870);

            // Act
            var miles = _calculator.CalculateDistance(point1, point2, UnitOfLength.Miles);
            var kilometers = _calculator.CalculateDistance(point1, point2, UnitOfLength.Kilometers);

            // Assert
            var expectedKm = miles * 1.609344;
            Assert.AreEqual(expectedKm, kilometers, 0.0001, "Kilometers should equal miles * 1.609344.");
        }

        [TestMethod]
        public void CalculateDistance_WhenOriginIsNull_ReturnsZero()
        {
            // Arrange
            var target = new GeoPoint(40.7557, -73.9870);

            // Act
            var distance = _calculator.CalculateDistance(null, target, UnitOfLength.Miles);

            // Assert
            Assert.AreEqual(0.0, distance);
        }

        [TestMethod]
        public void CalculateDistance_WhenDestinationIsNull_ReturnsZero()
        {
            // Arrange
            var origin = new GeoPoint(40.7527, -73.9772);

            // Act
            var distance = _calculator.CalculateDistance(origin, null, UnitOfLength.Miles);

            // Assert
            Assert.AreEqual(0.0, distance);
        }
    }
}
