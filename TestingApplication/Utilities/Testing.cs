// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:18 PM 2018/1/10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

using GUI_Testing_Automation;

namespace TestingApplication
{
    public class Testing
    {
        public static void MainTesting(Object arg)
        {
            //QuyDkTesting();
            //DuongTesting();
        }

        public static void DuongTesting()
        {
            //InspectElement inspectElement = new InspectElement();
            //AutomationElement window = inspectElement.Inspect("D:\\NCKH\\projects\\UI-Testing-Automation\\examples\\API-Sample20170825\\Sample2\\CombinedSampleApp-V20170822\\TestApplication_WinFrom\\TestApplication_WinFrom\\obj\\Debug\\TestApplication_WinFrom.exe");

            // analyzing
            //IElementsAnalyzer elementsAnalyzer = new ElementsAnalyzer();
            //IElement rootElement = elementsAnalyzer.Analyzing(window);
        }

        public static void QuyDkTesting()
        {
            //InspectElement inspectElement = new InspectElement();
            //AutomationElement window = inspectElement.Inspect("C:\\Users\\MyPC\\Desktop\\TestApplication_WinFrom\\TestApplication_WinFrom\\obj\\Debug\\TestApplication_WinFrom.exe");
            //inspectElement.TraverseElement(window, 0);
        }

        public static void DuongTesting1()
        {            
            IElement rootElement = fakeDataElements();
        }

        public static IElement fakeDataElements()
        {
            //IElement rootElement = new ContainerElement();
            
            //IElement child1 = new ButtonElement("child1");
            //child1.Name = "child1";
            //child1.SetParent(rootElement);

            //IElement child2 = new ButtonElement();
            //child2.Name = "child2";
            //child2.SetParent(rootElement);

            //rootElement.SetChildren(new List<IElement> { child1, child2 });

            //IElement child11 = new ButtonElement();
            //child11.Name = "child11";
            //child11.SetParent(child1);

            //IElement child12 = new ButtonElement();
            //child12.Name = "child12";
            //child12.SetParent(child1);

            //child1.SetChildren(new List<IElement> { child11, child12 });

            //IElement child21 = new ButtonElement();
            //child21.Name = "child21";
            //child21.SetParent(child2);

            //child2.SetChildren(new List<IElement> { child21 });

            return null;
        }
    }
}
