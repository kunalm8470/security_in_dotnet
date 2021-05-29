using Core.Entities;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user);
        Task<User> FetchUserAsync(string login);
    }
}
