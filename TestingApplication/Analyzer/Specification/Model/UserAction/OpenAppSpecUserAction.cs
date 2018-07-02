// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:25 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class OpenAppSpecUserAction : AbstractSpecUserAction
    {
        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string re = "Handler.OpenApp(";
            if (scriptGenerationParams.PathToApp != null &&
                File.Exists(scriptGenerationParams.PathToApp))
                re += "\"" + scriptGenerationParams.PathToApp + "\");";
            else 
                re += "Config.APP_PATH);";
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
            return new ScriptsExpression("Host.Local.RunApplication(@\"" +
                    pathToExeFile + "\", \"\", \"\", false);");
        }
    }
}
