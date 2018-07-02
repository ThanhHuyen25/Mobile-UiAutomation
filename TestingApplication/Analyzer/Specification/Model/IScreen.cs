// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 5:53 PM 2018/1/21
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public interface IScreen
    {
        string Name { get; set; }
        List<IScenario> Scenarios { get; set; }
        ListUIElements AllUIElements { get; set; }
    }
}
