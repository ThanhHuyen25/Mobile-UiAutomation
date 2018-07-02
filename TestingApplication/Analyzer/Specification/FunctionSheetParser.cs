// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 5:13 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestingApplication
{
    public class FunctionSheetParser
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int TITLE_ROW_INDEX = 0;
        public const string WORKSPACE = "Workspace";
        public const string CLASS = "Class";
        public const string FUNCTION_NAME = "Function Name";
        public const string ACCESSIBILITY = "Accessibility";
        public const string SUMMARY = "Summary";
        public const string PARAMETERS = "Parameters";
        public const string RETURNS = "Returns";
        public const string IMPLEMENTATION = "Implementation";

        public int WORKSPACE_COLUMN_INDEX = 0;
        public int CLASS_COLUMN_INDEX = 1;
        public int FUNCTION_NAME_COLUMN_INDEX = 2;
        public int ACCESSIBILITY_COLUMN_INDEX = 3;
        public int SUMMARY_COLUMN_INDEX = 4;
        public int PARAMETERS_COLUMN_INDEX = 5;
        public int RETURNS_COLUMN_INDEX = 6;
        public int IMPLEMENTATION_COLUMN_INDEX = 7;

        public List<ClassExpression> parse(FunctionDescriptionSheet funcSheet, MyLog myLog)
        {
            // parseOneFile title row
            Excel.Range xlRange = funcSheet.getSheet().UsedRange;
            int rowCount = xlRange.Rows.Count;
            int columnCount = xlRange.Columns.Count;
            Excel.Range firstRow = funcSheet.getSheet().Rows[1];
            setColumnIndex(firstRow, myLog, ref columnCount);
            if (columnCount < 0)
                return null;

            List<ClassExpression> classExpressions = new List<ClassExpression>();
            ClassExpression classExpression = new ClassExpression();
            for (int fi = 2; fi <= rowCount; fi++)
            {
                Excel.Range row = funcSheet.getSheet().Rows[fi];
                string funcName = getValueCell(row, FUNCTION_NAME_COLUMN_INDEX);
                // indicate end sheet
                if (funcName == null || funcName.Equals(""))
                {
                    break;
                }
                string workspace = getValueCell(row, WORKSPACE_COLUMN_INDEX);
                string _class = getValueCell(row, CLASS_COLUMN_INDEX);
                string accessibility = getValueCell(row, ACCESSIBILITY_COLUMN_INDEX);
                string summary = getValueCell(row, SUMMARY_COLUMN_INDEX);
                string parameters = getValueCell(row, PARAMETERS_COLUMN_INDEX);
                string _returns = getValueCell(row, RETURNS_COLUMN_INDEX);
                string implementation = getValueCell(row, IMPLEMENTATION_COLUMN_INDEX);

                string last_workspace = workspace == null || workspace.Equals("") ?
                        classExpression.getWorkspace() : workspace;
                string last_class = _class == null || _class.Equals("") ?
                        classExpression.getName() : _class;
                if (last_workspace == null || last_workspace.Equals(""))
                    myLog.Warn("Not know workspace for row #" + (fi + 1) +
                            " in \"" + funcSheet.getSheet().Name + "\" sheet", logger);
                if (last_class == null || last_workspace.Equals(""))
                    myLog.Warn("Not know workspace for row #" + (fi + 1) +
                            " in \"" + funcSheet.getSheet().Name + "\" sheet", logger);
                if (workspace != null && !workspace.Equals("") ||
                        (_class != null && !_class.Equals("")))
                {
                    classExpression = new ClassExpression(last_workspace, last_class);
                    classExpressions.Add(classExpression);
                }
                List <ParameterExpression> _params = parseParams(parameters, myLog);
                FunctionExpression funcEpx = new FunctionExpression(
                        funcName, accessibility, summary, _params);
                funcEpx.setReturnDescription(_returns);
                funcEpx.setContent(implementation);

                if (classExpression.getListFunction() == null)
                    classExpression.setListFunction(new List<FunctionExpression>());
                classExpression.getListFunction().Add(funcEpx);
            }
            return classExpressions;
        }

        private void setColumnIndex(Excel.Range firstRow, MyLog myLog, ref int columnCount)
        {
            //int columnCount = firstRow.Columns.Count;
            for (int fi = 1; fi <= columnCount; fi++)
            {
                string title = getValueCell(firstRow, fi);
                if (title == null)
                {
                    columnCount = fi - 1;
                    break;
                }
                switch (title)
                {
                    case WORKSPACE:
                        WORKSPACE_COLUMN_INDEX = fi;
                        break;
                    case CLASS:
                        CLASS_COLUMN_INDEX = fi;
                        break;
                    case FUNCTION_NAME:
                        FUNCTION_NAME_COLUMN_INDEX = fi;
                        break;
                    case ACCESSIBILITY:
                        ACCESSIBILITY_COLUMN_INDEX = fi;
                        break;
                    case SUMMARY:
                        SUMMARY_COLUMN_INDEX = fi;
                        break;
                    case PARAMETERS:
                        PARAMETERS_COLUMN_INDEX = fi;
                        break;
                    case RETURNS:
                        RETURNS_COLUMN_INDEX = fi;
                        break;
                    case IMPLEMENTATION:
                        IMPLEMENTATION_COLUMN_INDEX = fi;
                        break;
                    case "":
                        columnCount = fi - 1;
                        break;
                    default:
                        myLog.Warn("Don't know column \"" + title + "\"");
                        columnCount = -1;
                        return;
                }
            }
        }

        private string getValueCell(Excel.Range row, int columnIndex)
        {
            if (row.Cells[1, columnIndex] == null ||
                    row.Cells[1, columnIndex].Value2 == null)
                return null;
            string re = (string)row.Cells[1, columnIndex].Value2;
            return Regex.IsMatch(re, AbstractSpecUserAction.NA) ? null : re;
        }

        private List<ParameterExpression> parseParams(string paramsStr, MyLog myLog)
        {
            if (paramsStr == null || paramsStr.Equals("") || paramsStr.Equals(AbstractSpecUserAction.NA))
                return null;
            string[] paramStrList = paramsStr.Split('\n');
            List<ParameterExpression> re = new List<ParameterExpression>();
            foreach (string paramStr in paramStrList)
            {
                string[] paramAndDesc = paramStr.Split(':');
                if (paramAndDesc.Length != 2)
                    myLog.Warn("Parameter \"" + paramsStr + "\" is incorrect format!", logger);
                re.Add(new ParameterExpression(paramAndDesc[0].Trim(),
                        paramAndDesc.Length > 1 ? paramAndDesc[1].Trim() : ""));
            }
            return re;
        }
    }
}
