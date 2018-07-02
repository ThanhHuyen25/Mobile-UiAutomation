// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 1:59 PM 2018/1/21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class CaptureSpecUserAction : AbstractSpecUserAction
    {
        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string re = GetScriptCapture(scriptGenerationParams.SpecNode.UIElement, scriptGenerationParams.InstanceName);
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
            string scripts = getRanorexScriptCapture(
                getAppFolderObjectElement(scriptGenerationParams.SpecNode.UIElement, scriptGenerationParams.InstanceName),
                scriptGenerationParams.ScreenName, scriptGenerationParams.Id, scriptGenerationParams.SpecNode.GetNormalizedName());
            return new ScriptsExpression(scripts);
        }
    }
}
