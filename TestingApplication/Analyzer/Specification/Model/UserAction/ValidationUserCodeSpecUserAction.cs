// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:30 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class ValidationUserCodeSpecUserAction : UserCodeSpecUserAction
    {
        private List<string> listExps;
        public List<string> ListExps
        {
            get { return listExps; }
            set { listExps = value; }
        }

        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            return HandleAndGenScripts(scriptGenerationParams);
        }

        public override ScriptsExpression HandleAndGenScripts(IScriptGenerationParams scriptGenerationParams, ScriptType scriptType = ScriptType.Normal)
        {
            string re = GetComment(scriptGenerationParams) + NEW_LINE;
            if (scriptGenerationParams is ValidationUCScriptGenerationParams param)
            {
                string newFunctionContent = "";
                if (param.SpecNode.Attribute == null ||
                    param.SpecNode.Attribute == "")
                    param.SpecNode.Attribute = TEXT;
                foreach (string exp in param.ListExps)
                {
                    if (newFunctionContent != "")
                        newFunctionContent += NEW_LINE;
                    if (scriptType == ScriptType.Normal)
                        newFunctionContent += ValidationSpecUserAction.GetRawScripts(param, exp).Expression;
                    else if (scriptType == ScriptType.Ranorex)
                        newFunctionContent += ValidationSpecUserAction.GetRawRanorexScripts(param as IRanorexScriptGenerationParams, exp).Expression;
                }
                string newClassName = param.ClassName;
                string newFuncName = param.FunctionName;
                re += newClassName + "." + newFuncName + "();";
                var pair = CheckFunctionExisted(param.ClassExpressions, newClassName, newFuncName, 0, true);
                // if not existed
                if (!pair.Item1)
                {

                    FunctionExpression func = new FunctionExpression(newFuncName);
                    func.setContent(newFunctionContent);
                    UserCodeScriptsExpression re1 = new UserCodeScriptsExpression(re);
                    re1.MapClassAndFuncsAddition = new Dictionary<string, List<FunctionExpression>>
                    {
                        { newClassName, new List<FunctionExpression>() { func} }
                    };
                    return re1;
                }
                return new ScriptsExpression(re);
            }
            else
            {
                re += ValidationSpecUserAction.GetRawScripts(scriptGenerationParams, this.Expression).Expression;
                return new ScriptsExpression(re);
            }
        }

        public override IUserAction Clone()
        {
            var re = base.Clone() as ValidationUserCodeSpecUserAction;
            re.ListExps = this.listExps;
            return re;
        }
    }

    public class ValidationUCScriptGenerationParams : UserCodeScriptGenerationParams
    {
        public ValidationUCScriptGenerationParams() : base() { }
        protected List<string> listExps;
        protected string className, functionName;

        public ValidationUCScriptGenerationParams(
            List<ClassExpression> classExpressions, 
            //List<ClassExpression> classExpAddition, 
            List<string> additionStatements,
            Dictionary<string, string> mapAliasWithNode) :
            base(classExpressions, additionStatements, mapAliasWithNode)
        {
        }

        public ValidationUCScriptGenerationParams(
            List<ClassExpression> classExpressions, 
            //List<ClassExpression> classExpAddition, 
            List<string> additionStatements, 
            Dictionary<string, string> mapAliasWithNode,
            List<string> listExps, string className, string functionName) :
            base(classExpressions, additionStatements, mapAliasWithNode)
        {
            this.listExps = listExps;
            this.className = className;
            this.functionName = functionName;
        }

        public List<string> ListExps
        {
            get { return listExps; }
            set { listExps = value; }
        }
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        public string FunctionName
        {
            get { return functionName; }
            set { functionName = value; }
        }

        public void CopyAttributesFrom(ValidationUCScriptGenerationParams source)
        {
            base.CopyAttributesFrom(source);
            this.listExps = source.listExps;
            this.className = source.className;
            this.functionName = source.functionName;
        }
    }
}
