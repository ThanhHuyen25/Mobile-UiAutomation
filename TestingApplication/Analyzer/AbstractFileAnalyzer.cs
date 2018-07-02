// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:37 PM 2017/11/10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public abstract class AbstractFileAnalyzer : IFileAnalyzer
    {
        abstract public bool Process();
    }
}
