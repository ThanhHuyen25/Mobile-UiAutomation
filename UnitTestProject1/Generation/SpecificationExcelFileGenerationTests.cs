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
    public class SpecificationExcelFileGenerationTests
    {
        [TestMethod()]
        public void GenerateWithListAllElementsTest()
        {
            SpecificationExcelFileGeneration gen = new SpecificationExcelFileGeneration();
            IElement but = new ButtonElement();
            but.Attributes = new ElementAttributes();
            but.Attributes.Name = "button1";
            but.Alias = "ButtonAlias";

            IElement edit = new EditableTextElement();
            edit.Attributes = new ElementAttributes();
            edit.Attributes.Name = "textbox1";
            edit.Alias = "TextBoxAlias";

            List<IElement> elements = new List<IElement>()
            {
                but, edit
            };
            gen.GenerateWithListAllElements(@"D:\gen.xls", elements);
        }
    }
}