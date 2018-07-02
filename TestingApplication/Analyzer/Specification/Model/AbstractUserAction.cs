// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 11:37 AM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public abstract class AbstractUserAction : IUserAction
    {
        protected INode nodeAffect;

        public INode NodeAffect
        {
            get { return nodeAffect; }
            set { nodeAffect = value; }
        }

        public abstract IUserAction Clone();
    }
}
