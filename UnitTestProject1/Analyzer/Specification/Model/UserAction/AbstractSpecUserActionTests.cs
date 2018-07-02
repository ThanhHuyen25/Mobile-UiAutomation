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
    public class AbstractSpecUserActionTests
    {
        [TestMethod()]
        public void GetScriptAccessElementTest()
        {
            List<IElement> allElements = XmlFilesLoader.Load(
                @"D:\Research\projects\GUI-Testing-Automation\Solution Utilities\exported projects\MyTestProject\MyTestProject\MyTestProjectRepo.xml",
                @"D:\Research\projects\GUI-Testing-Automation\Solution Utilities\exported projects\MyTestProject\MyTestProject\MyTestProjectImageCapture.xml");
            Assert.AreEqual("CaptureElement.CaptureScreen(Window_MainWindowForm.Button_Copy);",
                new CaptureSpecUserAction().GetScriptCapture(allElements[0].Children[0], "elements"));
        }

        [TestMethod()]
        public void GetScriptAccessElementTest1()
        {
            List<IElement> allElements = XmlFilesLoader.Load(
                @"D:\Research\projects\GUI-Testing-Automation\Solution Utilities\exported projects\MyTestProject\MyTestProject\MyTestProjectRepo.xml",
                @"D:\Research\projects\GUI-Testing-Automation\Solution Utilities\exported projects\MyTestProject\MyTestProject\MyTestProjectImageCapture.xml");
            //Assert.AreEqual("Window_MainWindowForm.Button_Copy",
            //new CaptureSpecUserAction().GetScriptAccessElement(allElements[0].Children[0], "elements"));
        }

        [TestMethod()]
        public void GetScriptAccessRanorexElementTest()
        {
            RanorexScriptGenerationParams _params = new RanorexScriptGenerationParams();
            _params.SpecNode = new SpecNode("abc")
            {
                UIElement = new ButtonElement
                {
                    Attributes = new ElementAttributes { Name = "Button1" }
                }
            };
            _params.SpecNode.Attribute = "Row(1)"; //"Cell(1,1)";
            String re = AbstractSpecUserAction.getScriptAccessRanorexObject(_params);
        }
    }
}