using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication.Tests
{
    [TestClass()]
    public class ProjectRebuildingTests
    {
        [TestMethod()]
        public void CompileTest()
        {

        }

        [TestMethod()]
        public void ParseLogBuildTest()
        {
            ProjectRebuilding p = new ProjectRebuilding();
            LogProcess l = p.ParseLogBuild(Utils.ReadFileContent(@"..\..\..\TestingApplication\Log Output\building.log"));
            Assert.AreEqual(l.Warning_count, 0);
            Assert.AreEqual(l.Error_count, 2);
        }
    }
}