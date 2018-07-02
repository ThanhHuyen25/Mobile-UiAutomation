// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:52 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestingApplication
{
    public class NormalSheet : AbstractSheet
    {
        public NormalSheet(string featureName, Excel._Worksheet sheet) : base(featureName, sheet)
        {
        }
    }
}
