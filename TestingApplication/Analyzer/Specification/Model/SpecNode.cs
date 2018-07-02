// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 11:32 AM 2018/1/21
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingApplication
{
    /// <summary>
    /// represent for a node in excel file (first row)
    /// </summary>
    public class SpecNode : AbstractNode
    {
        public SpecNode(string expression)
        {
            this.expression = expression;
        }
        public SpecNode(IElement element, string attribute, string expression)
        {
            this.UIElement = element;
            this.attribute = attribute;
            this.expression = expression;
        }
        private string expression;

        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public string Attribute
        {
            get { return attribute; }
            set { attribute = value; }
        }
        private string attribute;

        public override INode Clone()
        {
            SpecNode re = new SpecNode(UIElement, attribute, expression);
            return re;
        }

        public override string ToString()
        {
            return expression.Trim();
        }

        public string GetNormalizedName()
        {
            return Regex.Replace(expression.Trim(), "[^\\w-]", "_");
        }

    }
}