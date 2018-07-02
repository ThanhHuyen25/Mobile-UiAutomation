// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:12 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class ScriptsExpression
    {
        public const string NEW_LINE = "\r\n";
        protected string expression;

        public ScriptsExpression() { }
        public ScriptsExpression(string expression)
        {
            this.expression = expression;
        }
        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public string Append(string addition)
        {
            if (expression == null)
                expression = addition;
            else
                expression += NEW_LINE + addition;
            return expression;
        }

        //private string beforeScript
    }
}
