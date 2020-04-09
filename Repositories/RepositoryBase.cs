using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WiseInventionsDataLayer.ServiceContracts;

namespace WiseInventionsDataLayer.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DbContext _dbContext;
        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> FindAll() => _dbContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindPaginatedByConditionOrdered(int pageNumber, int entitiesPerPage, out int totalCount, Expression<Func<T, object>> include,
            Func<T, object> orderExpression = null, Expression<Func<T, bool>> queryExpression = null)
        {
            var queryable = _dbContext.Set<T>().AsQueryable().Include("CreatedBy");

            if (include != null)
                queryable = queryable.Include(include);

            if (queryExpression != null)
                queryable = queryable.Where(queryExpression);

            if (orderExpression != null)
                queryable = queryable.OrderByDescending(orderExpression).AsQueryable();

            totalCount = queryable.Count();

            return queryable.Skip(entitiesPerPage * (Convert.ToInt32(pageNumber) - 1)).Take(entitiesPerPage);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => _dbContext.Set<T>().Where(expression).AsNoTracking();
       
        public async Task Create(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Update(string id, T entity)
        {
            _dbContext.Entry<T>(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbContext.Entry<T>(entity).State = EntityState.Deleted;
        }
    }
}
