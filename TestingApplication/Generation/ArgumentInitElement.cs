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
    /// <summary>
    /// args for constructing element
    /// current is: (designedId, className, variableName, elementType)
    /// </summary>
    public class ArgumentInitElement
    {
        public ArgumentInitElement(string id, string className, string variableName, string elementType)
        {
            this.id = id;
            this.className = className;
            this.variableName = variableName;
            this.elementType = elementType;
        }

        string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
       
        string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        string variableName;

        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; }
        }

        private string elementType;

        public string ElementType
        {
            get { return elementType; }
            set { elementType = value; }
        }
    }
}
