// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:05 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class CloseAppSpecUserAction : AbstractSpecUserAction
    {
        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string re = "Handler.CloseApp(\"" + scriptGenerationParams.PathToApp + "\");";
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
            string pathToExeFile = scriptGenerationParams.PathToApp;
            string re = "Host.Local.KillApplications(\"" +
                pathToExeFile.Substring(
                        pathToExeFile.LastIndexOf(@"\")+ 1,
                        pathToExeFile.LastIndexOf("."))
                        .Replace(@"\", @"\\")
                + "\");";
            return new ScriptsExpression(re);
        }
    }
}
