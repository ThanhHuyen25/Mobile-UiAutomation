// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 2:03 PM 2017/11/22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// represent for Windows Form Application project
/// </summary>
namespace TestingApplication
{
    public class WfaProject : AbstractProject
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private CsprojWfaFile csprojFile;
        public WfaProject() : base(){ }
        public WfaProject(Folder rootFolder) : base(rootFolder){ }

        //private XamlFile xamlFile;

        //public CsprojWfaFile CsprojFile
        //{
        //    get { return csprojFile; }
        //    set { csprojFile = value; }
        //}
        //public XamlFile XamlFile
        //{
        //    get { return xamlFile; }
        //    set { xamlFile = value; }
        //}
    }
}
