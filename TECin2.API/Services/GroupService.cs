using TECin2.API.Database.Entities;
using TECin2.API.DTOs;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface IGroupService
    {
        Task<List<GroupResponse?>> GetAllGroups();
        Task<GroupResponse?> GetGroupById(int groupId);
        Task<GroupResponse?> CreateGroup(GroupRequest newGroup, string accessToken);
        Task<GroupResponse?> UpdateGroup(int groupId, GroupRequest updateGroup, string accessToken);
        Task<GroupResponse?> DeleteGroup(int deletingGroupId, int newGroupId, string accessToken);
    }
    public class GroupService(IGroupRepository groupRepository, IUserRepository userRepository, ILoggerService loggerService) : IGroupService
    {
        private readonly IGroupRepository _groupRepository = groupRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILoggerService _loggerService = loggerService;

        private void WriteToLog(string task, Exception e)
        {
            LoggerRepository.WriteLog("Error caught in " + this.GetType().Name + " in method " + task + ": " + e.InnerException + " " + e.Message);
        }

        public async Task<GroupResponse?> CreateGroup(GroupRequest newGroup, string accessToken)
        {
            Group? group = MapGroupRequestToGroup(newGroup);

            if (group != null)
            {
                Group? insertedGroup = await _groupRepository.InsertNewGroup(group);

                if (insertedGroup != null)
                {
                    _loggerService.WriteLog("Create", accessToken, insertedGroup);
                    return MapGroupToGroupResponse(insertedGroup);
                }
            }
            return null;
        }

        public async Task<GroupResponse?> DeleteGroup(int deletingGroupId, int newGroupId, string accessToken)
        {
            Group? groupToBeDeleted = await _groupRepository.SelectGroupById(deletingGroupId);
            if (groupToBeDeleted != null)
            {
                Group? newGroupForUsers = await _groupRepository.SelectGroupById(deletingGroupId);
                List<User> _users = [];

                if (groupToBeDeleted.Users != null)
                    _users = groupToBeDeleted.Users.ToList() ?? [];

                foreach (User user in _users)
                {
                    if (user.Groups != null && newGroupForUsers != null)
                    {
                        user.Groups.Add(newGroupForUsers);
                        user.Groups.Remove(groupToBeDeleted);
                        User? _user = await _userRepository.UpdateUser(user.Id, user);
                        if (_user == null)//skal have lavet noget lidt klogt med error handling
                        {
                            WriteToLog("!!Update user " + user.Id + " failed. See log for updateUser!!", new());
                        }
                    }
                }

                Group? deletedGroup = await _groupRepository.DeleteGroup(deletingGroupId);

                if (deletedGroup != null)
                {
                    _loggerService.WriteLog("Delete", accessToken, deletedGroup);
                    return MapGroupToGroupResponse(deletedGroup);
                }
            }
            return null;
        }

        public async Task<List<GroupResponse?>> GetAllGroups()
        {
            List<Group> groups = await _groupRepository.SelectAllGroups();

            return groups.Select(group => MapGroupToGroupResponse(group)).ToList() ?? [];
        }

        public async Task<GroupResponse?> GetGroupById(int groupId)
        {
            try
            {
                Group? group = await _groupRepository.SelectGroupById(groupId);
                if (group != null)
                    return MapGroupToGroupResponse(group);

                return null;
            }
            catch (Exception e)
            {
                WriteToLog("GetGroupById", e);
                return null;
            }
        }

        public async Task<GroupResponse?> UpdateGroup(int groupId, GroupRequest updateGroup, string accessToken)
        {
            Group? group = MapGroupRequestToGroup(updateGroup);

            if (group != null)
            {
                Group? updatedGroup = await _groupRepository.UpdateGroup(groupId, group);

                if (updatedGroup != null)
                {
                    return MapGroupToGroupResponse(updatedGroup);
                }
            }
            return null;
        }
        private Group? MapGroupRequestToGroup(GroupRequest groupRequest)
        {
            try
            {
                return new Group
                {
                    Name = groupRequest.Name,
                    Deactivated = groupRequest.Deactivated,
                    DepartmentId = groupRequest.DepartmentId,
                    ArrivalTime = groupRequest.ArrivalTime,
                    IsLateBuffer = groupRequest.IsLatebuffer,
                    IsLateMessage = groupRequest.IsLateMessage,
                    WorkHoursInDay = groupRequest.WorkHoursInDay,
                    FlexibleAmount = groupRequest.FlexibleAmount,
                    FlexibleArrivalEnabled = groupRequest.FlexibleArrivalEnabled,
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapGroupRequestToGroup", e);
                return null;
            }
        }

        private GroupResponse? MapGroupToGroupResponse(Group group)
        {
            try
            {
                return new GroupResponse
                {
                    Id = group.Id,
                    Name = group.Name,
                    Deactivated = group.Deactivated,
                    ArrivalTime = group.ArrivalTime,
                    IsLateBuffer = group.IsLateBuffer,
                    IsLateMessage = group.IsLateMessage,
                    WorkHoursInDay = group.WorkHoursInDay,
                    FlexibleAmount = group.FlexibleAmount,
                    FlexibleArrivalEnabled = group.FlexibleArrivalEnabled,
                    Department = new GroupDepartmentResponse
                    {
                        Id = group.Department.Id,
                        Name = group.Department.Name,
                        Deactivated = group.Department.Deactivated
                    }
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapGroupToGroupResponse", e);
                return null;
            }
        }
    }
}
