using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class RemovingContactFromGroupTests : AuthTestBase
    {
        [Test]
        public void RemovingContactFromGroupTest()
        {
            app.Contacts.CreateContactIfDoesNotExist(new ContactData("new", "new"));
            app.Groups.CreateGroupIfDoesNotExist(new GroupData("new"));

            GroupData group = GroupData.GetAll()[0];
            List<ContactData> oldList = group.GetContacts();
            ContactData contact;

            if (oldList.Count() != 0)
            {
                contact = oldList.First();
            }
            else
            {
                contact = ContactData.GetAll().First();
                app.Contacts.AddContactToGroup(contact, group);
                oldList = group.GetContacts();
            }

            app.Contacts.RemoveContactFromGroup(contact, group);

            List<ContactData> newList = group.GetContacts();
            oldList.Remove(contact);
            newList.Sort();
            oldList.Sort();

            Assert.AreEqual(oldList, newList);
        }
    }
}
