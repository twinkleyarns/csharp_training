using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class AddingContactToGroupTests : AuthTestBase
    {
        [Test]
        public void AddingContactToGroupTest()
        {
            app.Contacts.CreateContactIfDoesNotExist(new ContactData("new", "contact"));
            app.Groups.CreateGroupIfDoesNotExist(new GroupData("new"));

            GroupData group = GroupData.GetAll()[0];
            List<ContactData> oldList = group.GetContacts();
            ContactData contact;
            try
            {
                contact = ContactData.GetAll().Except(group.GetContacts()).First();
            }
            catch (InvalidOperationException)
            {
                string uniqueString = DateTime.Now.ToString("mm/dd/yyyy hh:mm:ss");
                app.Contacts.Create(new ContactData(uniqueString, uniqueString));
                contact = ContactData.GetAll().Except(group.GetContacts()).First();
            }

            app.Contacts.AddContactToGroup(contact, group);

            List<ContactData> newList = group.GetContacts();
            oldList.Add(contact);
            newList.Sort();
            oldList.Sort();

            Assert.AreEqual(oldList, newList);
        }
    }
}
