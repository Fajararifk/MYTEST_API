using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MYTEST_Contracts;

namespace MYTEST_DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly IAuditingService _auditingService;
        private IDbContextTransaction? _transaction;
        private bool disposedValue;

        public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger, IAuditingService auditingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
            _auditingService = auditingService;
        }

        public bool HasTransaction => _transaction != null;

        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _context.Database.BeginTransaction();
            }
            else
            {
                _logger.LogWarning("A transaction already exists issue at {stacktrace}", StackTraceHelper.GetText(new StackTrace()));
            }
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
            else
            {
                throw new InvalidOperationException("A commit has been called without transaction");
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
            else
            {
                throw new InvalidOperationException("A rollback has been called without transaction");
            }
        }

        //load a context automatically
        // the context is disposed when this UOW obejct is disposed > at end of webrequest (simple injector weblifestyle request)

        public IGenericRepository<TEntity> GetGenericRepository<TEntity>()
            where TEntity : class
        {
            return new GenericRepository<TEntity>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            var updatedEntries = _context.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached).ToArray();
            foreach (var entry in updatedEntries)
            {
                _auditingService.Audit(entry);
            }

            return await _context.SaveChangesAsync();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                if (_transaction != null)
                {
                    _logger.LogError("A transaction has not been commited issue on {stacktrace}", StackTraceHelper.GetText(new StackTrace()));
                    _transaction.Dispose();
                }
                _context.Dispose();
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
