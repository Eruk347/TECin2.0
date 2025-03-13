using TECin2.Blazor.Models;
using TECin2.Blazor.Models.DTOs;

namespace TECin2.Blazor.Services
{
    public interface IGroupService
    {
        Task<Group?> CreateGroup(GroupRequest newGroup);
        Task<Group?> DeleteGroup(int deletingGroupId, int newGroupId);
        Task<List<Group?>> GetAllGroups();
        Task<List<Department?>> GetAllDepartments();
        Task<Group?> GetGroupById(int groupId);
        Task<Group?> UpdateGroup(Group updateGroup, int groupId);
    }
    public class GroupService : IGroupService
    {
        private const string _URL = Global.URL + "group/";
        private const string _URL2 = Global.URL + "department/";

        private void WriteToLog(string task, Exception e)
        {
            LoggerService.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }
        public async Task<Group?> CreateGroup(GroupRequest newGroup)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(_URL);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());

                var response = await client.PostAsJsonAsync(_URL, newGroup);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<GroupResponse>();

                    if (result == null)
                        return null;

                    return MapGroupResponseToGroup(result, null);
                }
            }
            catch (Exception e)
            {
                WriteToLog("CreateGroup", e);
                return null;
            }
            return null;
        }

        public async Task<Group?> DeleteGroup(int deletingGroupId, int newGroupId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.DeleteAsync(_URL + deletingGroupId + "," + newGroupId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<GroupResponse>();

                    if (result == null)
                        return null;

                    return MapGroupResponseToGroup(result, null);
                }
            }
            catch (Exception e)
            {
                WriteToLog("DeleteGroup", e);
                return null;
            }
            return null;
        }

        public async Task<List<Department?>> GetAllDepartments()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(_URL2);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<Department?>>();
                    return result ?? [];
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetAllDepartments", e);
                return [];
            }
            return [];
        }

        public async Task<List<Group?>> GetAllGroups()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(_URL);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<GroupResponse[]>();

                    if (result == null)
                        return [];

                    return [.. result.Select(gr => MapGroupResponseToGroup(gr, null))];
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetAllGroups", e);
                return [];
            }
            return [];
        }

        public async Task<Group?> GetGroupById(int groupId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL + groupId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<GroupResponse>();

                    if (result == null)
                        return null;

                    List<Student> students = [];
                    foreach (var item in result.Students)
                    {
                        students.Add(
                            new Student
                            {
                                Id = item.Id,
                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                Email = item.Email,
                                Phonenumber = item.Phonenumber,
                                LastCheckIn = item.LastCheckin,
                                Username = item.Username,
                                Deactivated = item.Deactivated,
                            });
                    }
                    return MapGroupResponseToGroup(result, students);
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetGroupById", e);
                return null;
            }
            return null;
        }

        public async Task<Group?> UpdateGroup(Group updateGroup, int groupId)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                client.BaseAddress = new Uri(_URL);
                var response = await client.PutAsJsonAsync(_URL + groupId, updateGroup);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<GroupResponse>();

                    if (result == null)
                        return null;

                    return MapGroupResponseToGroup(result, null);
                }
            }
            catch (Exception e)
            {
                WriteToLog("UpdateGroup", e);
                return null;
            }
            return null;
        }

        private Group? MapGroupResponseToGroup(GroupResponse groupResponse, List<Student>? students)
        {
            try
            {
                Group answer = new()
                {
                    Id = groupResponse.Id,
                    ArrivalTime = groupResponse.ArrivalTime,
                    IsLateBuffer = groupResponse.IsLateBuffer,
                    IsLateMessage = groupResponse.IsLateMessage ?? "",
                    Deactivated = groupResponse.Deactivated,
                    Name = groupResponse.Name,
                    FlexibleArrivalEnabled = groupResponse.FlexibleArrivalEnabled,
                    FlexibleAmount = groupResponse.FlexibleAmount,
                    Department = new Department
                    {
                        Id = groupResponse.Department.Id,
                        Name = groupResponse.Department.Name,
                        Deactivated = groupResponse.Department.Deactivated
                    },
                    DepartmentId = groupResponse.Department.Id,
                    WorkHoursInDay = groupResponse.WorkHoursInDay,
                    Students = students
                };
                return answer;
            }
            catch (Exception e)
            {
                WriteToLog("MapGroupResponseToGroup", e);
                return null;
            }
        }

        //private GroupRequest MapGroupToGroupRequest(Group updateGroup)
        //{
        //    try
        //    {
        //        return new GroupRequest
        //        {
        //            Name = updateGroup.Name,
        //            Deactivated = updateGroup.Deactivated,
        //            ArrivalTime = updateGroup.ArrivalTime,
        //            Departuretime = updateGroup.Departuretime,
        //            IsLateBuffer = updateGroup.IsLateBuffer,
        //            IsLateMessage = updateGroup.IsLateMessage,
        //            DepartmentId = updateGroup.DepartmentId
        //        };
        //    }
        //    catch (Exception e)
        //    {
        //        WriteToLog("MapGroupToGroupRequest", e);
        //        return null;
        //    }
        //}
    }
}
