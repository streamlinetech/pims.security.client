using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlitBit.Copy;
using FlitBit.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Streamline.Pims.Security.Client.Dtos;
using Utilities.DataTypes.ExtensionMethods;

namespace Streamline.Pims.Security.Client.Tests
{
    [TestClass]
    public class AuthorizationClientTests
    {
        // This hits real services.
        //[TestMethod]
        //public void AbilitiesTest()
        //{
        //    var client = new AuthorizationClient();

        //    var abilities = new List<string>
        //    {
        //        "Ability 1"
        //    };

        //    var result = client.Authorize("henry", "ilovehenry", abilities);

        //    Assert.IsFalse(result);

        //    abilities.Add("Recon Edit");

        //    result = client.Authorize("henry", "ilovehenry", abilities);

        //    Assert.IsTrue(result);
        //}
        [TestMethod]
        public void CreateDtoFromDynamic()
        {
            dynamic basicUser = new ExpandoObject();
            basicUser.UserName = "blah";
            basicUser.LastName = "blah";
            var dto = Create.AsIf<IBasicUser>(basicUser);

            Assert.AreEqual(basicUser.UserName, dto.UserName);
            Assert.AreEqual(basicUser.LastName, dto.LastName);
        }
    }
}
