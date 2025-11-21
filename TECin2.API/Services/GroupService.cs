using TECin2.ClassLibrary.Entities;
using TECin2.ClassLibrary.DTOs;
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
    public class GroupService(IGroupRepository groupRepository, IUserRepository userRepository, ILoggerService loggerService, IWorkHoursInDayRepository workHoursInDayRepository) : IGroupService
    {
        private readonly IGroupRepository _groupRepository = groupRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILoggerService _loggerService = loggerService;
        private readonly IWorkHoursInDayRepository _workHoursInDayRepository = workHoursInDayRepository;

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
                    await _loggerService.WriteLog("Create", accessToken, insertedGroup);
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
                    await _workHoursInDayRepository.DeleteWorkHoursInDay(deletedGroup.WorkHoursInDayId);
                    await _loggerService.WriteLog("Delete", accessToken, deletedGroup);
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
                Group answer =new Group
                {
                    Name = groupRequest.Name,
                    Deactivated = groupRequest.Deactivated,
                    DepartmentId = groupRequest.DepartmentId,
                    ArrivalTime = groupRequest.ArrivalTime,
                    IsLateMessagingEnabled = groupRequest.IsLateMessageEnabled,
                    IsLateBuffer = groupRequest.IsLatebuffer,
                    IsLateMessage = groupRequest.IsLateMessage ?? "",
                    CheckoutRequired = groupRequest.CheckoutRequired,
                    WorkHoursInDay = groupRequest.WorkHoursInDay,
                    FlexibleAmount = groupRequest.FlexibleAmount,
                    FlexibleArrivalEnabled = groupRequest.FlexibleArrivalEnabled,
                };
                if (!answer.CheckoutRequired)
                    answer.WorkHoursInDayId = 2;//kræver at der er oprettet en WorkHoursInDay 
                return answer;
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
                if (group.Department == null)
                {
                    return null;
                }
                return new GroupResponse
                {
                    Id = group.Id,
                    Name = group.Name,
                    Deactivated = group.Deactivated,
                    ArrivalTime = group.ArrivalTime,
                    IsLateMessageEnabled = group.IsLateMessagingEnabled,
                    IsLateBuffer = group.IsLateBuffer,
                    IsLateMessage = group.IsLateMessage,
                    CheckoutRequired = group.CheckoutRequired,
                    WorkHoursInDay = group.WorkHoursInDay,
                    FlexibleAmount = group.FlexibleAmount,
                    FlexibleArrivalEnabled = group.FlexibleArrivalEnabled,
                    Department = new GroupDepartmentResponse
                    {
                        Id = group.Department.Id,
                        Name = group.Department.Name,
                        Deactivated = group.Department.Deactivated
                    },
                    Students = group.Users!
                    .Where(u => u.IsStudent)
                    .OrderBy(u => u.FirstName)
                    .Select(u => MapUserToGroupUserResponse(u))
                    .ToList() ?? [],

                    Instructors = group.Users!
                    .Where(u => u.IsStudent == false)
                    .OrderBy(u => u.FirstName)
                    .Select(u => MapUserToGroupUserResponse(u))
                    .ToList() ?? []
                };
            }
            catch (Exception e)
            {
                WriteToLog("MapGroupToGroupResponse", e);
                return null;
            }
        }

        private GroupUsersResponse? MapUserToGroupUserResponse(User user)
        {
            try
            {
                if (user == null)
                    return null;

                return new GroupUsersResponse
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Deactivated = user.Deactivated,
                    Email = user.Email,
                    Phonenumber = user.Phonenumber,
                    LastCheckin = user.LastCheckin,
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
