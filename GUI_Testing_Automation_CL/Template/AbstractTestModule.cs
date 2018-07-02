// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 11:39 AM 2018/3/20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    /// <summary>
    /// all generated test module must extend this abstract class
    /// </summary>
    public abstract class AbstractTestModule:ITestModule
    {
        /// <summary>
        /// don't call this function directly
        /// </summary>
        public abstract void DoRun();

        public AbstractTestModule()
        {
            this.moduleName = this.GetType().Name;
        }

        string moduleName;

        public string ModuleName
        {
            get { return moduleName; }
            set
            {
                moduleName = value;
            }
        }

        /// <summary>
        /// run test module
        /// </summary>
        public void Run()
        {
            Handler.InitNewModule(moduleName);
            DoRun();
        }
    }
}
