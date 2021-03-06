﻿using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Infrastructure.Logging;
using Fingo.Auth.ManagementApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace Fingo.Auth.ManagementApp.Tests.Controllers
{
    public class UsersControllerTest
    {
        public UsersControllerTest()
        {
            eventWatcher = new Mock<IEventWatcher>();
        }

        private readonly Mock<IEventWatcher> eventWatcher;

        [Fact]
        public void Can_Get_Valid_User()
        {
            //Arrange

            var eventBusMock = new Mock<IEventBus>();
            var logger = new Mock<ILogger<UsersController>>();
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(new User
            {
                Id = 1 ,
                FirstName = "first" ,
                Login = "first"
            });

            var factoryMock = new Mock<IGetUserFactory>();
            factoryMock.Setup(m => m.Create()).Returns(new GetUser(repositoryMock.Object));

            //Act

            var target =
                (ViewResult)
                new UsersController(null , null , eventWatcher.Object , factoryMock.Object , null , null , null ,
                    eventBusMock.Object , null , logger.Object).GetById(1);

            var result = (BaseUserModelWithProjects) target.ViewData.Model;
            var resultViewName = target.ViewName;

            //Assert

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.True(result.FirstName == "first");
            Assert.True(resultViewName == "UserDetails");
        }

        [Fact]
        public void Cannot_Get_NonExisted_User()
        {
            //Arrange

            var eventBusMock = new Mock<IEventBus>();
            var logger = new Mock<ILogger<UsersController>>();
            var tempDataMock = new Mock<ITempDataDictionary>();
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(m => m.GetById(1)).Returns(new User
            {
                Id = 1 ,
                FirstName = "first" ,
                Login = "first"
            });

            var factoryMock = new Mock<IGetUserFactory>();

            factoryMock.Setup(m => m.Create()).Returns(new GetUser(repositoryMock.Object));

            //Act

            var target = new UsersController(null , null , eventWatcher.Object , factoryMock.Object , null , null , null ,
                eventBusMock.Object , null , logger.Object)
            {
                TempData = tempDataMock.Object
            };

            var resultViewName = ((ViewResult) target.GetById(2)).ViewName;

            //Assert

            Assert.True(resultViewName == "ErrorPage");
        }
    }
}