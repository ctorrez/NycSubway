using CsvHelper;
using CsvHelper.Configuration;
using NycSubway.Core.Common;
using NycSubway.Core.Services.Station;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace NycSubway.Infrastructure.Repositories
{
    public class StationRepo : IStationRepo
    {
        public StationRepo()
        {

        }

        public List<StationEntrance> GetStationEntrances()
        {
            var stations = new List<StationEntrance>();
            var csvs = GetSubwayEntranceCsvs();

            foreach(var x in csvs)
            {
                stations.Add(new StationEntrance()
                {
                    Name = x.Name,
                    Point = GetGeoPoint(x.Geom)
                });
            }

            return stations;
        }

        private GeoPoint GetGeoPoint(string geom)
        {
            // POINT (-73.86835600032798 40.84916900104506)

            var values = geom.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return new GeoPoint()
            {
                Latitude = double.Parse(values[1]),
                Longitude = double.Parse(values[2])
            };
        }

        private static List<SubwayEntranceCsv> GetSubwayEntranceCsvs()
        {
            var data = ResourceHelper.ReadResource("DOITT_SUBWAY_ENTRANCE_01_13SEPT2010.csv");

            using (var reader = new StringReader(data))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<SubwayEntranceCsvMap>();

                var records = csv.GetRecords<SubwayEntranceCsv>().ToList();

                return records;
            }
        }
    }
}
