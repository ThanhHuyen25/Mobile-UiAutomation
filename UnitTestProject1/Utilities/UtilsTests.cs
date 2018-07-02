using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TestingApplication.Tests
{
    [TestClass()]
    public class UtilsTests
    {
        [TestMethod()]
        public void SplitIgnoreInsideTest()
        {
            var children = Utils.SplitIgnoreInside("'ABCDEFG', 123542, 'XYZ 99,9'", ",", "'");
            Assert.AreEqual(children[0].Trim(), "'ABCDEFG'");
            Assert.AreEqual(children[1].Trim(), "123542");
            Assert.AreEqual(children[2].Trim(), "'XYZ 99,9'");
        }

        [TestMethod()]
        public void SplitIgnoreInsideTest1()
        {
            String text = "'ad;fg';gbg;Click;;'gh;fg';abd";
            String[] split = Utils.SplitIgnoreInside(text, ";", "'");
            System.Diagnostics.Debug.WriteLine(text);
            foreach (String s in split)
            {
                System.Diagnostics.Debug.WriteLine(s);
            }
        }
    }
}