using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DAL
{
    public class ArticleDAL : BaseDAL<Article>, IArticleDAL
    {
        private static BlogSystemEntities _Db = new BlogSystemEntities();

        public ArticleDAL() : base(_Db)
        {
        }
        public override bool DeleteEntity(Article entity)
        {
            _Db.Entry<Article>(entity).State = System.Data.Entity.EntityState.Unchanged;
            entity.ArticleIsRemoved = 1;
            return _Db.SaveChanges() > 0;
        }

        public Article GetArticleById(Guid articleId)
        {
            return _Db.Set<Article>().Where<Article>(m => m.ArticleId == articleId).FirstOrDefault();
        }
    }
}
