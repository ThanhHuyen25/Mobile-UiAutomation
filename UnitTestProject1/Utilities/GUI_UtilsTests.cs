using Microsoft.VisualStudio.TestTools.UnitTesting;
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation.Tests
{
    [TestClass()]
    public class GUI_UtilsTests
    {
        [TestMethod()]
        public void NormalizeStringTest()
        {
            string temp = GUI_Utils.NormalizeString("Header:Abc - Type__d");
            Assert.AreEqual(temp, "Header_Abc_Type_d");
        }
    }
}