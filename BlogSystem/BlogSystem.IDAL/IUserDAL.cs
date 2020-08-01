using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.IDAL
{
    public interface IUserDAL: IBaseDAL<User>
    {
        User GetUserByEmail(string email);
        User GetUserById(Guid userId);
    }
}
