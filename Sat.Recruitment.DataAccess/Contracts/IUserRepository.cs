using System.Collections.Generic;
using System.Threading.Tasks;
using Sat.Recruitment.Domain.Entities;

namespace Sat.Recruitment.DataAccess.Contracts
{
    public interface IUserRepository
    {
        Task<User> Add(User user);
        Task<bool> Edit(int id, User user);
        Task<bool> Delete(int id);
        IList<User> GetUsers();
    }
}