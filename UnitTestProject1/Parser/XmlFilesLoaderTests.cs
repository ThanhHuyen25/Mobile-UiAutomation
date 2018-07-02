using Microsoft.VisualStudio.TestTools.UnitTesting;
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation.Tests
{
    [TestClass()]
    public class XmlFilesLoaderTests
    {
        [TestMethod()]
        public void LoadTest()
        {
            List<IElement> re = XmlFilesLoader.Load(
                @"..\..\..\ProjectGenTemplate\ElementsRepo.xml",
                @"..\..\..\ProjectGenTemplate\ImageCapture.xml");
            Dictionary<string, IElement> re2 = XmlFilesLoader.Load2(
                @"..\..\..\ProjectGenTemplate\ElementsRepo.xml",
                @"..\..\..\ProjectGenTemplate\ImageCapture.xml");
        }
    }
}