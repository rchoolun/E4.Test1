using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test1.Business;
using Test1.Model;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddUser()
        {
            var user = new User() { Name = "Ravish", Surname = "Choolun", Phone = "58" };

            var business = new Business();

            var newUser = business.AddUser(user);

            Assert.IsTrue(newUser.IsSuccess);

        }

        [TestMethod]
        public void GetUsers()
        {
            var business = new Business();

            var userList = business.GetUsers();

            Assert.IsNotNull(userList);
        }

        [TestMethod]
        public void UpdateUser()
        {
            var userUpdate = new User() { Id = new Guid("68c1aea2-2062-4237-9c43-86a7bb25c930"), Name = "Test", Surname = "TestSurnamessss", Phone = "123456789" };

            var business = new Business();

            var updateUser = business.UpdateUser(userUpdate);

            Assert.IsTrue(updateUser.IsSuccess);
        }

        [TestMethod]
        public void RemoveUser()
        {
            var business = new Business();

            var removeUser = business.RemoveUser("78d55456-7aff-40f9-a658-9fd0dc0bc6a0");

            Assert.IsTrue(removeUser.IsSuccess);
        }


    }
}
