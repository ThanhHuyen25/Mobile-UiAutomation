// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:40 PM 2018/1/19
using GUI_Testing_Automation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.Formatting;
//using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class ElementCSharpCodeGeneration
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string NEW_LINE = "\r\n";
        public const string TAB = "\t";
        public const string REPLACE_NAMESPACE = "#replace_namespace";
        public const string REPLACE_CLASS_NAME = "#replace_classname";
        public const string CLASS_TEMPLATE =
            "using GUI_Testing_Automation;" + NEW_LINE +
            NEW_LINE +
            "namespace " + REPLACE_NAMESPACE + NEW_LINE +
            "{" + NEW_LINE +
                "public class " + REPLACE_CLASS_NAME + NEW_LINE +
                "{" + NEW_LINE +
                "}" + NEW_LINE +
            "}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRootElements"></param>
        /// <param name="folderOutPath">path to folder location to write file</param>
        /// <param name="className">file's name and class's name</param>
        /// <returns></returns>
        public FileInfo Generate(List<IElement> listRootElements, string folderOutPath, string className, string projectName)
        {
            string fileContent = "";
            List<string> rootElementVarName = new List<string>();
            foreach (IElement rootElement in listRootElements)
            {
                fileContent += ElementDefine(rootElement);
                rootElementVarName.Add("_" + FormatName(rootElement.Attributes.Name));//TODO: should merge into 1 function
            }
            ElementsDefinitionRewriter eleDefRewrite = new ElementsDefinitionRewriter();
            eleDefRewrite.ClassName = className;
            eleDefRewrite.ContentAppend = fileContent;
            eleDefRewrite.RootElementsVarName = rootElementVarName;

            SyntaxTree tree = CSharpSyntaxTree.ParseText(CLASS_TEMPLATE
                .Replace(REPLACE_NAMESPACE, projectName)
                .Replace(REPLACE_CLASS_NAME, className));
            var root = (CompilationUnitSyntax)tree.GetRoot();

            SyntaxNode newRoot = eleDefRewrite.Visit(root);

            //var workspace = MSBuildWorkspace.Create();
            //SyntaxNode formattedNode = Formatter.Format(newRoot, workspace);
            //logger.Debug("abc: " + formattedNode);

            String filePath = Path.Combine(folderOutPath, className + ".cs");
            Utils.RemoveOldFile(filePath, false);
            File.WriteAllText(filePath, Utils.ReformatCsCode(newRoot.ToFullString()));
                //formattedNode.ToFullString());
            return new FileInfo(filePath);
        }

        public string ElementDefine(IElement rootElement)
        {
            string replace = GenParentElementClassAndroid(rootElement, "");
            return replace;
        }

        //Test Android
        public string ElementDefineAndroid(IElement rootElement, AndroidDevice androidDevice, string folderOutPath)
        {
            string replace = NEW_LINE + DeviceAndroidDefine(androidDevice) + NEW_LINE + NEW_LINE;
            replace += GenParentElementClassTestAndroid(rootElement, "") + NEW_LINE + "}";
            return replace;
        }
        //Test
        public string GenParentElementClassTestAndroid(IElement element, string childrenContent)
        {
            //item-element
            if (element.Children == null || element.Children.Count <= 0)
            {
                return ItemElementDefineAndroid(new ArgumentInitElement(
                            element.Attributes.Xpath,
                            element.GetType().Name.Replace("GUI_Testing_Automation", ""),
                            FormatName(element.Attributes.Name),
                            element.Attributes.ElementType));
            }
            /**
             * container element
             **/
            else
            {
                NormalizeName(element.Children);
                string childrenScripts = "";
                List<string> childrenVars = new List<string>();
                foreach (IElement child in element.Children)
                {
                    childrenScripts += GenParentElementClassTestAndroid(child, childrenContent);
                    
                    childrenVars.Add("" + FormatName(child.Attributes.Name));
                }
                string elementScripts = ContainElementDefineAndroid(
                    new ArgumentInitElement(
                        element.Attributes.Xpath,
                        FormatName(element.Attributes.Name) + "_Class",
                        FormatName(element.Attributes.Name),
                        element.Attributes.ElementType),
                    childrenVars,
                    FormatName(element.Attributes.Name) + "_Class",
                    element.GetType().Name.Replace("GUI_Testing_Automation", ""),
                    childrenScripts);
                return elementScripts;
            }
        }
        // Android
        private string ContainElementDefineAndroid(ArgumentInitElement arg, List<string> childrenVars,
                string className, string classNameBase, string childrenContent)
        {
            string definitionElements = TAB + "IElement " + arg.VariableName.Replace(" ", "_") + " = new ElementBase(new ElementAttributes(\""
                + arg.VariableName.Replace(" ", "_") + "\", \"" + arg.Id + "\"));" + NEW_LINE;

            definitionElements += childrenContent;

            return definitionElements;
        }
        // Android
        private string ItemElementDefineAndroid(ArgumentInitElement arg)
        {
            string childClassName = arg.ClassName;
            string childVariableName = "" + arg.VariableName;
            string elementDefine = TAB + "IElement " + " " + childVariableName.Replace(" ", "_") + " = new ElementBase(new ElementAttributes( \""
                    + childVariableName.Replace(" ", "_") + "\", \"" + arg.Id + "\"));" + NEW_LINE;
            return elementDefine;
        }

        public string GenParentElementClassAndroid(IElement element, string childrenContent)
        {
            //item-element
            if (element.Children == null || element.Children.Count <= 0)
            {
                return ItemElementDefine(new ArgumentInitElement(
                            element.Id,
                            element.GetType().Name.Replace("GUI_Testing_Automation", ""),
                            FormatName(element.Attributes.Name),
                            element.Attributes.ElementType));
            }
            /**
             * container element
             **/
            else
            {
                NormalizeName(element.Children);
                string childrenScripts = "";
                List<string> childrenVars = new List<string>();
                foreach (IElement child in element.Children)
                {
                    childrenScripts += GenParentElementClassAndroid(child, childrenContent);
                    //if children is container, so must add statement declare
                    //if (child.Children != null && child.Children.Count > 0)
                    //{
                    //    FormatName(element.Name) + "_Class";
                    //}
                    childrenVars.Add("_" + FormatName(child.Attributes.Name));
                }
                string elementScripts = ContainElementDefine(
                    new ArgumentInitElement(
                        element.Id,
                        FormatName(element.Attributes.Name) + "_Class",
                        FormatName(element.Attributes.Name),
                        element.Attributes.ElementType),
                    childrenVars,
                    FormatName(element.Attributes.Name) + "_Class",
                    element.GetType().Name.Replace("GUI_Testing_Automation", ""),
                    childrenScripts);
                return elementScripts;
            }
        }

        private string ContainElementDefine(ArgumentInitElement arg, List<string> childrenVars,
                string className, string classNameBase, string childrenContent)
        {
            string definitionElements = className + " _" + arg.VariableName + " = new " +
                className + "(\"" + arg.Id + "\");" + NEW_LINE;
            definitionElements += "public " + className + " " + arg.VariableName + NEW_LINE +
                "{" + NEW_LINE +
                    "get { return _" + arg.VariableName + "; }" + NEW_LINE +
                "}" + NEW_LINE;
            definitionElements += "public class " + className + " : " + classNameBase + NEW_LINE;
            definitionElements += "{" + NEW_LINE;
            definitionElements += "public " + className + "(string id) : base(id)" + NEW_LINE +
                "{" + NEW_LINE;
            foreach (string childVar in childrenVars)
            {
                definitionElements += "this.Children.Add(" + childVar + ");" + NEW_LINE;
                definitionElements += childVar + ".Parent = this;" + NEW_LINE;
            }
            definitionElements += "}" + NEW_LINE;
            definitionElements += childrenContent;
            definitionElements += "}" + NEW_LINE;

            return definitionElements;
        }

        private string ItemElementDefine(ArgumentInitElement arg)
        {
            string childClassName = arg.ClassName;
            string childVariableName = "_" + arg.VariableName;
            string elementDefine = childClassName + " " + childVariableName + " = new " + childClassName + "(\""
                    + arg.Id + "\");" + NEW_LINE;
            elementDefine += "public " + childClassName + " " + FormatAttributeName(arg.VariableName) + NEW_LINE;
            elementDefine += "{" + NEW_LINE;
            elementDefine += "get { return " + childVariableName + "; }" + NEW_LINE;
            elementDefine += "}" + NEW_LINE;
            return elementDefine;
        }

        public static string FormatName(string name)
        {
            return name.Replace(" ", "_");
        }

        public string FormatAttributeName(string name)
        {
            if (!char.IsLetter(name[0]))
                return "AUTO_ADD" + name;
            if (char.IsLower(name[0]))
                return name[0].ToString().ToUpper() + name.Substring(1);
            return name;
        }

        private void NormalizeName(List<IElement> listElement)
        {
            foreach (IElement element in listElement)
            {
                List<int> indexOccurrence = SearchOccurrence(listElement, element);
                if (indexOccurrence.Count > 1)
                {
                    logger.Warn("Found element has 2 or more children element with the same name: " + listElement[indexOccurrence[0]].Attributes.Name);
                    for (int fi = 0; fi < indexOccurrence.Count; fi++)
                    {
                        listElement[indexOccurrence[fi]].Attributes.Name = listElement[indexOccurrence[fi]].Attributes.Name + (fi + 1).ToString();
                    }
                }
            }
        }

        private List<int> SearchOccurrence(List<IElement> listElement, IElement elementCompare)
        {
            List<int> re = new List<int>();
            for (int fi = 0; fi < listElement.Count; fi++)
            {
                IElement element = listElement[fi];
                if (element.Attributes.Name.Equals(elementCompare.Attributes.Name) &&
                    element.GetType().Name.Equals(elementCompare.GetType().Name))
                    re.Add(fi);
            }
            return re;
        }
        // Generate code device
        public string DeviceAndroidDefine(AndroidDevice androidDevice)
        {
            string deviceString = TAB + "AndroidDevices androidDevices = new AndroidDevices(\"";
            deviceString += androidDevice.Ip + "\",\"";
            deviceString += androidDevice.Version + ".0\",\"";
            deviceString += androidDevice.Activity + "\",\"";
            deviceString += androidDevice.Package + "\"";
            deviceString += ");";
            return deviceString;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ElementsDefinitionRewriter : CSharpSyntaxRewriter
    {
        public const string NEW_LINE = ElementCSharpCodeGeneration.NEW_LINE;
        string className;
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        public string ContentAppend
        {
            get { return contentAppend; }
            set { contentAppend = value; }
        }

        public List<string> RootElementsVarName
        {
            get { return rootElementsVarName; }
            set { rootElementsVarName = value; }
        }
        string contentAppend;
        List<string> rootElementsVarName;

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            string clname = node.Identifier.Text.ToString();
            if (node.Identifier.Text.ToString().Equals(className))
            {
                string constructorFuncExp =
                    "public " + className + "(string elementsRepoFilePath, string imageCaptureFilePath)" +
                    NEW_LINE + "{" +
                        NEW_LINE + "XmlFilesLoader.Load3(elementsRepoFilePath, imageCaptureFilePath";
                foreach (string rootVarName in rootElementsVarName)
                {
                    constructorFuncExp += "," + NEW_LINE + rootVarName;
                }
                constructorFuncExp += ");" + NEW_LINE + "}";
                SyntaxTree tree1 = CSharpSyntaxTree.ParseText(constructorFuncExp);
                var tempRoot1 = (CompilationUnitSyntax)tree1.GetRoot();
                var tempMethod1 = (MethodDeclarationSyntax)tempRoot1.Members[0];

                //add method and statement declare here
                string fakeClass = "class FakeClass \n{" + contentAppend + "\n}";
                SyntaxTree tree = CSharpSyntaxTree.ParseText(fakeClass);
                var tempRoot = (CompilationUnitSyntax)tree.GetRoot();
                var tempClass = (ClassDeclarationSyntax)tempRoot.Members[0];
                return node.WithMembers(tempClass.Members.Insert(0, tempMethod1));
            }
            return base.VisitClassDeclaration(node);
        }
    }
}
