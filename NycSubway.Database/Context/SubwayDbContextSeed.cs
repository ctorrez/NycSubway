using System.Linq;
using System.Text;

namespace NycSubway.Database.Context
{
    public class SubwayDbContextSeed
    {
        public static void SeedNycData(SubwayDbContext context)
        {
            if (!context.StationEntrances.Any())
            {
                context.StationEntrances.AddRange(SubwayData.GetStationEntrances());
                context.SaveChanges();
            }
        }
    }
}
