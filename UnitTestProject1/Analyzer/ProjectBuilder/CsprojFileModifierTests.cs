// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 9:30 PM 2017/11/26
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestingApplication.Tests
{
    [TestClass()]
    public class CsprojFileModifierTests
    {
        [TestMethod()]
        public void ModifyTest()
        {
            CsprojFileModifier csprojFileModifier = new CsprojFileModifier();
            csprojFileModifier.Modify();
            Assert.Fail();
        }
    }
}