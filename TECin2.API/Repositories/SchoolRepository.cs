using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.API.Database.Entities;

namespace TECin2.API.Repositories
{
    public interface ISchoolRepository
    {
        Task<School?> DeleteSchool(int schoolId);
        Task<School?> InsertNewSchool(School school);
        Task<List<School>> SelectAllSchools();
        Task<School?> SelectSchoolById(int schoolId);
        Task<School?> SelectSchoolByName(string schoolName);
        Task<School?> UpdateSchool(int schoolId, School school);
    }
    public class SchoolRepository(TECinContext context) : ISchoolRepository
    {
        private readonly TECinContext _context = context;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<School?> DeleteSchool(int schoolId)
        {
            try
            {
                School? deletedSchool = await _context.School.
                    FirstOrDefaultAsync(school => school.Id == schoolId);
                if (deletedSchool != null)
                {
                    _context.School.Remove(deletedSchool);
                    await _context.SaveChangesAsync();
                }
                return deletedSchool;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteSchool", e);
                return null;
            }
        }

        public async Task<School?> InsertNewSchool(School school)
        {
            try
            {
                _context.School.Add(school);
                await _context.SaveChangesAsync();
                return school;
            }
            catch (Exception e)
            {
                WriteToLog("InsertNewSchool", e);
                return null;
            }
        }

        public async Task<List<School>> SelectAllSchools()
        {
            try
            {
                return await _context.School.ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectAllSchools", e);
                return [];
            }
        }

        public async Task<School?> SelectSchoolById(int schoolId)
        {
            try
            {
                return await _context.School
                    .Include(s => s.Departments)
                    .FirstOrDefaultAsync(school => school.Id == schoolId);
            }
            catch (Exception e)
            {
                WriteToLog("SelectSchoolById", e);
                return null;
            }
        }

        public async Task<School?> SelectSchoolByName(string schoolName)
        {
            try
            {
                return await _context.School
                    .Include(s => s.Departments)
                    .FirstOrDefaultAsync(school => school.Name == schoolName);
            }
            catch (Exception e)
            {
                WriteToLog("SelectSchoolByName", e);
                return null;
            }
        }

        public async Task<School?> UpdateSchool(int schoolId, School school)
        {
            try
            {
                School? updatedSchool = await _context.School
                    .Include(s => s.Departments)
                    .FirstOrDefaultAsync(school => school.Id == schoolId);
                if (updatedSchool != null)
                {
                    updatedSchool.Name = school.Name;
                    updatedSchool.Departments = school.Departments;
                    updatedSchool.Principal = school.Principal;
                    updatedSchool.Deactivated = school.Deactivated;
                    await _context.SaveChangesAsync();
                }
                School? returnSchool = await _context.School
                    .Include(s => s.Departments)
                    .FirstOrDefaultAsync(school => school.Id == schoolId);

                return returnSchool;
            }
            catch (Exception e)
            {
                WriteToLog("UpdateSchool", e);
                return null;
            }
        }
    }
}
