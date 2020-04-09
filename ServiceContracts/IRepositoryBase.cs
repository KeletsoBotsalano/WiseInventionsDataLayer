using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WiseInventionsDataLayer.ServiceContracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindPaginatedByConditionOrdered(int pageNumber, int entitiesPerPage, out int totalCount, Expression<Func<T, object>> include, Func<T, object> orderExpression = null, Expression<Func<T, bool>> queryExpression = null);
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
