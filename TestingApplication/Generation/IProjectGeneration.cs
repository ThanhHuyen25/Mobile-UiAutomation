// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 9:18 AM 2017/10/5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI_Testing_Automation;

namespace TestingApplication
{
    public interface IProjectGeneration
    {
        /// <summary>
        /// generate new testing project
        /// </summary>
        /// <param name="rootElement"></param>
        /// <returns></returns>
        //bool Generate(List<IElement> rootElement);
        bool Generate(List<IElement> rootElement, string path, string defaultClassName);
    }
}
