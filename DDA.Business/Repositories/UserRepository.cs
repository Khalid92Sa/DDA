using DDA.DAL;
using SSSFramework;
using SSSFramework.Cache;
using SSSFramework.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DDA.Business.Repositories
{
    public class UserRepository : GenericRepository<DDAEntities>, IUserRepository
    {
        public UserRepository(IUnitOfWork<DDAEntities> unitOfWork)
           : base(unitOfWork)
        {

        }

        public UserDTO GetUserByUsername(string username)
        {
            var cacheKey = string.Format("{0}UserCache", username);
            var cacheUser = CachingHelper.GetItem(cacheKey) as UserDTO;

            if (cacheUser == null)
            {
                var user = _context.Users.Include(x => x.UserGroups.Select(r => r.Group))
                           .FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());

                if (user != null)
                {
                    cacheUser = new UserDTO
                    {
                        Id = user.Id,
                        ArabicName = user.NameAR,
                        EnglishName = user.NameEN,
                        Username = user.UserName,
                        Email = user.EMail,
                        GroupIds = user.UserGroups.Select(x => x.GroupId).ToList()
                    };

                    CachingHelper.Insert(cacheKey, cacheUser, TimeSpan.FromMinutes(15).TotalSeconds);
                }
            }

            return cacheUser;
        }

        public List<GroupPermission> GetAllGroupPermissions()
        {
            return _context.GroupPermissions.ToList();
        }

        public UserDTO GetUserById(int id)
        {
            var userDto = new UserDTO();
            var user = _context.Users.Find(id);
            if (user != null)
            {
                userDto.Id = user.Id;
                userDto.ArabicName = user.NameAR;
                userDto.EnglishName = user.NameEN;
                userDto.Email = user.EMail;
                userDto.Username = user.UserName;
            }
            return userDto;
        }

        public List<UserDTO> GetAllUsersOfGroupCode(string groupCode)
        {
            var usersList = new List<UserDTO>();
            var group = _context.Groups.FirstOrDefault(x => x.Code == groupCode);
            if (group != null)
            {
                var u = group.UserGroups.Select(e => e.User).ToList();

                foreach (var item in u)
                {
                    usersList.Add(new UserDTO
                    {
                        Id = item.Id,
                        ArabicName = item.NameAR,
                        EnglishName = item.NameEN,
                        Email = item.EMail,
                        Username = item.UserName
                    });
                }
            }

            return usersList;
        }
    }
}
