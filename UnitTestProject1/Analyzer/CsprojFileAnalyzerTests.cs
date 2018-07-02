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
    public class CsprojFileAnalyzerTests
    {
        [TestMethod()]
        public void FindApplicationDefinitionTest()
        {
        }

        [TestMethod()]
        public void FindProjectTypeGuidsTest()
        {
        }

        [TestMethod()]
        public void ParseFileContentWfaTest()
        {
            CsprojFileAnalyzer csprojFileAnalyzer = new CsprojFileAnalyzer();
            CsprojWfaFile csprojWfaFile = new CsprojWfaFile(@"..\..\..\Solution Utilities\WindowsFormsApp1\WindowsFormsApp1.csproj");
            csprojFileAnalyzer.ParseFileContentWfa(csprojWfaFile,
                Utils.ReadFileContent(csprojWfaFile.Path));
            //Assert.AreEqual(csprojWfaFile.ListForms[0].Name, "Form1.cs");
            //Assert.AreEqual(csprojWfaFile.ListForms[1].Name, "Form21.cs");
        }
    }
}