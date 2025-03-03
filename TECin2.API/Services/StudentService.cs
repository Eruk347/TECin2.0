using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface IStudentService
    {
        Task<StudentResponse?> CreateStudent(StudentRequest newStudent, string accesstoken);
        Task<StudentResponse?> DeleteStudent(string studentId, string accesstoken);
        Task<List<StudentResponse?>> GetAllStudents();
        Task<StudentResponse?> GetStudentById(string id);
        Task<StudentResponse?> GetStudentByName(string firstName, string lastName);
        Task<StudentResponse?> UpdateStudent(string studentId, StudentRequest updateStudent, string accesstoken);
    }
    public class StudentService(IUserRepository userRepository
            , ISecurityRepository securityRepository
            , ICheckInRepository checkInrepository
            , IRoleRepository roleRepository
            , ILoggerService loggerService) :IStudentService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISecurityRepository _securityRepository = securityRepository;
        private readonly ICheckInRepository _checkInrepository = checkInrepository;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly ILoggerService _loggerService = loggerService;
        private Guid id;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        private async Task<bool> GUIDIsFree(string id)
        {
            List<User> users = await _userRepository.SelectAllUsers();
            if (users == null)
            {
                return true;
            }

            foreach (User u in users)
            {
                if (u.Id == id.ToString())
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<StudentResponse?> CreateStudent(StudentRequest newStudent, string accesstoken)
        {
            //This codes generates a GUID for students and users. 
            //The while loop runs until the user-databse has ben checked for the GUID. If it does not exists, the loop breaks. If it does exist, a new GUID will be generated and checked.
            bool continueWhile = true;
            while (continueWhile)
            {
                id = Guid.NewGuid();
                bool guidIsFree = await GUIDIsFree(id.ToString());

                if (guidIsFree)
                    continueWhile = false;
            }
            newStudent.IsStudent = true;

            User? user = MapStudentRequestToUser(newStudent, id.ToString());
            SecurityNumb? securityNumb = MapStudentRequestToSecurityNumbAndEncrypt(newStudent, id.ToString());

            if(user==null || securityNumb == null)
            {
                return null;
            }

            Role? role = await _roleRepository.SelectRoleByName("Student");

            if(role == null)
            {
                return null;
            }
            user.RoleId = role.Id;


            if (await _securityRepository.SelectSecurityNumbByCPR(securityNumb.Cipher) == null)
            {
                User? insertedUser = await _userRepository.InsertNewUser(user);
                SecurityNumb? insertedSecurityNumb = await _securityRepository.InsertNewSecurityNumb(securityNumb);


                if (insertedUser != null && insertedSecurityNumb != null)
                {
                    _loggerService.WriteLog("Create", accesstoken, insertedUser);
                    return MapUserToStudentResponse(insertedUser);
                }
            }
            return null;
        }

        public async Task<StudentResponse?> DeleteStudent(string studentId, string accesstoken)
        {
            await _checkInrepository.DeleteAllCheckInStatusForUser(studentId);

            User? deletedUser = await _userRepository.DeleteUser(studentId);
            SecurityNumb? deletedSecurityNumb = await _securityRepository.DeleteSecurityNumb(studentId);
            if (deletedUser != null && deletedSecurityNumb != null)
            {
                _loggerService.WriteLog("Delete", accesstoken, deletedUser);
                return MapUserToStudentResponse(deletedUser);
            }
            return null;
        }

        public async Task<List<StudentResponse?>> GetAllStudents()
        {
            List<User> users = await _userRepository.SelectAllStudents();

            return users.Select(student => MapUserToStudentResponse(student)).ToList() ?? [];
        }

        public async Task<StudentResponse?> GetStudentById(string id)
        {
            User? user = await _userRepository.SelectUserById(id);

            if (user != null)
            {
                List<CheckInStatus> checkInStatuses = await _checkInrepository.SelectCheckInForUser(id);

                return MapUserToStudentResponse(user, checkInStatuses);
            }
            return null;
        }

        public Task<StudentResponse?> GetStudentByName(string firstName, string lastName)//Skal måske ikke laves
        {
            throw new NotImplementedException();
        }

        public async Task<StudentResponse?> UpdateStudent(string studentId, StudentRequest updateStudent, string accesstoken)
        {
            User? originalUser = await _userRepository.SelectUserById(studentId);
            User? user = MapStudentRequestToUser(updateStudent, studentId);

            if (user != null)
            {
                User? updatedUser = await _userRepository.UpdateUser(studentId, user);

                if (updatedUser != null && originalUser != null)
                {
                    _loggerService.WriteLog(accesstoken, originalUser, updatedUser);
                    return MapUserToStudentResponse(updatedUser);
                }
            }
            return null;
        }

        private User? MapStudentRequestToUser(StudentRequest studentRequest, string _id)
        {
            try
            {
                return new User
                {
                    Id = _id,
                    Username = studentRequest.Username,
                    FirstName = studentRequest.FirstName,
                    LastName = studentRequest.LastName,
                    Phonenumber = studentRequest.Phonenumber,
                    Email = studentRequest.Email,
                    IsStudent = studentRequest.IsStudent,
                    Deactivated = studentRequest.Deactivated,
                    RoleId = studentRequest.RoleId,
                    LastCheckin = studentRequest.LastCheckin,
                    Salt="student"
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapStudentRequestToUser", e);
                return null;
            }
        }

        private SecurityNumb? MapStudentRequestToSecurityNumbAndEncrypt(StudentRequest studentRequest, string _id)
        {
            try
            {
                if (studentRequest.CPR == null)
                {
                    return null;
                }
                string encryptedCPR = Hash.HashPassword(studentRequest.CPR, studentRequest.CPR);

                return new SecurityNumb
                {
                    Id = _id,
                    Cipher = encryptedCPR
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapStudentRequestToSecurityNumbAndEncrypt", e);
                return null;
            }
        }

        private StudentResponse? MapUserToStudentResponse(User user)
        {
            try
            {
                if (user.Groups == null)
                {
                    return null;
                }
                StudentGroupResponse groupResponse = new()
                {
                    Id = user.Groups.ToList()[0].Id,
                    Name = user.Groups.ToList()[0].Name,
                    IsLateBuffer = user.Groups.ToList()[0].IsLateBuffer,
                    FlexibleAmount = user.Groups.ToList()[0].FlexibleAmount,
                    FlexibleArrivalEnabled = user.Groups.ToList()[0].FlexibleArrivalEnabled,
                    WorkHoursInDay = user.Groups.ToList()[0].WorkHoursInDay,
                    ArrivalTime = user.Groups.ToList()[0].ArrivalTime,
                };

                StudentResponse response = new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phonenumber = user.Phonenumber,
                    Email = user.Email,
                    Username = user.Username,
                    LastCheckin = user.LastCheckin,
                    Deactivated = user.Deactivated,
                    Group = groupResponse
                };

                return response;
            }
            catch (Exception e)
            {
                WriteToLog("MapUserToStudentResponse", e);
                return null;
            }
        }

        private StudentResponse? MapUserToStudentResponse(User user, List<CheckInStatus> checkInStatuses)
        {
            try
            {
                if (user.Groups == null)
                {
                    return null;
                }
                StudentGroupResponse groupResponse = new()
                {
                    Id = user.Groups.ToList()[0].Id,
                    Name = user.Groups.ToList()[0].Name,
                    IsLateBuffer = user.Groups.ToList()[0].IsLateBuffer,
                    FlexibleAmount = user.Groups.ToList()[0].FlexibleAmount,
                    FlexibleArrivalEnabled = user.Groups.ToList()[0].FlexibleArrivalEnabled,
                    WorkHoursInDay = user.Groups.ToList()[0].WorkHoursInDay,
                    ArrivalTime = user.Groups.ToList()[0].ArrivalTime,
                };

                List<CheckInResponseLong> checkInResponses = [];
                foreach (var checkIn in checkInStatuses)
                {
                    if (checkIn.User != null)
                        checkInResponses.Add(new()
                        {
                            Id = checkIn.Id,
                            UserId = checkIn.User_Id,
                            FirstName = checkIn.User.FirstName,
                            LastName = checkIn.User.LastName,
                            Email = checkIn.User.Email,
                            Phonenumber = checkIn.User.Phonenumber,
                            Arrival = checkIn.ArrivalTime,
                            LastCheckin = checkIn.User.LastCheckin,
                            Departure = checkIn.Departure
                        });
                }

                StudentResponse response = new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phonenumber = user.Phonenumber,
                    Email = user.Email,
                    Username = user.Username,
                    LastCheckin = user.LastCheckin,
                    Deactivated = user.Deactivated,
                    Group = groupResponse,
                    CheckInResponses = checkInResponses
                };

                return response;
            }
            catch (Exception e)
            {
                WriteToLog("MapUserToStudentResponse", e);
                return null;
            }
        }
    }
}