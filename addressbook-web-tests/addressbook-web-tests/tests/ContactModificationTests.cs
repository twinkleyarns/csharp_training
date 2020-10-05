using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class ContactModificationTests : AuthTestBase
    {
        [Test]
        public void ContactModificationTest()
        {
            ContactData newData = new ContactData("A", "D");

            app.Contacts.CreateContactIfDoesNotExist(new ContactData("new", "new"));
            app.Contacts.Modify(1, newData);
        }
    }
}
