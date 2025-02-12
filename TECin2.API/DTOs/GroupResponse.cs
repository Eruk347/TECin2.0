﻿using TECin2.API.Database.Entities;

namespace TECin2.API.DTOs
{
    public class GroupResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool Deactivated { get; set; }
        public required TimeOnly ArrivalTime { get; set; }
        public bool FlexibleArrivalEnabled { get; set; }
        public TimeOnly? FlexibleAmount { get; set; }
        public TimeOnly? IsLateBuffer { get; set; }
        public string? IsLateMessage { get; set; }
        public WorkHoursInDay? WorkHoursInDay { get; set; }
        public required GroupDepartmentResponse Department { get; set; }
        public List<GroupUsersResponse> Users { get; set; } = [];
    }

    public class GroupDepartmentResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool Deactivated { get; set; }
    }

    public class GroupUsersResponse
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Phonenumber { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public DateOnly? LastCheckin { get; set; }
        public bool Deactivated { get; set; }
    }
}
