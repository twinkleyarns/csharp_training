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
            ProjectData project = new ProjectData(DateTime.Now.ToString("mm/dd/yyyy hh:mm:ss"));
            List<ProjectData> oldProjects = app.Projects.GetProjectsList();
            app.Projects.Create(project);

            Assert.AreEqual(oldProjects.Count + 1, app.Projects.GetProjectsCount());

            List<ProjectData> newProjects = app.Projects.GetProjectsList();
            oldProjects.Add(project);
            oldProjects.Sort();
            newProjects.Sort();
            Assert.AreEqual(oldProjects, newProjects);
        }
    }
}
