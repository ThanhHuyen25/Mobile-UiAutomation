// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:31 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class ValidationSpecUserAction : AbstractSpecUserAction
    {
        public static ScriptsExpression GetRawScripts(IScriptGenerationParams param, string expression)
        {
            string re = "Validate.";
            string scriptAccessEle = GetScriptAccessElement(param.SpecNode.UIElement, param.InstanceName);
            switch (param.SpecNode.Attribute)
            {
                case TEXT:
                    re += "TextEquals(" + scriptAccessEle + ", \"" + NormalizeExpression(expression) + "\");";
                    break;
                case WIDTH:
                    re += "WidthEquals(" + scriptAccessEle + ", " + expression + ");";
                    break;
                case HEIGHT:
                    re += "HeightEquals(" + scriptAccessEle + ", " + expression + ");";
                    break;
                //case BOUND_RECT:
                //    // TODO: remove hard code
                //    re += ".ValidateBoundRectAttribute(" + expression + ");";
                //    break;
            }
            return new ScriptsExpression(re);
        }

        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            return GetRawScripts(scriptGenerationParams, expression);
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
            return GetRawRanorexScripts(scriptGenerationParams, this.expression);
        }

        public static ScriptsExpression GetRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams, string expression)
        {
            string value = null;
            if (checkValidValueExpress(expression))
            {
                value = formatValue(expression);
                value = replaceReturnChar(value);
            }
            else
            {
                string log = "[Error] Expression: " + expression + " at sheet " + scriptGenerationParams.ScreenName +
                ", testcase #" + scriptGenerationParams.Id + NEW_LINE + "Invalid expression: " + expression;
                scriptGenerationParams.MyLog.Error(log);
                logger.Error(log);
                return null;
            }
            string re = "Validate.Attribute(" + getScriptAccessRanorexObject(scriptGenerationParams) + ", ";
            re += "\"" + getRealAttribute(scriptGenerationParams.SpecNode) + "\", \"" + value + "\"" + ");";
            return new ScriptsExpression(re);
        }
    }
}
