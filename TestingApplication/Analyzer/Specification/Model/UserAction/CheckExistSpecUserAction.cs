// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:03 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class CheckExistSpecUserAction : AbstractSpecUserAction
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GenerateStatement(string objectElement)
        {
            string re = "";
            switch (this.expression)
            {
                case EXIST:
                    re += "Validate.Exists(";
                    re += objectElement + ");";
                    break;
                case NOT_EXIST:
                    re += "Validate.NotExists(";
                    re += objectElement + ");";
                    break;
                default:
                    logger.Error("expression fail: " + expression);
                    break;
            }
            return re;
        }

        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string objectElement = GetScriptAccessElement(
                scriptGenerationParams.SpecNode.UIElement,
                scriptGenerationParams.InstanceName);
            string re = GenerateStatement(objectElement);
            return new ScriptsExpression(re);
        }

        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string reStr = GetComment(scriptGenerationParams) + NEW_LINE;
            var re = GenRawScripts(scriptGenerationParams);
            if (re == null)
                return null;
            re.Expression = SoundWithTryCatch(re.Expression, "");
            re.Expression = reStr + re.Expression;
            return re;
        }

        public override ScriptsExpression GenRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string reStr = GetComment(scriptGenerationParams) + NEW_LINE;
            var re = GenRawRanorexScripts(scriptGenerationParams);
            re.Expression = SoundWithTryCatch(re.Expression, 
                getRanorexScriptCapture(
                    getAppFolderObjectElement(scriptGenerationParams.SpecNode.UIElement, scriptGenerationParams.InstanceName),
                    scriptGenerationParams.ScreenName, scriptGenerationParams.Id, scriptGenerationParams.SpecNode.GetNormalizedName()));
            re.Expression = reStr + re.Expression;
            return re;
        }

        public override ScriptsExpression GenRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string objectElement = getScriptAccessRanorexObject(scriptGenerationParams);
            string re = GenerateStatement(objectElement);
            return new ScriptsExpression(re);
        }
    }
}
