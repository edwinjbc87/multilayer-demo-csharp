using System;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Services.Contracts;
using Xunit;
using Moq;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.DataAccess.Contracts;
using Sat.Recruitment.Services.Implementations;
using System.Collections.Generic;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("UserService Tests", DisableParallelization = true)]
    public class UserServiceTest
    {
        [Fact(DisplayName = "Add Correct User")]
        public void Add_Correct_User()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(r => r.GetUsers()).Returns(new List<User>());
            mockUserRepository.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync((User user) => {
                user.Id = 1;
                return user;
            });
            var userService = new UserService(mockUserRepository.Object);

            //Act
            var resp = userService.Add(new User()
            {
                Name = "Mike",
                Email = "mike.perez+sistemas@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 123
            });
            resp.Wait();

            //Assert
            Assert.NotNull(resp.Result);
            Assert.Equal("mikeperez@gmail.com", resp.Result.NormalizedEmail);
            Assert.True(resp.Result.Id > 0);
        }

        [Fact(DisplayName = "Add Duplicated User")]
        public void Add_Duplicated_User()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(r => r.GetUsers()).Returns(new List<User>() {
                new User()
                {
                    Id = 4,
                    Email = "mike@gmail.com"
                }
            });
            mockUserRepository.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync((User user) => null);
            var userService = new UserService(mockUserRepository.Object);

            //Act
            var ex = Record.Exception(() => {
                var resp = userService.Add(new User()
                {
                    Name = "Mike",
                    Email = "mike@gmail.com",
                    Address = "Av. Juan G",
                    Phone = "+349 1122354215",
                    UserType = "Normal",
                    Money = 123
                });
                resp.Wait();
            });
            //Assert
            Assert.Contains("The user is duplicated.", ex.Message);
        }

        [Fact(DisplayName = "Edit Correct User")]
        public void Edit_Correct_User()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(r => r.GetUsers()).Returns(new List<User>() {
                new User()
                {
                    Id = 1
                }
            });
            mockUserRepository.Setup(r => r.Edit(It.IsAny<int>(), It.IsAny<User>())).ReturnsAsync((int id, User user) => true);
            var userService = new UserService(mockUserRepository.Object);

            //Act
            var resp = userService.Edit(1, new User()
            {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 123
            });
            resp.Wait();

            //Assert
            Assert.NotNull(resp.Result);
        }

        [Fact(DisplayName = "Edit Duplicated User")]
        public void Edit_Duplicated_User()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(r => r.GetUsers()).Returns(new List<User>() {
                new User()
                {
                    Id = 4,
                    Email = "mike@gmail.com"
                }
            });
            mockUserRepository.Setup(r => r.Edit(It.IsAny<int>(),It.IsAny<User>())).ReturnsAsync((int id, User user) => true);
            var userService = new UserService(mockUserRepository.Object);

            //Act
            var ex = Record.Exception(() => {
                var resp = userService.Edit(1, new User()
                {
                    Name = "Mike",
                    Email = "mike@gmail.com",
                    Address = "Av. Juan G",
                    Phone = "+349 1122354215",
                    UserType = "Normal",
                    Money = 123
                });
                resp.Wait();
            });

            //Assert
            Assert.Contains("The user is duplicated.", ex.Message);
        }

        [Fact(DisplayName = "Edit Unknown User")]
        public void Edit_Unknown_User()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(r => r.GetUsers()).Returns(new List<User>() {
                new User()
                {
                    Id = 3
                }
            });
            mockUserRepository.Setup(r => r.Edit(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new Exception("The user does not exist."));
            var userService = new UserService(mockUserRepository.Object);

            //Act
            var ex = Record.Exception(() =>
            {
                var resp = userService.Edit(1, new User()
                {
                    Name = "Mike",
                    Email = "mike@gmail.com",
                    Address = "Av. Juan G",
                    Phone = "+349 1122354215",
                    UserType = "Normal",
                    Money = 123
                });
                resp.Wait();

            });

            //Assert
            Assert.Contains("The user does not exist.", ex.Message);
        }


        [Fact(DisplayName = "Delete Correct User")]
        public void Delete_Correct_User()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(r => r.GetUsers()).Returns(new List<User>() {
                new User()
                {
                    Id = 1
                }
            });
            mockUserRepository.Setup(r => r.Delete(It.IsAny<int>())).ReturnsAsync((int id) => true);
            var userService = new UserService(mockUserRepository.Object);

            //Act
            var resp = userService.Delete(1);
            resp.Wait();

            //Assert
            Assert.True(resp.Result);
        }

        [Fact(DisplayName = "Delete Unknown User")]
        public void Delete_Unknown_User()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(r => r.GetUsers()).Returns(new List<User>() {
                new User()
                {
                    Id = 3
                }
            });
            mockUserRepository.Setup(r => r.Delete(It.IsAny<int>())).ReturnsAsync((int id) => false);
            var userService = new UserService(mockUserRepository.Object);

            //Act
            var resp = userService.Delete(1);
            resp.Wait();

            //Assert
            Assert.False(resp.Result);
        }
    }
}
