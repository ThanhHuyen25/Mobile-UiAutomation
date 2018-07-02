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
    public class RanorexRxrepAnalyzerTests
    {
        [TestMethod()]
        public void AnalyzeTest()
        {
            new RanorexRxrepAnalyzer().Analyze(@"D:\Research\projects\UI-Testing-Automation\examples\Mapping and UserCode\NewRepository-V20170822.rxrep");

        }
    }
}