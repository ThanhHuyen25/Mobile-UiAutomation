// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 11:36 AM 2018/3/20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    public interface ITestModule
    {
        /// <summary>
        /// don't call this function directly
        /// </summary>
        void DoRun();
    }
}
