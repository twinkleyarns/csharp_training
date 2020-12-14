using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace addressbook_tests_white
{
    [TestFixture]
    public class GroupRemovingTests : TestBase
    {
        [Test]
        public void TestGroupRemoving()
        {
            List<GroupData> oldGroups = app.Groups.GetGroupList();

            app.Groups.Remove();
            List<GroupData> newGroups = app.Groups.GetGroupList();
            oldGroups.Remove(oldGroups[0]);
            oldGroups.Sort();
            newGroups.Sort();

            Assert.AreEqual(oldGroups, newGroups);
        }
    }
}
