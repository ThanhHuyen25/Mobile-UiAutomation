// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 1:09 AM 2018/3/2
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    public interface ITestingItemReport
    {
        string Name { get; set; }

        ObservableCollection<ITestingItemReport> Children { get; set; }

        ITestingItemReport Parent { get; set; }
        bool IsPass();
    }
}
