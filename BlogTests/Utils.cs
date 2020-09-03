using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FakeItEasy;

namespace BlogTests
{
    public static class Utils
    {
        public static DbSet<T> CreateDbSetFake<T>(IEnumerable<T> elements) where T : class
        {
            var queryable = elements.AsQueryable();
            var fakeDbSet = A.Fake<DbSet<T>>(x => x.Implements(typeof(IQueryable<T>)).Implements(typeof(IDbAsyncEnumerable<T>)));

            A.CallTo(() => ((IQueryable<T>)fakeDbSet).Provider).Returns(queryable.Provider);
            A.CallTo(() => ((IQueryable<T>)fakeDbSet).Expression).Returns(queryable.Expression);
            A.CallTo(() => ((IQueryable<T>)fakeDbSet).ElementType).Returns(queryable.ElementType);
            A.CallTo(() => ((IQueryable<T>)fakeDbSet).GetEnumerator()).Returns(queryable.GetEnumerator());

            return fakeDbSet;
        }
    }
}