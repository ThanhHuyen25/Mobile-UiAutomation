// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:04 PM 2017/11/30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public abstract class AbstractFile : IFile
    {
        protected string path;
        IFile parent;
        List<IFile> children;
        public string Path
        {
            get { return path; }
            set { this.path = value; }
        }
        
        public IFile Parent
        {
            get { return parent; }
            set { this.parent = value; }
        }
        public List<IFile> Children {
            get { return children; }
            set { this.children = value; }
        }

        public AbstractFile() { }
        public AbstractFile(string path)
        {
            this.path = path;
        }

        public AbstractFile(string path, IFile parent)
        {
            this.parent = parent;
            this.path = path;
            Init();
            if (parent != null)
            {
                List<IFile> children = parent.Children;
                children.Add(this);
                parent.Children = children;
            }
        }

        private void Init()
        {
            this.children = new List<IFile>();
        }
    }
}
