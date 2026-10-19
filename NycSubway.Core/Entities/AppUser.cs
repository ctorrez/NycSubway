using System.Collections.Generic;

namespace NycSubway.Core.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public virtual List<AppUserStationEntrance> AppUserStationEntrances { get; set; }
    }
}
