using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;

namespace mantis_tests
{
    [TestFixture]
    public class AddProjectTests : AuthTestBase
    {
        [Test]
        public void AddProjectTest()
        {
            AccountData account = new AccountData()
            {
                Name = "administrator",
                Password = "root"
            };
            ProjectData project = new ProjectData(DateTime.Now.ToString("mm/dd/yyyy hh:mm:ss"));
            List<ProjectData> oldProjects = app.API.GetProjectsList(account);
            app.API.Create(account, project);

            Assert.AreEqual(oldProjects.Count + 1, app.Projects.GetProjectsCount());

            List<ProjectData> newProjects = app.API.GetProjectsList(account);
            oldProjects.Add(project);
            oldProjects.Sort();
            newProjects.Sort();
            Assert.AreEqual(oldProjects, newProjects);
        }
    }
}
