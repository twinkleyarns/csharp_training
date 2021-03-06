﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;

namespace WebAddressbookTests
{
    public class ContactHelper : HelperBase
    {
        public ContactHelper(ApplicationManager manager)
            : base(manager)
        {
        }

        public ContactData GetContactInformationFromTable(int index)
        {
            manager.Navigator.GoToHomePage();
            IList<IWebElement> cells = driver.FindElements(By.Name("entry"))[index]
                .FindElements(By.TagName("td"));
            string lastName = cells[1].Text;
            string firstName = cells[2].Text;
            string address = cells[3].Text;
            string allEmails = cells[4].Text;
            string allPhones = cells[5].Text;

            return new ContactData(firstName, lastName)
            {
                Address = address,
                AllEmails = allEmails,
                AllPhones = allPhones
            };
        }

        public string GetContactInformationFromDetails(int index)
        {
            manager.Navigator.GoToHomePage();
            OpenContactDetails(index);

            string details = driver.FindElement(By.Id("content")).Text;

            return details;
        }

        public ContactData GetContactInformationFromEditForm(int index)
        {
            manager.Navigator.GoToHomePage();
            InitContactModification(index);

            string firstName = driver.FindElement(By.Name("firstname")).GetAttribute("value");
            string lastName = driver.FindElement(By.Name("lastname")).GetAttribute("value");
            string address = driver.FindElement(By.Name("address")).GetAttribute("value");

            string homePhone = driver.FindElement(By.Name("home")).GetAttribute("value");
            string mobilePhone = driver.FindElement(By.Name("mobile")).GetAttribute("value");
            string workPhone = driver.FindElement(By.Name("work")).GetAttribute("value");

            string email = driver.FindElement(By.Name("email")).GetAttribute("value");
            string email2 = driver.FindElement(By.Name("email2")).GetAttribute("value");
            string email3 = driver.FindElement(By.Name("email3")).GetAttribute("value");

            return new ContactData(firstName, lastName)
            {
                Address = address,
                HomePhone = homePhone,
                MobilePhone = mobilePhone,
                WorkPhone = workPhone,
                Email = email,
                Email2 = email2,
                Email3 = email3
            };
        }

        public ContactHelper Create(ContactData contact)
        {
            InitContactCreation();
            FillContactForm(contact);
            SubmitContactCreation();
            ReturnToHomePage();
            return this;
        }

        public ContactHelper Modify(int p, ContactData newData)
        {
            InitContactModification(p);
            FillContactForm(newData);
            SubmitContactModification();
            ReturnToHomePage();
            return this;
        }

        public ContactHelper Modify(ContactData oldData, ContactData newData)
        {
            InitContactModification(oldData.Id);
            FillContactForm(newData);
            SubmitContactModification();
            ReturnToHomePage();
            return this;
        }

        public ContactHelper Remove(int p)
        {
            SelectContactCheckbox(p);
            ConfirmContactDeletion();
            // wait for remove
            int attempt = 0;
            while (attempt < 5)
            {
                Thread.Sleep(1000);
                if (IsElementPresent(By.XPath("//input[@value='Send e-Mail']"))) break;
                attempt++;
            }
            return this;
        }

        public ContactHelper Remove(ContactData contact)
        {
            SelectContactCheckbox(contact.Id);
            ConfirmContactDeletion();
            // wait for remove
            int attempt = 0;
            while (attempt < 5)
            {
                Thread.Sleep(1000);
                if (IsElementPresent(By.XPath("//input[@value='Send e-Mail']"))) break;
                attempt++;
            }
            return this;
        }

        public ContactHelper CreateContactIfDoesNotExist(ContactData newData)
        {
            manager.Navigator.GoToHomePage();
            if (!IsThereAnyContactCreated())
            {
                Create(newData);
            }
            return this;
        }

        public bool IsThereAnyContactCreated()
        {
            return IsElementPresent(By.XPath("//tr/td/input"));
        }

        public ContactHelper ConfirmContactDeletion()
        {
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();
            driver.SwitchTo().Alert().Accept();
            contactCache = null;
            return this;
        }

        public ContactHelper SubmitContactModification()
        {
            driver.FindElement(By.XPath("(//input[@name='update'])[2]")).Click();
            contactCache = null;
            return this;
        }

        public ContactHelper SelectContactCheckbox(int index)
        {
            driver.FindElement(By.XPath("//tr[" + (index+2) + "]/td/input")).Click();
            return this;
        }

        public ContactHelper SelectContactCheckbox(String id)
        {
            driver.FindElement(By.XPath("//input[@value='" + id + "']")).Click();
            return this;
        }

        public ContactHelper InitContactModification(int index)
        {
            driver.FindElement(By.XPath("(//img[@alt='Edit'])[" + (index + 1) + "]")).Click();
            return this;
        }

        public ContactHelper InitContactModification(String id)
        {
            driver.FindElement(By.XPath("//a[@href='edit.php?id=" + id + "']/img")).Click();
            return this;
        }

        public ContactHelper OpenContactDetails(int index)
        {
            driver.FindElement(By.XPath("(//img[@alt='Details'])[" + (index + 1) + "]")).Click();
            return this;
        }

        public ContactHelper ReturnToHomePage()
        {
            driver.FindElement(By.LinkText("home page")).Click();
            return this;
        }

        public ContactHelper SubmitContactCreation()
        {
            driver.FindElement(By.XPath("(//input[@name='submit'])[2]")).Click();
            contactCache = null;
            return this;
        }

        public ContactHelper FillContactForm(ContactData contact)
        {
            Type(By.Name("firstname"), contact.FirstName);
            Type(By.Name("lastname"), contact.LastName);
            return this;
        }

        public ContactHelper InitContactCreation()
        {
            driver.FindElement(By.LinkText("add new")).Click();
            return this;
        }

        private List<ContactData> contactCache = null;

        public List<ContactData> GetContactsList()
        {
            if (contactCache == null)
            {
                manager.Navigator.GoToHomePage();
                contactCache = new List<ContactData>();
                ICollection<IWebElement> elements = driver.FindElements(By.CssSelector("tr[name='entry']"));
                foreach (IWebElement element in elements)
                {
                    contactCache.Add(new ContactData(element.FindElement(By.CssSelector("*:nth-child(3)")).Text,
                        element.FindElement(By.CssSelector("*:nth-child(2)")).Text));
                }
            }
            return new List<ContactData>(contactCache);
        }

        public int GetContactCount()
        {
            return driver.FindElements(By.CssSelector("tr[name='entry']")).Count;
        }

        public int GetNumberOfSearchResults()
        {
            manager.Navigator.GoToHomePage();
            string text = driver.FindElement(By.TagName("label")).Text;
            Match m = new Regex(@"\d+").Match(text);
            return Int32.Parse(m.Value);
        }

        public string ConvertContactEditFormToDetailsFormat(ContactData fromForm)
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
                converted = converted + "\r\n" + fromForm.Email.Trim();
            }
            if (fromForm.Email2 != null && fromForm.Email2 != "")
            {
                converted = converted + "\r\n" + fromForm.Email2.Trim();
            }
            if (fromForm.Email3 != null && fromForm.Email3 != "")
            {
                converted = converted + "\r\n" + fromForm.Email3.Trim();
            }
            return converted.Trim();
        }

        public void AddContactToGroup(ContactData contact, GroupData group)
        {
            manager.Navigator.GoToHomePage();
            ClearGroupFilter();
            SelectContactCheckbox(contact.Id);
            SelectGroupToAdd(group.Name);
            CommitAddingContactToGroup();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElements(By.CssSelector("div.msgbox")).Count() > 0);
        }

        public void RemoveContactFromGroup(ContactData contact, GroupData group)
        {
            manager.Navigator.GoToHomePage();
            SetGroupFilter(group.Name);
            SelectContactCheckbox(contact.Id);
            CommitRemovingContactFromGroup();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElements(By.CssSelector("div.msgbox")).Count() > 0);
        }

        public void CommitAddingContactToGroup()
        {
            driver.FindElement(By.Name("add")).Click();
        }

        public void CommitRemovingContactFromGroup()
        {
            driver.FindElement(By.Name("remove")).Click();
        }

        public void SelectGroupToAdd(string name)
        {
            new SelectElement(driver.FindElement(By.Name("to_group"))).SelectByText(name);
        }

        public void SetGroupFilter(string name)
        {
            new SelectElement(driver.FindElement(By.Name("group"))).SelectByText(name);
        }

        public void ClearGroupFilter()
        {
            new SelectElement(driver.FindElement(By.Name("group"))).SelectByText("[all]");
        }
    }
}
