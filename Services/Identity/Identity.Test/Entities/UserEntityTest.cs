using Identity.API.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Identity.Test.Entities
{
    [TestClass]
    public class UserEntityTest
    {
        [TestMethod]
        public void ShouldUsernameEqualsMarvin()
        {
            var username = It.IsAny<string>();

            var user = new User(username);
            Assert.AreEqual(username, user.Username);
        }
    }
}