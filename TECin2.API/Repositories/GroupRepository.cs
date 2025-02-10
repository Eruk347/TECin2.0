using Microsoft.EntityFrameworkCore;
using TECin2.API.Database;
using TECin2.API.Database.Entities;

namespace TECin2.API.Repositories
{
    public interface IGroupRepository
    {

        Task<List<Group>?> SelectAllGroups();
        Task<Group?> SelectGroupById(int groupId);
        Task<Group?> InsertNewGroup(Group group);
        Task<Group?> UpdateGroup(int groupId, Group group);
        Task<Group?> DeleteGroup(int groupId);
    }
    public class GroupRepository(TECinContext context) : IGroupRepository
    {
        private readonly TECinContext _context = context;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<Group?> DeleteGroup(int groupId)
        {
            try
            {
                Group? deletedGroup = await _context.Group
                    .Include(g => g.Department)
                    .Include(g=>g.DepartureTimes)
                    .FirstOrDefaultAsync(group => group.Id == groupId);
                if (deletedGroup != null)
                {
                    _context.Group.Remove(deletedGroup);
                    await _context.SaveChangesAsync();
                }

                return deletedGroup;
            }
            catch (Exception e)
            {
                WriteToLog("DeleteGroup", e);
                return null;
            }
        }

        public async Task<Group?> SelectGroupById(int groupId)
        {
            try
            {
                return await _context.Group
                    .Include(g => g.Department)
                    .Include(g => g.DepartureTimes)
                    .Include(u => u.Users)
                    .FirstOrDefaultAsync(group => group.Id == groupId);
            }
            catch (Exception e)
            {
                WriteToLog("SelectGroupById", e);
                return null;
            }
        }

        public async Task<Group?> InsertNewGroup(Group group)
        {
            try
            {
                _context.Group.Add(group);
                await _context.SaveChangesAsync();
                return await _context.Group
                    .Include(g => g.Department)
                    .Include(g => g.DepartureTimes)
                    .FirstOrDefaultAsync(group => group.Id == group.Id);
            }
            catch (Exception e)
            {
                WriteToLog("InsertNewGroup", e);
                return null;
            }
        }

        public async Task<List<Group>?> SelectAllGroups()
        {
            try
            {
                return await _context.Group
                    .Include(g => g.Department)
                    .Include(g => g.DepartureTimes)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                WriteToLog("SelectAllGroups", e);
                return null;
            }
        }

        public async Task<Group?> UpdateGroup(int groupId, Group group)
        {
            try
            {
                Group? updatedGroup = await _context.Group
                    .Include(g => g.Department)
                    .Include(g => g.DepartureTimes)
                    .FirstOrDefaultAsync(group => group.Id == groupId);
                if (updatedGroup != null)
                {
                    updatedGroup.Name = group.Name;
                    updatedGroup.DepartmentId = group.DepartmentId;
                    updatedGroup.ArrivalTime = group.ArrivalTime;
                    updatedGroup.IsLateBuffer = group.IsLateBuffer;
                    updatedGroup.IsLateMessage = group.IsLateMessage;
                    updatedGroup.DepartureTimes = group.DepartureTimes;
                    updatedGroup.Deactivated = group.Deactivated;
                    updatedGroup.FlexibleArrival = group.FlexibleArrival;
                    updatedGroup.FlexibleTime = group.FlexibleTime;
                    await _context.SaveChangesAsync();
                }

                Group? returnGroup = await _context.Group
                    .Include(g => g.Department)
                    .Include(g => g.DepartureTimes)
                    .FirstOrDefaultAsync(group => group.Id == groupId);

                return returnGroup;
            }
            catch (Exception e)
            {
                WriteToLog("UpdateGroup", e);
                return null;
            }
        }

        //public async Task<List<User>?> SelectAllUsersFromGroup(int groupId)
        //{
        //    try
        //    {
        //        List<User> usersInGroup = await _context.User
        //            .Where()
        //            .SelectMany(c => c.)
        //            .Include(r => r.Role)
        //            .ToListAsync();

        //        return usersInGroup;
        //    }
        //    catch (Exception e)
        //    {
        //        WriteToLog("SelectAllUsersFromGroup", e);
        //        return null;
        //    }
        //}
    }
}
