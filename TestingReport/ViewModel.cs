// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:10 PM 2018/3/6
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI_Testing_Automation;

namespace TestingReport
{
    public class ViewModel
    {
        TestingReportModel testingReportModel;
        string name;

        public string Name { get => name; set => name = value; }
        public TestingReportModel TestingReportModel { get => testingReportModel; set => testingReportModel = value; }

        public ViewModel(TestingReportModel testingReportModel)
        {
            this.testingReportModel = testingReportModel;
        }
        public event EventHandler ViewModelChanged;
    }
}
