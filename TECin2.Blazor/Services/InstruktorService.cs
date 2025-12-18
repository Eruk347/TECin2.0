using TECin2.ClassLibrary.DTOs;
using TECin2.ClassLibrary.Entities;

namespace TECin2.Blazor.Services
{
    public interface IInstructorService
    {
        Task<Instructor?> CreateInstructor(InstructorRequest newInstructor);
        Task<Instructor?> DeleteInstructor(string InstructorId);
        Task<List<Instructor>> GetAllInstructors();
        Task<Instructor?> GetInstructorById(string InstructorId);
        // Task<List<Group>> GetGroups();
        Task<Instructor?> UpdateInstructor(InstructorRequest updateInstructor, string InstructorId);
    }
    public class InstructorService : IInstructorService
    {
        private const string _URL = Global.URL + "Instructor/";
        private const string _URL2 = Global.URL + "group/";

        private void WriteToLog(string task, Exception e)
        {
            LoggerService.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<Instructor?> CreateInstructor(InstructorRequest newInstructor)
        {
            newInstructor.Email = "";
            newInstructor.Settings = [];
            newInstructor.Phonenumber = 0;
            if (newInstructor.Password == null)
                return null;
            newInstructor.Password = Hash.HashPassword(newInstructor.Password, newInstructor.Password);
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(_URL);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());

                var response = await client.PostAsJsonAsync(_URL, newInstructor);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<InstructorResponse>();

                    return MapInstructorResponseToInstructor(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("CreateInstructor", e);
                return null;
            }
            return null;
        }

        public async Task<Instructor?> DeleteInstructor(string InstructorId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.DeleteAsync(_URL + InstructorId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<InstructorResponse>();

                    return MapInstructorResponseToInstructor(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("DeleteInstructor", e);
                return null;
            }
            return null;
        }

        public async Task<List<Instructor>> GetAllInstructors()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<InstructorResponse>>();

                    if (result == null)
                        return [];

                    List<Instructor> answer = [];
                    foreach (var instructor in result)
                    {
                        var map = MapInstructorResponseToInstructor(instructor);
                        if (map != null)
                            answer.Add(map);
                    }
                    return answer;
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetAllInstructors", e);
                return [];
            }
            return [];
        }

        //public async Task<List<Group?>> GetGroups()//skal vi beholde den her?
        //{
        //    try
        //    {
        //        using var client = new HttpClient();
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
        //        var response = await client.GetAsync(_URL2);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = await response.Content.ReadFromJsonAsync<List<Group?>>();

        //            return result ?? [];
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        WriteToLog("GetGroups", e);
        //        return [];
        //    }
        //    return [];
        //}

        public async Task<Instructor?> GetInstructorById(string InstructorId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL + InstructorId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<InstructorResponse>();
                    return MapInstructorResponseToInstructor(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetInstructorById", e);
                return null;
            }
            return null;
        }

        public async Task<Instructor?> UpdateInstructor(InstructorRequest updateInstructor, string InstructorId)
        {
            if (updateInstructor.Password != null)//|| updateInstructor.Password != "")
            {
                updateInstructor.Password = Hash.HashPassword(updateInstructor.Password!, updateInstructor.Password!);
            }
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                client.BaseAddress = new Uri(_URL);
                var response = await client.PutAsJsonAsync(_URL + InstructorId, updateInstructor);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<InstructorResponse>();

                    return MapInstructorResponseToInstructor(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("UpdateInstructor", e);
                return null;
            }
            return null;
        }

        private Instructor? MapInstructorResponseToInstructor(InstructorResponse? InstructorResponse)
        {
            if (InstructorResponse == null)
                return null;
            try
            {
                return new Instructor
                {
                    Id = InstructorResponse.Id,
                    FirstName = InstructorResponse.FirstName,
                    LastName = InstructorResponse.LastName,
                    Email = InstructorResponse.Email,
                    UserName = InstructorResponse.UserName,
                    Phonenumber = InstructorResponse.Phonenumber,
                    PrimaryGroupId = InstructorResponse.PrimaryGroupId,
                    Groups = [.. InstructorResponse.Groups.Select(MapInstructorGroupResponseToGroup)],
                    Role = new Role
                    {
                        Id = InstructorResponse.Role.Id,
                        Rank = InstructorResponse.Role.Rank,
                        Name = InstructorResponse.Role.Name,
                        Description = InstructorResponse.Role.Description
                    },
                    Settings = InstructorResponse.Settings,
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapInstructorResponseToInstructor", e);
                return null;
            }
        }

        private Group? MapInstructorGroupResponseToGroup(InstructorGroupResponse instructorGroupResponse)
        {
            try
            {
                return new Group
                {
                    Id = instructorGroupResponse.Id,
                    Name = instructorGroupResponse.Name,
                    ArrivalTime = instructorGroupResponse.ArrivalTime,
                    DepartmentId = instructorGroupResponse.DepartmentId,
                    IsLateMessage = instructorGroupResponse.IsLateMessage,
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapInstructorResponseToInstructor", e);
                return null;
            }
        }
    }
}
