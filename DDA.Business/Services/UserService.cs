using DDA.Business.Interfaces;
using DDA.Business.Repositories;
using DDA.DAL;
using DDA.ViewModels.Enum;
using DDA.ViewModels.Users;
using SSSFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDA.Business.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRep;
        public UserService(IUserRepository userRep)
        {
            _userRep = userRep;
        }

        public List<UserViewModel> GetAll()
        {
            return _userRep.All<User>().Where(x => x.IsActive).ToList()
                .Select(o => new UserViewModel
                {
                    Id = o.Id,
                    ArabicName = o.NameAR,
                    EnglishName = o.NameEN,
                    Email = o.EMail,
                    UserName = o.UserName,
                    GroupIds = o.UserGroups.Select(x => x.Id).ToList()
                }).ToList();
        }

        public UserViewModel GetUserById(int id)
        {
            var user = _userRep.Find<User>(id);
            if (user == null) return null;
            return new UserViewModel
            {
                Id = user.Id,
                ArabicName = user.NameAR,
                EnglishName = user.NameEN,
                Email = user.EMail,
                UserName = user.UserName,
                GroupIds = user.UserGroups.Select(x => x.Id).ToList()
            };
        }

        public bool CheckCurrentUserPermissions(string username, params int[] permissionCode)
        {
            if (string.IsNullOrEmpty(username)) return false;
            var user = GetUserDetailsByName(username);
            if (user == null) return false;

            foreach (var groupId in user.GroupIds)
            {
                var permissions = _userRep.GetAllGroupPermissions()
                     .Where(r => r.GroupId == groupId)
                     .Select(rp => rp.Permission.Code)
                     .ToList();

                if (permissions.Any(permissionCode.Contains)) return true;
            }

            return false;
        }

        public UserViewModel GetUserDetailsByName(string name)
        {
            return _userRep.All<User>().Where(user => user.UserName == name && user.IsActive).Select(
                    user => new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        GroupIds = user.UserGroups.Select(x => x.GroupId).ToList(),
                    }).SingleOrDefault();
        }

        public UserDTO GetUserByUsername(string username)
        {
            return _userRep.GetUserByUsername(username);
        }

        public List<int> GetUserPermissions()
        {
            var currentUser = ((UserIdentity)System.Threading.Thread.CurrentPrincipal.Identity).User;

            List<int> groupIds = currentUser.GroupsIds.ToList();
            List<int> permissionCodes = new List<int>();
            foreach (var id in groupIds)
            {
                var permissions = _userRep.GetAllGroupPermissions()
                      .Where(r => r.GroupId == id)
                      .Select(rp => rp.Permission.Code)
                      .ToList();
                foreach (var permissionCode in permissions)
                {
                    permissionCodes.Add(permissionCode);
                }

            }
            return permissionCodes;
        }

        public List<int> GetUserPermissionsForReportViewer()
        {
            var currentUser = System.Threading.Thread.CurrentPrincipal.Identity;
            var permissionCodes = new List<int>();

            if (!string.IsNullOrEmpty(currentUser.Name))
            {
                var userDto = GetUserByUsername(currentUser.Name);

                List<int> groupIds = userDto.GroupIds.ToList();
                foreach (var id in groupIds)
                {
                    var permissions = _userRep.GetAllGroupPermissions()
                          .Where(r => r.GroupId == id)
                          .Select(rp => rp.Permission.Code)
                          .ToList();
                    foreach (var permissionCode in permissions)
                    {
                        permissionCodes.Add(permissionCode);
                    }

                }
            }

            return permissionCodes;
        }

        public List<int> GetPermissionsCodesByGroupId(int groupId)
        {
            var permissions = _userRep.GetAllGroupPermissions()
                    .Where(r => r.GroupId == groupId)
                    .Select(rp => rp.Permission.Code)
                    .ToList();
            return permissions;
        }

        public List<int> GetGroupCodesByUserId(int userId)
        {
            User user = _userRep.Find<User>(userId);
            List<int> codes = user.UserGroups.Select(c => Convert.ToInt32(c.Group.Code)).ToList();
            return codes;
        }

        public List<LookupDTO> GetAllUsersAsDTO()
        {
            bool isArabic = CultureHelper.IsArabic;
            return GetAll().Select(l => new LookupDTO
            {
                Text = isArabic ? string.IsNullOrEmpty(l.ArabicName) ? l.EnglishName : l.ArabicName : string.IsNullOrEmpty(l.EnglishName) ? l.ArabicName : l.EnglishName,
                Value = l.Id
            }).OrderBy(l => l.Text).ToList();
        }

        public UserDTO GetSignalRUserByUserName(string username)
        {
            return _userRep.GetUserByUsername(username);
        }

        public List<UserViewModel> GetUsersByGroupCode(string groupCode)
        {
            var users = new List<UserViewModel>();
            var usersIDs = _userRep.All<UserGroup>()
                .Where(x => x.Group.Code == groupCode)
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    ArabicName = user.User.NameAR,
                    EnglishName = user.User.NameEN,
                    Email = user.User.EMail,
                    UserName = user.User.UserName,
                    GroupIds = user.User.UserGroups.Select(x => x.Id).ToList()
                });

            return users;
        }

        public List<UserViewModel> GetUsersByGroupId(int groupId)
        {
            return _userRep.All<UserGroup>()
                .Where(x => x.GroupId == groupId)
                .Select(o => new UserViewModel
                {
                    Id = o.Id,
                    ArabicName = o.User.NameAR,
                    EnglishName = o.User.NameEN,
                    Email = o.User.EMail,
                    UserName = o.User.UserName,
                    GroupIds = o.User.UserGroups.Select(x => x.Id).ToList()
                }).ToList();
        }

        public List<UserInfoViewModel> GetUsersInfoByGroupId(int groupId)
        {
            return _userRep.All<UserGroup>()
                .Where(x => x.GroupId == groupId)
                .Select(o => new UserInfoViewModel
                {
                    Id = o.Id,
                    ArabicName = o.User.NameAR,
                    EnglishName = o.User.NameEN,
                    Email = o.User.EMail,
                    UserName = o.User.UserName,
                    GroupIds = o.User.UserGroups.Select(x => x.Id).ToList(),
                    DeliverEmail = false,
                    DeliverPushNotification = false
                }).ToList();
        }

        public UserInfoViewModel GetUserInfoById(int id)
        {
            var user = _userRep.Find<User>(id);
            if (user == null) return null;
            return new UserInfoViewModel
            {
                Id = user.Id,
                ArabicName = user.NameAR,
                EnglishName = user.NameEN,
                Email = user.EMail,
                UserName = user.UserName,
                GroupIds = user.UserGroups.Select(x => x.Id).ToList(),
                DeliverEmail = false,
                DeliverPushNotification = false
            };
        }

        public List<int> GetGroupsIdsByGroupsCodes(List<string> GroupsCodes)
        {
            var IDs = _userRep.All<Group>()
               .Where(x => GroupsCodes.Contains(x.Code))
               .Select(x => x.Id);
            if (IDs.Count() != 0)
                return IDs.ToList();
            else return new List<int>();
        }
        public int GetGroupsIdByGroupsCode(int GroupCode)
        {
            var ID = _userRep.All<Group>()
               .Where(x => GroupCode.ToString() == x.Code)
               .Select(x => x.Id).FirstOrDefault();
            return ID;

        }

        public int GetTheFirstAdminId()
        {
            var IDs = _userRep.All<UserGroup>()
                .Where(x => x.Group.Code == ((int)Groups.Admin).ToString())
                .Select(x => x.UserId);

            if (IDs.Count() != 0)
                return IDs.ToList().FirstOrDefault();

            return 0;
        }

        public string GetGroupNameById(int groupId)
        {
            Group neededGroup = _userRep.Find<Group>(groupId);
            return neededGroup != null ? neededGroup.Name : "";
        }

        public List<int> GetAllGroupsIds()
        {
            return _userRep.All<Group>().Select(g => g.Id).ToList();
        }

    }
}
