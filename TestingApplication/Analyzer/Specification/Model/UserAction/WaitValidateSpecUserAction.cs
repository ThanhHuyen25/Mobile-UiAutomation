// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:33 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public abstract class WaitValidateSpecUserAction : AbstractSpecUserAction
    {
        const string TEMPLATE = 
            "int MAX_STEP#idx1 = #r1;" + NEW_LINE +
            "bool[] checks#idx1 = new bool[#r2];" + NEW_LINE +
            "int step#idx1 = 0;" + NEW_LINE +
            "while (step#idx1 < MAX_STEP#idx1 && System.Array.IndexOf(checks#idx1, false) >= 0)" + NEW_LINE +
            "{" + NEW_LINE +
                "#r3" + NEW_LINE +
                "Delay.Duration(Config.DURATION);" + NEW_LINE +
                "step#idx1 += 1;" + NEW_LINE +
            "}";

        const string TEMPLATE_2 =
            //"cmt#1" + NEW_LINE +
            "if (!checks#idx1[#idx2])" + NEW_LINE +
                "checks#idx1[#idx2] = #r1";
            //"if (checks#idx1[#idx2] == 0)" + NEW_LINE +
            //"{" + NEW_LINE +
                //"#r2" + NEW_LINE +
            //"}";

        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            throw new NotImplementedException();
        }

        //public abstract ScriptsExpression GenScripts(ScriptGenerationParams scriptGenerationParams);

        public string GetWhileWaitScripts(WaitValidateScriptGenerationParams param)
        {
            string re = TEMPLATE
                .Replace("#idx1", param.WaitIndex + "")
                .Replace("#r1", this.Expression)
                .Replace("#r2", param.ListUserActions.Count + "");
            string r3 = "";
            for (int fi = 0; fi < param.ListUserActions.Count; fi++)
            {
                var userAction = param.ListUserActions[fi];
                if (r3 != "")
                    r3 += NEW_LINE;
                r3 += TEMPLATE_2
                    .Replace("#idx1", param.WaitIndex + "")
                    .Replace("#idx2", fi + "");
                //.Replace("#r1", userAction.GenRawScripts(userAction.Params).Expression);
                if (userAction is ValidationSpecUserAction || userAction is ContainSpecUserAction ||
                    userAction is CheckExistSpecUserAction)
                {
                    r3 = r3.Replace("#r1", userAction.GenRawScripts(userAction.Params).Expression);
                }
                else if (userAction is SetSpecUserAction || userAction is PressSpecUserAction)
                {
                    if (userAction.Params.SpecNode.Attribute == null ||
                        userAction.Params.SpecNode.Attribute == "")
                        userAction.Params.SpecNode.Attribute = TEXT;
                    r3 = r3.Replace("#r1", 
                        ValidationSpecUserAction.GetRawScripts(userAction.Params, userAction.Expression).Expression);
                }
            }
            re = re.Replace("#r3", r3);
            return re;
        }

        public override ScriptsExpression GenRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            throw new NotImplementedException();
        }

        public override ScriptsExpression GenRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            throw new NotImplementedException();
        }
    }

    public class WaitValidateScriptGenerationParams : ScriptGenerationParams
    {
        protected List<AbstractSpecUserAction> listUserActions;

        public List<AbstractSpecUserAction> ListUserActions
        {
            get { return listUserActions; }
            set
            {
                listUserActions = value;
            }
        }
        public int WaitIndex
        {
            get { return waitIndex; }
            set
            {
                waitIndex = value;
            }
        }

        protected int waitIndex;

        public void CopyAttributesFrom(WaitValidateScriptGenerationParams source)
        {
            base.CopyAttributesFrom(source);
            this.listUserActions = source.listUserActions;
            this.waitIndex = source.waitIndex;
        }
    }
}