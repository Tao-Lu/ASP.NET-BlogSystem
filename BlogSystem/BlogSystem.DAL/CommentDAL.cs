using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DAL
{
    public class CommentDAL : BaseDAL<Comment>, ICommentDAl
    {
        private static BlogSystemEntities _Db = new BlogSystemEntities();

        public CommentDAL() : base(_Db)
        {
        }

        public override bool DeleteEntity(Comment entity)
        {
            _Db.Entry<Comment>(entity).State = System.Data.Entity.EntityState.Unchanged;
            entity.CommentIsRemoved = 1;
            return _Db.SaveChanges() > 0;
        }

        public Comment GetCommentByArticleId(Guid articleId)
        {
            return _Db.Set<Comment>().Where<Comment>(m => m.CommentArticleId == articleId).FirstOrDefault();
        }
    }
}
