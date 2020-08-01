using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DAL
{
    public class UserDAL: BaseDAL<User>, IUserDAL
    {
        private static BlogSystemEntities _Db = new BlogSystemEntities();

        public UserDAL(): base(_Db)
        {
        }
        public override bool DeleteEntity(User entity)
        {
            _Db.Entry<User>(entity).State = System.Data.Entity.EntityState.Unchanged;
            entity.UserIsRemoved = 1;
            return _Db.SaveChanges() > 0;
        }

        public User GetUserByEmail(string email)
        {
            return _Db.Set<User>().Where<User>(m => m.UserEmail == email).FirstOrDefault();
        }

        public User GetUserById(Guid userId)
        {
            return _Db.Set<User>().Where<User>(m => m.UserId == userId).FirstOrDefault();
        }
    }
}
