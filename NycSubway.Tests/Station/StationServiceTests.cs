using Microsoft.VisualStudio.TestTools.UnitTesting;
using NycSubway.Core.Entities;
using NycSubway.Core.Services.GeoDistance;
using NycSubway.Core.Services.Station;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NycSubway.Tests.Station
{
    [TestClass]
    public class StationServiceTests
    {
        private class FakeStationRepo : IStationRepo
        {
            public List<StationEntranceModel> EntrancesToReturn { get; set; } = new List<StationEntranceModel>();
            public List<StationEntrance> BoundingBoxEntrancesToReturn { get; set; } = new List<StationEntrance>();

            public Task<List<StationEntranceModel>> GetStationEntrancesAsync()
            {
                return Task.FromResult(EntrancesToReturn);
            }

            public Task<List<StationEntrance>> GetStationEntrancesInBoundingBoxAsync(
                double minLat, double maxLat, double minLon, double maxLon)
            {
                return Task.FromResult(BoundingBoxEntrancesToReturn);
            }
        }

        private FakeStationRepo _repo = null!;
        private IGeoCalculator _geoCalculator = null!;
        private StationService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _repo = new FakeStationRepo();
            _geoCalculator = new HaversineGeoCalculator();
            _service = new StationService(_repo, _geoCalculator);
        }

        [TestMethod]
        public async Task GetStationEntrancesAsync_ReturnsEntrancesFromRepository()
        {
            // Arrange
            _repo.EntrancesToReturn = new List<StationEntranceModel>
            {
                new StationEntranceModel { Name = "34 St - Herald Sq", Point = new GeoPoint(40.7497, -73.9878) },
                new StationEntranceModel { Name = "Times Sq - 42 St", Point = new GeoPoint(40.7557, -73.9870) }
            };

            // Act
            var result = await _service.GetStationEntrancesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("34 St - Herald Sq", result[0].Name);
        }

        [TestMethod]
        public void GetStationDistance_WithValidEntrances_CalculatesCorrectDistance()
        {
            // Arrange
            var entrance1 = new StationEntranceModel
            {
                Name = "Grand Central - 42 St",
                Point = new GeoPoint(40.7527, -73.9772)
            };
            var entrance2 = new StationEntranceModel
            {
                Name = "Times Sq - 42 St",
                Point = new GeoPoint(40.7557, -73.9870)
            };

            // Act
            var distance = _service.GetStationDistance(entrance1, entrance2);

            // Assert
            Assert.IsNotNull(distance);
            Assert.AreEqual("Miles", distance.Unit);
            Assert.AreEqual(0.55, distance.Value, 0.05);
        }

        [TestMethod]
        public void GetStationDistance_WhenEntranceIsNull_ReturnsZeroDistance()
        {
            // Arrange
            var entrance = new StationEntranceModel
            {
                Name = "Grand Central - 42 St",
                Point = new GeoPoint(40.7527, -73.9772)
            };

            // Act
            var distance = _service.GetStationDistance(null, entrance);

            // Assert
            Assert.IsNotNull(distance);
            Assert.AreEqual("Miles", distance.Unit);
            Assert.AreEqual(0.0, distance.Value);
        }

        [TestMethod]
        public async Task GetNearbyStationEntrancesAsync_ReturnsStationsWithinRadiusOrderedByDistance()
        {
            // Arrange - User standing near Bryant Park (40.7536, -73.9832)
            var userLocation = new GeoPoint(40.7536, -73.9832);

            // Station 1: 5 Ave - 42 St (~0.12 miles away)
            // Station 2: Times Sq - 42 St (~0.25 miles away)
            // Station 3: Wall St (~4.2 miles away - outside 0.5 radius)
            _repo.BoundingBoxEntrancesToReturn = new List<StationEntrance>
            {
                new StationEntrance { Id = 1, Name = "Times Sq - 42 St", Latitude = 40.7557, Longitude = -73.9870 },
                new StationEntrance { Id = 2, Name = "5 Ave - 42 St", Latitude = 40.7539, Longitude = -73.9810 },
                new StationEntrance { Id = 3, Name = "Wall St", Latitude = 40.7069, Longitude = -74.0090 }
            };

            // Act
            var results = await _service.GetNearbyStationEntrancesAsync(userLocation, radiusMiles: 0.5, limit: 10);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count, "Wall St should be excluded because it exceeds 0.5 miles.");
            Assert.AreEqual("5 Ave - 42 St", results[0].Name, "Closest station must be first.");
            Assert.AreEqual("Times Sq - 42 St", results[1].Name, "Second closest station must be second.");
            Assert.IsTrue(results[0].DistanceInMiles < results[1].DistanceInMiles);
        }

        [TestMethod]
        public async Task GetNearbyStationEntrancesAsync_WhenNoCandidatesInRadius_ReturnsEmptyList()
        {
            // Arrange
            var userLocation = new GeoPoint(40.7536, -73.9832);
            _repo.BoundingBoxEntrancesToReturn = new List<StationEntrance>();

            // Act
            var results = await _service.GetNearbyStationEntrancesAsync(userLocation, radiusMiles: 0.5, limit: 10);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public async Task GetNearbyStationEntrancesAsync_WhenLocationIsNull_ReturnsEmptyList()
        {
            // Act
            var results = await _service.GetNearbyStationEntrancesAsync(null!, radiusMiles: 0.5, limit: 10);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }
    }
}
