using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI_Testing_Automation;
using System.Text.RegularExpressions;

namespace TestingApplication.Tests
{
    [TestClass()]
    public class ExcelSpecificationParserTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            List<IElement> allElements = XmlFilesLoader.Load(
                @"D:\Research\projects\GUI-Testing-Automation\Solution Utilities\exported projects\MyTestProject\MyTestProject\MyTestProjectRepo.xml",
                @"D:\Research\projects\GUI-Testing-Automation\Solution Utilities\exported projects\MyTestProject\MyTestProject\MyTestProjectImageCapture.xml");
            ExcelSpecificationParser excelSpecificationParser = new ExcelSpecificationParser();
            excelSpecificationParser.ParseWithRootElements(
                //@"D:\Research\projects\UI-Testing-Automation\examples\Mapping and UserCode\TSDV-Sample-UserCode-Spec.xlsx",
                @"D:\Research\projects\GUI-Testing-Automation\ProjectGenTemplate\Copy of TSDV-Sample-Mapping-Spec.xlsx",
                allElements, new MyLog());
        }

        [TestMethod()]
        public void Call1()
        {
            
        }
    }
}