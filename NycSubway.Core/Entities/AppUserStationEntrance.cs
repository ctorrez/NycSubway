namespace NycSubway.Core.Entities
{
    public class AppUserStationEntrance
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int StationEntranceId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual StationEntrance StationEntrance { get; set; }
    }
}
