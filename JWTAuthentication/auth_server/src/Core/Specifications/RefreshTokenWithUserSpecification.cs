using Ardalis.Specification;
using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class RefreshTokenWithUserSpecification : Specification<RefreshToken>
    {
        public RefreshTokenWithUserSpecification(string token)
        {
            Query
                .Where(x => x.Token == token)
                .Include(x => x.User);
        }
    }
}
