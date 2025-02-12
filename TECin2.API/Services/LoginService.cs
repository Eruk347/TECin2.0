using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface ILoginService
    {
        Task<LogInResponse?> Login(LogInRequest login);
    }
    public class LoginService(IConfiguration config,
        IUserRepository userRepository,
        IPasswordRepository passwordRepository) : ILoginService
    {
        private readonly IConfiguration _config = config;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordRepository _passwordRepository = passwordRepository;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        private string GenerateJsonWebToken()
        {
            try
            {
                var key = _config["Jwt:Key"];
                if (key != null)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                      _config["Jwt:Issuer"],
                      null,
                      expires: DateTime.Now.AddMinutes(30).ToUniversalTime(),
                      signingCredentials: credentials);

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                return string.Empty;
            }
            catch (Exception e)
            {
                WriteToLog("GenerateJsonWebToken", e);
                return string.Empty;
            }
        }

        public async Task<LogInResponse?> Login(LogInRequest login)
        {
            string username = login.UserName;
            string password = login.Password;
            try
            {
                User? user = await _userRepository.SelectUserByUsername(username);

                if (user != null)
                {
                    Password? selectedPassword = await _passwordRepository.SelectPassword(user.Id);
                    if (selectedPassword != null)
                    {
                        bool validate = Hash.Validate(password, selectedPassword.Cipher);
                        if (validate == true)
                        {

                            LogInResponse response = new()
                            {
                                Id = user.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email,
                                Phonenumber = user.Phonenumber,
                                Username = user.Username,
                                Token = GenerateJsonWebToken(),
                                RoleRank = user.Role.Rank,
                            };

                            ClearOldTokens(response.Id);
                            Global.AddTokens("" + response.Token + "," + response.Id);

                            if (user.Groups != null)
                            {
                                response.Groups = user.Groups.Select(group => new LogInGroupResponse
                                {
                                    Id = group.Id,
                                    Name = group.Name,
                                    ArrivalTime = group.ArrivalTime,
                                    FlexibleAmount = group.FlexibleAmount,
                                    FlexibleArrivalEnabled = group.FlexibleArrivalEnabled,
                                    IsLateBuffer = group.IsLateBuffer,
                                    IsLateMessage = group.IsLateMessage,
                                    WorkHoursInDay = group.WorkHoursInDay,
                                }).ToList();
                            }
                            else
                                response.Groups = [];

                            if (user.Settings != null)
                            {
                                response.Settings = user.Settings.Select(setting => new LogInSettingResponse
                                {
                                    Id = setting.Id,
                                    Name = setting.Name,
                                    Description = setting.Description,
                                    Deactivated = setting.Deactivated
                                }).ToList();
                            }
                            else
                                response.Settings = [];
                            return response;
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                WriteToLog("Login", e);
                return null;
            }
        }

        public void ClearOldTokens(string userId)
        {
            List<string> _tokens = [.. Global.GetTokens()];

            foreach (var token in _tokens)
            {
                if (token.Split(',')[1] == userId)
                {
                    Global.RemoveToken(token);
                }
                if (!Global.CheckTokenValid(token))
                    Global.RemoveToken(token);
            }
        }
    }
}