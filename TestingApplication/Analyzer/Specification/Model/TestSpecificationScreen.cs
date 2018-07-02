// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 6:19 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class TestSpecificationScreen : SpecScreen
    {
        private List<ClassExpression> classExpressions;

        public List<ClassExpression> ClassExpressions
        {
            get { return classExpressions; }
            set { classExpressions = value; }
        }
    }
}
