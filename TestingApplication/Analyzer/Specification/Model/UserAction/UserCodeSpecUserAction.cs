// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:30 PM 2018/1/24
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class UserCodeSpecUserAction : AbstractSpecUserAction
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            return HandleAndGenScripts(scriptGenerationParams);
        }

        public virtual ScriptsExpression HandleAndGenScripts(IScriptGenerationParams scriptGenerationParams, ScriptType scriptType = ScriptType.Normal)
        {
            if (scriptGenerationParams is UserCodeScriptGenerationParams ucParams)
            {
                Regex regex = new Regex("(?<class_group>.*?)\\.(?<func_group>.*)");
                Match match = regex.Match(expression);
                if (match.Success)
                {
                    string className = match.Groups["class_group"].Value;
                    string funcName = match.Groups["func_group"].Value;
                    Regex regex2 = new Regex("(?<func_group>.*)\\((?<params_group>.*)\\)$");
                    Match match2 = regex2.Match(funcName);
                    int paramsCount = 0;
                    string[] _params = null;
                    if (match2.Success && match2.Groups["params_group"] != null &&
                        match2.Groups["params_group"].Value.Trim() != "")
                    {
                        funcName = match2.Groups["func_group"].Value;
                        _params = match2.Groups["params_group"].Value.Split(',');
                        paramsCount = _params.Count();
                    }
                    bool voidReturn = true;
                    if (Regex.IsMatch(ucParams.SpecNode.Expression, USER_CODE_WITH_VARIABLE_DECLARE))
                        voidReturn = false;
                    else if (!Regex.IsMatch(ucParams.SpecNode.Expression, AbstractSpecUserAction.USER_CODE))
                        logger.Error("Incorrect Expression: " + ucParams.SpecNode.Expression);
                    var re = new UserCodeScriptsExpression();
                    var pair = CheckFunctionExisted(ucParams.ClassExpressions, className, funcName, paramsCount, voidReturn);
                    if (!pair.Item1)
                    {
                        FunctionExpression func = new FunctionExpression(funcName);
                        if (_params != null)
                            foreach (string param in _params)
                            {
                                ParameterExpression parameterExpression = new ParameterExpression();
                                parameterExpression.setName(param);
                                if (func.getParams() == null)
                                    func.setParams(new List<ParameterExpression>());
                                func.getParams().Add(parameterExpression);
                            }
                        func.setReturnDescription("no description");
                        ClassExpression classExpExisted = pair.Item2;
                        // if  class existed
                        if (classExpExisted != null)
                            Utils.MergeFunctions(classExpExisted.getListFunction(), func);
                        else
                            re.AppendNewAdditionFunc(className, func);
                    }
                    DoGenRawScripts(className, funcName, _params, voidReturn, ucParams, re, scriptType);
                    return re;
                }
                else
                    throw new NotImplementedException();
            }
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// notice to trim() _params
        /// </summary>
        /// <param name="className"></param>
        /// <param name="functionName"></param>
        /// <param name="_params">need to call Trim()</param>
        /// <param name="ucParams"></param>
        /// <returns></returns>
        public void DoGenRawScripts(string className, string functionName, string[] _params, bool voidReturn,
            UserCodeScriptGenerationParams ucParams, UserCodeScriptsExpression re, ScriptType scriptType = ScriptType.Normal)
        {
            string expressionRe = GetComment(ucParams) + NEW_LINE;
            if (!voidReturn)
            {
                re.GlobalScripts = "object " + ucParams.SpecNode.Expression + ";";
                expressionRe += ucParams.SpecNode.Expression + " = ";
            }
            expressionRe += className + "." + functionName + "(";
            if (_params != null)
            {
                string tempParamsExp = "";
                foreach (string param in _params)
                {
                    string param1 = param.Trim();
                    if (param1.Equals(""))
                        continue;
                    if (tempParamsExp != "")
                        tempParamsExp += ", ";
                    tempParamsExp += ParamExp2String(param1, ucParams.ListUIElements, ucParams.MapAliasWithNode, 
                        ucParams.MyLog, ucParams.InstanceName, scriptType);
                }
                expressionRe += tempParamsExp;
            }
            expressionRe += ");";
            re.Expression = expressionRe;
        }

        /// <summary>
        /// 'GetAttributeValue<object>("Text")' for Ranorex
        /// </summary>
        /// <param name="nodeExp"></param>
        /// <param name="listUIElements"></param>
        /// <param name="mappingAlias"></param>
        /// <param name="myLog"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        public static string Node2ParamExp(string nodeExp, ListUIElements listUIElements, Dictionary<string, string> mappingAlias, MyLog myLog, string instanceName, ScriptType scriptType)
        {
            Tuple<IElement, string> elementAndAttibute = NormalSheetParser.parseNodeAndAttribute(nodeExp, listUIElements, mappingAlias, myLog);
            if (elementAndAttibute == null || elementAndAttibute.Item1 == null)
            {
                myLog.Warn("cannot find node: " + nodeExp);
                return "null";
            }
            if (scriptType == ScriptType.Normal)
                return GetScriptAccessElement(elementAndAttibute.Item1, instanceName);
            //Ranorex scripts
            string re = getScriptAccessRanorexElement(
                new SpecNode(elementAndAttibute.Item1, elementAndAttibute.Item2, null), instanceName);
            if (elementAndAttibute.Item2 != null && !elementAndAttibute.Item2.Equals(""))
                re += ".GetAttributeValue<object>(\"" + elementAndAttibute.Item2 + "\")";
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="existedClasses"></param>
        /// <param name="className"></param>
        /// <param name="funcName"></param>
        /// <param name="paramsCount"></param>
        /// <param name="voidReturn"></param>
        /// <returns>{indicate function existed, classExpression with className}</returns>
        public Tuple<bool, ClassExpression> CheckFunctionExisted(List<ClassExpression> existedClasses, 
            string className, string funcName, int paramsCount, bool voidReturn)
        {
            if (existedClasses == null || existedClasses.Count == 0)
                return new Tuple<bool, ClassExpression>(false, null);
            ClassExpression classExpExisted = null;
            foreach (ClassExpression classExp in existedClasses)
            {
                if (classExp.getName().Equals(className)) {
                    classExpExisted = classExp;
                    List<FunctionExpression> listFuncs = classExp.getListFunction();
                    if (listFuncs != null)
                        foreach (FunctionExpression funcExp in listFuncs)
                        {
                            if (funcExp.getName().Equals(funcName) &&
                                voidReturn == funcExp.GetCorrectReturnType().Equals(FunctionExpression.VOID) &&
                                paramsCount == (funcExp.getParams() == null ? 0 : funcExp.getParams().Count))
                            return new Tuple<bool, ClassExpression>(true, classExp);
                        }
                }
            }
            return new Tuple<bool, ClassExpression>(false, classExpExisted); ;
        }

        public static string ParamExp2String(string param, ListUIElements listUIElements, 
            Dictionary<string, string> mapAliasWithNode, MyLog myLog, string instanceName, ScriptType scriptType)
        {
            string tempParamExp = "";
            // Variable_ControlCPUStatus
            if (Regex.IsMatch(param, USER_CODE_WITH_VARIABLE_DECLARE))
                tempParamExp += param;
            // Text Box One.Text
            else
            {
                tempParamExp += Node2ParamExp(param, listUIElements, mapAliasWithNode, myLog, instanceName, scriptType);
            }
            return tempParamExp;
        }

        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            throw new NotImplementedException();
        }

        public override ScriptsExpression GenRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            return HandleAndGenScripts(scriptGenerationParams, ScriptType.Ranorex);
        }

        public override ScriptsExpression GenRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            throw new NotImplementedException();
        }
    }

    public class UserCodeScriptGenerationParams : ScriptGenerationParams
    {
        public UserCodeScriptGenerationParams() : base() { }
        protected List<ClassExpression> classExpressions;
        //protected List<ClassExpression> classExpAddition;
        protected List<String> additionStatements;
        protected Dictionary<String, String> mapAliasWithNode = new Dictionary<string, string>();
        public UserCodeScriptGenerationParams(
            List<ClassExpression> classExpressions, 
            //List<ClassExpression> classExpAddition, 
            List<string> additionStatements,
            Dictionary<String, String> mapAliasWithNode)
        {
            this.classExpressions = classExpressions;
            //this.classExpAddition = classExpAddition;
            this.additionStatements = additionStatements;
            this.mapAliasWithNode = mapAliasWithNode;
        }

        public List<ClassExpression> ClassExpressions
        {
            get { return classExpressions; }
            set { classExpressions = value; }
        }
        //public List<ClassExpression> ClassExpAddition
        //{
        //    get { return classExpAddition; }
        //    set { classExpAddition = value; }
        //}
        public List<string> AdditionStatements
        {
            get { return additionStatements; }
            set { additionStatements = value; }
        }

        public Dictionary<String, String> MapAliasWithNode
        {
            get { return mapAliasWithNode; }
            set { mapAliasWithNode = value; }
        }

        public void CopyAttributesFrom(UserCodeScriptGenerationParams source)
        {
            base.CopyAttributesFrom(source);
            this.classExpressions = source.classExpressions;
            this.additionStatements = source.additionStatements;
            this.mapAliasWithNode = source.mapAliasWithNode;
        }
    }

    public class UserCodeScriptsExpression : ScriptsExpression
    {
        private Dictionary<string, List<FunctionExpression>> mapClassAndFuncsAddition = new Dictionary<string, List<FunctionExpression>>();

        public Dictionary<string, List<FunctionExpression>> MapClassAndFuncsAddition
        {
            get { return mapClassAndFuncsAddition; }
            set { mapClassAndFuncsAddition = value; }
        }

        public void AppendNewAdditionFunc(string className, FunctionExpression func)
        {
            if (mapClassAndFuncsAddition == null)
                mapClassAndFuncsAddition = new Dictionary<string, List<FunctionExpression>>();
            if (mapClassAndFuncsAddition.ContainsKey(className))
                Utils.MergeFunctions(mapClassAndFuncsAddition[className], func);
            else
                mapClassAndFuncsAddition.Add(className, new List<FunctionExpression> { func });
        }
        public string GlobalScripts
        {
            get { return globalScripts; }
            set { globalScripts = value; }
        }
        private string globalScripts;

        public UserCodeScriptsExpression(string expression) :base(expression)
        {
        }
        public UserCodeScriptsExpression() { }
        public UserCodeScriptsExpression(string expression, Dictionary<string, List<FunctionExpression>> mapClassAndFuncsAddition) : 
            base(expression)
        {
            this.mapClassAndFuncsAddition = mapClassAndFuncsAddition;
        }
    }
}
