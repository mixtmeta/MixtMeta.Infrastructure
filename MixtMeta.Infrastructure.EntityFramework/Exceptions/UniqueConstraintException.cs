using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixtMeta.Infrastructure.EntityFramework.Exceptions
{
    public class UniqueConstraintException : Exception
    {
        public UniqueConstraintException(Exception e) : base("Unique constraint violation occurred while saving object.", e) { }
    }
}
