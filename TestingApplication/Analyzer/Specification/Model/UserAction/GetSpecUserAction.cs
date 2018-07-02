// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:24 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class GetSpecUserAction : AbstractSpecUserAction
    {
        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            return base.GenScripts(scriptGenerationParams);
        }

        public override ScriptsExpression GenRawScripts(IScriptGenerationParams _params)
        {
            string re = GetScriptAccessElement(_params.SpecNode.UIElement, _params.InstanceName);
            string attribute = _params.SpecNode.Attribute.Trim();
            if (attribute == null || attribute == "" || attribute.Equals("text", StringComparison.OrdinalIgnoreCase))
            {
                re += ".GetText();";
            }
            else if (attribute.Equals("width", StringComparison.OrdinalIgnoreCase))
            {
                re += ".GetWidth();";
            }
            else if (attribute.Equals("height", StringComparison.OrdinalIgnoreCase))
            {
                re += ".GetHeight();";
            }
            return new ScriptsExpression(re);
        }

        public override ScriptsExpression GenRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            return base.GenRanorexScripts(scriptGenerationParams);
        }

        public override ScriptsExpression GenRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string re = getScriptAccessRanorexElement(scriptGenerationParams.SpecNode, scriptGenerationParams.InstanceName) + ".Element.";
            Regex pattern1 = new Regex("Get\\s+(?<word>\\w+)");
            Match matcher1 = pattern1.Match(expression);
            if (matcher1.Success)
            {
                re += "GetAttributeValue(" + matcher1.Groups["word"] + ");";
            }
            else
            {
                LogError(logger, "expression fail :" + expression);
            }
            return new ScriptsExpression(re);
        }
    }
}
