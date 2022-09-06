using CsvHelper.Configuration;

namespace NycSubway.Database.Context
{
    internal class SubwayEntranceCsv
    {
        public int ObjectId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Geom { get; set; }
        public string Line { get; set; }
    }

    internal class SubwayEntranceCsvMap : ClassMap<SubwayEntranceCsv>
    {
        public SubwayEntranceCsvMap()
        {
            // OBJECTID,URL,NAME,the_geom,LINE
            Map(m => m.ObjectId).Name("OBJECTID");
            Map(m => m.Url).Name("URL");
            Map(m => m.Name).Name("NAME");
            Map(m => m.Geom).Name("the_geom");
            Map(m => m.Line).Name("LINE");
        }
    }
}
