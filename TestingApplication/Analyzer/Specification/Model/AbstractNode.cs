// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 11:34 AM 2018/1/21
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public abstract class AbstractNode : INode
    {
        protected IElement _UIElemnt;
        public IElement UIElement
        {
            get { return _UIElemnt; }
            set { this._UIElemnt = value; }
        }

        public abstract INode Clone();
    }
}
