// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 5:00 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestingApplication
{
    public class TestSpecificationSheet : AbstractSheet
    {
        public TestSpecificationSheet(string featureName, Excel._Worksheet sheet) : base(featureName, sheet)
        {
        }
    }
}
