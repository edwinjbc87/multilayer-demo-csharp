using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Sat.Recruitment.Services.Contracts;

using Sat.Recruitment.Api.Models;
using System.Linq;
using Sat.Recruitment.Domain.Entities;

namespace Sat.Recruitment.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {

        private readonly IUserService service;

        public UsersController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("")]
        public Result<IList<UserModel>> GetUsers()
        {
            Result<IList<UserModel>> res = null;
            try
            {
                var users = service.GetUsers();
                res = new Result<IList<UserModel>>()
                {
                    IsSuccess = true,
                    Value = users.Select(u => new UserModel() {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Phone = u.Phone,
                        Address = u.Address,
                        Money = u.Money.ToString()
                    }).ToList()
                };
            }
            catch(Exception ex)
            {
                res = new Result<IList<UserModel>>()
                {
                    IsSuccess = false,
                    Errors = ex.Message
                };
            }
            return res; 
        }


        [HttpPost]
        [Route("")]
        public async Task<Result<UserModel>> CreateUser(UserModel user)
        {
            Result<UserModel> res = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception(string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(er => er.ErrorMessage)));
                }


                decimal money = 0;
                if (!Decimal.TryParse(user.Money, out money))
                {
                    money = 0;
                }

                User nUser = await service.Add(new User()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address,
                    Money = money
                });

                if(nUser == null)
                {
                    throw new Exception("User was not created");
                }

                res = new Result<UserModel>() {
                    IsSuccess = true,
                    Value = new UserModel() {
                        Name = nUser.Name,
                        Email = nUser.Email,
                        Phone = nUser.Phone,
                        Address = nUser.Address,
                        Money = nUser.Money.ToString()
                    },
                    Message = "User Created"
                }; 
            }
            catch(Exception ex)
            {
                res = new Result<UserModel>()
                {
                    IsSuccess = false,
                    Errors = ex.Message
                };
            }

            return res;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<Result<UserModel>> UpdateUser([FromRoute]int id, [FromBody]UserModel user)
        {
            Result<UserModel> res = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception(string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(er => er.ErrorMessage)));
                }


                decimal money = 0;
                if (!Decimal.TryParse(user.Money, out money))
                {
                    money = 0;
                }

                User userSaved = await service.Edit(id, new User()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address,
                    Money = money
                });

                if(userSaved == null)
                {
                    throw new Exception("The user could not be updated.");
                }

                res = new Result<UserModel>()
                {
                    IsSuccess = true,
                    Value = new UserModel()
                    {
                        Id = userSaved.Id,
                        Name = userSaved.Name,
                        Email = userSaved.Email,
                        Phone = userSaved.Phone,
                        Address = userSaved.Address,
                        Money = userSaved.Money.ToString()
                    },
                    Message = "User Updated."
                };
            }
            catch (Exception ex)
            {
                res = new Result<UserModel>()
                {
                    IsSuccess = false,
                    Errors = ex.Message
                };
            }

            return res;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<Result> DeleteUser([FromRoute] int id)
        {
            Result res = null;
            try
            {
                bool resDelete = await service.Delete(id);

                if (!resDelete)
                {
                    throw new Exception("The user could not be deleted.");
                }

                res = new Result()
                {
                    IsSuccess = true,
                    Message = "User Deleted."
                };
            }
            catch (Exception ex)
            {
                res = new Result()
                {
                    IsSuccess = false,
                    Errors = ex.Message
                };
            }

            return res;
        }
    }
}
