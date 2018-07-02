// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 10:31 AM 2017/10/19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class RelativeElementsScriptGeneration
    {
        private string parent;

        public string Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        private List<string> children;

        public List<string> Children
        {
            get { return children; }
            set { children = value; }
        }

        public RelativeElementsScriptGeneration()
        {
            this.children = new List<string>();
        }

        public RelativeElementsScriptGeneration(string parent, List<string> children)
        {
            this.children = children;
            this.parent = parent;
        }

        public string Generate(string minTab)
        {

            return null;
        }

    }
}
