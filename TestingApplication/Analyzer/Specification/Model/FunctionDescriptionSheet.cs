// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:57 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestingApplication
{
    public class FunctionDescriptionSheet : AbstractSheet
    {
        private List<ClassExpression> classes;

        public FunctionDescriptionSheet(string featureName, Excel._Worksheet sheet) : base(featureName, sheet)
        {
        }

        public FunctionDescriptionSheet(string featureName, List<ClassExpression> classes, Excel._Worksheet sheet) : base (featureName, sheet)
        {
            this.classes = classes;
        }

        public List<ClassExpression> getClasses()
        {
            return classes;
        }

        public void setClasses(List<ClassExpression> classes)
        {
            this.classes = classes;
        }
    }
}
