using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;

namespace mantis_tests
{
    [TestFixture]
    public class DeleteProjectTests : AuthTestBase
    {
        [Test]
        public void DeleteProjectTest()
        {
            AccountData account = new AccountData()
            {
                Name = "administrator",
                Password = "root"
            };

            app.Projects.CreateProjectIfDoesNotExist(new ProjectData(DateTime.Now.ToString("mm/dd/yyyy hh:mm:ss")));

            List<ProjectData> oldProjects = app.API.GetProjectsList(account);
            ProjectData toBeRemoved = oldProjects[0];
            app.Projects.Remove(toBeRemoved);

            Assert.AreEqual(oldProjects.Count - 1, app.Projects.GetProjectsCount());

            List<ProjectData> newProjects = app.API.GetProjectsList(account);
            oldProjects.RemoveAt(0);
            oldProjects.Sort();
            newProjects.Sort();
            Assert.AreEqual(oldProjects, newProjects);

            foreach (ProjectData project in newProjects)
            {
                Assert.AreNotEqual(project.Name, toBeRemoved.Name);
            }
        }
    }
}
