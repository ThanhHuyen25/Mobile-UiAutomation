// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 10:57 AM 2017/11/23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class SearchFile
    {
        public static List<IFile> Search(IFile root, ISearchCondition condition)
        {
            List<IFile> re = new List<IFile>();
            if (condition.IsSatisfiable(root))
                re.Add(root);
            foreach(IFile child in root.Children)
            {
                re.AddRange(Search(child, condition));
            }
            return re;
        }
    }
}
