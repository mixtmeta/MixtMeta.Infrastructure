using MixtMeta.Infrastructure.EntityFramework.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MixtMeta.Infrastructure.EntityFramework.Interfaces.Connection
{
    public interface IMixtMetaRepository<T> : IDisposable where T : IMixtMetaEntity
    {
		IEnumerable<T> GetAll();
		IEnumerable<T> GetAll(params Expression<Func<T, IMixtMetaEntity>>[] includeExpressions);
        T GetEntityById(int entityId);
		T GetEntityById(Expression<Func<T, bool>> keySelector, params Expression<Func<T, IMixtMetaEntity>>[] includeExpressions);
        T NewObject();
        void Insert(T entity);
        void Delete(int entityId);
        void Delete(T entity);
        void Update(T entity);
    }
}
