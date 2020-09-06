using System;
using System.Collections;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlogContext
{
    public interface IDbSetExtended<T> : IDbSet<T> where T : class 
    {
        public virtual Task<T> FindAsync (params object[] keyValues)
        {
            throw new NotImplementedException ("IDbSetExtended.FindAsync is not implemented yet");
        }

        public virtual EntityEntry<T> Update (T entity)
        {
            throw new NotImplementedException ("IDbSetExtended.Update is not implemented yet");
        }

        public virtual IEnumerable RemoveRange (IEnumerable entities)
        {
            throw new NotImplementedException("IDbSetExtended.RemoveRange is not implemented yet");
        }
    }
}
