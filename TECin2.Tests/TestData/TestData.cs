using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECin2.API.Database.Entities;
using TECin2.API.DTOs;

namespace TECin2.Tests.TestData
{
    public static class TestData
    {
        public static readonly byte[] turkis = [0xff, 0x28, 0xcd, 0xaf];
        public static readonly byte[] koralRoed = [0xFF, 0xfa, 0x50, 0x50];
        public static readonly byte[] sennepsGul = [0xFF, 0xcd, 0xa0, 0x1e];

        public static School GetSchoolTestData()
        {
            return new School()
            {
                Id = 1,
                Name = "Test School",
                Deactivated = false,
                Departments = [],
                Principal = "1"
            };
        }

        public static Department GetDepartmentTestData()
        {
            return new Department()
            {
                Id = 1,
                Name = "Test Department",
                Deactivated = false,
                DepartmentHead = "1",
                Groups = [],
                SchoolId = 1,

            };
        }

        public static Group GetGroupTestData()
        {
            return new Group()
            {
                Id = 1,
                Name = "Test Group",
                Deactivated = false,
                DepartmentId = 1,
                ArrivalTime = new TimeOnly(7, 30, 0),
                WorkHoursInDayId = 1,
                FlexibleArrivalEnabled = false,
                FlexibleAmount = new TimeOnly(0, 0, 0),
                IsLateBuffer = new TimeOnly(0, 0, 0),
                IsLateMessage = "Du er forsinket",
                Users = [],
                WorkHoursInDay = GetWorkHoursInDayTestData()
            };
        }

        public static WorkHoursInDay GetWorkHoursInDayTestData()
        {
            return new WorkHoursInDay()
            {
                Id = 1,
                Monday = new TimeOnly(7, 30, 0),
                Tuesday = new TimeOnly(7, 30, 0),
                Wednesday = new TimeOnly(7, 30, 0),
                Thursday = new TimeOnly(7, 30, 0),
                Friday = new TimeOnly(7, 0, 0),
            };
        }

        public static Role GetRoleTestData(int id)
        {
            return new Role()
            {
                Id = id,
                Name = "Test Role",
                Deactivated = false,
                Rank = 1,
                Description = "test"
            };
        }

        public static User GetUserTestData(string id)
        {
            return new User()
            {
                Id = id,
                FirstName = "Test",
                LastName = "User",
                Email = "@",
                Role = GetRoleTestData(1),
                Username = "Test",
                Deactivated = false,
                Groups = [],
                IsStudent = true,
                LastCheckin = new DateOnly(2021, 1, 1),
                Phonenumber = 12345678,
                RoleId = 1,
                Salt = "salt",
                Settings = []
            };
        }

        public static CheckInResponseLong GetCheckInResponseLong(int id, string userId)
        {
            return new()
            {
                Id = id,
                UserId = userId,
                Arrival = new(7, 30, 0),
                FirstName = "Test",
                LastName = "Test",
            };
        }
    
  
    }
}
