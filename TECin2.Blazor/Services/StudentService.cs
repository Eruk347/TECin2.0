using System.Runtime.CompilerServices;
using TECin2.Blazor.Models;
using TECin2.Blazor.Models.DTOs;

namespace TECin2.Blazor.Services
{
    public interface IStudentService
    {
        Task<Student?> CreateStudent(StudentRequest newStudent);
        Task<Student?> DeleteStudent(string studentId);
        Task<List<Student?>> GetAllStudents(int groupId);
        Task<List<Group?>> GetGroups();//skal vi ahve den her??
        Task<Student?> GetStudentById(string studentId);
        Task<Student?> UpdateStudent(StudentRequest updateStudent, string studentId);
    }
    public class StudentService : IStudentService
    {
        private const string _URL = Global.URL + "Student/";
        private const string _URL2 = Global.URL + "Group/";

        private void WriteToLog(string task, Exception e)
        {
            LoggerService.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<Student?> CreateStudent(StudentRequest newStudent)
        {
            newStudent.CPR = Hash.HashPassword(newStudent.CPR!, newStudent.CPR!);
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                client.BaseAddress = new Uri(_URL);

                var response = await client.PostAsJsonAsync(_URL, newStudent);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<StudentResponse>();

                    return MapStudentResponseToStudent(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("Create", e);
                return null;
            }
            return null;
        }

        public async Task<Student?> DeleteStudent(string studentId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.DeleteAsync(_URL + studentId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<StudentResponse>();

                    return MapStudentResponseToStudent(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("Delete", e);
                return null;
            }
            return null;
        }

        public async Task<List<Student?>> GetAllStudents(int groupId)//sammen ligenet med de andre, er det her en forkert måde at gøre det på. Men det er nemmere
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<StudentResponse[]>();

                    if (result == null)
                        return [];

                    return [.. result.Select(stu => MapStudentResponseToStudent(stu))];
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetAll", e);
                return [];
            }
            return [];
        }

        public async Task<List<Group?>> GetGroups()//why is this here????
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL2);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<Group?>>();
                    return result ?? [];
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetGroups", e);
                return [];
            }
            return [];
        }

        public async Task<Student?> GetStudentById(string studentId)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                var response = await client.GetAsync(_URL + studentId);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<StudentResponse>();

                    return MapStudentResponseToStudent(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("GetById", e);
                return null;
            }
            return null;
        }

        public async Task<Student?> UpdateStudent(StudentRequest updateStudent, string studentId)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Global.GetToken());
                client.BaseAddress = new Uri(_URL);
                var response = await client.PutAsJsonAsync(_URL + studentId, updateStudent);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<StudentResponse>();

                    return MapStudentResponseToStudent(result);
                }
            }
            catch (Exception e)
            {
                WriteToLog("Update", e);
                return null;
            }
            return null;
        }
        private StudentRequest? MapStudentoStudentRequest(Student? _student)
        {
            if (_student == null)
                return null;
            try
            {
                return new StudentRequest
                {
                    FirstName = _student.FirstName,
                    LastName = _student.LastName,
                    Phonenumber = _student.Phonenumber,
                    Email = _student.Email,
                    Username = _student.Username,
                    CPR = _student.CPR,
                    GroupId = _student.Group.Id,
                    Deactivated = _student.Deactivated,
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapStudentoStudentRequest", e);
                return null;
            }
        }

        private Student? MapGroupUserResponseToStudent(GroupUsersResponse? _studentResponse)
        {
            if (_studentResponse == null)
                return null;
            try
            {
                return new Student
                {
                    Id = _studentResponse.Id,
                    FirstName = _studentResponse.FirstName,
                    LastName = _studentResponse.LastName,
                    Username = _studentResponse.Username,
                    Phonenumber = _studentResponse.Phonenumber,
                    Email = _studentResponse.Email,
                    LastCheckIn = _studentResponse.LastCheckin,
                    Deactivated = _studentResponse.Deactivated,
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapGroupUserResponseToStudent", e);
                return null;
            }
        }
        private Student? MapStudentResponseToStudent(StudentResponse? _studentResponse)
        {
            if (_studentResponse == null)
                return null;
            try
            {
                return new Student
                {
                    Id = _studentResponse.Id,
                    FirstName = _studentResponse.FirstName,
                    LastName = _studentResponse.LastName,
                    Username = _studentResponse.Username,
                    Phonenumber = _studentResponse.Phonenumber,
                    Email = _studentResponse.Email,
                    LastCheckIn = _studentResponse.LastCheckin,
                    Deactivated = _studentResponse.Deactivated,
                    Group = new Models.Group
                    {
                        Id = _studentResponse.Group.Id,
                        Name = _studentResponse.Group.Name,
                        DepartmentId = _studentResponse.Group.DepartmentId,
                        ArrivalTime = _studentResponse.Group.ArrivalTime,
                        WorkHoursInDay = _studentResponse.Group.WorkHoursInDay,
                        IsLateBuffer = _studentResponse.Group.IsLateBuffer,
                        IsLateMessage = "",
                        FlexibleArrivalEnabled = _studentResponse.Group.FlexibleArrivalEnabled,
                        FlexibleAmount = _studentResponse.Group.FlexibleAmount,
                    },
                    CheckInStatuses = [.. _studentResponse.CheckInResponses!.Select(checkin => new CheckInStatus
                    {
                        Id = checkin.Id,
                        User_Id = _studentResponse.Id,
                        ArrivalDate = checkin.ArrivalDate,
                        ArrivalTime = checkin.ArrivalTime,
                        Departure = checkin.Departure
                    })]
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapStudentResponseToStudent", e);
                return null;
            }
        }
    }
}
