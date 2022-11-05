using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sat.Recruitment.Domain.Entities;

namespace Sat.Recruitment.Services.Contracts
{
    public interface IUserService
    {
        IList<User> GetUsers();
        Task<User> Add(User user);
        Task<User> Edit(int id, User user);
        Task<bool> Delete(int id);
    }
}
