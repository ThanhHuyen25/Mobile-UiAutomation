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
    public class PictMasterFileGenerationTests
    {
        [TestMethod()]
        public void GenerateTest()
        {
            List<IElement> allElements = XmlFilesLoader.Load(
                @"C:\Users\duongtd\Desktop\samples\exported\ToDoTest1\ToDoTest1\ToDoTest1Repo.xml",
                @"C:\Users\duongtd\Desktop\samples\exported\ToDoTest1\ToDoTest1\ToDoTest1ImageCapture.xml");
            List<IElement> listElements = new List<IElement>()
            {
                allElements[0],
                allElements[1],
                allElements[0].Children[0],
                allElements[0].Children[1]
            };
            new PictMasterFileGeneration().Generate(@"C:\Users\duongtd\Documents\abc.xls", 
                new ListUIElements(ListElementsIndicator.AllElements, listElements));
        }
    }
}