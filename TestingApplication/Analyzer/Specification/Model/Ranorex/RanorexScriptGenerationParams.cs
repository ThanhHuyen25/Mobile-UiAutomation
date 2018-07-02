// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 1:32 PM 2018/5/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class RanorexScriptGenerationParams : ScriptGenerationParams, IRanorexScriptGenerationParams
    {
        // here we are, put some attibutes into it
        public RanorexScriptGenerationParams()
        {
        }
        public RanorexScriptGenerationParams(ScriptGenerationParams p) : this()
        {
            CopyAttributes(this, p);
        }

        public static IRanorexScriptGenerationParams CloneFromNormal(IScriptGenerationParams para)
        {
            IRanorexScriptGenerationParams re;
            if (para is RanorexValidationUCScriptGenerationParams)
            {
                re = new RanorexValidationUCScriptGenerationParams();
                ((RanorexValidationUCScriptGenerationParams)re).CopyAttributesFrom(para as RanorexValidationUCScriptGenerationParams);
            }
            else if (para is RanorexWaitValidateScriptGenerationParams)
            {
                re = new RanorexWaitValidateScriptGenerationParams();
                ((RanorexWaitValidateScriptGenerationParams)re).CopyAttributesFrom(para as RanorexWaitValidateScriptGenerationParams);
            }
            else if (para is RanorexUCScriptGenerationParams)
            {
                re = new RanorexUCScriptGenerationParams();
                ((RanorexUCScriptGenerationParams)re).CopyAttributesFrom(para as RanorexUCScriptGenerationParams);
            }
            else if (para is RanorexScriptGenerationParams)
            {
                re = new RanorexScriptGenerationParams();
                ((RanorexScriptGenerationParams)re).CopyAttributesFrom(para as RanorexScriptGenerationParams);
            }
            else if (para is ValidationUCScriptGenerationParams)
            {
                re = new RanorexValidationUCScriptGenerationParams();
                ((ValidationUCScriptGenerationParams)re).CopyAttributesFrom(para as ValidationUCScriptGenerationParams);
            }
            else if (para is WaitValidateScriptGenerationParams)
            {
                re = new RanorexWaitValidateScriptGenerationParams();
                ((WaitValidateScriptGenerationParams)re).CopyAttributesFrom(para as WaitValidateScriptGenerationParams);
            }
            else if (para is UserCodeScriptGenerationParams)
            {
                re = new RanorexUCScriptGenerationParams();
                ((UserCodeScriptGenerationParams)re).CopyAttributesFrom(para as UserCodeScriptGenerationParams);
            }
            else
            {
                re = new RanorexScriptGenerationParams();
                ((ScriptGenerationParams)re).CopyAttributesFrom(para as ScriptGenerationParams);
            }
            CopyAttributes(re, para);
            return re;
        }
    }
}
