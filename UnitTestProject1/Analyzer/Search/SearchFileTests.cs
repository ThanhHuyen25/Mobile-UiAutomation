// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 9:27 PM 2017/11/23
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
    public class SearchFileTests
    {
        [TestMethod()]
        public void SearchTest()
        {
            AbstractProject project =
               ProjectLoader.Load(@"D:\Research\projects\GUI-Testing-Automation\Solution Utilities\SampleInput");
            List<IFile> re = SearchFile.Search(project.RootFolder, new CsprojSearchCondition());
            Assert.AreEqual(re.Count, 1);
            Assert.AreEqual(re[0].Path, @"D:\Research\projects\GUI-Testing-Automation\Solution Utilities\SampleInput\SampleInput.csproj");
        }
    }
}