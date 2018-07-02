// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:26 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class PostWaitValidateSpecUserAction : WaitValidateSpecUserAction
    {
        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string re = GetComment(scriptGenerationParams);
            var param = scriptGenerationParams as WaitValidateScriptGenerationParams;
            // generate while check
            return new ScriptsExpression(re + NEW_LINE + GetWhileWaitScripts(param));
        }
    }
}
