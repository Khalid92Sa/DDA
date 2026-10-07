using SSSFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDA.Business.Interfaces
{
    public interface IUserService
    {
        UserDTO GetUserByUsername(string username);
        bool CheckCurrentUserPermissions(string username, params int[] permissionCode);
    }
}
