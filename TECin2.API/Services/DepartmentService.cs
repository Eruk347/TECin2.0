using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentResponse?> CreateDepartment(DepartmentRequest newDepartment, string accesstoken);
        Task<DepartmentResponse?> DeleteDepartment(int departmentId, string accesstoken);
        Task<List<DepartmentResponse?>> GetAllDepartments();//måske ikke nødvendigt med nullable
        Task<DepartmentResponse?> GetDepartmentById(int departmentId);
        Task<DepartmentResponse?> UpdateDepartment(int departmentId, DepartmentRequest updateDepartment, string accesstoken);
    }
    public class DepartmentService(IDepartmentRepository departmentRepository, ILoggerService loggerService) : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly ILoggerService _loggerService = loggerService;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<DepartmentResponse?> CreateDepartment(DepartmentRequest newDepartment, string accesstoken)
        {
            Department? department = MapDepartmentRequestToDepartment(newDepartment);

            if (department != null)
            {
                Department? insertedDepartment = await _departmentRepository.InsertNewDepartment(department);

                if (insertedDepartment != null)
                {
                    _loggerService.WriteLog("Create", accesstoken, insertedDepartment);
                    return MapDepartmentToDepartmentResponse(insertedDepartment);
                }
            }
            return null;
        }

        public async Task<DepartmentResponse?> DeleteDepartment(int departmentId, string accesstoken)//transfer groups to another department??
        {
            Department? deletedDepartment = await _departmentRepository.DeleteDepartment(departmentId);

            if (deletedDepartment != null)
            {
                _loggerService.WriteLog("Delete", accesstoken, deletedDepartment);
                return MapDepartmentToDepartmentResponse(deletedDepartment);
            }
            return null;
        }

        public async Task<List<DepartmentResponse?>> GetAllDepartments()
        {
            List<Department> departments = await _departmentRepository.SelectAllDepartments();

            return departments.Select(department => MapDepartmentToDepartmentResponse(department)).ToList() ?? [];
        }

        public async Task<DepartmentResponse?> GetDepartmentById(int departmentId)
        {
            Department? department = await _departmentRepository.SelectDepartmentById(departmentId);
            if (department != null)
            {
                return MapDepartmentToDepartmentResponse(department);
            }
            return null;
        }

        public async Task<DepartmentResponse?> UpdateDepartment(int departmentId, DepartmentRequest updateDepartment, string accesstoken)
        {
            Department? originalDepartment = await _departmentRepository.SelectDepartmentById(departmentId);
            Department? department = MapDepartmentRequestToDepartment(updateDepartment);

            if (department != null)
            {
                Department? updatedDepartment = await _departmentRepository.UpdateDepartment(departmentId, department);

                if (updatedDepartment != null && originalDepartment != null)
                {
                    _loggerService.WriteLog(accesstoken, originalDepartment, updatedDepartment);
                    return MapDepartmentToDepartmentResponse(updatedDepartment);
                }
            }
            return null;
        }

        private Department? MapDepartmentRequestToDepartment(DepartmentRequest departmentRequest)
        {
            try
            {
                return new Department
                {
                    Name = departmentRequest.Name,
                    Deactivated = departmentRequest.Deactivated,
                    DepartmentHead = departmentRequest.DepartmentHead,
                    SchoolId = departmentRequest.SchoolId
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapDepartmentRequestToDepartment", e);
                return null;
            }
        }

        private DepartmentResponse? MapDepartmentToDepartmentResponse(Department department)//skal vi måske hente school op? der skla laves noget check på school....
        {
            try
            {
                department.School ??= new School()
                {
                    Name = "Error",
                };
                DepartmentResponse response = new()
                {
                    Id = department.Id,
                    Name = department.Name,
                    Deactivated = department.Deactivated,
                    DepartmentHead = department.DepartmentHead ?? "",
                    School = new DepartmentSchoolResponse
                    {
                        Id = department.School.Id,
                        Name = department.School.Name,
                        Deactivated = department.Deactivated,
                        Principal = department.School.Principal ?? ""
                    }
                };
                if (department.Groups != null)
                {
                    response.Groups = [.. department.Groups.Select(group => new DepartmentGroupResponse
                    {
                        Id = group.Id,
                        Name = group.Name,
                        ArrivalTime = group.ArrivalTime,
                    })];
                }
                return response;
            }
            catch (Exception e)
            {
                WriteToLog("MapDepartmentToDepartmentResponse", e);
                return null;
            }
        }
    }
}