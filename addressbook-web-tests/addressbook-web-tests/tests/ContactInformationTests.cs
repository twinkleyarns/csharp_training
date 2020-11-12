using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class ContactInformationTests : AuthTestBase
    {
        [Test]
        public void ContactInformationTest()
        {
            app.Contacts.CreateContactIfDoesNotExist(new ContactData("new", "new"));

            ContactData fromTable = app.Contacts.GetContactInformationFromTable(0);
            ContactData fromForm = app.Contacts.GetContactInformationFromEditForm(0);
            Assert.AreEqual(fromTable, fromForm);
            Assert.AreEqual(fromTable.Address, fromForm.Address);
            Assert.AreEqual(fromTable.AllPhones, fromForm.AllPhones);
        }

        [Test]
        public void ContactDetailsTest()
        {
            app.Contacts.CreateContactIfDoesNotExist(new ContactData("new", "new"));

            string fromDetails = app.Contacts.GetContactInformationFromDetails(0);
            ContactData fromForm = app.Contacts.GetContactInformationFromEditForm(0);

            string fromEditForm = ConvertFormToDetailsFormat(fromForm);
            Assert.AreEqual(fromDetails, fromEditForm);
        }

        private string ConvertFormToDetailsFormat(ContactData fromForm)
        {
            string converted = String.Empty;
            if (fromForm.FirstName != null && fromForm.FirstName != "")
            {
                converted = converted + fromForm.FirstName;
            }
            if (fromForm.LastName != null && fromForm.LastName != "")
            {
                converted = converted + " " + fromForm.LastName;
            }
            if (fromForm.Address != null && fromForm.Address != "")
            {
                converted = converted + "\r\n" + fromForm.Address;
            }
            if (fromForm.HomePhone != null && fromForm.HomePhone != ""
                || fromForm.MobilePhone != null && fromForm.MobilePhone != ""
                || fromForm.WorkPhone != null && fromForm.WorkPhone != "")
            {
                converted = converted + "\r\n";
            }
            if (fromForm.HomePhone != null && fromForm.HomePhone != "")
            {
                converted = converted + "\r\nH: " + fromForm.HomePhone;
            }
            if (fromForm.MobilePhone != null && fromForm.MobilePhone != "")
            {
                converted = converted + "\r\nM: " + fromForm.MobilePhone;
            }
            if (fromForm.WorkPhone != null && fromForm.WorkPhone != "")
            {
                converted = converted + "\r\nW: " + fromForm.WorkPhone;
            }
            if (fromForm.Email != null && fromForm.Email != ""
                || fromForm.Email2 != null && fromForm.Email2 != ""
                || fromForm.Email3 != null && fromForm.Email3 != "")
            {
                converted = converted + "\r\n";
            }
            if (fromForm.Email != null && fromForm.Email != "")
            {
                converted = converted + "\r\n" + fromForm.Email;
            }
            if (fromForm.Email2 != null && fromForm.Email2 != "")
            {
                converted = converted + "\r\n" + fromForm.Email2;
            }
            if (fromForm.Email3 != null && fromForm.Email3 != "")
            {
                converted = converted + "\r\n" + fromForm.Email3;
            }
            return converted.Trim();
        }
    }
}
