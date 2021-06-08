using Ardalis.Specification;
using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class DeleteRefreshTokensForUserSpecification : Specification<RefreshToken>
    {
        public DeleteRefreshTokensForUserSpecification(int userId)
        {
            Query.Where(r => r.Id == userId);
        }
    }
}
