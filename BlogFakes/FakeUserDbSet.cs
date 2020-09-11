using BlogData.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BlogFakes
{
    public class FakeUserDbSet : FakeDbSet<User>
    {
        public override User Find(params object[] keyValues)
        {
            if (keyValues[0] == null)
                return null;
            if (int.TryParse(keyValues[0].ToString(), out int a))
                return data.FirstOrDefault(x => x.Id == (int)keyValues[0]);
            return data.FirstOrDefault(x => x.Email == (string)keyValues[0]);
        }

        public override ValueTask<User> FindAsync(params object[] keyValues)
        {
            if (keyValues[0] == null)
                return new ValueTask<User>();
            if (int.TryParse(keyValues[0].ToString(), out int a))
                return new ValueTask<User>(data.FirstOrDefault(x => x.Id == (int)keyValues[0]));
            return new ValueTask<User>(data.FirstOrDefault(x => x.Email == (string)keyValues[0]));
        }

        public override EntityEntry<User> Update(User entity)
        {
            var item = data.FirstOrDefault(t => t.Id == entity.Id);
            if (item == null)
                return null;
            var index = data.IndexOf(item);

            data[index] = entity;
            return null;
            //return new EntityEntry<User>(data[index]);

            //return new ValueTask<User>(data[index]);
        }
    }
}