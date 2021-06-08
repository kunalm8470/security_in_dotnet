using Ardalis.Specification;
using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class UserByLoginSpecification : Specification<User>
    {
        public UserByLoginSpecification(string login)
        {
            Query.Where(x => x.Username == login || x.Username == login);
        }
    }
}
