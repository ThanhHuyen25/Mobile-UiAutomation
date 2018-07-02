// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 5:13 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class TestSpecSheetParser
    {
        public TestSpecificationScreen parse(TestSpecificationSheet testSpecSheet, 
            ListUIElements elementsAndIndicator, MyLog myLog, List<ClassExpression> classExpressions)
        {
            NormalSheetParser normalSheetParser = new NormalSheetParser();
            SpecScreen screen = normalSheetParser.Parse(testSpecSheet, elementsAndIndicator, myLog);
            if (screen == null)
                return null;
            TestSpecificationScreen re = new TestSpecificationScreen();
            re.AllUIElements = elementsAndIndicator;
            re.ListSpecNodes = screen.ListSpecNodes;
            re.ListValidationUserCode = screen.ListValidationUserCode;
            re.MappingAliasWithNode = screen.MappingAliasWithNode;
            re.Name = testSpecSheet.getFeatureName();
            re.Scenarios = screen.Scenarios;
            re.ClassExpressions = classExpressions;
            return re;
        }
    }
}
