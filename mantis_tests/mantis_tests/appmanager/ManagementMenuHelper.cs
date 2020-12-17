using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace mantis_tests
{
    public class ManagementMenuHelper : HelperBase
    {
        private string baseURL;

        public ManagementMenuHelper(ApplicationManager manager, string baseURL)
            : base(manager)
        {
            this.baseURL = baseURL;
        }

        public void GoToProjectsPage()
        {
            if (driver.Url == (baseURL + "manage_proj_page.php")
                && IsElementPresent(By.XPath("//button[@type='submit']")))
            {
                return;
            }
            driver.FindElement(By.XPath("//li/a/span[contains(text(),'Manage')]")).Click();
            driver.FindElement(By.LinkText("Manage Projects")).Click();
        }
    }
}
