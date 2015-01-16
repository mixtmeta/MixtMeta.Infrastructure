using MixtMeta.Infrastructure.EntityFramework.Exceptions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixtMeta.Infrastructure.EntityFramework.Connection
{
    public abstract class UnitOfWork : IDisposable
    {
        protected readonly MixtMetaDbContext _context;
        private bool _disposed = false;

        protected abstract string NameOrConnectionString { get; }
        protected abstract MixtMetaDbContext DbContext { get; }

        public UnitOfWork()
        {
            _context = DbContext;
        }

        public void Save()
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    trans.Commit();
                }
                catch (DbUpdateException e)
                {
                    trans.Rollback();
                    if (ConfigurationManager.ConnectionStrings[NameOrConnectionString].ProviderName == "MySql.Data.MySqlClient")
                    {
                        MySqlException ex = GetMySqlException(e.InnerException);
                        if (ex.Number == 1062)
                        {
                            throw new UniqueConstraintException(ex);
                        }
                    }
                    else if (ConfigurationManager.ConnectionStrings[NameOrConnectionString].ProviderName == "System.Data.SqlClient")
                    {
                        SqlException ex = GetSqlException(e.InnerException);
                        if (ex.Number == 2627 || ex.Number == 2601) //2627 Unique Constraint, 2601 Unique Index
                        {
                            throw new UniqueConstraintException(ex);
                        }
                    }
                    throw;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Functions

        private MySqlException GetMySqlException(Exception e)
        {
            if (e.InnerException is MySqlException)
            {
                return e.InnerException as MySqlException;
            }
            else
            {
                if (e.InnerException == null)
                {
                    return null;
                }
                else
                {
                    return GetMySqlException(e.InnerException);
                }
            }
        }

        private SqlException GetSqlException(Exception e)
        {
            if (e.InnerException is SqlException)
            {
                return e.InnerException as SqlException;
            }
            else
            {
                if (e.InnerException == null)
                {
                    return null;
                }
                else
                {
                    return GetSqlException(e.InnerException);
                }
            }
        }

        #endregion
    }
}
