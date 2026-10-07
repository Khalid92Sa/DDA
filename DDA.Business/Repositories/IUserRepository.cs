using DDA.DAL;
using SSSFramework;
using SSSFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDA.Business.Repositories
{
    public interface IUserRepository : IGenericRepository
    {
        UserDTO GetUserByUsername(string username);
        List<GroupPermission> GetAllGroupPermissions();
        UserDTO GetUserById(int id);
        List<UserDTO> GetAllUsersOfGroupCode(string groupCode);
    }
}
