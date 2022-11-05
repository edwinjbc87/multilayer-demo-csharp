using System;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Services.Contracts;
using Xunit;
using Moq;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.DataAccess.Contracts;
using Sat.Recruitment.Services.Implementations;
using System.Collections.Generic;
using Sat.Recruitment.DataAccess.Implementations;
using System.Text;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("UserService Tests", DisableParallelization = true)]
    public class UserRepositoryTest
    {
        [Fact(DisplayName = "Add Correct User")]
        public void Add_Correct_User()
        {
            //Arrange
            var mockFileHelper = new Mock<IFileHelper>();
            mockFileHelper.Setup(f => f.GetFileData(It.IsAny<String>())).Returns(new System.IO.MemoryStream());
            mockFileHelper.Setup(f => f.SaveFileData(It.IsAny<String>(), It.IsAny<System.IO.MemoryStream>()));

            var userRepository = new UserRepository("c:\\fakepath\\users.txt", mockFileHelper.Object);

            //Act
            var resp = userRepository.Add(new User()
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
            Assert.True(resp.Result.Id > 0);
        }

        [Fact(DisplayName = "Add Null User")]
        public void Add_Null_User()
        {
            //Arrange
            var mockFileHelper = new Mock<IFileHelper>();
            mockFileHelper.Setup(f => f.GetFileData(It.IsAny<String>())).Returns(new System.IO.MemoryStream());
            mockFileHelper.Setup(f => f.SaveFileData(It.IsAny<String>(), It.IsAny<System.IO.MemoryStream>()));

            var userRepository = new UserRepository("c:\\fakepath\\users.txt", mockFileHelper.Object);

            //Act
            var ex = Record.Exception(()=> {
                var resp = userRepository.Add(null);
                resp.Wait();
            });
            

            //Assert
            Assert.Contains("User is null.", ex.Message);
        }

        [Fact(DisplayName = "Edit Correct User")]
        public void Edit_Correct_User()
        {
            //Arrange
            var mockFileHelper = new Mock<IFileHelper>();
            mockFileHelper.Setup(f => f.GetFileData(It.IsAny<String>())).Returns(()=> {
                return new System.IO.MemoryStream(Encoding.UTF8.GetBytes($"1,Angel,angel@gmail.com,angel@gmail.com,+51967969,Av Surco 334,Normal,1500,1600"));
            });
            mockFileHelper.Setup(f => f.SaveFileData(It.IsAny<String>(), It.IsAny<System.IO.MemoryStream>()));

            var userRepository = new UserRepository("c:\\fakepath\\users.txt", mockFileHelper.Object);

            //Act
            var resp = userRepository.Edit(1, new User()
            {
                Name = "Mike",
                Email = "mike.perez+sistemas@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 1230
            });
            resp.Wait();

            //Assert
            Assert.True(resp.Result);
        }

        [Fact(DisplayName = "Edit Unknown User")]
        public void Edit_Unknown_User()
        {
            //Arrange
            var mockFileHelper = new Mock<IFileHelper>();
            mockFileHelper.Setup(f => f.GetFileData(It.IsAny<String>())).Returns(() => {
                return new System.IO.MemoryStream(Encoding.UTF8.GetBytes($"1,Angel,angel@gmail.com,angel@gmail.com,+51967969,Av Surco 334,Normal,1500,1600"));
            });
            mockFileHelper.Setup(f => f.SaveFileData(It.IsAny<String>(), It.IsAny<System.IO.MemoryStream>()));

            var userRepository = new UserRepository("c:\\fakepath\\users.txt", mockFileHelper.Object);

            //Act
            var ex = Record.Exception(()=> {
                var resp = userRepository.Edit(3, new User()
                {
                    Name = "Mike",
                    Email = "angel+sistemas@gmail.com",
                    Address = "Av. Juan G",
                    Phone = "+349 1122354215",
                    UserType = "Normal",
                    Money = 1230
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
            var mockFileHelper = new Mock<IFileHelper>();
            mockFileHelper.Setup(f => f.GetFileData(It.IsAny<String>())).Returns(() => {
                return new System.IO.MemoryStream(Encoding.UTF8.GetBytes($"1,Angel,angel@gmail.com,angel@gmail.com,+51967969,Av Surco 334,Normal,1500,1600"));
            });
            mockFileHelper.Setup(f => f.SaveFileData(It.IsAny<String>(), It.IsAny<System.IO.MemoryStream>()));

            var userRepository = new UserRepository("c:\\fakepath\\users.txt", mockFileHelper.Object);

            //Act
            var resp = userRepository.Delete(1);
            resp.Wait();

            //Assert
            Assert.True(resp.Result);
        }


        [Fact(DisplayName = "Delete Unknown User")]
        public void Delete_Unknown_User()
        {
            //Arrange
            var mockFileHelper = new Mock<IFileHelper>();
            mockFileHelper.Setup(f => f.GetFileData(It.IsAny<String>())).Returns(() => {
                return new System.IO.MemoryStream(Encoding.UTF8.GetBytes($"1,Angel,angel@gmail.com,angel@gmail.com,+51967969,Av Surco 334,Normal,1500,1600"));
            });
            mockFileHelper.Setup(f => f.SaveFileData(It.IsAny<String>(), It.IsAny<System.IO.MemoryStream>()));

            var userRepository = new UserRepository("c:\\fakepath\\users.txt", mockFileHelper.Object);

            //Act
            var ex = Record.Exception(() => {
                var resp = userRepository.Delete(3);
                resp.Wait();
            });

            //Assert
            Assert.Contains("The user does not exist.", ex.Message);
        }
    }
}
