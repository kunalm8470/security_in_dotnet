using Ardalis.Specification;
using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class UserByUsernameSpecification : Specification<User>
    {
        public UserByUsernameSpecification(string username)
        {
            Query.Where(x => x.Username == username);
        }
    }
}
