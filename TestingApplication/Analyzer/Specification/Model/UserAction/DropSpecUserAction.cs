// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:23 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class DropSpecUserAction : AbstractSpecUserAction
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
            string re = "Mouse.MoveTo(" + getScriptAccessRanorexElement(scriptGenerationParams.SpecNode, scriptGenerationParams.InstanceName) + ");"
                + NEW_LINE
                + "Mouse.ButtonDown(System.Windows.Forms.MouseButtons.Left);"
                + NEW_LINE
                + "Mouse.MoveTo(";

            Regex pattern1 = new Regex("Drop\\s+(?<word>\\w+)");
            Regex pattern2 = new Regex("Drop\\s*\\((?<first>\\d+),\\s*(?<second>\\d+)\\)");

            Match matcher1 = pattern1.Match(expression);
            Match matcher2 = pattern2.Match(expression);

            if (matcher1.Success)
            {
                string otherElementStr = matcher1.Groups["word"].Value;
                var otherElement =
                        Utils.SearchIElement(otherElementStr, scriptGenerationParams.ListUIElements);
                re += GetScriptAccessElement(otherElement, scriptGenerationParams.InstanceName);
            }
            else if (matcher2.Success)
            {
                string arg1Str = matcher2.Groups["first"].Value;
                string arg2Str = matcher2.Groups["second"].Value;
                re += Int32.Parse(arg1Str) + ";" + Int32.Parse(arg2Str);
            }
            else
            {
                LogError(logger, "problem with expression: " + expression);
                return null;
            }
            re += ");";
            re += NEW_LINE;
            re += "Mouse.ButtonUp(System.Windows.Forms.MouseButtons.Left);";
            return new ScriptsExpression(re);
        }
    }
}
