using Identity.API.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identity.Test.Entities
{
    [TestClass]
    public class UserEntityTest
    {
        [TestMethod]
        public void ShouldUsernameEqualsMarvin()
        {
            var user = new User("Marvin");
            Assert.AreEqual("Marvin", user.Username);
        }

        [TestMethod]
        public void ShouldUsernameNotEqualsMarvin()
        {
            var user = new User("MarvinJ");
            Assert.AreNotEqual("Marvin", user.Username);
        }
    }
}