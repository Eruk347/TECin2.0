using TECin2.API.Database.Entities;
using TECin2.API.Repositories;

namespace TECin2.API.Services
{
    public interface ILoggerService
    {
        void WriteLog(string _action, string _accessToken, Department _entity);
        void WriteLog(string _accessToken, Department _originalEntity, Department _updatedEntity);
        void WriteLog(string _action, string _accessToken, Group _entity);
        Task<bool> WriteLog(string _accessToken, Group _originalEntity, Group _updatedEntity);
        void WriteLog(string _action, string _accessToken, User _entity);
        void WriteLog(string _accessToken, User _originalEntity, User _updatedEntity);
        void WriteLog(string _action, string _accessToken, Role _entity);
        void WriteLog(string _accessToken, Role _originalEntity, Role _updatedEntity);
    }
    public class LoggerService(LoggerRepository loggerRepository) : ILoggerService
    {
        private readonly LoggerRepository _loggerRepository = loggerRepository;

        /// <summary>
        /// Department - create/delete
        /// </summary>
        /// <param name="_action"></param>
        /// <param name="_accessToken"></param>
        /// <param name="_entity"></param>
        public void WriteLog(string _action, string _accessToken, Department _entity)
        {
            string _message = "Department";
            if (_action == "Create")
            {
                _message += " " + _entity.Name + " was created.";
            }
            else if (_action == "Delete")
            {
                _message += " " + _entity.Name + " was deleted.";
            }

            WriteLog(_message, _accessToken);
        }

        /// <summary>
        /// Department - update
        /// </summary>
        /// <param name="_accessToken"></param>
        /// <param name="_originalEntity"></param>
        /// <param name="_updatedEntity"></param>
        public void WriteLog(string _accessToken, Department _originalEntity, Department _updatedEntity)
        {
            string _message = "Department";
            _message += " " + _originalEntity.Name;

            if (_originalEntity.Name != _updatedEntity.Name)
            {
                _message += ", changed its name to " + _updatedEntity.Name;
            }
            if (_originalEntity.DepartmentHead != _updatedEntity.DepartmentHead)
            {
                _message += ", changed its deparmenthead from " + _originalEntity.DepartmentHead + " to " + _updatedEntity.DepartmentHead;
            }

            WriteLog(_message, _accessToken);
        }

        /// <summary>
        /// Group - create/delete
        /// </summary>
        /// <param name="_action"></param>
        /// <param name="_accessToken"></param>
        /// <param name="_entity"></param>
        public void WriteLog(string _action, string _accessToken, Group _entity)
        {
            string _message = "Group";
            if (_action == "Create")
            {
                _message += " " + _entity.Name;
                _message += " was created in " + _entity.Department.Name;
            }
            else if (_action == "Delete")
            {
                _message += " " + _entity.Name;
                _message += " was deleted from " + _entity.Department.Name;
            }

            WriteLog(_message, _accessToken);
        }

        /// <summary>
        /// Group - update
        /// </summary>
        /// <param name="_accessToken"></param>
        /// <param name="_originalEntity"></param>
        /// <param name="_updatedEntity"></param>
        public async Task<bool> WriteLog(string _accessToken, Group _originalEntity, Group _updatedEntity)
        {
            string _message = "Group";
            _message += " " + _originalEntity.Name;

            if (_originalEntity.Name != _updatedEntity.Name)
            {
                _message += ", changed its name to " + _updatedEntity.Name;
            }
            if (_originalEntity.Department.Id != _updatedEntity.Department.Id)
            {
                _message += ", changed its department from " + _originalEntity.Department.Name + " to " + _updatedEntity.Department.Name;
            }
            if (_originalEntity.ArrivalTime != _updatedEntity.ArrivalTime)
            {
                _message += ", changed its arrivaltime from " + _originalEntity.ArrivalTime + " to " + _updatedEntity.ArrivalTime;
            }
            //if (_originalEntity.DepartureTime != _updatedEntity.DepartureTime)
            //{
            //    _message += ", changed its departuretime from " + _originalEntity.DepartureTime + " to " + _updatedEntity.DepartureTime;
            //}
            if (_originalEntity.IsLateBuffer != _updatedEntity.IsLateBuffer)
            {
                _message += ", changed its late buffer from " + _originalEntity.IsLateBuffer + " to " + _updatedEntity.IsLateBuffer;
            }
            if (_originalEntity.IsLateMessage != _updatedEntity.IsLateMessage)
            {
                _message += ", changed its late message from " + _originalEntity.IsLateMessage + " to " + _updatedEntity.IsLateMessage;
            }

            await WriteLog(_message, _accessToken);
            return true;
        }

        /// <summary>
        /// Instruktor and student - create/delete
        /// </summary>
        /// <param name="_action"></param>
        /// <param name="_accessToken"></param>
        /// <param name="_entity"></param>
        public void WriteLog(string _action, string _accessToken, User _entity)
        {
            string _message;
            if (_entity.IsStudent)
                _message = "Instruktor";
            else
                _message = "Student";

            if (_action == "Create")
            {
                _message += " " + _entity.Username;
                //_message += " was created in " + _entity.Group.Name;
            }
            else if (_action == "Delete")
            {
                _message += " " + _entity.Username;
                _message += " was deleted";
            }

            WriteLog(_message, _accessToken);
        }

        /// <summary>
        /// Instruktor and student - update
        /// </summary>
        /// <param name="_accessToken"></param>
        /// <param name="_originalEntity"></param>
        /// <param name="_updatedEntity"></param>
        public void WriteLog(string _accessToken, User _originalEntity, User _updatedEntity)
        {
            string _message;
            if (_originalEntity.IsStudent)
                _message = "Instruktor";
            else
                _message = "Student";

            _message += " " + _originalEntity.Username;

            if (_originalEntity.FirstName != _updatedEntity.FirstName)
            {
                _message += ", changed its firstname from " + _originalEntity.FirstName + " to " + _updatedEntity.FirstName;
            }
            if (_originalEntity.LastName != _updatedEntity.LastName)
            {
                _message += ", changed its lastname from " + _originalEntity.LastName + " to " + _updatedEntity.LastName;
            }
            if (_originalEntity.Phonenumber != _updatedEntity.Phonenumber)
            {
                _message += ", changed its Phonenumber from " + _originalEntity.Phonenumber + " to " + _updatedEntity.Phonenumber;
            }
            if (_originalEntity.Email != _updatedEntity.Email)
            {
                _message += ", changed its Email from " + _originalEntity.Email + " to " + _updatedEntity.Email;
            }
            //if (_originalEntity.Group.Id != _updatedEntity.Group.Id)
            //{
            //    _message += ", has moved group from " + _originalEntity.Group.Name + " to " + _updatedEntity.Group.Name;
            //}
            if (_originalEntity.Role.Id != _updatedEntity.Role.Id)
            {
                _message += ", has changed role from " + _originalEntity.Role.Name + " to " + _updatedEntity.Role.Name;
            }
            if (_originalEntity.LastCheckin != _updatedEntity.LastCheckin)
            {
                _message += ", changed its lastcheckin from " + _originalEntity.LastCheckin + " to " + _updatedEntity.LastCheckin;
            }

            WriteLog(_message, _accessToken);
        }

        /// <summary>
        /// Role - Create/delete
        /// </summary>
        /// <param name="_action"></param>
        /// <param name="_accessToken"></param>
        /// <param name="_entity"></param>
        public void WriteLog(string _action, string _accessToken, Role _entity)
        {
            string _message = "Role";
            if (_action == "Create")
            {
                _message += " " + _entity.Name;
                _message += " was created with the rank: " + _entity.Rank + " and description: " + _entity.Description;
            }
            else if (_action == "Delete")
            {
                _message += " " + _entity.Name;
                _message += " was deleted";
            }

            WriteLog(_message, _accessToken);
        }

        /// <summary>
        /// Role - Update
        /// </summary>
        /// <param name="_accessToken"></param>
        /// <param name="_originalEntity"></param>
        /// <param name="_updatedEntity"></param>
        public void WriteLog(string _accessToken, Role _originalEntity, Role _updatedEntity)
        {
            string _message = "Role";
            _message += " " + _originalEntity.Name;

            if (_originalEntity.Name != _updatedEntity.Name)
            {
                _message += ", changed its name to " + _updatedEntity.Name;
            }
            if (_originalEntity.Rank != _updatedEntity.Rank)
            {
                _message += ", changed its rank from " + _originalEntity.Rank + " to " + _updatedEntity.Rank;
            }
            if (_originalEntity.Description != _updatedEntity.Description)
            {
                _message += ", changed its Description from " + _originalEntity.Description + " to " + _updatedEntity.Description;
            }
            WriteLog(_message, _accessToken);
        }


        private async Task<bool> WriteLog(string _message, string _accessToken)
        {
            Log log = new()
            {
                DateAndTime = DateTime.Now,
                Message = _message,
                User = GetUserIdFromAccessToken(_accessToken)
            };

            await _loggerRepository.WriteLog(log);
            return true;
        }

        private static string GetUserIdFromAccessToken(string _accessToken)
        {
            foreach (var item in Global.Tokens)
            {
                if (item.Split(',')[0] == _accessToken)
                    return item.Split(',')[1];
            }
            return string.Empty;
        }
    }
}
