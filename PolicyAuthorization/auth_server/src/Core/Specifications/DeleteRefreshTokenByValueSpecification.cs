using Ardalis.Specification;
using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class DeleteRefreshTokenByValueSpecification : Specification<RefreshToken>
    {
        public DeleteRefreshTokenByValueSpecification(string token)
        {
            Query.Where(b => b.Token == token);
        }
    }
}
