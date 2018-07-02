// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 10:33 AM 2017/12/1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class FormWfa
    {
        private CsFile csFileCode;

        private string name;

        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }

        public CsFile CsFileCode
        {
            get { return csFileCode; }
            set { csFileCode = value; }
        }

        public FormWfa(CsFile csFileCode, string name)
        {
            this.csFileCode = csFileCode;
            this.name = name;
        }
    }
}
