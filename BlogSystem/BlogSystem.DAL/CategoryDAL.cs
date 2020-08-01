using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DAL
{
    public class CategoryDAL: BaseDAL<Category>, ICategoryDAL
    {
        private static BlogSystemEntities _Db = new BlogSystemEntities();

        public CategoryDAL() : base(_Db)
        {
        }
        public override bool DeleteEntity(Category entity)
        {
            _Db.Entry<Category>(entity).State = System.Data.Entity.EntityState.Unchanged;
            entity.CategoryIsRemoved = 1;
            return _Db.SaveChanges() > 0;
        }

        public Category GetCategoryById(Guid categoryId)
        {
            return _Db.Set<Category>().Where<Category>(m => m.CategoryId == categoryId).FirstOrDefault();
        }

        public Category GetCategoryByName(string name)
        {
            return _Db.Set<Category>().Where<Category>(m => m.CategoryName == name).FirstOrDefault();
        }
    }
}
