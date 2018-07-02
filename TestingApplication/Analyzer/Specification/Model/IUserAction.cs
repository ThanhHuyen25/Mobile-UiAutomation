// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 11:35 AM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public interface IUserAction
    {
        INode NodeAffect { get; set; }
        IUserAction Clone();
    }
}
