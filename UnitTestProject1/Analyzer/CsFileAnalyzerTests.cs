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
    public class CsFileAnalyzerTests
    {
        [TestMethod()]
        public void ProcessTest()
        {
            CsFileAnalyzer csFileAnalyzer = new CsFileAnalyzer();
            CsFile csFile = new CsFile(@"..\..\..\Solution Utilities\WindowsFormsApp1\Program1.cs");
            csFileAnalyzer.CsFile = csFile;
            csFileAnalyzer.Process();
            Assert.AreEqual(csFile.Classes[0].Name, "MainWindow");
            Assert.AreEqual(csFile.Classes[0].BaseOnClass[0], "Window");
        }
    }
}