// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:32 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class WaitExistSpecUserAction : AbstractSpecUserAction
    {
        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            throw new NotImplementedException();
        }

        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            throw new NotImplementedException();
        }

        public override ScriptsExpression GenRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            return base.GenRanorexScripts(scriptGenerationParams);
        }

        public override ScriptsExpression GenRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string re = getScriptAccessRanorexElement(scriptGenerationParams.SpecNode, scriptGenerationParams.InstanceName) + "Info.";
            if (expression.StartsWith(WAIT_EXIST))
            {
                re += "WaitForExists(";
                re += expression.Substring(WAIT_EXIST.Length).Trim();
                re += ");";
            }
            else if (expression.StartsWith(WAIT_NOT_EXIST))
            {
                re += "WaitForNotExists(";
                re += expression.Substring(WAIT_NOT_EXIST.Length).Trim();
                re += ");";
            }
            else
                LogError(logger, "not handled for expression: " + expression);
            return new ScriptsExpression(re);
        }
    }
}
