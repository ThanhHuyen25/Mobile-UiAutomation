// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 3:10 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class ClassExpression
    {
        private String workspace;
        private List<FunctionExpression> listFunction;
        private String name;

        public ClassExpression()
        {
        }

        public ClassExpression(String workspace, String name)
        {
            this.workspace = workspace;
            this.name = name;
        }

        public String getWorkspace()
        {
            return workspace;
        }

        public void setWorkspace(String workspace)
        {
            this.workspace = workspace;
        }

        public List<FunctionExpression> getListFunction()
        {
            return listFunction;
        }

        public void setListFunction(List<FunctionExpression> listFunction)
        {
            this.listFunction = listFunction;
        }

        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public ClassExpression clone()
        {
            ClassExpression re = new ClassExpression(workspace, name);
            if (listFunction != null)
                re.listFunction = new List<FunctionExpression>(listFunction);
            return re;
        }

        override
        public bool Equals(object other)
        {
            if (!(other is ClassExpression))
                return false;
            ClassExpression other1 = (ClassExpression)other;
            return compareStr(this.getName(), other1.getName());
            //                compareStr(this.getWorkspace(), other1.getWorkspace());
        }

        private bool compareStr(String str1, String str2)
        {
            if (str1 == null)
                return str2 == null;
            return str1.Equals(str2);
        }

        public string GetCorrectWorkspace()
        {
            if (workspace == null)
                return "";
            string re = workspace;
            if (workspace.StartsWith("ProjectName.", StringComparison.OrdinalIgnoreCase))
                re = workspace.Substring("ProjectName.".Length);
            return re.Replace('.', '\\');
        }
    }
}
