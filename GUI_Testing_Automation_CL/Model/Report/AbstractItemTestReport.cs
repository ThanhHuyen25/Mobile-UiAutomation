// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 1:08 AM 2018/3/2
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    public abstract class AbstractItemTestReport : ITestingItemReport
    {
        protected string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        public ObservableCollection<ITestingItemReport> Children
        {
            get { return children; }
            set
            {
                children = value;
            }
        }
        public ITestingItemReport Parent
        {
            get { return parent; }
            set
            {
                parent = value;
            }
        }

        protected ObservableCollection<ITestingItemReport> children;

        protected ITestingItemReport parent;

        public AbstractItemTestReport() { }
        public AbstractItemTestReport(string name) : this()
        {
            this.name = name;
        }
        public abstract bool IsPass();
    }
}
