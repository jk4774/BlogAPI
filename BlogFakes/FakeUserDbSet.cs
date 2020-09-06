using BlogContext;
using BlogEntities;
using Microsoft.EntityFrameworkCore;

namespace BlogFakes
{
    public class FakeUserDbSet : FakeDbSet<User>
    {
        
    }
}