using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DAL
{
    public class ArticleToCategoryDAL: BaseDAL<ArticleToCategory>, IArticleToCategoryDAL
    {
        private static BlogSystemEntities _Db = new BlogSystemEntities();

        public ArticleToCategoryDAL() : base(_Db)
        {
        }
        public override bool DeleteEntity(ArticleToCategory entity)
        {
            _Db.Entry<ArticleToCategory>(entity).State = System.Data.Entity.EntityState.Unchanged;
            entity.ArticleToCategoryIsRemoved = 1;
            return _Db.SaveChanges() > 0;
        }

        public IQueryable<ArticleToCategory> GetCategoriesByArticleId(Guid articleId)
        {
            return _Db.Set<ArticleToCategory>().Where<ArticleToCategory>(m => m.ArticleToCategoryArticleId == articleId && m.ArticleToCategoryIsRemoved != 1);
        }
    }
}
