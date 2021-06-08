using Core.Entities;

namespace Core.Services
{
    public interface IAccessTokenService
    {
        public string Generate(User user);
    }
}
