using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlogFakes
{
    public class FakeDbSet<T> : DbSet<T> where T : class
    {
        public ObservableCollection<T> data { get; set; }
        public IQueryable queryable { get { return data == null ? new ObservableCollection<T> { }.AsQueryable() : data.AsQueryable(); } }

        //public virtual T Find(params object[] keyValues)
        //{
        //    throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
        //}

        //public virtual ValueTask<T> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        //{
        //    return base.FindAsync(keyValues, cancellationToken);
        //}

        //public override T Add(T item)
        //{
        //    _data.Add(item);
        //    return item;
        //}

        //public override T Remove(T item)
        //{
        //    _data.Remove(item);
        //    return item;
        //}

        //public T Attach(T item)
        //{
        //    _data.Add(item);
        //    return item;
        //}

        //public T Detach(T item)
        //{
        //    _data.Remove(item);
        //    return item;
        //}

        //public T Create()
        //{
        //    return Activator.CreateInstance<T>();
        //}

        //public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        //{
        //    return Activator.CreateInstance<TDerivedEntity>();
        //}

        //public ObservableCollection<T> Local
        //{
        //    get { return _data; }
        //}

        //Type IQueryable.ElementType
        //{
        //    get { return _query.ElementType; }
        //}

        //System.Linq.Expressions.Expression IQueryable.Expression
        //{
        //    get { return _query.Expression; }
        //}

        //IQueryProvider IQueryable.Provider
        //{
        //    get { return _query.Provider; }
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return _data.GetEnumerator();
        //}


    }
}
