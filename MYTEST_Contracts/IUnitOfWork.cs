using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTEST_Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();

        bool HasTransaction { get; }

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        IGenericRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : class;
    }
}
