// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 3:48 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestingApplication
{
    public abstract class AbstractSheet
    {
        protected string featureName;
        protected Excel._Worksheet sheet;

        public AbstractSheet(string featureName, Excel._Worksheet sheet)
        {
            this.featureName = featureName;
            this.sheet = sheet;
        }

        public string getFeatureName()
        {
            return featureName;
        }

        public void setFeatureName(string featureName)
        {
            this.featureName = featureName;
        }

        public Excel._Worksheet getSheet()
        {
            return sheet;
        }

        public void setSheet(Excel._Worksheet sheet)
        {
            this.sheet = sheet;
        }
    }
}
