namespace TECin2.API.DTOs
{
    public class SettingResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool Deactivated { get; set; }
        public List<SettingUserResponse> Users { get; set; } = [];
    }

    public class SettingUserResponse
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Phonenumber { get; set; }
        public string? Email { get; set; }
        public bool Deactivated { get; set; }
        public int GroupId { get; set; }
        public int RoleId { get; set; }
    }
}
