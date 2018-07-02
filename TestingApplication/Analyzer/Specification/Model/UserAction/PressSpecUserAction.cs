// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:27 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GUI_Testing_Automation;

namespace TestingApplication
{
    public class PressSpecUserAction : AbstractSpecUserAction
    {
        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string re = "";
            string scriptAccessEle = GetScriptAccessElement(
                scriptGenerationParams.SpecNode.UIElement, scriptGenerationParams.InstanceName);
            string value = NormalizeExpression(this.expression);
            re += scriptAccessEle + ".InputString(\"" + value + "\");";
            return new ScriptsExpression(re);
        }

        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string reStr = GetComment(scriptGenerationParams) + NEW_LINE;
            var re = GenRawScripts(scriptGenerationParams);
            if (re == null)
                return null;
            re.Expression = reStr + re.Expression;
            // if procedure, need to validate
            if (scriptGenerationParams.Color.Equals(PRE_CONDITION_COLOR))
            {
                string scriptAccessEle = GetScriptAccessElement(
                    scriptGenerationParams.SpecNode.UIElement, scriptGenerationParams.InstanceName);
                string value = NormalizeExpression(this.expression);
                re.Append(NEW_LINE + "Validate.TextEquals(" + scriptAccessEle + ", \"" + value + "\");");
            }
            return re;
        }

        public override ScriptsExpression GenRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string reStr = GetComment(scriptGenerationParams) + NEW_LINE;
            var re = GenRawRanorexScripts(scriptGenerationParams);
            if (re == null)
                return null;
            re.Expression = reStr + re.Expression;
            // if procedure, need to validate
            if (scriptGenerationParams.Color.Equals(PRE_CONDITION_COLOR))
            {
                re.Append(NEW_LINE +
                    getValidation(
                        getScriptAccessRanorexObject(scriptGenerationParams),
                        scriptGenerationParams.SpecNode,
                        getRealAttribute(scriptGenerationParams.SpecNode),
                        RemoveSingleQuote(expression),
                        scriptGenerationParams.ScreenName,
                        scriptGenerationParams.Id, 
                        scriptGenerationParams.SpecNode.ToString()));
            }
            return re;
        }

        public override ScriptsExpression GenRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string re = "";
            if (!Regex.IsMatch(expression, SINGLE_QUOTE))
            {
                return null;
            }
            string value = RemoveSingleQuote(expression);
            value = replaceReturnChar(value);
            string node_name = getScriptAccessRanorexElement(scriptGenerationParams.SpecNode, scriptGenerationParams.InstanceName);

            /**
             * modified by duongtd on 12/10/17, consider to revert if necessary
             */
            //        re += ".PressKeys(\"" + value + "\");";
            if (scriptGenerationParams.SpecNode.UIElement is ComboBoxElement)
                re += (node_name + ".SelectedItemText = \"" + value + "\";");
            /**
             * repo.MainForm.DataGridView11.DataGridView1.Rows[1].Cells[0]
             */
            else if (Regex.IsMatch(node_name, "(?s).*\\.Rows\\[\\d+\\].*"))
            {
                re += node_name + ".DoubleClick();" + NEW_LINE;
                re += node_name + ".PressKeys(\"" + value + "\");" + NEW_LINE;
                re += node_name + ".PressKeys(\"{RETURN}\");";
            }
            else
                re += (node_name + ".TextValue = \"" + value + "\";");
            return new ScriptsExpression(re);
        }
    }
}
