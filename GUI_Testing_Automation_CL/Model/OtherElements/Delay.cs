// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:37 PM 2018/3/20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    public class Delay
    {
        public static void Duration(int miliSeconds)
        {
            System.Threading.Thread.Sleep(miliSeconds);
        }
    }
}
