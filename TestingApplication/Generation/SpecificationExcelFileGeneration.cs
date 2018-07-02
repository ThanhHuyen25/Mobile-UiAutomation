using GUI_Testing_Automation;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
namespace TestingApplication
{
    public class SpecificationExcelFileGeneration
    {
        #region const
        public const int NUMBER_OF_SHEETS = 2;
        public const string ALIAS_NAME = "ALIAS NAME";
        public const string REAL_NAME = "REAL NAME";
        public const string NO = "No.";
        public const string SHEET_1_NAME = "MappingSample-001";
        public const string SHEET_2_NAME = "Mapping alias";
        #endregion const

        /// <summary>
        /// Generate .xlsx file
        /// </summary>
        /// <param name="filePath">"C:\spec.xlsx"</param>
        /// <param name="listAllElements">all elements</param>
        /// <returns>a .xlsx file</returns>
        public bool GenerateWithListAllElements(string filePath, List<IElement> listAllElements)
        {
            Excel.Application xlApp = new Excel.Application();
            object misValue = System.Reflection.Missing.Value;
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            while (xlWorkBook.Sheets.Count < NUMBER_OF_SHEETS)
            {
                xlWorkBook.Worksheets.Add();
            }
            Excel.Worksheet sheet1 = xlWorkBook.Worksheets[1];
            sheet1.Name = SHEET_1_NAME;
            
            Excel.Worksheet sheet2 = xlWorkBook.Worksheets[2];
            sheet2.Name = SHEET_2_NAME;

            sheet1.Cells[1, 1] = NO;
            sheet2.Cells[1, 1] = NO;
            sheet2.Cells[1, 2] = ALIAS_NAME;
            sheet2.Cells[1, 3] = REAL_NAME;

            int rowIndex = 2;
            int columnIndex = 2;
            foreach (IElement ele in listAllElements)
            {
                DoGenerateElements(ele, columnIndex, sheet1);
                DoGenerateAlias(ele, ref rowIndex, sheet2);
                columnIndex++;
            }

            xlWorkBook.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            releaseObject(sheet1);
            releaseObject(sheet2);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="rootElements">only root elements (not contain children)</param>
        /// <returns></returns>
        public bool GenerateWithRootElements(string filePath, List<IElement> rootElements)
        {
            Excel.Application xlApp = new Excel.Application();
            object misValue = System.Reflection.Missing.Value;
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            while (xlWorkBook.Sheets.Count < NUMBER_OF_SHEETS)
            {
                xlWorkBook.Worksheets.Add();
            }
            Excel.Worksheet sheet1 = xlWorkBook.Worksheets[1];
            sheet1.Name = SHEET_1_NAME;

            Excel.Worksheet sheet2 = xlWorkBook.Worksheets[2];
            sheet2.Name = SHEET_2_NAME;

            int rowIndex = 2;
            int columnIndex = 2;
            foreach (IElement ele in rootElements)
            {
                genRecursive(ele, ref rowIndex, ref columnIndex, sheet1, sheet2);
            }

            xlWorkBook.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            releaseObject(sheet1);
            releaseObject(sheet2);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            return true;
        }

        private void genRecursive(IElement element, ref int rowIndex, ref int columnIndex,
            Excel.Worksheet sheet1, Excel.Worksheet sheet2)
        {
            DoGenerateElements(element, columnIndex, sheet1);
            DoGenerateAlias(element, ref rowIndex, sheet2);
            if (element.Children != null && element.Children.Count > 0)
                foreach(IElement child in element.Children)
                {
                    columnIndex++;
                    genRecursive(child, ref rowIndex, ref columnIndex, sheet1, sheet2);
                }
        }

        private void DoGenerateElements(IElement element, int columnIndex, Excel.Worksheet xlWorkSheet)
        {
            string value = element.Alias;
            if (element.Alias == null)
                value = element.Attributes.Name;
            xlWorkSheet.Cells[1, columnIndex] = value;
        }

        private void DoGenerateAlias(IElement element, ref int rowIndex, Excel.Worksheet workSheet)
        {
            if (element.Alias != null)
            {
                workSheet.Cells[rowIndex, 1] = rowIndex - 1;
                workSheet.Cells[rowIndex, 2] = element.Alias;
                workSheet.Cells[rowIndex, 3] = element.Attributes.Name;
                rowIndex++;
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
