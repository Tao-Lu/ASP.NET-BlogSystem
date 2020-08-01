using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DAL
{
    public abstract class BaseDAL<T>: IBaseDAL<T> where T: class, new()
    {
        private readonly BlogSystemEntities _Db;
        public BaseDAL(BlogSystemEntities Db)
        {
            _Db = Db;
        }

        public bool CreateEntity(T entity)
        {
            _Db.Set<T>().Add(entity);
            return _Db.SaveChanges() > 0;
        }

        public abstract bool DeleteEntity(T entity);
        //{
            //_Db.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
            //return _Db.SaveChanges() > 0;
        //}

        public bool EditEntity(T entity)
        {
            _Db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return _Db.SaveChanges() > 0;
        }

        
        public IQueryable<T> GetEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return _Db.Set<T>().Where<T>(whereLambda);
        }

        
        public IQueryable<T> GetPageEntitiesOrdered<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderByLambda, bool isAsc)
        {
            var temp = _Db.Set<T>().Where<T>(whereLambda);
            totalCount = temp.Count();

            // order in Asc
            if (isAsc)
            {
                temp = temp.OrderBy<T, s>(orderByLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            // order in Desc
            else
            {
                temp = temp.OrderByDescending<T, s>(orderByLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }

            return temp;
        }
    }
}
