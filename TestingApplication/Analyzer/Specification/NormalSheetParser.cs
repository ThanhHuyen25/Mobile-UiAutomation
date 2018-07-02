// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 5:12 PM 2018/1/21
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestingApplication
{
    public class NormalSheetParser
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int ROW_TITLE_TESTCASE = 1;
        private const string NO = "No.";

        private const string ALIAS_NAME_COLUMN_TITLE = "(?i)^ALIAS NAME$";
        private const string REAL_NAME_COLUMN_TITLE = "(?i)^REAL NAME$";

        private const int ROOT_ELEMENTS_INDICATOR = 0;
        private const int LIST_ALL_ELEMENTS_INDICATOR = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="normalSheet"></param>
        /// <param name="elementsAndIndicator">indication to determine root elements or list of all elements</param>
        /// <param name="myLog"></param>
        /// <returns></returns>
        public SpecScreen Parse(AbstractSheet normalSheet, ListUIElements elementsAndIndicator, MyLog myLog)
        {
            return parserOneSheet(normalSheet.getSheet(), elementsAndIndicator, myLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="elementsAndIndicator">indication to determine root elements or list of all elements</param>
        /// <param name="myLog"></param>
        /// <returns></returns>
        private SpecScreen parserOneSheet(Excel._Worksheet sheet,
                                   ListUIElements elementsAndIndicator,
                                   MyLog myLog)
        {
            
            Excel.Range xlRange = sheet.UsedRange;
            int rowCount = xlRange.Rows.Count;
            int columnCount = xlRange.Columns.Count;

            SpecScreen screen = new SpecScreen();
            // get mapping alias
            Dictionary<string, string> mapAliasWithNode = getMappingAlias(sheet, rowCount, columnCount);
            screen.MappingAliasWithNode = mapAliasWithNode;
            /**
             * read first row to get all element Nodes
             */
            //Row firstRow = sheet.getRow(ROW_TITLE_TESTCASE);
            string no = getCellValue(sheet, ROW_TITLE_TESTCASE, 1);
            //if (searchElement(no, null, allUIElements) == null && !no.Equals(NO, stringComparison.OrdinalIgnoreCase))
            //{
            //    logWarnings.add("First column must numbered column!");
            //    logger.error("First column must numbered column!");
            //}

            List<SpecNode> listSpecNodes = new List<SpecNode>();
            // <start, end>-s
            List<Tuple<int, int>> listValidationUsercode = new List<Tuple<int, int>>();
            // get each node element
            //int lastColumn = firstRow.getLastCellNum();
            bool link = false;
            int startColumnIdx = 2;
            for (int fi = startColumnIdx; fi <= columnCount; fi++)
            {
                string value = getCellValue(sheet, ROW_TITLE_TESTCASE, fi);
                if (value == null || value.Trim().Equals(""))
                {
                    if (checkLastColumn(sheet, fi, rowCount))
                    {
                        columnCount = fi - 1;
                        break;
                    }
                    else
                    {
                        if (listValidationUsercode.Count == 0)
                            listValidationUsercode.Add(new Tuple<int, int>(fi - 1 - startColumnIdx, fi - startColumnIdx));
                        else
                        {
                            Tuple<int, int> lastPair = listValidationUsercode[listValidationUsercode.Count - 1];
                            if ((fi - lastPair.Item2 - startColumnIdx) == 1)
                            {
                                listValidationUsercode.RemoveAt(listValidationUsercode.Count - 1);
                                listValidationUsercode.Add(new Tuple<int, int>(lastPair.Item1, fi - startColumnIdx));
                            }
                            else
                                listValidationUsercode.Add(new Tuple<int, int>(fi - 1 - startColumnIdx, fi - startColumnIdx));
                        }
                        listSpecNodes.Add(listSpecNodes.Last());
                    }
                }
                else
                {
                    if (Regex.IsMatch(value, AbstractSpecUserAction.LINKS))
                    {
                        columnCount = fi - 1;
                        link = true;
                        break;
                    }
                    else
                    {
                        SpecNode specNode = parseNode(value, elementsAndIndicator, mapAliasWithNode, myLog);
                        if (specNode == null)
                            return null;
                        listSpecNodes.Add(specNode);
                    }
                }
            }
            screen.Name = sheet.Name;
            screen.AllUIElements = elementsAndIndicator;
            screen.ListSpecNodes = listSpecNodes;
            screen.ListValidationUserCode = listValidationUsercode;
            /**
             * parseOneFile each scenario
             */
            for (int fi = ROW_TITLE_TESTCASE + 1; fi <= rowCount; fi++)
            {
                //Row row = sheet.getRow(fi);
                bool isRowNotTest = true;
                if (getCellValue(sheet, fi, 1) == null || getCellValue(sheet, fi, 2) == null)
                {
                    rowCount = fi - 1;
                    break;
                }
                SpecScenario scenario = new SpecScenario();
                for (int se = 2; se <= columnCount; se++)
                {
                    Color color = Color.Empty;
                    string value = getCellValue(sheet, fi, se);
                    /**
                     * check if result indicate 'not_test'
                     */
                    if (listSpecNodes[se - 2].Expression.Equals(AbstractSpecUserAction.RESULT))
                    {
                        if (value.Equals(AbstractSpecUserAction.NOT_TEST))
                        {
                            isRowNotTest = true;
                            break;
                        }
                        else if (value.Equals(AbstractSpecUserAction.TEST) || value.Equals("")) ;
                        else
                            myLog.Warn("[WARNING] Result must be 'Test' or 'Not Test', not " + value +
                                    ", it will be treated as 'Test' by default", logger);
                    }
                    color = getCellBgColor(sheet, fi, se);
                    if (color.Equals(Color.Empty) || (
                            !color.Equals(AbstractSpecUserAction.NOT_TEST_COLOR1) &&
                                    !color.Equals(AbstractSpecUserAction.NOT_TEST_COLOR2)))
                        isRowNotTest = false;
                    scenario.UserActionsInString.Add(value);
                    scenario.Colors.Add(color);
                }
                if (isRowNotTest)
                    continue;
                /**
                 * pre-condition
                 */
                string linkExpress = "";
                string lastCellValue = getCellValue(sheet, fi, columnCount + 1);
                if (link && lastCellValue != null && !lastCellValue.Equals(""))
                {
                    linkExpress = lastCellValue;
                    string[] preConsExp = linkExpress.Split(',');
                    foreach (string preConExp in preConsExp)
                    {
                        scenario.PreConditionsExpression.Add(preConExp.Trim());
                    }
                }
                scenario.Id = fi - ROW_TITLE_TESTCASE;
                //scenario.ScreenName = screen.Name;
                if (screen.Scenarios == null)
                    screen.Scenarios = new List<IScenario>();
                screen.Scenarios.Add(scenario);
            }
            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(sheet);
            return screen;
        }

        private SpecNode parseNode(string expression, ListUIElements elementsAndIndicator, 
            Dictionary<string, string> mappingAlias, MyLog myLog)
        {
            /**
             * determine if Delay
             */
            if (Regex.IsMatch(expression, AbstractSpecUserAction.DELAY_TITLE_EXPRESSION))
            {
                return new SpecNode(expression);
            }
            /**
             * determine if Wait
             */
            if (Regex.IsMatch(expression, AbstractSpecUserAction.PRE_WAIT + "(_\\d*)*") ||
                    Regex.IsMatch(expression, AbstractSpecUserAction.POST_WAIT + "(_\\d*)*"))
            {
                return new SpecNode(expression);
            }
            /**
             * determine if Keyboard
             */
            if (Regex.IsMatch(expression, AbstractSpecUserAction.KEYBOARD_PRESS_REGEX))
            {
                return new SpecNode(expression);
            }
            /**
             * determine if Result
             */
            if (Regex.IsMatch(expression, AbstractSpecUserAction.RESULT))
            {
                return new SpecNode(expression);
            }
            /**
             * determine if UserCode
             */
            if (Regex.IsMatch(expression, AbstractSpecUserAction.USER_CODE))
            {
                return new SpecNode(expression);
            }
            if (Regex.IsMatch(expression, AbstractSpecUserAction.USER_CODE_WITH_VARIABLE_DECLARE))
            {
                return new SpecNode(expression);
            }
            Tuple<IElement, string> pair =
                    parseNodeAndAttribute(expression, elementsAndIndicator, mappingAlias, myLog);
            if (pair == null)
                return null;
            return new SpecNode(pair.Item1, pair.Item2, expression);
        }

        public static Tuple<IElement, string> parseNodeAndAttribute(string expression, ListUIElements elementsAndIndicator,
                                                               Dictionary<string, string> mappingAlias, MyLog myLog)
        {
            if (mappingAlias != null && mappingAlias.ContainsKey(expression))
                expression = mappingAlias[expression];
            string[] subs = expression.Split('.');
            string screenName = null; string nodeName = null; string attribute = null;
            IElement element = null;
            
            //case MainForm
            if (subs.Length == 1)
            {
                nodeName = subs[0];
                element = searchElement(nodeName, screenName, elementsAndIndicator);
            }
            //case MainForm.Button
            else if (subs.Length == 2)
            {
                screenName = subs[0];
                nodeName = subs[1];
                element = searchElement(nodeName, screenName, elementsAndIndicator);
                //case MainForm.Text
                if (element == null)
                {
                    attribute = nodeName;
                    element = searchElement(screenName, null, elementsAndIndicator);
                }
            }
            //case MainForm.Button.Text and MainForm.DataGridView.Cell(1,1).Text
            else
            {
                screenName = subs[0];
                nodeName = subs[1];
                attribute = subs[2];
                for (int i = 3; i < subs.Length; i++)
                    attribute += "." + subs[i];
                element = searchElement(nodeName, screenName, elementsAndIndicator);
            }
            if (element == null)
            {
                string alias = expression.Split('.')[0];
                if (mappingAlias != null && mappingAlias.ContainsKey(alias))
                    return parseNodeAndAttribute(expression.Replace(alias, mappingAlias[alias]),
                            elementsAndIndicator, mappingAlias, myLog);
                else
                {
                    myLog.Warn("Can not find element " + expression, logger);
                    return null;
                }
            }
            return new Tuple<IElement, string>(element, attribute);
        }

        private Dictionary<string, string> getMappingAlias(Excel._Worksheet sheet, int rowCount, int columnCount)
        {
            for (int fi = rowCount; fi >= 1; fi--)
            {
                for (int se = 1; se <= columnCount; se++)
                {
                    string value = getCellValue(sheet, fi, se);
                    if (value != null && Regex.IsMatch(value, ALIAS_NAME_COLUMN_TITLE))
                    {
                        int realNameColumnIdx = findRealNameColumnIndex(sheet, fi, se, columnCount);
                        if (realNameColumnIdx > 0)
                            return getMappingAlias(sheet, fi, se, realNameColumnIdx, rowCount, columnCount);
                    }
                }
            }
            return null;
        }

        private Dictionary<string, string> getMappingAlias(Excel._Worksheet sheet, int rowIdx,
                                                int aliasNameColumnIdx, int realNameColumnIdx, int rowCount, int columnCount)
        {
            Dictionary<string, string> re = new Dictionary<string, string>();
            for (int fi = rowIdx + 1; fi <= rowCount; fi++)
            {
                string alias = getCellValue(sheet, fi, aliasNameColumnIdx);
                string real = getCellValue(sheet, fi, realNameColumnIdx);
                if (alias == null || alias.Equals("") || real == null || real.Equals(""))
                    break;
                re.Add(alias, real);
            }
            return re;
        }

        private static IElement searchElement(string name, string screenName, ListUIElements elementsAndIndicator)
        {
            List<IElement> elements = elementsAndIndicator.Elements;
            ListElementsIndicator indicator = elementsAndIndicator.Indicator;
            return DoSearchElement(name, screenName, elements, indicator == ListElementsIndicator.OnlyRootElements);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="screenName"></param>
        /// <param name="listElements"></param>
        /// <param name="searchRecursive"> do search recursively</param>
        /// <returns></returns>
        private static IElement DoSearchElement(string name, string screenName, List<IElement> listElements, bool searchRecursive)
        {
            foreach (IElement element in listElements)
            {
                if (element.Attributes.Name.Equals(name))
                {
                    if (screenName == null || screenName.Equals(""))
                        return element;
                    IElement tempElement = element.Parent;
                    while (tempElement != null)
                    {
                        if (tempElement.Attributes.Name.Equals(screenName))
                            return element;
                        tempElement = tempElement.Parent;
                    }
                }
                if (searchRecursive)
                {
                    List<IElement> children = element.Children;
                    if (children != null && children.Count > 0)
                    {
                        IElement temp = DoSearchElement(name, screenName, children, searchRecursive);
                        if (temp != null)
                            return temp;
                    }
                }
            }
            return null;
        }

        private int findRealNameColumnIndex(Excel._Worksheet sheet, int rowIdx, int columnIdx, int columnCount)
        {
            for (int fi = columnIdx + 1; fi <= columnCount; fi++)
            {
                string cellValue = getCellValue(sheet, rowIdx, fi);
                if (cellValue != null && Regex.IsMatch(cellValue, REAL_NAME_COLUMN_TITLE))
                    return fi;
            }
            return -1;
        }

        private Color getCellBgColor(Excel._Worksheet sheet, int rowIdx, int columnIdx)
        {
            return ColorTranslator.FromOle((int)sheet.Cells[rowIdx, columnIdx].Interior.Color);
        }

        private bool checkLastColumn(Excel._Worksheet sheet, int columnIdx, int rowCount)
        {
            for (int fi = 1; fi <= rowCount; fi++)
            {
                if (getCellValue(sheet, fi, columnIdx) != null &&
                    !getCellValue(sheet, fi, columnIdx).Equals(""))
                    return false;
            }
            return true;
        }

        private string getCellValue(Excel._Worksheet sheet, int rowIdx, int columnIdx)
        {
            if (sheet.Cells[rowIdx, columnIdx] == null ||
                    sheet.Cells[rowIdx, columnIdx].Value2 == null)
                return null;
            return sheet.Cells[rowIdx, columnIdx].Value2.ToString().Trim();
        }
    }
}
