// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:14 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public interface IScenario
    {
        List<IUserAction> UserActions { get; set; }
        IScenario Clone();
    }
}
