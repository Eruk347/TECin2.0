using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface IInstructorService
    {
        Task<InstructorResponse?> CreateInstructor(InstructorRequest newInstructor, string accesstoken);
        Task<InstructorResponse?> DeleteInstructor(string instructorId, string accesstoken);
        Task<List<InstructorResponse?>> GetAllInstructors();
        Task<InstructorResponse?> GetInstruktoById(string instructorId);
        Task<InstructorResponse?> UpdateInstructor(string instructorId, InstructorRequest updateInstructor, string accesstoken);
    }
    public class InstructorService(IPasswordRepository passwordRepository
                    , IUserRepository userRepository
                    , ISettingRepository settingRepository
                    , ILoggerService loggerService
                    , IRoleRepository roleRepository) : IInstructorService
    {
        private readonly IPasswordRepository _passwordRepository = passwordRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISettingRepository _settingRepository = settingRepository;
        private readonly ILoggerService _loggerService = loggerService;
        private readonly IRoleRepository _roleRepository = roleRepository;
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

        public async Task<InstructorResponse?> CreateInstructor(InstructorRequest newInstructor, string accesstoken)
        {
            bool breakWhile = false;
            while (!breakWhile)
            {
                id = Guid.NewGuid();
                bool doesExist = await GUIDIsFree(id.ToString());

                if (!doesExist)
                    breakWhile = true;
            }

            string salt = Guid.NewGuid().ToString();

            User? user = MapInstructorRequestToUser(newInstructor, id.ToString(), salt);
            Password? password = CreatePassword(newInstructor.Password, id.ToString(), salt);

            if (user != null && password != null)
            {
                User? insertedUser = await _userRepository.InsertNewUser(user);
                Password? insertedPassword = await _passwordRepository.InsertNewPassword(password);

                if (insertedUser != null && insertedPassword != null)
                {
                    _loggerService.WriteLog("Create", accesstoken, insertedUser);
                    return MapUserToInstructorResponse(insertedUser);
                }
            }
            return null;
        }

        public async Task<InstructorResponse?> DeleteInstructor(string instructorId, string accesstoken)
        {

            User? deletedUser = await _userRepository.DeleteUser(instructorId);
            Password? deletedPassword = await _passwordRepository.DeletePassword(instructorId);

            if (deletedUser != null && deletedPassword != null)
            {
                _loggerService.WriteLog("Delete", accesstoken, deletedUser);
                return MapUserToInstructorResponse(deletedUser);
            }
            return null;
        }

        public async Task<List<InstructorResponse?>> GetAllInstructors()
        {
            List<User> users = await _userRepository.SelectAllStaff();

            return [.. users.Select(instructor => MapUserToInstructorResponse(instructor))];
        }

        public async Task<InstructorResponse?> GetInstruktoById(string instructorId)
        {
            User? user = await _userRepository.SelectUserById(instructorId);
            if (user != null)
            {
                return MapUserToInstructorResponse(user);
            }
            return null;
        }

        public async Task<InstructorResponse?> UpdateInstructor(string instructorId, InstructorRequest updateInstructor, string accesstoken)//skal laves om i forhold til ikke idntastede data, husk password!!!
        {
            User? originalUser = await _userRepository.SelectUserById(instructorId);
            if (originalUser == null)
            {
                return null;
            }

            if (updateInstructor.Password != null)
            {
                Password? password = CreatePassword(updateInstructor.Password, instructorId, originalUser.Salt);
                if (password != null)
                {
                    Password? updatedPassword = await _passwordRepository.UpdatePassword(password);
                    if (updatedPassword == null)
                    {
                        return null;
                    }
                }
            }

            User? user = MapInstructorRequestToUser(updateInstructor, instructorId, originalUser.Salt);

            if (user != null)
            {
                User? updatedUser = await _userRepository.UpdateUser(instructorId, user);

                if (updatedUser != null)
                {
                    _loggerService.WriteLog(accesstoken, originalUser, updatedUser);
                    return MapUserToInstructorResponse(updatedUser);
                }
            }
            return null;
        }


        private User? MapInstructorRequestToUser(InstructorRequest instructorRequest, string _id, string salt)
        {
            try
            {

                return new User
                {
                    Id = _id,
                    Username = instructorRequest.Username,
                    FirstName = instructorRequest.FirstName,
                    LastName = instructorRequest.LastName,
                    Phonenumber = instructorRequest.Phonenumber,
                    Email = instructorRequest.Email,
                    Groups = instructorRequest.Groups,
                    RoleId = instructorRequest.RoleId,
                    Settings = instructorRequest.Settings,
                    Deactivated = instructorRequest.Deactivated,
                    Salt = salt
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapInstructorRequestToUser", e);
                return null;
            }
        }

        //private User? MapInstructorUpdateRequestToUser(InstructorUpdateRequest instructorRequest, string _id, string salt)
        //{
        //    try
        //    {
        //        return new User
        //        {
        //            Id = _id,
        //            Username = instructorRequest.Username,
        //            FirstName = instructorRequest.FirstName,
        //            LastName = instructorRequest.LastName,
        //            Phonenumber = instructorRequest.Phonenumber,
        //            Email = instructorRequest.Email,
        //            Groups = instructorRequest.Groups,
        //            RoleId = instructorRequest.RoleId,
        //            Settings = instructorRequest.Settings,
        //            Deactivated = instructorRequest.Deactivated,
        //            Salt = salt
        //        };
        //    }
        //    catch (Exception e)
        //    {
        //        WriteToLog("MapInstructorUpdateRequestToUser", e);
        //        return null;
        //    }
        //}

        private Password? CreatePassword(string password, string _id, string salt)
        {
            try
            {
                string encryptedPassword = Hash.HashPassword(password, salt);

                return new Password
                {
                    Id = _id,
                    Cipher = encryptedPassword
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapInstructorRequestToPassword", e);
                return null;
            }
        }

        private InstructorResponse? MapUserToInstructorResponse(User user)
        {
            try
            {
                user.Groups ??= [];
                Role? role = _roleRepository.SelectRoleById(user.RoleId).Result;

                if (role == null)
                {
                    return null;
                }

                InstructorResponse response = new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phonenumber = user.Phonenumber,
                    Username = user.Username,
                    Groups = [.. user.Groups.Select(group => new InstructorGroupResponse
                    {
                        Id = group.Id,
                        Name = group.Name
                    })],
                    Role = new InstructorRoleResponse
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Description = role.Description,
                        Rank = role.Rank
                    }
                };
                return response;
            }
            catch (Exception e)
            {
                WriteToLog("MapUserToInstructorResponse", e);
                return null;
            }
        }


    }
}