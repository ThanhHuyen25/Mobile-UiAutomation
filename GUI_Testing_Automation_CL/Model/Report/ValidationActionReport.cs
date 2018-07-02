// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:19 PM 2018/2/25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    public class ValidationActionReport : ActionReport
    {
        string imageCapPath;

        public string ImageCapPath
        {
            get { return imageCapPath; }
            set
            {
                imageCapPath = value;
            }
        }

        public ValidationActionReport(string message, string status, string category, string imageCapPath) : 
            base(message, status, category, null)
        {
            this.imageCapPath = imageCapPath;
        }
    }
}
