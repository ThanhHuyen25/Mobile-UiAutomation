// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:42 AM 2017/12/13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using GUI_Testing_Automation;


namespace TestingApplication
{
    public interface IElementsAnalyzer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRootAutomationElement"></param>
        /// <param name="elementDiscorerManual"></param>
        /// <returns></returns>
        List<IElement> Analyzing(
            AutomationElementCollection listRootAutomationElement,
            ElementDiscoverManual elementDiscorerManual);
        //IElement analyzing(string pathToProgram);
    }
}
