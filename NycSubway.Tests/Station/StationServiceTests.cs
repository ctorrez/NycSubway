using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            public Task<List<StationEntranceModel>> GetStationEntrancesAsync()
            {
                return Task.FromResult(EntrancesToReturn);
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
    }
}
