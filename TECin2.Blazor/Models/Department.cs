using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TECin2.Blazor.Models
{
    public class Department
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Group?>? Groups { get; set; }
        public bool Deactivated { get; set; }
        public string? DepartmentHead { get; set; }
        public int SchoolId { get; set; }
        public School? School { get; set; }
    }
}
