// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:19 PM 2018/2/25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GUI_Testing_Automation
{
    public interface IActionReport
    {
        string Message { get; set; }
        string Category { get; set; }
        string Status { get; set; }
        string ImageCapPath { get; set; }
        Brush BgColor { get; }
        Brush FgColor { get; }
        string AbsoluteImgPath { get; }
        bool IsPass();
    }
}
