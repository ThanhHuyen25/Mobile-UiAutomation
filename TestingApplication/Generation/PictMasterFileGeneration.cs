// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 1:28 PM 2018/4/16
using GUI_Testing_Automation;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class PictMasterFileGeneration
    {
        public const string GENERATED_FILE_NAME = "PictTemp";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="listUIElements">only root elements or all elements, depend on indicator attribute</param>
        /// <returns></returns>
        public bool Generate(string filePath, ListUIElements listUIElements)
        {
            // create a temp file
            string tempFilePath = Path.Combine(Path.GetTempPath(), GENERATED_FILE_NAME + "_" + DateTime.Now.ToString("yyMMdd-HHmmss"));
            File.Copy(
                @"..\..\Resources\PictMaster-Template.xls"
                //@"D:\Research\projects\GUI-Testing-Automation\TestingApplication\Resources\PictMaster-Template.xls"
                , tempFilePath);

            Application excel = new Application();

            Workbook workbook = excel.Workbooks.Open(tempFilePath, ReadOnly: false, Editable: true);
            Worksheet worksheet = workbook.Worksheets.Item[1] as Worksheet;
            if (worksheet == null)
                return false;

            int rowIdx = 9;
            int colIdx = 2;
            int rowConstraintIdx = 66;

            if (listUIElements.Indicator == ListElementsIndicator.AllElements)
                GenerateElements(listUIElements.Elements, ref rowIdx, ref rowConstraintIdx, ref colIdx, worksheet);
            else if (listUIElements.Indicator == ListElementsIndicator.OnlyRootElements)
                GenerateWithElementsRecursive(listUIElements.Elements, ref rowIdx, ref rowConstraintIdx, ref colIdx, worksheet);

            excel.Application.ActiveWorkbook.Save();
            excel.Application.Quit();
            excel.Quit();

            // move temp file to return
            File.Move(tempFilePath, filePath);
            return true;
        }

        private void GenerateElements(List<IElement> elements, ref int rowIdx, ref int rowConstraintIdx, ref int colIdx, Worksheet xlWorkSheet)
        {
            foreach (var element in elements)
            {
                DoGenerateElements(element, ref rowIdx, ref rowConstraintIdx, ref colIdx, xlWorkSheet);
            }
        }

        private void GenerateWithElementsRecursive(List<IElement> elements, ref int rowIdx, ref int rowConstraintIdx, ref int colIdx, Worksheet xlWorkSheet)
        {
            foreach(var element in elements)
            {
                DoGenerateElements(element, ref rowIdx, ref rowConstraintIdx, ref colIdx, xlWorkSheet);
                if (element.Children != null)
                    GenerateWithElementsRecursive(element.Children, ref rowIdx, ref rowConstraintIdx, ref colIdx, xlWorkSheet);
            }
        }

        private void DoGenerateElements(IElement element, ref int rowIdx, ref int rowConstraintIdx, ref int colIdx, Worksheet xlWorkSheet)
        {
            string value = element.Alias;
            if (element.Alias == null)
                value = element.Attributes.Name;
            xlWorkSheet.Cells[rowIdx, colIdx] = value;
            xlWorkSheet.Cells[rowConstraintIdx, colIdx] = value;
            rowIdx++;
            rowConstraintIdx++;
        }
    }
}
