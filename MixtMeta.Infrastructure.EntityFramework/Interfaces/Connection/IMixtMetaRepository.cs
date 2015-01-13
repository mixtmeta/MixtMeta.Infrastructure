using MixtMeta.Infrastructure.EntityFramework.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixtMeta.Infrastructure.EntityFramework.Interfaces.Connection
{
    public interface IMixtMetaRepository<T> : IDisposable where T : IMixtMetaEntity
    {
        IEnumerable<T> GetAll();
        T GetEntityById(int entityId);
        T NewObject();
        void Insert(T entity);
        void Delete(int entityId);
        void Delete(T entity);
        void Update(T entity);
    }
}
