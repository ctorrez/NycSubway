using Microsoft.VisualStudio.TestTools.UnitTesting;
using NycSubway.Core.Models;
using NycSubway.Core.Services.Station;
using System;

namespace NycSubway.Tests.GeoDistance
{
    [TestClass]
    public class GeoBoundingBoxTests
    {
        [TestMethod]
        public void FromCenterAndRadius_CalculatesExpectedBoundsAndCorners()
        {
            // Arrange
            var center = new GeoPoint(40.7536, -73.9832);
            double radiusMiles = 0.5;

            // Act
            var box = GeoBoundingBox.FromCenterAndRadius(center, radiusMiles);

            // Assert
            Assert.IsTrue(box.MinLatitude < center.Latitude);
            Assert.IsTrue(box.MaxLatitude > center.Latitude);
            Assert.IsTrue(box.MinLongitude < center.Longitude);
            Assert.IsTrue(box.MaxLongitude > center.Longitude);

            // Verify SouthWest corner (MinLat, MinLon)
            Assert.AreEqual(box.MinLatitude, box.SouthWest.Latitude);
            Assert.AreEqual(box.MinLongitude, box.SouthWest.Longitude);

            // Verify NorthEast corner (MaxLat, MaxLon)
            Assert.AreEqual(box.MaxLatitude, box.NorthEast.Latitude);
            Assert.AreEqual(box.MaxLongitude, box.NorthEast.Longitude);
        }

        [TestMethod]
        public void FromCenterAndRadius_WithNullCenter_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                GeoBoundingBox.FromCenterAndRadius(null!, 1.0));
        }

        [TestMethod]
        [DataRow(0.0)]
        [DataRow(-1.5)]
        public void FromCenterAndRadius_WithNonPositiveRadius_ThrowsArgumentOutOfRangeException(double invalidRadius)
        {
            var center = new GeoPoint(40.7536, -73.9832);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                GeoBoundingBox.FromCenterAndRadius(center, invalidRadius));
        }

        [TestMethod]
        public void Contains_EvaluatesPointPositionCorrectly()
        {
            // Arrange
            var center = new GeoPoint(40.7536, -73.9832);
            var box = GeoBoundingBox.FromCenterAndRadius(center, radiusMiles: 1.0);

            var insidePoint = new GeoPoint(40.7538, -73.9830);
            var outsidePoint = new GeoPoint(41.0000, -74.5000);

            // Act & Assert
            Assert.IsTrue(box.Contains(insidePoint), "Inside point should return true.");
            Assert.IsFalse(box.Contains(outsidePoint), "Outside point should return false.");
            Assert.IsFalse(box.Contains(null!), "Null point should return false.");
        }
    }
}
