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
    public class DropSpecUserActionTests
    {
        [TestMethod()]
        public void GenRawRanorexScriptsTest()
        {
            string expression = "Drop (23, 45)";
            Regex pattern1 = new Regex("Drop\\s+(?<word>\\w+)");
            Regex pattern2 = new Regex("Drop\\s*\\((?<first>\\d+),\\s*(?<second>\\d+)\\)");
            Match matcher1 = pattern1.Match(expression);
            Match matcher2 = pattern2.Match(expression);
            if (matcher1.Success)
            {
                var s = matcher1.Groups["word"].Value;
            }
            else if (matcher2.Success)
            {
                string arg1Str = matcher2.Groups["first"].Value;
                string arg2Str = matcher2.Groups["second"].Value;
            }
        }
    }
}