using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.IDAL
{
    public interface IBaseDAL<T> where T: class, new()
    {
        // create
        bool CreateEntity(T entity);
        
        // delete
        bool DeleteEntity(T entity);
        
        // edit
        bool EditEntity(T entity);
        
        // get
        IQueryable<T> GetEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);

        // paging
        IQueryable<T> GetPageEntitiesOrdered<s> (int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderByLambda, bool isAsc);
    }
}
