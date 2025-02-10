namespace TECin2.API.DTOs
{
    public class InstruktorResponse
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Phonenumber { get; set; }
        public string? Email { get; set; }
        public required string Username { get; set; }
        public required InstruktorGroupResponse Group { get; set; }
        public required InstruktorRoleResponse Role { get; set; }
    }

    public class InstruktorRoleResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Rank { get; set; }
    }

    public class InstruktorGroupResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }


}

