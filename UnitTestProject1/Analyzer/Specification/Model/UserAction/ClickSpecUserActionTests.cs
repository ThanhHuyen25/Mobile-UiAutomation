using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI_Testing_Automation;

namespace TestingApplication.Tests
{
    [TestClass()]
    public class ClickSpecUserActionTests
    {
        [TestMethod()]
        public void GenRawRanorexScriptsTest()
        {
            ClickSpecUserAction clickSpecUserAction = new ClickSpecUserAction();
            clickSpecUserAction.Expression = @"Click[1;2]";// 'File' -> 'Open' -> 'Cloud'";

            List<IElement> allElements = new RanorexRxrepAnalyzer().Analyze(@"C:\Users\duongtd\Desktop\samples\Ranorex\Sample1\NewRepository-V20170822.rxrep");
            MyLog myLog = new MyLog();

            RanorexScriptGenerationParams param = new RanorexScriptGenerationParams();
            param.ListUIElements = new ListUIElements(ListElementsIndicator.OnlyRootElements, allElements);
            param.InstanceName = "repo";
            param.Id = 1;
            param.MyLog = myLog;
            param.ScreenName = "Testing";
            param.SpecNode = new SpecNode("MainForm")
            {
                UIElement = allElements[0],
            };
            var re = clickSpecUserAction.GenRanorexScripts(param);
        }
    }
}