using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.UnitTests.AppLogic
{
    [TestClass]
    public class BankAccountTests
    {
        [TestMethod]
        public void Create_Throws_InvalidIBANAccountException_When_AccountNumberIsInvalid()
        {
            //Arrange
            var accNo = "ROIBAN00299456453245";
            

            //Assert
            Assert.ThrowsException<InvalidIBANAccountException>(
                () => {
                    //Act
                    var account = BankAccount.Create(accNo);
                });
        }
    }
}
