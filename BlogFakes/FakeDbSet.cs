using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlogFakes
{
    public class FakeDbSet<T> : DbSet<T> where T : class
    {
        public ObservableCollection<T> data { get; set; }
        public IQueryable queryable { get { return data == null ? new ObservableCollection<T> { }.AsQueryable() : data.AsQueryable(); } }

        public override EntityEntry<T> Add(T entity)
        {
            data.Add(entity);
            return null;
        }       

        public override EntityEntry<T> Remove(T entity)
        {
            data.Remove(entity);
            return null;
        }

        public override void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                data.Remove(entity);
        }

        public override void RemoveRange(params T[] entities)
        {
            foreach (var entity in entities)
                data.Remove(entity);
        }
    }
}