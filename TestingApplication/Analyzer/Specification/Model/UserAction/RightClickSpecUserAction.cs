// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:28 PM 2018/1/24
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class RightClickSpecUserAction : ClickSpecUserAction
    {
        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string re = GetScriptAccessElement(scriptGenerationParams.SpecNode.UIElement, scriptGenerationParams.InstanceName)
                 + ".RightClick();";
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
            Regex pattern1 = new Regex("RightClick\\s+'[\\w|\\s]+'\\s*(->\\s*'[\\w|\\s]+'\\s*)*");
            Match matcher1 = pattern1.Match(expression);

            Regex pattern4 = new Regex("RightClick(\\s*\\[(\\d+?);\\s*(\\d+?)\\]){0,1}");
            Match matcher4 = pattern4.Match(expression);
            string re = "";
            if (matcher1.Success)
            {
                string[] elements = Regex.Split(expression.Substring(RIGHT_CLICK.Length).Trim(), "->");
                /**
                 * modified by duongtd on 12/10/17, consider to revert if necessary
                 */
                for (int fi = 0; fi < elements.Length; fi++)
                {
                    string element = elements[fi];
                    element = element.Trim();
                    if (!Regex.IsMatch(element, SINGLE_QUOTE))
                    {
                        LogError(logger);
                        re = MISSING_STATEMENT;
                        break;
                    }
                    element = RemoveSingleQuote(element);
                    if (re != null && !re.Equals(""))
                        re += NEW_LINE;
                    IElement otherNode = Utils.SearchIElement(element, scriptGenerationParams.ListUIElements);
                    //if not found
                    if (otherNode == null)
                    {
                        LogError(logger, "Can not find element: " + element);
                        return null;
                    }
                    if (re != null && !re.Equals(""))
                        re += getElementExp(otherNode) + ".RightClick();";
                }
                //return re;
            }
            else if (matcher4.Success)
            {
                re += getElementExp1(scriptGenerationParams.SpecNode.UIElement,
                    getScriptAccessRanorexElement(scriptGenerationParams.SpecNode, scriptGenerationParams.InstanceName)) + ".";
                re += "RightClick(";
                if (matcher4.Groups[1] != null && !matcher4.Groups[1].Value.Trim().Equals(""))
                {
                    string arg1Str = matcher4.Groups[2].Value;
                    string arg2Str = matcher4.Groups[3].Value;
                    re += "\"" + Int32.Parse(arg1Str) + ";" + Int32.Parse(arg2Str) + "\"";
                }
                re += ");";
            }
            else
            {
                LogError(logger, "problem with expression: " + expression);
                return null;
            }
            return new ScriptsExpression(re);
        }
    }
}
