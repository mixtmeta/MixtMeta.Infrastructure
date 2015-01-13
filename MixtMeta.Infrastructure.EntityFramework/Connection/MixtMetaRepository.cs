using MixtMeta.Infrastructure.EntityFramework.Interfaces.Connection;
using MixtMeta.Infrastructure.EntityFramework.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixtMeta.Infrastructure.EntityFramework.Connection
{
    public class MixtMetaRepository<TEntity> : IMixtMetaRepository<TEntity> where TEntity : class, IMixtMetaEntity
    {
        private readonly MixtMetaDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        private bool _disposed;

        public MixtMetaRepository(MixtMetaDbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public TEntity NewObject()
        {
            return _dbSet.Create<TEntity>();
        }

        public TEntity GetEntityById(int entityId)
        {
            return _dbSet.Find(entityId);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(int entityId)
        {
            TEntity entity = _dbSet.Find(entityId);
            Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }

}
