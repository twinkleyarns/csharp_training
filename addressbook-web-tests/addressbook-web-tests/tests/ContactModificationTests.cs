using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
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
            List<ContactData> oldContacts = app.Contacts.GetContactsList();
            app.Contacts.Modify(0, newData);
            List<ContactData> newContacts = app.Contacts.GetContactsList();
            oldContacts[0].FirstName = newData.FirstName;
            oldContacts[0].LastName = newData.LastName;
            oldContacts.Sort();
            newContacts.Sort();
            Assert.AreEqual(oldContacts, newContacts);
        }
    }
}
