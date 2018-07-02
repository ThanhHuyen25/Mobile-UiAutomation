// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 9:18 AM 2017/10/5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    class ClassGeneration
    {
        public const string NEW_LINE = "\r\n";
        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        private string classNameBase;

        public string ClassNameBase
        {
            get { return classNameBase; }
            set { classNameBase = value; }
        }

        private List<ArgumentInitElement> childrenElement;
        public List<ArgumentInitElement> ChildrenElement
        {
            get { return childrenElement; }
            set { childrenElement = value; }
        }

        /// <summary>
        /// generate class definition code
        /// </summary>
        /// <param name="minTab"></param>
        /// <returns></returns>
        public string Generate(string childrenContent)
        {
            string relativeElements = "";
            string definitionElements = "public class " + this.className + " : " + this.classNameBase + NEW_LINE;
            definitionElements += "{" + NEW_LINE;
            definitionElements += "public " + this.className + "(string id) : base(id)" + NEW_LINE +
                "{" + NEW_LINE ;
                //"ComboBox_Log_Mode_Class(string designedId, string designedName) : base(designedId, designedName) { }\n";

            string tempElementDefine = "";
            foreach(ArgumentInitElement child in childrenElement)
            {
                /**
                 * define new element
                 **/
                string childClassName = child.ClassName;
                string childVariableName = "_" + child.VariableName;
                tempElementDefine +=childClassName + " " + childVariableName + " = new " + childClassName + "(\""
                        + child.Id + "\");" + NEW_LINE;
                tempElementDefine += "public " + childClassName + " " + FormatAttributeName(child.VariableName) + NEW_LINE;
                tempElementDefine += "{" + NEW_LINE;
                tempElementDefine += "get { return " + childVariableName + "; }" + NEW_LINE;
                tempElementDefine += "}" + NEW_LINE;
                
                /**
                 * define relative elements
                 **/
                relativeElements += "this.Children.Add(" + childVariableName + ");" + NEW_LINE;
                relativeElements += childVariableName + ".Parent = this;" + NEW_LINE;
            }
            definitionElements += relativeElements;
            definitionElements += "}" + NEW_LINE;
            definitionElements += tempElementDefine;

            //add
            definitionElements += childrenContent;

            definitionElements += "}" + NEW_LINE;
            return definitionElements;
        }

        public string FormatAttributeName(string name)
        {
            if (!char.IsLetter(name[0]))
                return "AUTO_ADD" + name;
            if (char.IsLower(name[0]))
                return name[0].ToString().ToUpper() + name.Substring(1);
            return name;
        }
    }
}
