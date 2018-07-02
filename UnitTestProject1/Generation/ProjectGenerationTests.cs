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
    public class ProjectGenerationTests
    {
        [TestMethod()]
        public void GenerateRanorexProjectTest()
        {
            string repoFilePath = @"C:\Users\duongtd\Desktop\samples\Ranorex\Sample1\NewRepository.rxrep";
            List<IElement> allElements = new RanorexRxrepAnalyzer().Analyze(repoFilePath);
            MyLog myLog = new MyLog();
            ExcelSpecificationParser excelSpecificationParser = new ExcelSpecificationParser();
            List<SpecScreen> specScreens = excelSpecificationParser.ParseWithRootElements(
                //@"C:\Users\duongtd\Desktop\samples\Ranorex\Sample1\TSDV-Sample-UserCode-Spec.xlsx",
                @"C:\Users\duongtd\Desktop\samples\Ranorex\Sample1\TSDV-Sample-Test.xlsx",
                //@"D:\Research\projects\GUI-Testing-Automation\ProjectGenTemplate\Copy of TSDV-Sample-Mapping-Spec.xlsx",
                allElements, myLog);

            UserActionSpecAnalyzer specAnalyzer = new UserActionSpecAnalyzer();
            specAnalyzer.Expand(specScreens, @"C:\MyApp.exe", myLog);

            ProjectGeneration projectGeneration = new ProjectGeneration();
            projectGeneration.GenerateRanorexProject(allElements,
                specScreens,
                repoFilePath,
                @"C:\Users\duongtd\Desktop\samples\Ranorex\exported",
                @"C:\MyApp.exe",
                myLog);
            System.Diagnostics.Debug.WriteLine(String.Join(",\n", myLog.GetErrorLogs().ToArray()));
        }
    }
}