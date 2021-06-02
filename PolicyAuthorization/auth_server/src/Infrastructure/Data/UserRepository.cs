using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UserRepository : EfRepository<User, int>, IUserRepository
    {
        public UserRepository(UserContext context) : base(context)
        {

        }
    }
}
