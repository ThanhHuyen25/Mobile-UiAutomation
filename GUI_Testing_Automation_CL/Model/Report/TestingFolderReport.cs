// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 1:09 AM 2018/3/2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GUI_Testing_Automation
{
    public class TestingFolderReport : AbstractItemTestReport
    {
        public TestingFolderReport() : base() { }
        public TestingFolderReport(string name) : base(name) { }

        //public string Icon
        public Brush StateColor
        {
            get
            {
                return GetStateColor();
            }
        }

        public Brush GetStateColor()
        {
            if (IsPass())
                return new SolidColorBrush(Colors.Green);
            else
                return new SolidColorBrush(Colors.Red);
        }

        public override bool IsPass()
        {
            foreach (var child in children)
                if (!child.IsPass())
                    return false;
            return true;
        }
    }
}
