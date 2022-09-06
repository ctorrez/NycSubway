using CsvHelper;
using NycSubway.Core.Services.Station;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NycSubway.Infrastructure.Repositories
{
    public class StationRepo : IStationRepo
    {
        public StationRepo()
        {

        }

        public List<StationEntranceModel> GetStationEntrances()
        {
            var stations = new List<StationEntranceModel>();
            var csvs = GetSubwayEntranceCsvs();

            foreach(var x in csvs)
            {
                stations.Add(new StationEntranceModel()
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

            var lat = values[1].Replace("(", "");
            var longitude = values[2].Replace(")", "");

            return new GeoPoint()
            {
                Latitude = double.Parse(lat),
                Longitude = double.Parse(longitude)
            };
        }

        private static List<SubwayEntranceCsv> GetSubwayEntranceCsvs()
        {
            var data = ReadResource("DOITT_SUBWAY_ENTRANCE_01_13SEPT2010.csv");

            using (var reader = new StringReader(data))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<SubwayEntranceCsvMap>();

                var records = csv.GetRecords<SubwayEntranceCsv>().ToList();

                return records;
            }
        }

        private static string ReadResource(string name)
        {
            // https://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = assembly.GetManifestResourceNames()
                    .First(str => str.EndsWith(name));

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
