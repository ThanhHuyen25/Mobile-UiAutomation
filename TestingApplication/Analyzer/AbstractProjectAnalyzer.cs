// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:37 AM 2017/11/9
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public abstract class AbstractProjectAnalyzer : IProjectAnalyser
    {
        public abstract Tuple<string, LogProcess> Process(IProject inputProject);

        //public static AbstractProject
    }
}
