using System;
using System.Threading.Tasks;
using EventBus.Abstractions;
using EventBus.Events;
using Identity.API.Application.Commands.Handlers;
using Identity.API.Application.Commands.Models;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharedKernel.Handlers;

namespace Identity.Test.Commands
{
    [TestClass]
    public class CreateUserCommandTest
    {
        private readonly Mock<IEventBus> _eventBusMock;

        private readonly Mock<INotificationHandler> _notificationHandlerMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        private CreateUserCommandHandler _handler;

        public CreateUserCommandTest()
        {
            _eventBusMock = new Mock<IEventBus>();
            _notificationHandlerMock = new Mock<INotificationHandler>();

            var userStore = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [TestMethod]
        public void ShouldInvalidWhenUsernameIsNull()
        {
            _handler = new CreateUserCommandHandler(
                _eventBusMock.Object,
                _notificationHandlerMock.Object,
                _userManagerMock.Object,
                _userRepositoryMock.Object);

            var model = new CreateUserCommandModel(null, "123456", "James Odrin", It.IsAny<DateTime>(), "23562524122");
            var handled = _handler.Handle(model).Result;

            Assert.IsFalse(handled.Success);
        }

        [TestMethod]
        public void ShouldInvalidWhenPasswordIsNull()
        {
            _handler = new CreateUserCommandHandler(
                _eventBusMock.Object,
                _notificationHandlerMock.Object,
                _userManagerMock.Object,
                _userRepositoryMock.Object);

            var model = new CreateUserCommandModel("jodrin", null, "James Odrin", It.IsAny<DateTime>(), "23562524122");
            var handled = _handler.Handle(model).Result;

            Assert.IsFalse(handled.Success);
        }

        [TestMethod]
        public void ShouldInvalidWhenUserExists()
        {
            _userManagerMock.Setup(userManager => userManager.FindByNameAsync("jodrin")).Returns(Task.FromResult(new User("jodrin")));

            _handler = new CreateUserCommandHandler(
                _eventBusMock.Object,
                _notificationHandlerMock.Object,
                _userManagerMock.Object,
                _userRepositoryMock.Object);

            var model = new CreateUserCommandModel("jodrin", "123456", "James Odrin", It.IsAny<DateTime>(), "23562524122");
            var handled = _handler.Handle(model).Result;

            Assert.IsFalse(handled.Success);
        }

        [TestMethod]
        public void ShouldInvalidWhenFailOnCreateUser()
        {
            _userManagerMock.Setup(userManager => userManager.FindByNameAsync("jodrin")).Returns(Task.FromResult(null as User));

            _userManagerMock.Setup(
                userManager => userManager
                    .CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Failed()));

            _handler = new CreateUserCommandHandler(
                _eventBusMock.Object,
                _notificationHandlerMock.Object,
                _userManagerMock.Object,
                _userRepositoryMock.Object);

            var model = new CreateUserCommandModel("jodrin", "123456", "James Odrin", It.IsAny<DateTime>(), "23562524122");
            var handled = _handler.Handle(model).Result;

            Assert.IsFalse(handled.Success);
        }

        [TestMethod]
        public void ShouldValidWhenCreateUserSuccesfully()
        {
            _userManagerMock.Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(null as User));

            _userManagerMock.Setup(
                userManager => userManager
                    .CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));

            _userRepositoryMock.Setup(repository => repository.Commit()).Returns(Task.CompletedTask);

            _eventBusMock.Setup(bus => bus.Publish(It.IsAny<IntegrationEvent>()));

            _handler = new CreateUserCommandHandler(
                _eventBusMock.Object,
                _notificationHandlerMock.Object,
                _userManagerMock.Object,
                _userRepositoryMock.Object);

            var model = new CreateUserCommandModel("jodrin", "123456", "James Odrin", It.IsAny<DateTime>(), "23562524122");
            var handled = _handler.Handle(model).Result;

            Assert.IsTrue(handled.Success);
        }
    }
}
