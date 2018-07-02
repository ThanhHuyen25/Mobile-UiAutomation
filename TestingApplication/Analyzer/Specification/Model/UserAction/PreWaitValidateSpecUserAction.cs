// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:28 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class PreWaitValidateSpecUserAction : WaitValidateSpecUserAction
    {
        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            var param = scriptGenerationParams as WaitValidateScriptGenerationParams;
            string re = GetComment(scriptGenerationParams);
            // generate procedure
            foreach (var userAction in param.ListUserActions)
            {
                if (re != "")
                    re += NEW_LINE;
                re += userAction.GenRawScripts(userAction.Params).Expression;
            }
            re += NEW_LINE;
            // generate while check
            re += GetWhileWaitScripts(param);
            return new ScriptsExpression(re);
        }
    }
}
