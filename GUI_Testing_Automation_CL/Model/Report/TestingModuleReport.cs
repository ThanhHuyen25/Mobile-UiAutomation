// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:32 AM 2018/2/26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GUI_Testing_Automation
{
    /// <summary>
    /// represent for 1 file e.g. FeatureX_1.cs
    /// </summary>
    public class TestingModuleReport : AbstractItemTestReport
    {
        string relativeFilePath;

        List<IActionReport> listActionReport;

        public List<IActionReport> ListActionReport
        {
            get { return listActionReport; }
            set
            {
                listActionReport = value;
            }
        }
        public string RelativeFilePath
        {
            get { return relativeFilePath; }
            set
            {
                relativeFilePath = value;
            }
        }
        public Brush StateColor
        {
            get
            {
                return GetStateColor();
            }
        }


        public TestingModuleReport(string name, List<IActionReport> listActionReport, string relativeFilePath)
        {
            this.name = name;
            this.listActionReport = listActionReport;
            this.relativeFilePath = relativeFilePath;
        }

        public TestingModuleReport(string name, string relativeFilePath)
        {
            this.name = name;
            this.relativeFilePath = relativeFilePath;
            this.listActionReport = new List<IActionReport>();
        }
        public override bool IsPass()
        {
            if (listActionReport == null)
                return true;
            foreach (var child in listActionReport)
            {
                if (!child.IsPass())
                    return false;
            }
            return true;
        }

        public Brush GetStateColor()
        {
            if (IsPass())
                return new SolidColorBrush(Colors.Green);
            else
                return new SolidColorBrush(Colors.Red);
        }
    }
}
