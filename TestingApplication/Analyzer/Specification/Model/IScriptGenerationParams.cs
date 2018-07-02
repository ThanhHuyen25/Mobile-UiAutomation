// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 1:51 PM 2018/6/6
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public interface IScriptGenerationParams
    {
        SpecNode SpecNode { get; set; }
        ListUIElements ListUIElements { get; set; }
        Color Color { get; set; }
        MyLog MyLog { get; set; }
        string ScreenName { get; set; }
        //scenario id ??
        int Id { get; set; }
        string InstanceName { get; set; }
        string PathToApp { get; set; }
        IScriptGenerationParams Clone();
        //void CopyAttributesFrom();
    }
}
