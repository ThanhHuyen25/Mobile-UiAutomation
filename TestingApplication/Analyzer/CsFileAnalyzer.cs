// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 1:41 PM 2017/12/6
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.MSBuild;

namespace TestingApplication
{
    /// <summary>
    /// represent for analyzer .cs file
    /// </summary>
    public class CsFileAnalyzer : AbstractFileAnalyzer
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private CsFile csFile;

        public CsFile CsFile
        {
            get { return csFile; }
            set { csFile = value; }
        }

        public override bool Process()
        {
            ClassVisitor classVisitor = new ClassVisitor();
            SyntaxTree tree = CSharpSyntaxTree.ParseText(
                Utils.ReadFileContent(csFile.Path));

            var root = (CompilationUnitSyntax)tree.GetRoot();
            classVisitor.Visit(root);
            csFile.Classes = classVisitor.classes;
            return true;
        }
    }

    /// <summary>
    /// visit class to get info
    /// </summary>
    public class ClassVisitor : CSharpSyntaxWalker
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public readonly List<CsClass> classes = new List<CsClass>();

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            string name = node.Identifier.ValueText;
            CsClass csClass = new CsClass();
            csClass.Name = name;
            if (node.BaseList != null && node.BaseList.Types != null)
                foreach (BaseTypeSyntax baseType in node.BaseList.Types)
                {
                    string baseTypeStr = baseType.Type.ToString();
                    //logger.Debug("-----Base type: " + baseTypeStr);
                    if (csClass.BaseOnClass == null)
                        csClass.BaseOnClass = new List<string>();
                    csClass.BaseOnClass.Add(baseTypeStr);
                }
            classes.Add(csClass);
        }
    }
}
