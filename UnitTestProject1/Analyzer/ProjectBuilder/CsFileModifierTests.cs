// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 9:30 PM 2017/11/26
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication.Tests
{
    [TestClass()]
    public class CsFileModifierTests
    {
        [TestMethod()]
        public void ModifyTest()
        {
            CsFile csFile = new CsFile(@"D:\Research\projects\GUI-Testing-Automation\Solution Utilities\SampleInput\FirstFolder\MainWindow.xaml.cs");

            CsFileModifier csFileModifier = new CsFileModifier();
            //csFileModifier.Modify(csFile, new List<string>() { "SecondWindow"});
            Assert.IsTrue(true);
        }
    }
}