using TECin2.API.Database.Entities;

namespace TECin2.API.DTOs
{
    public class SettingRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool Deactivated { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
