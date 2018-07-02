// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:12 PM 2017/11/11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{ 
    public interface IProjectAnalyser
    {
        Tuple<string, LogProcess> Process(IProject inputProject);
    }
}
