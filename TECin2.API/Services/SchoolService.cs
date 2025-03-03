using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface ISchoolService
    {
        Task<SchoolResponse?> CreateSchool(SchoolRequest newSchool, string accesstoken);
        Task<SchoolResponse?> DeleteSchool(int schoolId, string accesstoken);
        Task<List<SchoolResponse?>> GetAllSchools();
        Task<SchoolResponse?> GetSchoolById(int schoolId);
        Task<SchoolResponse?> GetSchoolByName(string schoolName);
        Task<SchoolResponse?> UpdateSchool(int schoolId, SchoolRequest updateSchool, string accesstoken);
    }
    public class SchoolService(ISchoolRepository schoolRepository, ILoggerService loggerService) : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;
        private readonly ILoggerService _loggerService = loggerService;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<SchoolResponse?> CreateSchool(SchoolRequest newSchool, string accesstoken)
        {
            try
            {
                School? school = MapSchoolRequestToSchool(newSchool);
                if (school != null)
                {
                    School? insertedSchool = await _schoolRepository.InsertNewSchool(school);
                    if (insertedSchool != null)
                    {
                        _loggerService.WriteLog("Create", accesstoken, insertedSchool);
                        return MapSchoolToSchoolResponse(insertedSchool);
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                WriteToLog("CreateSchool", e);
                return null;
            }
        }

        public async Task<SchoolResponse?> DeleteSchool(int schoolId, string accesstoken)
        {
            try
            {
                School? deletedSchool = await _schoolRepository.DeleteSchool(schoolId);
                if (deletedSchool != null)
                {
                    _loggerService.WriteLog("Delete", accesstoken, deletedSchool);
                    return MapSchoolToSchoolResponse(deletedSchool);
                }
                return null;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteSchool", e);
                return null;
            }
        }

        public async Task<List<SchoolResponse?>> GetAllSchools()
        {
            try
            {
                List<School> schools = await _schoolRepository.SelectAllSchools();
                return schools.Select(school => MapSchoolToSchoolResponse(school)).ToList() ?? [];
            }
            catch (Exception e)
            {
                WriteToLog("GetAllSchools", e);
                return [];
            }
        }

        public async Task<SchoolResponse?> GetSchoolById(int schoolId)
        {
            try
            {
                School? school = await _schoolRepository.SelectSchoolById(schoolId);
                if (school != null)
                {
                    return MapSchoolToSchoolResponse(school);
                }
                return null;
            }
            catch (Exception e)
            {
                WriteToLog("GetSchoolById", e);
                return null;
            }
        }

        public async Task<SchoolResponse?> GetSchoolByName(string schoolName)
        {
            try
            {
                School? school = await _schoolRepository.SelectSchoolByName(schoolName);
                if (school != null)
                {
                    return MapSchoolToSchoolResponse(school);
                }
                return null;
            }
            catch (Exception e)
            {
                WriteToLog("GetSchoolByName", e);
                return null;
            }
        }

        public async Task<SchoolResponse?> UpdateSchool(int schoolId, SchoolRequest updateSchool, string accesstoken)
        {
            try
            {
                School? school = MapSchoolRequestToSchool(updateSchool);
                if (school != null)
                {
                    School? updatedSchool = await _schoolRepository.UpdateSchool(schoolId, school);
                    if (updatedSchool != null)
                    {
                        _loggerService.WriteLog("Update", accesstoken, updatedSchool);
                        return MapSchoolToSchoolResponse(updatedSchool);
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                WriteToLog("UpdateSchool", e);
                return null;
            }
        }

        private School? MapSchoolRequestToSchool(SchoolRequest schoolRequest)
        {
            try
            {
                return new School
                {
                    Name = schoolRequest.Name,
                    Deactivated = schoolRequest.Deactivated,
                    Principal = schoolRequest.Principal
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapSchoolRequestToSchool", e);
                return null;
            }
        }

        private SchoolResponse? MapSchoolToSchoolResponse(School school)
        {
            try
            {
                SchoolResponse schoolResponse = new()
                {
                    Id = school.Id,
                    Name = school.Name,
                    Deactivated = school.Deactivated,
                    Principal = school.Principal ?? "",
                };
                if (school.Departments != null)
                {
                    schoolResponse.Departments = school.Departments.Select(department => new SchoolDepartmentResponse
                    {
                        Id = department.Id,
                        Name = department.Name,
                        DepartmentHead = department.DepartmentHead ?? ""
                    }).ToList() ?? [];
                }

                return schoolResponse;
            }
            catch (Exception e)
            {
                WriteToLog("MapSchoolToSchoolResponse", e);
                return null;
            }
        }
    }
}
