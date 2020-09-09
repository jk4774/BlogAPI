using BlogData.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;

namespace BlogFakes
{
    public class FakeUserDbSet : FakeDbSet<User>
    {
        public override User Find(params object[] keyValues)
        {
            return data.First(x => x.Id == (int)keyValues.First());
        }

        public override ValueTask<User> FindAsync(params object[] keyValues)
        {
            var article = data.First(x => x.Id == (int)keyValues.First());
            return new ValueTask<User>(article);
        }

        public override EntityEntry<User> Update(User entity)
        {
            var item = data.FirstOrDefault(t => t.Id == entity.Id);
            var index = data.IndexOf(item);

            data[index] = entity;

            return null;
        }
    }
}