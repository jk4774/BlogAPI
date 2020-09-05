using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BlogFakes
{
    public static class Utils
    {
        public static DbSet<T> CreateFakeDbSet<T>(IEnumerable<T> elements) where T : class
        {
            var queryable = elements.AsQueryable();
            var fakeDbSet = A.Fake<DbSet<T>>(x => x.Implements(typeof(IQueryable<T>)).Implements(typeof(IAsyncQueryProvider)));

            //A.CallTo(() => fakeDbSet.Remove(null)).Invokes(r => queryable)

            //A.CallTo(() => fakeDbSet.Find()).Invokes(x => elements.SingleOrDefault(x => x ));
            //A.CallTo(() => fakeDbSet.FindAsync()).Invokes(x => elements.First(x => x .) )
            //A.CallTo(() => fakeDbSet.Add())
            //A.CallTo(() => fakeDbSet.Update())
            //A.CallTo(() => fakeDbSet.Remove()).

            A.CallTo(() => ((IQueryable<T>)fakeDbSet).Provider).Returns(queryable.Provider);
            A.CallTo(() => ((IQueryable<T>)fakeDbSet).Expression).Returns(queryable.Expression);
            A.CallTo(() => ((IQueryable<T>)fakeDbSet).ElementType).Returns(queryable.ElementType);
            A.CallTo(() => ((IQueryable<T>)fakeDbSet).GetEnumerator()).Returns(queryable.GetEnumerator());

            return fakeDbSet;
        }
    }
}
