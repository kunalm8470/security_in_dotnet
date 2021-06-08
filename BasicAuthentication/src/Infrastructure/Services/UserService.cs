﻿using Core.Entities;
using Core.Interfaces;
using Core.Services;
using Core.Specifications;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> FetchUserAsync(string login)
        {
            return await _repository.FirstOrDefaultAsync(new UserByLoginSpecification(login));
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            return await _repository.AddAsync(user);
        }
    }
}
