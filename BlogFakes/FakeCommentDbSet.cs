using BlogEntities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;

namespace BlogFakes
{
    public class FakeCommentDbSet : FakeDbSet<Comment>
    {
        public override Comment Find(params object[] keyValues)
        {
            return data.First(x => x.Id == (int)keyValues.First());
        }

        public override ValueTask<Comment> FindAsync(params object[] keyValues)
        {
            var article = data.First(x => x.Id == (int)keyValues.First());
            return new ValueTask<Comment>(article);
        }

        public override EntityEntry<Comment> Update(Comment entity)
        {
            var item = data.FirstOrDefault(t => t.Id == entity.Id);
            var index = data.IndexOf(item);

            data[index] = entity;

            return null;
        }
    }
}