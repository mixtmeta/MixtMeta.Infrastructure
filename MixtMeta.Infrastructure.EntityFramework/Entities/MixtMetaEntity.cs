using MixtMeta.Infrastructure.EntityFramework.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixtMeta.Infrastructure.EntityFramework.Entities
{
    public abstract class MixtMetaEntity : IMixtMetaEntity
    {
        protected ICollection<IType> MapCollection<IType, TType>(ICollection<TType> collection) where TType : IType
        {
            ICollection<IType> coll = new Collection<IType>();
            foreach (TType item in collection)
            {
                coll.Add(item);
            }
            return coll;
        }
    }
}
