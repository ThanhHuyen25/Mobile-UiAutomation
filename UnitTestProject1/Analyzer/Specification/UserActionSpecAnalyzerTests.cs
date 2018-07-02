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
    public class UserActionSpecAnalyzerTests
    {
        [TestMethod()]
        public void ExpandTest()
        {
            List<IElement> allElements = XmlFilesLoader.Load(
                @"D:\Research\projects\GUI-Testing-Automation\Solution Utilities\exported projects\MyTestProject\MyTestProject\MyTestProjectRepo.xml",
                @"D:\Research\projects\GUI-Testing-Automation\Solution Utilities\exported projects\MyTestProject\MyTestProject\MyTestProjectImageCapture.xml");
            MyLog myLog = new MyLog();
            ExcelSpecificationParser excelSpecificationParser = new ExcelSpecificationParser();
            List<SpecScreen> specScreens = excelSpecificationParser.ParseWithRootElements(
                @"C:\Users\duongtd\Desktop\TSDV-Sample-UserCode-Spec.xlsx",
                //@"D:\Research\projects\GUI-Testing-Automation\ProjectGenTemplate\Copy of TSDV-Sample-Mapping-Spec.xlsx",
                allElements, myLog);

            UserActionSpecAnalyzer specAnalyzer = new UserActionSpecAnalyzer();
            //specAnalyzer.Expand(specScreens, @"C:\MyApp.exe", myLog);
        }
    }
}