using System;
using TestingApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string csCode = File.ReadAllText(@"Resources\abc.txt");
            var text = Utils.ReformatCsCode(csCode);
        }
    }
}
