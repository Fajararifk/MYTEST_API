using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MYTEST_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MYTEST_BusinessObjects;

namespace MYTEST_DAL
{
    public class AuditingService : IAuditingService
    {
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly ILogger<AuditingService> _logger;

        public AuditingService(IAuthenticationHelper authenticationHelper, ILogger<AuditingService> logger)
        {
            _authenticationHelper = authenticationHelper;
            _logger = logger;
        }

        public void Audit(EntityEntry entry)
        {
            if (entry.Entity is AuditedEntity auditedEntity)
            {
                var userId = _authenticationHelper.GetConnectedUserId();
                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            auditedEntity.CreatedAt = DateTime.Now;
                            _logger.LogTrace("An update has been done", new { entry.CurrentValues, entry.OriginalValues });
                            break;
                        }
                    case EntityState.Deleted:
                        {
                            auditedEntity.DeletedAt = DateTime.Now;
                            entry.State = EntityState.Modified;
                            _logger.LogTrace("A soft delete has been done", new { entry.CurrentValues });
                            break;
                        }
                    case EntityState.Added:
                        {
                            auditedEntity.CreatedAt = DateTime.Now;
                            _logger.LogTrace("A new entry has been created", new { entry.CurrentValues });
                            break;
                        }
                }
            }
            else
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            _logger.LogTrace("An update has been done", new { entry.CurrentValues, entry.OriginalValues });
                            break;
                        }
                    case EntityState.Deleted:
                        {
                            _logger.LogTrace("A delete has been done", new { entry.CurrentValues });
                            break;
                        }
                    case EntityState.Added:
                        {
                            _logger.LogTrace("A new entry has been created", new { entry.CurrentValues });
                            break;
                        }
                }
            }
        }

    }
}
