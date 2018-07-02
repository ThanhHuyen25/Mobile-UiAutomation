// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:29 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI_Testing_Automation;
using GUI_Testing_Automation.Ranorex;

namespace TestingApplication
{
    public class SetSpecUserAction : AbstractSpecUserAction
    {
        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string reStr = GetComment(scriptGenerationParams) + NEW_LINE;
            var re = GenRawScripts(scriptGenerationParams);
            if (re == null)
                return null;
            re.Expression = reStr + re.Expression;

            string scriptAccessEle = GetScriptAccessElement(scriptGenerationParams.SpecNode.UIElement, 
                                                            scriptGenerationParams.InstanceName);
            string tempStr = scriptAccessEle;
            string attributeValue = this.expression;

            // combobox select item
            if (scriptGenerationParams.SpecNode.UIElement is ComboBoxElement &&
                int.TryParse(attributeValue.Trim(), out int n))
            {
            }
            else if(scriptGenerationParams.Color.Equals(PRE_CONDITION_COLOR))
            {
                string attribute = scriptGenerationParams.SpecNode.Attribute.Trim();
                if (attribute == null || attribute == "" || attribute.Equals("text", StringComparison.OrdinalIgnoreCase))
                {
                    tempStr += NEW_LINE + "Validate.TextEquals(" + scriptAccessEle + ", \"" + attributeValue + "\");";
                }
                else if (attribute.Equals("width", StringComparison.OrdinalIgnoreCase))
                {
                    tempStr += NEW_LINE + "Validate.WidthEquals(" + scriptAccessEle + ", " + attributeValue + ");";
                }
                else if (attribute.Equals("height", StringComparison.OrdinalIgnoreCase))
                {
                    tempStr += NEW_LINE + "Validate.HeightEquals(" + scriptAccessEle + ", " + attributeValue + ");";
                }
                re.Append(tempStr);
            }
            return re;
        }

        public override ScriptsExpression GenRawScripts(IScriptGenerationParams _params)
        {
            return new ScriptsExpression(GenRawScripts(_params, this.expression));
        }

        public string GenRawScripts(IScriptGenerationParams _params, string attributeValue)
        {
            string scriptAccessEle = GetScriptAccessElement(_params.SpecNode.UIElement, _params.InstanceName);
            string re = scriptAccessEle;
            // combobox select item
            if (_params.SpecNode.UIElement is ComboBoxElement &&
                int.TryParse(attributeValue.Trim(), out int n))
            {
                re += ".ChooseOption(" + attributeValue.Trim() + ");";
            }
            else
            {
                string attribute = _params.SpecNode.Attribute.Trim();
                if (attribute == null || attribute == "" || attribute.Equals("text", StringComparison.OrdinalIgnoreCase))
                {
                    re += ".InputString(\"" + attributeValue + "\");";
                }
                else if (attribute.Equals("width", StringComparison.OrdinalIgnoreCase))
                {
                    re += ".SetWidth(" + attributeValue + ");";
                }
                else if (attribute.Equals("height", StringComparison.OrdinalIgnoreCase))
                {
                    re += ".SetHeight(" + attributeValue + ");";
                }
            }
            return re;
        }

        public override ScriptsExpression GenRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            SetScriptExpression reAndValue = GenRawRanorexScripts(scriptGenerationParams) as SetScriptExpression;
            if (reAndValue == null)
            {
                LogError(logger, expression);
                return null;
            }
            string re = reAndValue.Expression;
            if (re == null)
            {
                LogError(logger, expression);
                return null;
            }
            string value = reAndValue.Value;
            if (re == null || value == null)
            {
                LogError(logger, expression);
                return null;
            }

            string reStr = GetComment(scriptGenerationParams) + NEW_LINE;
            reAndValue.Expression = reStr + reAndValue.Expression;
            // if procedure, need to validate
            if (scriptGenerationParams.Color.Equals(PRE_CONDITION_COLOR))
            {
                reAndValue.Append(NEW_LINE +
                    getValidation(
                        getScriptAccessRanorexObject(scriptGenerationParams),
                        scriptGenerationParams.SpecNode,
                        getRealAttribute(scriptGenerationParams.SpecNode),
                        RemoveSingleQuote(expression),
                        scriptGenerationParams.ScreenName,
                        scriptGenerationParams.Id,
                        scriptGenerationParams.SpecNode.ToString()));
            }
            return reAndValue;
        }

        public override ScriptsExpression GenRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string value = null;
            string re = getScriptAccessRanorexObject(scriptGenerationParams);
            re += ".SetAttributeValue(\"" + getRealAttribute(scriptGenerationParams.SpecNode) + "\", \"";
            if (checkValidValueExpress(expression))
            {
                value = formatValue(expression);
                value = replaceReturnChar(value);
            }
            //rest
            else
                return null;
            re += value + "\");";
            return new SetScriptExpression(re, value);
        }

        public string getScriptAccessRanorexObject(RanorexScriptGenerationParams param)
        {
            SpecNode nodeAndAttribute = param.SpecNode;
            IElement node = nodeAndAttribute.UIElement;
            String re = getScriptAccessRanorexElement(nodeAndAttribute, param.InstanceName);

            if (node is AppFolderRanorexElement ||
                    node is FolderRanorexElement) {
                re += ".Self";
            }
            return re + ".Element";
        }
    }

    public class SetScriptExpression : ScriptsExpression
    {
        string value;

        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
            }
        }

        public SetScriptExpression(string expression, string value) : base(expression)
        {
            this.value = value;
        }
    }
}
