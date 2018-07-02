using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI_Testing_Automation;
using System.Windows.Automation;

namespace TestingApplication.Tests
{
    [TestClass()]
    public class ElementXmlGenerationTests
    {
        [TestMethod()]
        public void StoreTest()
        {
            IInspecting inspecting = new InspectElement();
            AutomationElementCollection window = inspecting.Inspect(@"..\..\..\Solution Utilities\Notepad Demo\bin\Debug\WPFNotepad.exe");
            //AutomationElementCollection window = inspecting.Inspect(@"..\..\..\Solution Utilities\SampleInput\bin\Debug\SampleInput.exe");
            IElementsAnalyzer elementsAnalyzer = new ElementsAnalyzer();
            //List<IElement> listRootElement = elementsAnalyzer.Analyzing(window);
            //ElementXmlGeneration elementXmlGeneration = new ElementXmlGeneration();
            //elementXmlGeneration.Store(listRootElement);
        }
    }
}