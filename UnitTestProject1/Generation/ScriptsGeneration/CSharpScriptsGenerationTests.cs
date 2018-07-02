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
    public class CSharpScriptsGenerationTests
    {
        [TestMethod()]
        public void GenerateTest()
        {
            List<IElement> allElements = XmlFilesLoader.Load(
                @"C:\Users\duongtd\Desktop\samples\exported\ToDoTest2\ToDoTest2\ToDoTest2Repo.xml",
                @"C:\Users\duongtd\Desktop\samples\exported\ToDoTest2\ToDoTest2\ToDoTest2ImageCapture.xml");
            MyLog myLog = new MyLog();
            ExcelSpecificationParser excelSpecificationParser = new ExcelSpecificationParser();
            List<SpecScreen> specScreens = excelSpecificationParser.ParseWithRootElements(
                @"C:\Users\duongtd\Desktop\samples\TODO_spec_sample.xlsx",
                //@"D:\Research\projects\GUI-Testing-Automation\ProjectGenTemplate\Copy of TSDV-Sample-Mapping-Spec.xlsx",
                allElements, myLog);

            UserActionSpecAnalyzer specAnalyzer = new UserActionSpecAnalyzer();
            specAnalyzer.Expand(specScreens, @"C:\MyApp.exe", myLog);

            CSharpScriptsGeneration scriptsGeneration = new CSharpScriptsGeneration();
            scriptsGeneration.Generate(
                specScreens,
                @"D:\temp\out\project1",
                @"D:\temp\ElementsRepo.xml",
                @"D:\temp\ImageCapture.xml",
                @"D:\temp\app.exe",
                "MyTestProjectDefinition",
                "MyTestProject",
                "Instance",
                myLog);
        }

        [TestMethod()]
        public void GenerateTest1()
        {
            List<IElement> allElements = new RanorexRxrepAnalyzer().Analyze(@"C:\Users\duongtd\Desktop\samples\Ranorex\Sample1\NewRepository-V20170822.rxrep");
            MyLog myLog = new MyLog();
            ExcelSpecificationParser excelSpecificationParser = new ExcelSpecificationParser();
            List<SpecScreen> specScreens = excelSpecificationParser.ParseWithRootElements(
                @"C:\Users\duongtd\Desktop\samples\Ranorex\Sample1\TSDV-Sample-UserCode-Spec.xlsx",
                //@"D:\Research\projects\GUI-Testing-Automation\ProjectGenTemplate\Copy of TSDV-Sample-Mapping-Spec.xlsx",
                allElements, myLog);

            UserActionSpecAnalyzer specAnalyzer = new UserActionSpecAnalyzer();
            specAnalyzer.Expand(specScreens, @"C:\MyApp.exe", myLog);

            CSharpScriptsGeneration scriptsGeneration = new RanorexScriptsGeneration();
            scriptsGeneration.Generate(
                specScreens,
                @"D:\temp\out\project1",
                @"D:\temp\ElementsRepo.xml",
                @"D:\temp\ImageCapture.xml",
                @"D:\temp\app.exe",
                "MyTestProjectDefinition",
                "MyTestProject",
                "Instance",
                myLog);
        }
    }
}