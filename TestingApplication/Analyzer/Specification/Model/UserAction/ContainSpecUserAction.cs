// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:09 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class ContainSpecUserAction : AbstractSpecUserAction
    {
        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string attribute = scriptGenerationParams.SpecNode.Attribute.Trim();
            string re = "";
            if (attribute == null || attribute == "" || attribute.Equals("text", StringComparison.OrdinalIgnoreCase))
            {
                re += "Validate.";
                string value = expression;
                if (expression.StartsWith(ATTRIBUTE_CONTAIN))
                {
                    value = new Regex(ATTRIBUTE_CONTAIN).Replace(expression, "", 1);
                    re += "TextContain(";
                }
                else if (expression.StartsWith(ATTRIBUTE_NOT_CONTAIN))
                {
                    value = new Regex(ATTRIBUTE_NOT_CONTAIN).Replace(expression, "", 1);
                    re += "TextNotContain(";
                }
                value = NormalizeExpression(value);
                re += GetScriptAccessElement(scriptGenerationParams.SpecNode.UIElement, scriptGenerationParams.InstanceName);
                re += ", \"" + value + "\");";
                return new ScriptsExpression(re);
            }
            else
                throw new NotImplementedException();
        }

        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            return base.GenScripts(scriptGenerationParams);
        }

        public override ScriptsExpression GenRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string reStr = GetComment(scriptGenerationParams) + NEW_LINE;
            var re = GenRawRanorexScripts(scriptGenerationParams);
            if (re == null)
                return null;
            re.Expression = SoundWithTryCatch(re.Expression,
                getRanorexScriptCapture(
                    getAppFolderObjectElement(scriptGenerationParams.SpecNode.UIElement, scriptGenerationParams.InstanceName),
                    scriptGenerationParams.ScreenName, scriptGenerationParams.Id, scriptGenerationParams.SpecNode.GetNormalizedName()));
            re.Expression = reStr + re.Expression;
            return re;
        }

        public override ScriptsExpression GenRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string value = getValue(expression);
            if (value == null)
            {
                return null;
            }
            string re = "Validate.";
            re += "Attribute(" + getScriptAccessRanorexObject(scriptGenerationParams) + ", ";
            re += "\"" + getRealAttribute(scriptGenerationParams.SpecNode) + "\", ";
            re += "new Regex(";
            if (expression.StartsWith(ATTRIBUTE_CONTAIN))
            {
                re += "Regex.Escape(";
                re += "\"" + value + "\")));";
            }
            else if (expression.StartsWith(ATTRIBUTE_NOT_CONTAIN))
            {
                re += "\"^((?!(\"+Regex.Escape(";
                re += "\"" + value + "\")+\"))(.|\\n))*$\"));";
            }
            return new ScriptsExpression(re);
        }

        private string getValue(string exp)
        {
            string re = "";
            if (exp.StartsWith(ATTRIBUTE_CONTAIN))
                re = exp.Substring(ATTRIBUTE_CONTAIN.Length);
            if (exp.StartsWith(ATTRIBUTE_NOT_CONTAIN))
                re = exp.Substring(ATTRIBUTE_NOT_CONTAIN.Length);
            re = re.Trim();
            if (!Regex.IsMatch(re, SINGLE_QUOTE))
                return null;
            re = RemoveSingleQuote(re);
            re = replaceReturnChar(re);
            return re;
        }
    }
}
