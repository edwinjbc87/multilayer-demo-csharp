using System;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.DataAccess.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using Sat.Recruitment.Services.Contracts;

namespace Sat.Recruitment.Services.Implementations
{
    public partial class UserService : IUserService
    {
        private readonly IUserRepository repository;

        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public IList<User> GetUsers()
        {
            return repository.GetUsers();
        }

        public async Task<User> Add(User user)
        {
            User res = null;
            try
            {
                user.NormalizedEmail = GetNormalizedEmail(user.Email);
                user.CalculatedMoney = GetCalculatedMoney(user.Money, user.UserType);

                if (IsDuplicated(user))
                {
                    throw new Exception("The user is duplicated.");
                }
                res = await repository.Add(user);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return res;
        }

        public async Task<User> Edit(int id, User user)
        {
            User res = null;
            try
            {
                user.NormalizedEmail = GetNormalizedEmail(user.Email);
                user.CalculatedMoney = GetCalculatedMoney(user.Money, user.UserType);

                user.Id = id;
                if (IsDuplicated(user))
                {
                    throw new Exception("The user is duplicated.");
                }
                bool response = await repository.Edit(id, user);
                if (!response)
                {
                    res = null;
                } else
                {
                    res = user;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return res;
        }

        public async Task<bool> Delete(int id)
        {
            bool res = false;
            try
            {
                res = await repository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        private bool IsDuplicated(User user)
        {
            var users = repository.GetUsers();
            return users.Count(u =>
                u.Id != user.Id &&
                (
                    u.Name.ToLower().Equals(user.Name.ToLower()) ||
                    u.Email.ToLower().Equals(user.Email.ToLower()) ||
                    u.Phone.ToLower().Equals(user.Phone.ToLower())
                )                
            ) > 0;
        }
    }
}
