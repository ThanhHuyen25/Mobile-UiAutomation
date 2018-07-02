// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 3:17 PM 2018/1/21
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestingApplication
{
    public class ExcelSpecificationParser
    {
        private static Tuple<string, string> SHEET_NAME_TEMPLATE1 =
            new Tuple<string, string>("^TestSpecification_(.*)?", "^FunctionDescription_(.*)?");
        private static Tuple<string, string>[] SHEET_NAME_TEMPLATE_LIST = new Tuple<string, string>[]
            {SHEET_NAME_TEMPLATE1};

        private const string NO = "No.";
        private const string XLS = "xls";
        private const string XLSX = "xlsx";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="rootElements">all root elements (not contain children)</param>
        /// <param name="myLog"></param>
        public List<SpecScreen> ParseWithRootElements(string filePath, List<IElement> rootElements, MyLog myLog)
        {
            //handleSheetsType(
            return DoParse(filePath, //myLog), 
                new ListUIElements(ListElementsIndicator.OnlyRootElements, rootElements), myLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="allElements">a list of all ui elements</param>
        /// <param name="myLog"></param>
        public List<SpecScreen> ParseWithListAllElements(string filePath, List<IElement> allElements, MyLog myLog)
        {
            //handleSheetsType(
            return DoParse(filePath, //myLog,
                new ListUIElements(ListElementsIndicator.AllElements, allElements), myLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="elementsAndIndicator">indication to determine root elements or list of all elements</param>
        /// <param name="myLog"></param>
        /// <returns></returns>
        private List<SpecScreen> DoParse(string filePath, ListUIElements elementsAndIndicator, MyLog myLog)
        {
            string newFilePath = Utils.CreateTempFile(filePath);
            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(newFilePath);
            //Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];

            List<AbstractSheet> listAbstractSheets = new List<AbstractSheet>();
            for (int fi = 1; fi <= xlWorkbook.Sheets.Count; fi++)
            {
                Excel._Worksheet sheet = xlWorkbook.Sheets[fi];
                string name = sheet.Name;
                AbstractSheet abstractSheet = null;
                foreach (Tuple<string, string> pairSheetNameRule in SHEET_NAME_TEMPLATE_LIST)
                {
                    Regex regex = new Regex(pairSheetNameRule.Item1);
                    MatchCollection matches = regex.Matches(name);
                    if (matches.Count > 0)
                    {
                        string featureName = matches[0].Groups[1].Value;
                        abstractSheet = new TestSpecificationSheet(featureName, sheet);
                    }
                    else
                    {
                        Regex regex2 = new Regex(pairSheetNameRule.Item2);
                        MatchCollection matches2 = regex2.Matches(name);
                        if (matches2.Count > 0)
                        {
                            string featureName = matches2[0].Groups[1].Value;
                            abstractSheet = new FunctionDescriptionSheet(featureName, sheet);
                        }
                    }
                }
                if (abstractSheet == null)
                    abstractSheet = new NormalSheet(name, sheet);
                listAbstractSheets.Add(abstractSheet);
            }
            List<NormalSheet> normalSheets = new List<NormalSheet>();
            List<Tuple<TestSpecificationSheet, FunctionDescriptionSheet>> pairSheets =
                new List<Tuple<TestSpecificationSheet, FunctionDescriptionSheet>>();

            foreach (AbstractSheet abstractSheet in listAbstractSheets)
            {
                if (abstractSheet is NormalSheet)
                    normalSheets.Add((NormalSheet)abstractSheet);
                else if (abstractSheet is TestSpecificationSheet)
                {
                    FunctionDescriptionSheet funcDescSheet =
                        findFuncDesc(abstractSheet.getFeatureName(), listAbstractSheets);
                    pairSheets.Add(new Tuple<TestSpecificationSheet, FunctionDescriptionSheet>(
                        (TestSpecificationSheet)abstractSheet, funcDescSheet));
                }
            }
            List<SpecScreen> screenList = new List<SpecScreen>();
            foreach (Tuple<TestSpecificationSheet, FunctionDescriptionSheet> pairSheet in pairSheets)
            {
                SpecScreen screen = parsePairSheet(pairSheet, elementsAndIndicator, myLog);
                if (screen != null)
                    screenList.Add(screen);
            }
            foreach (NormalSheet normalSheet in normalSheets)
            {
                SpecScreen screen = parseNormalSheet(normalSheet, elementsAndIndicator, myLog);
                if (screen != null)
                    screenList.Add(screen);
            }
            //return screenList;
            //cleanup
            //GC.Collect();
            //GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //close and release
            xlWorkbook.Close(false);
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
            return screenList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pairSheet"></param>
        /// <param name="elementsAndIndicator">indication to determine root elements or list of all elements</param>
        /// <param name="myLog"></param>
        /// <returns></returns>
        private TestSpecificationScreen parsePairSheet(Tuple<TestSpecificationSheet, FunctionDescriptionSheet> pairSheet,
                                ListUIElements elementsAndIndicator, MyLog myLog)
        {
            FunctionSheetParser functionSheetParser = new FunctionSheetParser();
            List<ClassExpression> classExpressions = functionSheetParser.parse(pairSheet.Item2, myLog);

            TestSpecSheetParser testSpecSheetParser = new TestSpecSheetParser();
            return testSpecSheetParser.parse(pairSheet.Item1, elementsAndIndicator, myLog, classExpressions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="normalSheet"></param>
        /// <param name="elementsAndIndicator">indication to determine root elements or list of all elements</param>
        /// <param name="myLog"></param>
        /// <returns></returns>
        private SpecScreen parseNormalSheet(NormalSheet normalSheet, ListUIElements elementsAndIndicator, MyLog myLog)
        {
            NormalSheetParser normalSheetParser = new NormalSheetParser();
            return normalSheetParser.Parse(normalSheet, elementsAndIndicator, myLog);
        }

        private FunctionDescriptionSheet findFuncDesc(String featureName, List<AbstractSheet> abstractSheetList)
        {
            foreach (AbstractSheet abstractSheet in abstractSheetList)
            {
                if (abstractSheet is FunctionDescriptionSheet &&
                        abstractSheet.getFeatureName().Equals(featureName))
                return (FunctionDescriptionSheet)abstractSheet;
            }
            return null;
        }
    }
}
