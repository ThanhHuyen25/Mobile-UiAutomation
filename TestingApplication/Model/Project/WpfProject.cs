// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 9:08 PM 2017/11/10
using GUI_Testing_Automation;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class WpfProject : AbstractProject
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private CsprojWpfFile csprojFile;
        public WpfProject() : base(){}
        public WpfProject(Folder rootFolder) : base(rootFolder){}

        //private XamlFile applicationDefinition;

        //public CsprojWpfFile CsprojFile
        //{
        //    get { return csprojFile; }
        //    set { csprojFile = value; }
        //}
        //public XamlFile ApplicationDefinition
        //{
        //    get { return applicationDefinition; }
        //    set { applicationDefinition = value; }
        //}
    }
}
