// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:22 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class DelaySpecUserAction : AbstractSpecUserAction
    {
        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string re = "Delay.Duration(" + this.Expression + "*Config.DURATION);";
            return new ScriptsExpression(re);
        }

        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            return base.GenScripts(scriptGenerationParams);
        }

        public override ScriptsExpression GenRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            return base.GenRanorexScripts(scriptGenerationParams);
        }

        public override ScriptsExpression GenRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string re = "Delay.Milliseconds(" + NormalizeExpression1(expression) + "*Config.DURATION);";
            return new ScriptsExpression(re);
        }

        private string NormalizeExpression1(string exp)
        {
            Match matcher1 = Regex.Match(exp, "Delay\\s*\\{(\\d+?)\\}");
            if (matcher1.Success)
                return matcher1.Groups[1].Value;
            return exp;
        }
    }
}
