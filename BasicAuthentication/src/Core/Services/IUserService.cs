using Core.Entities;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IUserService
    {
        public Task<User> RegisterUserAsync(User user);
        public Task<User> FetchUserAsync(string login);
    }
}
