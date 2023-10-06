using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MYTEST_Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        ValueTask<T?> GetByIdAsync(int id);

        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

        void Add(T entity);

        void Delete(T entity);
        void Update(T entity);

    }
}
