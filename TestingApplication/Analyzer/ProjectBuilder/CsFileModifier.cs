// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:30 PM 2017/11/26
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.MSBuild;
//using Microsoft.CodeAnalysis.Formatting;
using System.Reflection;
using System.Text;
using System;
using System.IO;

namespace TestingApplication
{
    /// <summary>
    /// represent for modifing .cs file
    /// </summary>
    public class CsFileModifier
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string METHOD_NAME = "Open_All_Windows";
        public const string INIT_COMPONENT_STATEMENT = "InitializeComponent();";
        public const string NEW_LINE = "\r\n";

        /// <summary>
        /// append 2 statements:
        ///     System.IO.File.AppendAllText("temp_file_155133.txt", "ThisWindow");
        ///     Open_All_Windows(); //call to function which open all other window
        /// </summary>
        /// <param name="csFile"></param>
        /// <param name="listWindows"></param>
        /// <param name="tempFileName"></param>
        /// <returns></returns>
        public bool ModifyConstructorFunc(CsFile csFile, List<CsClass> listWindows, string tempFileName)
        {
            try
            {
                //The code that causes the error goes here.
                SyntaxTree tree = CSharpSyntaxTree.ParseText(
                    Utils.ReadFileContent(csFile.Path));
                var root = (CompilationUnitSyntax)tree.GetRoot();
                var classRewrite = new ClassRewriter();
                classRewrite.ListWindows = listWindows;
                classRewrite.TempFileName = tempFileName;
                SyntaxNode newRoot = classRewrite.Visit(root);

                //var workspace = MSBuildWorkspace.Create();
                //SyntaxNode formattedNode = Formatter.Format(newRoot, workspace);
                //logger.Debug("abc: " + formattedNode);

                System.IO.File.WriteAllText(csFile.Path, Utils.ReformatCsCode(newRoot.ToFullString()));
                    //formattedNode.ToFullString());
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                //Display or log the error based on your application.
            }
            return true;
        }
    }

    /// <summary>
    /// append new function to the body of the class
    ///     Open_All_Windows();
    /// if class not contain constructor without arg, so create this constructor
    ///     public ThisWindow(){}
    /// </summary>
    public class ClassRewriter : CSharpSyntaxRewriter
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CsClass> listWindows;
        public const string NEW_LINE = CsFileModifier.NEW_LINE;
        public List<CsClass> ListWindows
        {
            get { return listWindows; }
            set { listWindows = value; }
        }
        public string TempFileName
        {
            get { return tempFileName; }
            set { tempFileName = value; }
        }
        private string tempFileName;

        private CsClass CheckClassNeedToModify(ClassDeclarationSyntax classSyntax, List<CsClass> screens)
        {
            foreach (CsClass screen in screens)
                if (screen.Name.Equals(classSyntax.Identifier.ValueText))
                    return screen;
            return null;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            //Debug.WriteLine("class name: " + node.Identifier.ValueText);
            var screens1 = CheckClassNeedToModify(node, listWindows);
            if (screens1 != null)
            {
                var constructorReWrite = new ConstructorRewriter();
                constructorReWrite.TempFileName = tempFileName;
                constructorReWrite.ClassParent = node;
                ClassDeclarationSyntax newClassNode = (ClassDeclarationSyntax)constructorReWrite.Visit(node);

                if (constructorReWrite.VisitedConstructorHasArgs)
                {
                    logger.Warn("Class " + node.Identifier.ValueText + " contains constructor function with args, the corresponding window might visible incorrectly");
                }
                
                // if not catch constructor function with no args, so must create this func
                if (!constructorReWrite.VisitedConstructorNoArgs)
                {
                    if (!constructorReWrite.ContainDefaultInitStatement)
                        logger.Warn("Cannot find the init components statement (" + CsFileModifier.INIT_COMPONENT_STATEMENT +
                            ") in the constructor functions of class " + node.Identifier.ValueText + ", the corresponding window might visible incorrectly");
                    logger.Warn("Class " + node.Identifier.ValueText + " doesn't contain constructor function without args, automaticaly add this function");
                    string funcExp =
                        "public " + screens1.Name + "()" +
                        NEW_LINE + "{" +
                            "\nSystem.IO.File.AppendAllText(\"" + tempFileName + "\", \"" + screens1.Name + "\\t\");";
                    if (constructorReWrite.ContainDefaultInitStatement)
                        funcExp +=
                            NEW_LINE + CsFileModifier.INIT_COMPONENT_STATEMENT; //InitializeComponent();
                    funcExp +=
                            NEW_LINE + CsFileModifier.METHOD_NAME + "();" +
                        NEW_LINE + "}";
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(funcExp);
                    var tempRoot = (CompilationUnitSyntax)tree.GetRoot();
                    var tempMethod = (MethodDeclarationSyntax)tempRoot.Members[0];
                    newClassNode = newClassNode.AddMembers(tempMethod);
                }
                else if (constructorReWrite.ContainDefaultInitStatement)
                {
                    bool containInitComStatement = false; //indicate constructor no arg func contain init statement
                    var oldMembers = newClassNode.Members;
                    var oldConstructorNode = (ConstructorDeclarationSyntax)oldMembers[constructorReWrite.IndexConstructorFuncNoArg];
                    foreach (StatementSyntax statement in oldConstructorNode.Body.Statements)
                    {
                        if (statement.ToFullString().Equals(CsFileModifier.INIT_COMPONENT_STATEMENT))
                        {
                            containInitComStatement = true;
                            break;
                        }
                    }
                    //if visited but this constructor has no "InitComponent();" statement
                    if (!containInitComStatement)
                    {
                        string fakeFuncExp = "void fakeFunc()\n{" + CsFileModifier.INIT_COMPONENT_STATEMENT + "\n}";
                        SyntaxTree tree = CSharpSyntaxTree.ParseText(fakeFuncExp);
                        var tempRoot = (CompilationUnitSyntax)tree.GetRoot();
                        var tempMethod = (MethodDeclarationSyntax)tempRoot.Members[0];
                        
                        var newConstructorNode = oldConstructorNode.AddBodyStatements(tempMethod.Body.Statements[0]);
                        //var mem = newClassNode.Members.RemoveAt(constructorReWrite.IndexConstructorFuncNoArg);
                        var newMembers = oldMembers.RemoveAt(constructorReWrite.IndexConstructorFuncNoArg);
                        newMembers = newMembers.Insert(constructorReWrite.IndexConstructorFuncNoArg, newConstructorNode);
                        newClassNode = newClassNode.WithMembers(newMembers);
                    }
                }

                List<CsClass> newListCsClass = new List<CsClass>(listWindows);
                newListCsClass.Remove(screens1);

                return newClassNode.AddMembers(MethodDeclare(newListCsClass, screens1));
            }
            return base.VisitClassDeclaration(node);
        }

        /// <summary>
        /// private void Open_All_Windows() {
        ///     if (!System.IO.File.ReadAllText("temp_file_HHmmss.txt").Contains("AnotherWindow"))
        ///         new AnotherWindow().Show();
        ///     ....
        /// }
        /// </summary>
        /// <param name="listWindows"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public MemberDeclarationSyntax MethodDeclare(List<CsClass> listWindows, CsClass current)
        {
            string methodEpx =
                "private void " + CsFileModifier.METHOD_NAME + "()" +
                NEW_LINE + "{";
            foreach(CsClass screen in listWindows)
            {
                methodEpx +=
                    NEW_LINE + "if (!System.IO.File.ReadAllText(\"" + tempFileName + "\").Contains(\"" + screen.Name + "\"))" +
                        NEW_LINE + "new " + screen.Name + "().Show();";
            }
            methodEpx +=
                NEW_LINE + "}";
            SyntaxTree tree = CSharpSyntaxTree.ParseText(methodEpx);
            return ((CompilationUnitSyntax)tree.GetRoot()).Members[0];
        }
    }

    /// <summary>
    /// append statements to constructor function
    /// </summary>
    public class ConstructorRewriter : CSharpSyntaxRewriter
    {
        public string TempFileName
        {
            get { return tempFileName; }
            set { tempFileName = value; }
        }
        public bool VisitedConstructorNoArgs
        {
            get { return visitedConstructorNoArgs; }
            set { visitedConstructorNoArgs = value; }
        }
        public bool VisitedConstructorHasArgs
        {
            get { return visitedConstructorHasArgs; }
            set { visitedConstructorHasArgs = value; }
        }
        public bool ContainDefaultInitStatement
        {
            get { return containDefaultInitStatement; }
            set { containDefaultInitStatement = value; }
        }
        public int IndexConstructorFuncNoArg
        {
            get { return indexConstructorFuncNoArg; }
            set { indexConstructorFuncNoArg = value; }
        }

        public ClassDeclarationSyntax ClassParent
        {
            get { return classParent; }
            set { classParent = value; }
        }

        private string tempFileName;
        private bool visitedConstructorNoArgs = false;
        private bool visitedConstructorHasArgs = false;
        private bool containDefaultInitStatement = false;
        private int indexConstructorFuncNoArg = -1;
        private ClassDeclarationSyntax classParent = null;

        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            // only append to constructor with no args
            if (node.ParameterList.Parameters == null |
                node.ParameterList.Parameters.Count == 0)
            {
                visitedConstructorNoArgs = true;
                indexConstructorFuncNoArg = classParent.Members.IndexOf(node);

                string statementExp = "void func(){\nSystem.IO.File.AppendAllText(\"" + tempFileName + "\", \"" + 
                    node.Identifier.ValueText + "\\t\");\n}";
                SyntaxTree tree = CSharpSyntaxTree.ParseText(statementExp);
                var tempRoot = (CompilationUnitSyntax)tree.GetRoot();
                var tempMethod = (MethodDeclarationSyntax)tempRoot.Members[0];
                var newStatement = tempMethod.Body.Statements[0];

                var tempNode = node.AddBodyStatements(LocalStatementCaller(CsFileModifier.METHOD_NAME));
                var firstStatement = tempNode.Body.Statements[0];
                return tempNode.InsertNodesBefore(firstStatement, new List<SyntaxNode>() { newStatement });
            }
            else
            {
                visitedConstructorHasArgs = true;
                foreach(StatementSyntax statement in node.Body.Statements)
                {
                    string temp = statement.ToFullString();
                    if (statement.ToFullString().Contains(CsFileModifier.INIT_COMPONENT_STATEMENT))
                    {
                        this.containDefaultInitStatement = true;
                        break;
                    }
                }
            }
            return base.VisitConstructorDeclaration(node);
        }

        public ExpressionStatementSyntax LocalStatementCaller(string type)
        {
            return SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(
                SyntaxFactory.IdentifierName(CsFileModifier.METHOD_NAME),
                SyntaxFactory.ArgumentList(
                    SyntaxFactory.Token(SyntaxKind.OpenParenToken),
                    new SeparatedSyntaxList<ArgumentSyntax>(),
                    SyntaxFactory.Token(SyntaxKind.CloseParenToken))));
        }
    }
}
