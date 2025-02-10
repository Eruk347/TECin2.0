namespace TECin2.API.DTOs
{
    public class RoleResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool Deactivated { get; set; }
        public int Rank { get; set; }
    }
}
