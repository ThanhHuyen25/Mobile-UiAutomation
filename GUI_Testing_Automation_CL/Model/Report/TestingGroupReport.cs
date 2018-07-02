// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 12:33 PM 2018/2/26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    /// <summary>
    /// represent for folder FeatureX
    /// </summary>
    public class TestingGroupReport
    {
        private string name;

        List<TestingModuleReport> listTestingModules;

        public List<TestingModuleReport> ListTestingModules
        {
            get { return listTestingModules; }
            set
            {
                listTestingModules = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public TestingGroupReport(string name, List<TestingModuleReport> listTestingModules)
        {
            this.listTestingModules = listTestingModules;
            this.name = name;
        }

        public TestingGroupReport() { }
    }
}
