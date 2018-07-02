// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 3:11 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class FunctionExpression
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string VOID = "void";
        public const string PUBLIC = "public";
        public const string PRIVATE = "private";
        public const string PROTECTED = "protected";

        private String name;
        private String accessibility = PUBLIC;
        private String summary;
        private List<ParameterExpression> _params;

        //"void" or a description
        private String returnDescription = VOID;
        private String content;

        public FunctionExpression(String name)
        {
            this.name = name;
        }

        public FunctionExpression(String name, String accessibility, String summary, List<ParameterExpression> _params)
        {
            this.name = name;
            this.accessibility = accessibility;
            this.summary = summary;
            this._params = _params;
        }

        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public String getAccessibility()
        {
            return accessibility;
        }

        public void setAccessibility(String accessibility)
        {
            this.accessibility = accessibility;
        }

        public String getSummary()
        {
            return summary;
        }

        public void setSummary(String summary)
        {
            this.summary = summary;
        }

        public List<ParameterExpression> getParams()
        {
            return _params;
        }

        public void setParams(List<ParameterExpression> _params)
        {
            this._params = _params;
        }

        public String getReturnDescription()
        {
            return returnDescription;
        }

        public void setReturnDescription(String returnDescription)
        {
            this.returnDescription = returnDescription;
        }

        public String getContent()
        {
            return content;
        }

        public void setContent(String content)
        {
            this.content = content;
        }

        override
        public bool Equals(object other)
        {
            if (!(other is FunctionExpression))
                return false;
            FunctionExpression other1 = (FunctionExpression)other;
            if (!this.name.Equals(other1.name))
                return false;
            if (GetNoParams(this) != GetNoParams(other1))
                return false;
            if (IsVoidFunc(this) != IsVoidFunc(other1))
                return false;
            return true;
        }

        public static int GetNoParams(FunctionExpression func)
        {
            if (func.getParams() == null)
                return 0;
            return func.getParams().Count;
        }

        public static bool IsVoidFunc(FunctionExpression func)
        {
            if (func.getReturnDescription() == null ||
                func.getReturnDescription().Equals(VOID, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        public string GetCorrectAccessibility()
        {
            if (accessibility == null)
                return PUBLIC;
            if (accessibility == "")
                return "";
            accessibility = accessibility.Trim().ToLower();
            if (accessibility.Equals(PUBLIC) || accessibility.Equals(PRIVATE) || accessibility.Equals(PROTECTED))
                return accessibility;
            logger.Error("unknow accessibility type: " + accessibility + ". Choose 'public' by default");
            return PUBLIC;
        }

        public string GetCorrectReturnType()
        {
            if (returnDescription == null ||
                returnDescription.Equals("") ||
                returnDescription.Trim().Equals(VOID, StringComparison.OrdinalIgnoreCase))
                return VOID;
            return "object";
        }
    }
}
