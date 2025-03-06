using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.API.Database.Entities;

namespace TECin2.API.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department?> DeleteDepartment(int departmentId);
        Task<Department?> InsertNewDepartment(Department department);
        Task<List<Department>> SelectAllDepartments();
        Task<Department?> SelectDepartmentById(int departmentId);
        Task<Department?> SelectDepartmentByName(string departmentName);
        Task<Department?> UpdateDepartment(int departmentId, Department department);

    }
    public class DepartmentRepository(TECinContext context) : IDepartmentRepository
    {
        private readonly TECinContext _context = context;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<Department?> DeleteDepartment(int departmentId)
        {
            try
            {
                Department? deletedDepartment = await _context.Department.
                    FirstOrDefaultAsync(department => department.Id == departmentId);
                if (deletedDepartment != null)
                {
                    _context.Department.Remove(deletedDepartment);
                    await _context.SaveChangesAsync();
                }
                return deletedDepartment;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteDepartment", e);
                return null;
            }
        }

        public async Task<Department?> InsertNewDepartment(Department department)
        {
            try
            {
                _context.Department.Add(department);
                await _context.SaveChangesAsync();
                return _context.Department
                    .Include(a => a.Groups)
                    .Include(d => d.School)
                    .FirstOrDefault(department => department.Name == department.Name);
            }
            catch (Exception e)
            {
                WriteToLog("InsertNewDepartment", e);
                return null;
            }
        }

        public async Task<List<Department>> SelectAllDepartments()
        {
            try
            {
                return await _context.Department
                    .Include(d => d.School)
                    .Include(a => a.Groups)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectAllDepartments", e);
                return [];
            }
        }

        public async Task<Department?> SelectDepartmentById(int departmentId)
        {
            try
            {
                return await _context.Department
                    .Include(a => a.Groups)
                    .Include(d => d.School)
                    .FirstOrDefaultAsync(department => department.Id == departmentId);
            }
            catch (Exception e)
            {
                WriteToLog("SelectDepartmentById", e);
                return null;
            }
        }

        public async Task<Department?> SelectDepartmentByName(string departmentName)
        {
            try
            {
                return await _context.Department
                    .Include(a => a.Groups)
                    .FirstOrDefaultAsync(department => department.Name == departmentName);
            }
            catch (Exception e)
            {
                WriteToLog("SelectDepartmentById", e);
                return null;
            }
        }

        public async Task<Department?> UpdateDepartment(int departmentId, Department department)
        {
            try
            {
                Department? updatedDepartment = await _context.Department.FirstOrDefaultAsync(department => department.Id == departmentId);
                if (updatedDepartment != null)
                {
                    updatedDepartment.Name = department.Name;
                    updatedDepartment.Deactivated = department.Deactivated;
                    updatedDepartment.DepartmentHead = department.DepartmentHead;
                    updatedDepartment.SchoolId = department.SchoolId;
                    await _context.SaveChangesAsync();
                }
                return updatedDepartment;
            }
            catch (Exception e)
            {
                WriteToLog("UpdateDepartment", e);
                return null;
            }
        }
    }
}
