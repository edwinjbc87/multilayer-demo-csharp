using System;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Services.Contracts;
using Xunit;
using Moq;
using Sat.Recruitment.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("User Controller Tests", DisableParallelization = true)]    
    public class UserControllerTest
    {
        [Fact(DisplayName = "Add Correct User")]        
        public void Add_Correct_User()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Add(It.IsAny<User>())).ReturnsAsync((User user)=> {
                user.Id = 1;
                return user;
            });
            var userController = new UsersController(mockUserService.Object);

            //Act
            var resp = userController.CreateUser(new Api.Models.UserModel() {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = "124"
            });
            resp.Wait();
            
            //Assert
            Assert.True(resp.Result.IsSuccess);
            Assert.Equal("User Created", resp.Result.Message);
        }

        [Fact(DisplayName = "Add Duplicated User")]
        public void Add_Duplicated_User()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Add(It.IsAny<User>())).Throws(new Exception("The user is duplicated"));
            var userController = new UsersController(mockUserService.Object);

            //Act
            var resp = userController.CreateUser(new Api.Models.UserModel()
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = "124"
            });
            resp.Wait();

            //Assert
            Assert.False(resp.Result.IsSuccess);
            Assert.Equal("The user is duplicated", resp.Result.Errors);
        }

        [Fact(DisplayName = "Add Unfilled User")]
        public void Add_Unfilled_User()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Add(It.IsAny<User>())).ReturnsAsync(()=>null);
            var userController = new UsersController(mockUserService.Object);
            userController.ModelState.AddModelError("", "The name is required");
            userController.ModelState.AddModelError("", "The email is required");
            userController.ModelState.AddModelError("", "The phone is required");

            var user = new Api.Models.UserModel()
            {
                UserType = "Normal",
                Money = "124"
            };

            //Act
            var resp = userController.CreateUser(user);
            resp.Wait();

            //Assert
            Assert.False(resp.Result.IsSuccess);
            Assert.Equal("The name is required\nThe email is required\nThe phone is required", resp.Result.Errors);
        }

        [Fact(DisplayName = "Edit Correct User")]
        public void Edit_Correct_User()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Edit(It.IsAny<int>(),It.IsAny<User>())).ReturnsAsync((int id, User user) => {
                user.Id = id;
                return user;
            });
            var userController = new UsersController(mockUserService.Object);

            //Act
            var resp = userController.UpdateUser(1, new Api.Models.UserModel()
            {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = "124"
            });
            resp.Wait();

            //Assert
            Assert.True(resp.Result.IsSuccess);
            Assert.Equal("User Updated.", resp.Result.Message);
        }

        [Fact(DisplayName = "Edit Duplicated User")]
        public void Edit_Duplicated_User()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Edit(It.IsAny<int>(), It.IsAny<User>())).Throws(new Exception("The user is duplicated"));
            var userController = new UsersController(mockUserService.Object);

            //Act
            var resp = userController.UpdateUser(1,new Api.Models.UserModel()
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = "124"
            });
            resp.Wait();

            //Assert
            Assert.False(resp.Result.IsSuccess);
            Assert.Equal("The user is duplicated", resp.Result.Errors);
        }

        [Fact(DisplayName = "Edit Unfilled User")]
        public void Edit_Unfilled_User()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Add(It.IsAny<User>())).ReturnsAsync(() => null);
            var userController = new UsersController(mockUserService.Object);
            userController.ModelState.AddModelError("", "The name is required");
            userController.ModelState.AddModelError("", "The email is required");
            userController.ModelState.AddModelError("", "The phone is required");

            var user = new Api.Models.UserModel()
            {
                UserType = "Normal",
                Money = "124"
            };

            //Act
            var resp = userController.UpdateUser(1, user);
            resp.Wait();

            //Assert
            Assert.False(resp.Result.IsSuccess);
            Assert.Equal("The name is required\nThe email is required\nThe phone is required", resp.Result.Errors);
        }

        [Fact(DisplayName = "Edit User Failed")]
        public void Edit_User_Failed()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Edit(It.IsAny<int>(), It.IsAny<User>())).ReturnsAsync(()=>null);
            var userController = new UsersController(mockUserService.Object);

            //Act
            var resp = userController.UpdateUser(1, new Api.Models.UserModel()
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = "124"
            });
            resp.Wait();

            //Assert
            Assert.False(resp.Result.IsSuccess);
            Assert.Equal("The user could not be updated.", resp.Result.Errors);
        }

        [Fact(DisplayName = "Edit Unknown User")]
        public void Edit_Unknown_User()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Edit(It.IsAny<int>(), It.IsAny<User>())).Throws(new Exception("The user does not exist."));
            var userController = new UsersController(mockUserService.Object);

            //Act
            var resp = userController.UpdateUser(1, new Api.Models.UserModel()
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = "124"
            });
            resp.Wait();

            //Assert
            Assert.False(resp.Result.IsSuccess);
            Assert.Equal("The user does not exist.", resp.Result.Errors);
        }

        [Fact(DisplayName = "Delete Correct User")]
        public void Delete_Correct_User()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(true);
            var userController = new UsersController(mockUserService.Object);

            //Act
            var resp = userController.DeleteUser(1);
            resp.Wait();

            //Assert
            Assert.True(resp.Result.IsSuccess);
            Assert.Equal("User Deleted.", resp.Result.Message);
        }

        [Fact(DisplayName = "Delete User Failed")]
        public void Delete_User_Failed()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(false);
            var userController = new UsersController(mockUserService.Object);

            //Act
            var resp = userController.DeleteUser(1);
            resp.Wait();

            //Assert
            Assert.False(resp.Result.IsSuccess);
            Assert.Equal("The user could not be deleted.", resp.Result.Errors);
        }

        [Fact(DisplayName = "Delete Unknown User")]
        public void Delete_Unknown_User()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Delete(It.IsAny<int>())).Throws(new Exception("The user does not exist."));
            var userController = new UsersController(mockUserService.Object);

            //Act
            var resp = userController.DeleteUser(1);
            resp.Wait();

            //Assert
            Assert.False(resp.Result.IsSuccess);
            Assert.Equal("The user does not exist.", resp.Result.Errors);
        }
    }
}
