// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 11:47 AM 2018/3/20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    /// <summary>
    /// test run instance must extend this class
    /// </summary>
    public abstract class AbstractTestRun:ITestRun
    {
        private void Initialize(string projectName, string runningTestFilePath)
        {
            Handler.Init(projectName, runningTestFilePath);
        }
        private void Finish()
        {
            Handler.Finish();
        }

        /// <summary>
        /// don't call this function directly
        /// </summary>
        public abstract void DoRun();

        public void Run(string projectName, string runningTestFilePath)
        {
            Initialize(projectName, runningTestFilePath);
            DoRun();
            Finish();
        }
    }
}
