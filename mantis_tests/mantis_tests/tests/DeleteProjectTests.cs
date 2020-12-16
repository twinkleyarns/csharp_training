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
            app.Projects.CreateProjectIfDoesNotExist(new ProjectData(DateTime.Now.ToString("mm/dd/yyyy hh:mm:ss")));

            List<ProjectData> oldProjects = app.Projects.GetProjectsList();
            ProjectData toBeRemoved = oldProjects[0];
            app.Projects.Remove(toBeRemoved);

            Assert.AreEqual(oldProjects.Count - 1, app.Projects.GetProjectsCount());

            List<ProjectData> newProjects = app.Projects.GetProjectsList();
            oldProjects.RemoveAt(0);
            Assert.AreEqual(oldProjects, newProjects);

            foreach (ProjectData project in newProjects)
            {
                Assert.AreNotEqual(project.Name, toBeRemoved.Name);
            }
        }
    }
}
