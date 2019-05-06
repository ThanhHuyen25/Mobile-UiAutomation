// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:48 AM 2018/1/31
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.Formatting;
//using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class CSharpScriptsGeneration
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region const
        public const string INSTANCE_NAME = "elements";
        public const string NEW_LINE = AbstractSpecUserAction.NEW_LINE;
        public const string TAB = AbstractSpecUserAction.TAB;
        public const string NAMESPACE_REPLACE = "ns_replace";
        public const string CLASS_REPLACE = "class_replace";
        public const string CONTENT_REPLACE = "content_replace";
        public const string GLOBAL_REPLACE = "#my_point_global_replace";

        public const string ABSTRACT_TEST_MODULE = "AbstractTestModule";
        public const string ABSTRACT_TEST_RUN = "AbstractTestRun";

        public const string FILE_TEMPLATE =
            "using GUI_Testing_Automation;" + NEW_LINE +
            "using System.Threading;" + NEW_LINE +
            "namespace " + NAMESPACE_REPLACE + NEW_LINE +
            "{" + NEW_LINE +
            "}";
        public const string CLASS_TEMPLATE =
            "public class " + CLASS_REPLACE + NEW_LINE +
            "{" + NEW_LINE +
                GLOBAL_REPLACE +
                "public override void DoRun()" + NEW_LINE +
                "{" + NEW_LINE +
                //"Handler.InitNewModule(this.GetType().Name);" + NEW_LINE +
                CONTENT_REPLACE + NEW_LINE +
                "}" + NEW_LINE +
            "}" + NEW_LINE;
        public const string CLASS_TEMPLATE1 =
            "public class " + CLASS_REPLACE + NEW_LINE +
            "{" + NEW_LINE +
            CONTENT_REPLACE + NEW_LINE +
            "}" + NEW_LINE;
        #endregion const

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenList"></param>
        /// <param name="folderPath">="D:\MyTestProject"</param>
        /// <param name="repoFilePath">="D:\ElementsRepo.xml"</param>
        /// <param name="imageCapFilePath">="D:\ImageCapture.xml"</param>
        /// <param name="appFilePath">="D:\app.exe"</param>
        /// <param name="classRepoName">="MyTestProjectDefinition"</param>
        /// <param name="name_space">="MyTestProject"</param>
        /// <param name="mainClassInstance">="Instance"</param>
        /// <param name="myLog"></param>
        /// <returns>map (folder_path, list_files_in_folder)</returns>
        public Tuple<Dictionary<string, string>, List<string>> Generate(List<IScreen> screenList, string folderPath,
            string repoFilePath, string imageCapFilePath, string appFilePath, string classRepoName,
            string name_space, string mainClassInstance, MyLog myLog, string instanceName = INSTANCE_NAME)
        {
            //List<string> runningFilePath = new List<string>();
            Dictionary<string, string> mapFilePathAndId = new Dictionary<string, string>();
            List<UserCodeScriptsExpression> additionScripts = new List<UserCodeScriptsExpression>();
            List<ClassExpression> additionClasses = new List<ClassExpression>();
            SpecScreen aScreen = null;
            foreach (IScreen screen in screenList)
            {
                if (screen is SpecScreen specScreen)
                {
                    if (aScreen == null)
                        aScreen = specScreen;
                    ProcessScreen(specScreen, mapFilePathAndId, additionScripts, additionClasses,
                        folderPath, repoFilePath, imageCapFilePath, appFilePath, classRepoName, name_space,
                        mainClassInstance, myLog, instanceName);
                }
                else
                {
                    logger.Error("Not handled yet!");
                }
            }
            List<string> otherFilePath = GenerateAdditionClassses(additionScripts, additionClasses, aScreen, myLog, folderPath, repoFilePath,
                imageCapFilePath, appFilePath, classRepoName, name_space, mainClassInstance, instanceName, mapFilePathAndId);
            return new Tuple<Dictionary<string, string>, List<string>>(mapFilePathAndId, otherFilePath);
        }
        public Tuple<Dictionary<string, string>> GenerateAndroid(List<SpecScreen> screenList, string projFolderPath,
            string appFilePath, string projectName, string ClassInstance, MyLog myLog, string instanceName = INSTANCE_NAME)
        {
            Dictionary<string, string> mapFilePathAndId = new Dictionary<string, string>();
            List<UserCodeScriptsExpression> additionScripts = new List<UserCodeScriptsExpression>();
            List<ClassExpression> additionClasses = new List<ClassExpression>();
            SpecScreen aScreen = null;

            return new Tuple<Dictionary<string, string>>(mapFilePathAndId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenList"></param>
        /// <param name="projFolderPath"></param>
        /// <param name="repoFilePath"></param>
        /// <param name="imageCapFilePath"></param>
        /// <param name="appFilePath"></param>
        /// <param name="classRepoName"></param>
        /// <param name="name_space"></param>
        /// <param name="mainClassInstance">="Instance"</param>
        /// <param name="myLog"></param>
        /// <returns>{running test file path, other source file path} - relative path</returns>
        public Tuple<Dictionary<string, string>, List<string>> Generate(List<SpecScreen> screenList, string projFolderPath,
            string repoFilePath, string imageCapFilePath, string appFilePath, string classRepoName,
            string name_space, string mainClassInstance, MyLog myLog, string instanceName = INSTANCE_NAME)
        {
            //List<string> runningFilePath = new List<string>();
            Dictionary<string, string> mapFilePathAndId = new Dictionary<string, string>();
            List<UserCodeScriptsExpression> additionScripts = new List<UserCodeScriptsExpression>();
            List<ClassExpression> additionClasses = new List<ClassExpression>();
            SpecScreen aScreen = null;
            foreach (SpecScreen specScreen in screenList)
            {
                if (aScreen == null)
                    aScreen = specScreen;
                ProcessScreen(specScreen, mapFilePathAndId, additionScripts, additionClasses,
                        projFolderPath, repoFilePath, imageCapFilePath, appFilePath, classRepoName, name_space,
                        mainClassInstance, myLog, instanceName);
            }

            List<string> otherFilePath = GenerateAdditionClassses(additionScripts, additionClasses, aScreen, myLog, projFolderPath, repoFilePath,
                imageCapFilePath, appFilePath, classRepoName, name_space, mainClassInstance, instanceName, mapFilePathAndId);
            return new Tuple<Dictionary<string, string>, List<string>>(mapFilePathAndId, otherFilePath);
        }
        // Android script code
        public string GenerateScriptAndroid(List<SpecScreen> specScreens, string folderOutPath, string nameFile)
        {
            string result = "";
            List<string> listNameClass = new List<string>();
            File.Move(folderOutPath + nameFile + ".java", folderOutPath + specScreens[0].Name + ".java");
            nameFile = specScreens[0].Name;
            listNameClass.Add(nameFile);
            for (int j = 0; j < specScreens.Count; j++)
            {
                if (j < (specScreens.Count - 1))
                {
                    string newNameFile = specScreens[j + 1].Name;
                    File.Copy(folderOutPath + nameFile + ".java", folderOutPath + newNameFile + ".java", true);
                    listNameClass.Add(newNameFile);
                }
                using (StreamWriter sw = new StreamWriter(folderOutPath + listNameClass[j] + ".java", true, Encoding.UTF8))
                {
                    result = TAB + "public class " + listNameClass[j] + " implements Runnable" + " {" + NEW_LINE;
                    result += TAB + "String sheetName = \"" + listNameClass[j] +"\";" + NEW_LINE;
                    result += TAB + "MyTestProjectDefinition element = new MyTestProjectDefinition();" + NEW_LINE;

                    // element
                    List<SpecNode> listSpecNodes = specScreens[j].ListSpecNodes;
                    string methodRun = "";
                    for (int k = 0; k < specScreens[j].Scenarios.Count; k++)
                    {
                        result += TAB + "@SuppressLint(\"Assert\")" + NEW_LINE;
                        result += TAB + "@Test" + NEW_LINE;
                        result += TAB + "public void testCal" + (k+1) + "() throws Exception {" + NEW_LINE;
                        result += TAB + TAB + "element.connectDevice();" + NEW_LINE;
                        SpecScenario scenario = specScreens[j].Scenarios[k] as SpecScenario;
                        
                        List<string> listActionExp = scenario.UserActionsInString;
                        List<Color> listColor = scenario.Colors;
                        for (int i = 0; i < listSpecNodes.Count; i++)
                        {
                            result += GenScriptCode(listColor[i], listSpecNodes[i], listActionExp[i]);
                        }
                        result += NEW_LINE;
                        result += TAB + TAB + "element.closeDevice();" + NEW_LINE + TAB + "}" + NEW_LINE;
                        methodRun += TAB + TAB + TAB + "testCal" + (k + 1) + "();" + NEW_LINE;
                    }
                    // run method
                    result += TAB + "@Override" + NEW_LINE + TAB + "public void run() {" + NEW_LINE;
                    result += TAB + TAB + "try {" + NEW_LINE;
                    result += TAB + TAB + TAB + "GeneratingTestingReport generatingTestingReport = new GeneratingTestingReport();" + NEW_LINE;
                    result += TAB + TAB + TAB + "generatingTestingReport.scenarioInfoEmpty();" + NEW_LINE;
                    result += methodRun;
                    result += TAB + TAB + TAB + "generatingTestingReport.writeReport(sheetName, element.excelReportPath);" + NEW_LINE;
                    result += TAB + TAB + "} catch (Exception e) {" + NEW_LINE + TAB + TAB + TAB + "e.printStackTrace();" + NEW_LINE;
                    result += TAB + TAB + "}";
                    result += NEW_LINE + TAB + "}" + NEW_LINE + "}";
                    sw.WriteLine(result);

                }


            }
            GenMainClassCode(folderOutPath, listNameClass);
            return result;
        }
        // gen main class
        public void GenMainClassCode(string folderOutPath, List<string> listNameClass)
        {
            folderOutPath = folderOutPath + "Main.java";
            string result = "";
            using (StreamWriter sw = new StreamWriter(folderOutPath, true, Encoding.UTF8))
            {
                foreach (string name in listNameClass)
                {
                    result += TAB + TAB + "new " + name + "().run();" + NEW_LINE;
                }
                result += TAB + TAB + "generatingTestingReport.openFile(excelReportPath);" + NEW_LINE;
                result += TAB + "}" + NEW_LINE + "}";
                sw.WriteLine(result);
            }
        }
        public string GenScriptCode(Color color, SpecNode specNode, string actionExp)
        {
            string result="";
            string TAB3 = TAB + TAB;
            if (!actionExp.Equals("N/A"))
            {
                string elementName = specNode.UIElement.Attributes.Name.Replace(" ", "_");
                if (color.Equals(AbstractSpecUserAction.PROCEDURES_COLOR))
                {
                    result += TAB3 + "//[Proccedure] " + elementName + ": " + actionExp + NEW_LINE;
                    result += TAB3 + "element." + elementName + "." + actionExp + "();" + NEW_LINE;
                }
                else if (color.Equals(AbstractSpecUserAction.PRE_CONDITION_COLOR))
                {
                    result += TAB3 + "//[Pre-condition] " + specNode.UIElement.Attributes.Name + ": " + actionExp + NEW_LINE;
                    result += TAB3 + "element." + elementName + ".EditText(" + actionExp.Replace('\'', '\"') + ");" + NEW_LINE;
                }
                else if (color.Equals(AbstractSpecUserAction.ENVIRONMENT_COLOR))
                {
                    result += TAB3 + "//[Environment] " + elementName + ": " + actionExp + NEW_LINE;
                    result += TAB3 + "element." + elementName + "." + actionExp + "();" + NEW_LINE;
                }
                else if (color.Equals(AbstractSpecUserAction.VALIDATION_COLOR))
                {
                    result += TAB3 + "//[Validation] " + elementName + ": " + actionExp + NEW_LINE;
                    if (!actionExp.Contains("\'"))
                    {
                        result += TAB3 + "Validate." + actionExp + "(element." + elementName + ");" + NEW_LINE;
                    }
                    else
                    {
                        result += TAB3 + "Validate.textEquals(element." + elementName + ", " + actionExp.Replace("\'", "\"") + ");" + NEW_LINE;
                    }

                }
            }
            
            return result;
        }

        /// <summary>
        /// generate user code, main file, etc
        /// </summary>
        /// <param name="additionScripts"></param>
        /// <param name="additionClasses"></param>
        /// <param name="aScreen"></param>
        /// <param name="myLog"></param>
        /// <param name="folderPath"></param>
        /// <param name="repoFilePath"></param>
        /// <param name="imageCapFilePath"></param>
        /// <param name="appFilePath"></param>
        /// <param name="classRepoName"></param>
        /// <param name="name_space"></param>
        /// <param name="mainClassInstance"></param>
        /// <param name="instanceName"></param>
        /// <param name="runningFilePath"></param>
        /// <returns></returns>
        protected List<string> GenerateAdditionClassses(List<UserCodeScriptsExpression> additionScripts, List<ClassExpression> additionClasses,
            SpecScreen aScreen, MyLog myLog, string folderPath, string repoFilePath, string imageCapFilePath, string appFilePath,
            string classRepoName, string name_space, string mainClassInstance, string instanceName, Dictionary<string, string> mapFilePathAndId)
        {
            List<string> otherFilePath = new List<string>();
            otherFilePath.AddRange(GenerateAdditionScripts(additionScripts, folderPath, name_space, aScreen.AllUIElements,
                aScreen.MappingAliasWithNode, myLog, instanceName));
            otherFilePath.AddRange(GenerateClassExpScripts(additionClasses, folderPath, name_space, aScreen.AllUIElements,
                aScreen.MappingAliasWithNode, myLog, instanceName));
            GenerateOthers(myLog, folderPath, repoFilePath, imageCapFilePath, appFilePath, classRepoName, name_space, 
                mainClassInstance, instanceName, mapFilePathAndId, otherFilePath);
            return otherFilePath;
        }

        /// <summary>
        /// put generating special scripts here
        /// </summary>
        /// <param name="myLog"></param>
        /// <param name="folderPath"></param>
        /// <param name="repoFilePath"></param>
        /// <param name="imageCapFilePath"></param>
        /// <param name="appFilePath"></param>
        /// <param name="classRepoName"></param>
        /// <param name="name_space"></param>
        /// <param name="mainClassInstance"></param>
        /// <param name="instanceName"></param>
        /// <param name="runningFilePath"></param>
        /// <param name="otherFilePath"></param>
        protected virtual void GenerateOthers(MyLog myLog, string folderPath, string repoFilePath, string imageCapFilePath, 
            string appFilePath, string classRepoName, string name_space, string mainClassInstance, string instanceName,
            Dictionary<string, string> mapFilePathAndId, List<string> otherFilePath)
        {
            otherFilePath.Add(GenerateMainFile(mapFilePathAndId, folderPath, repoFilePath, imageCapFilePath, appFilePath,
                classRepoName, name_space, mainClassInstance, myLog, instanceName));
        }

        /// <summary>
        /// process for a screen
        /// </summary>
        /// <param name="specScreen"></param>
        /// <param name="aScreen"></param>
        /// <param name="runningFilePath"></param>
        /// <param name="additionScrips"></param>
        /// <param name="additionClasses"></param>
        /// <param name="folderPath"></param>
        /// <param name="repoFilePath"></param>
        /// <param name="imageCapFilePath"></param>
        /// <param name="appFilePath"></param>
        /// <param name="classRepoName"></param>
        /// <param name="name_space"></param>
        /// <param name="mainClassInstance"></param>
        /// <param name="myLog"></param>
        /// <param name="instanceName"></param>
        public void ProcessScreen(SpecScreen specScreen, Dictionary<string, string> mapFilePathAndId,
            List<UserCodeScriptsExpression> additionScrips, List<ClassExpression> additionClasses,
            string folderPath, string repoFilePath, string imageCapFilePath, string appFilePath, string classRepoName,
            string name_space, string mainClassInstance, MyLog myLog, string instanceName = INSTANCE_NAME)
        {
            //Dictionary<string, string> mapFilePathAndId = new Dictionary<string, string>();
            //List<string> listFilePath = new List<string>();
            foreach (IScenario scenario in specScreen.Scenarios)
            {
                if (scenario is SpecScenario specScenario)
                {
                    HandleWaitActions(specScenario);
                    Tuple<string, List<UserCodeScriptsExpression>> pair = GenerateAScenario(specScenario, myLog, instanceName);
                    if (pair == null)
                        continue;
                    var fileAndGuid = CreateNewRunCsFile(pair.Item1, pair.Item2,
                        folderPath, specScreen.Name, repoFilePath, imageCapFilePath,
                        appFilePath, classRepoName, name_space, specScenario.Id, mainClassInstance, instanceName);
                    string filePath = fileAndGuid.Item1;
                    string guid = fileAndGuid.Item2;
                    mapFilePathAndId.Add(filePath, guid);
                    if (pair.Item2 != null && pair.Item2.Count > 0)
                    {
                        Utils.MergeUcScriptsEpx(additionScrips, pair.Item2);
                        additionScrips.AddRange(pair.Item2);
                    }
                }
                else
                {
                    logger.Error("Not handled yet");
                }
            }
            //runningFilePath.AddRange(listFilePath);
            if (specScreen is TestSpecificationScreen testSpecScreen)
            {
                Utils.MergeClassesExpression(additionClasses, testSpecScreen.ClassExpressions);
            }
        }
        
        /// <summary>
        /// handle Wait user action
        /// </summary>
        /// <param name="specScenario"></param>
        private void HandleWaitActions(SpecScenario specScenario)
        {
            int count = 0;
            for (int fi = 0; fi < specScenario.UserActions.Count; fi++)
            {
                var userAction = specScenario.UserActions[fi];
                if (userAction is PreWaitValidateSpecUserAction preWaitUserAction)
                {
                    var param = preWaitUserAction.Params as WaitValidateScriptGenerationParams;
                    param.ListUserActions = new List<AbstractSpecUserAction>();
                    int se = fi + 1;
                    while (se < specScenario.UserActions.Count &&
                        !(specScenario.UserActions[se] is WaitValidateSpecUserAction) &&
                        specScenario.UserActions[se] is AbstractSpecUserAction &&
                        (specScenario.UserActions[se] as AbstractSpecUserAction).Params.Color.Equals(
                            AbstractSpecUserAction.PRE_CONDITION_COLOR))
                    {
                        param.ListUserActions.Add(specScenario.UserActions[se] as AbstractSpecUserAction);
                        (specScenario.UserActions[se] as AbstractSpecUserAction).IgnoreGenerateScripts = true;
                        se++;
                    }
                    param.WaitIndex = count;
                    count++;
                }
                else if (userAction is PostWaitValidateSpecUserAction postWaitUserAction)
                {
                    var param = postWaitUserAction.Params as WaitValidateScriptGenerationParams;
                    param.ListUserActions = new List<AbstractSpecUserAction>();
                    int se = fi + 1;
                    while (se < specScenario.UserActions.Count &&
                        !(specScenario.UserActions[se] is WaitValidateSpecUserAction) &&
                        specScenario.UserActions[se] is AbstractSpecUserAction &&
                        (specScenario.UserActions[se] as AbstractSpecUserAction).Params.Color.Equals(
                            AbstractSpecUserAction.VALIDATION_COLOR))
                    {
                        param.ListUserActions.Add(specScenario.UserActions[se] as AbstractSpecUserAction);
                        (specScenario.UserActions[se] as AbstractSpecUserAction).IgnoreGenerateScripts = true;
                        se++;
                    }
                    param.WaitIndex = count;
                    count++;
                }
            }
        }

        /// <summary>
        /// return {function content, List[UserCodeScriptsExpression]} 
        /// </summary>
        /// <param name="specScenario"></param>
        /// <param name="instanceName"></param>
        /// <param name="countLineBreak">determine new_line with blank line or only break new line</param>
        /// <param name="ignoreScenarioIfError">indicate whether ignore current scenario if an error occur</param>
        /// <returns></returns>
        protected Tuple<string, List<UserCodeScriptsExpression>> GenerateAScenario(SpecScenario specScenario, 
            MyLog myLog,
            string instanceName = INSTANCE_NAME,
            int countLineBreak = 1,
            bool ignoreScenarioIfError = true)
        {
            string newLine = string.Concat(Enumerable.Repeat(NEW_LINE, countLineBreak + 1));
            string funcContent = "";
            List<UserCodeScriptsExpression> listUcScriptExp = new List<UserCodeScriptsExpression>();
            List<IScenario> preConditions = specScenario.PreConditions;
            if (preConditions != null)
            {
                foreach (IScenario preCondition in preConditions)
                {
                    if (preCondition is SpecScenario preConditionSpec)
                    {
                        if (!funcContent.Equals(""))
                            funcContent += newLine;
                        Tuple<string, List<UserCodeScriptsExpression>> pair = GenerateAScenario(
                            preConditionSpec, myLog, instanceName, countLineBreak, ignoreScenarioIfError);
                        funcContent += pair.Item1;
                        var temp = pair.Item2;
                        if (temp != null && temp.Count > 0)
                        {
                            Utils.MergeUcScriptsEpx(listUcScriptExp, temp);
                            listUcScriptExp.AddRange(temp);
                        }
                    }
                    else
                        logger.Error("Not handled yet!");
                }
            }
            foreach (IUserAction userAction in specScenario.UserActions)
            {
                if (userAction is AbstractSpecUserAction specUserAction)
                {
                    if (specUserAction.IgnoreGenerateScripts)
                        continue;
                    if (!funcContent.Equals(""))
                        funcContent += newLine;
                    try
                    {
                        ScriptsExpression scriptsExpression = GenScriptType(specUserAction, instanceName);
                        // if error
                        if (scriptsExpression == null)
                        {
                            LogError(specUserAction.Expression, specUserAction.Params.ScreenName, 
                                specUserAction.Params.Id, myLog, specUserAction.NodeAffect.ToString());
                            if (ignoreScenarioIfError)
                                return null; // ignore current scenario, process with the next one
                            // in contrast, continue with the remains UserAction-s
                            continue;
                        }
                        funcContent += scriptsExpression.Expression;
                        if (scriptsExpression is UserCodeScriptsExpression ucScriptsExp)
                        {
                            // remove duplicate function
                            Utils.MergeUcScriptsEpx(listUcScriptExp, ucScriptsExp);
                            listUcScriptExp.Add(ucScriptsExp);
                        }
                    }
                    catch (NotImplementedException)
                    {
                        logger.Error("Not implement with " + userAction.GetType().Name);
                    }
                }
                else
                    logger.Error("Not handled yet!");
            }
            return new Tuple<string, List<UserCodeScriptsExpression>>(funcContent, listUcScriptExp);
        }

        /// <summary>
        /// create Feature_1.cs file
        /// </summary>
        /// <param name="funcContent"></param>
        /// <param name="projectPath"></param>
        /// <param name="screenName"></param>
        /// <param name="repoFilePath"></param>
        /// <param name="imageCapFilePath"></param>
        /// <param name="appFilePath"></param>
        /// <param name="classRepoName"></param>
        /// <param name="name_space"></param>
        /// <param name="id"></param>
        /// <returns>relative file path</returns>
        protected Tuple<string, string> CreateNewRunCsFile(string funcContent, List<UserCodeScriptsExpression> listUcScriptsExp,
            string projectPath, string screenName, string repoFilePath, string imageCapFilePath, 
            string appFilePath, string classRepoName, string name_space, int id, string mainClassInstance, string instanceName = INSTANCE_NAME)
        {
            string className = screenName + "_" + id;
            Directory.CreateDirectory(Path.Combine(projectPath, screenName));
            //funcContent = classRepoName + " " + INSTANCE_NAME + " = new " + classRepoName + "(@\"" +
                //repoFilePath + "\", @\"" + imageCapFilePath + "\");" + NEW_LINE +
                //"Handler.OpenApp(@\"" + appFilePath + "\");" + NEW_LINE +
                //funcContent;
            string fileContentTemplate = GetTemplateRunCsFileContent()
                .Replace(NAMESPACE_REPLACE, name_space);
            string classContent = GetTemplateRunCsClassContent();
            string guid = ModifyOtherRunCsFileContent(ref classContent, className);
            classContent = classContent.Replace(CONTENT_REPLACE, funcContent);

            string globalStatementsAndFunc = GetGlobalStatementsAndFunc(listUcScriptsExp, classRepoName, instanceName, name_space, mainClassInstance);
            if (globalStatementsAndFunc != null && globalStatementsAndFunc != "")
                globalStatementsAndFunc += NEW_LINE;
            classContent = classContent.Replace(GLOBAL_REPLACE, globalStatementsAndFunc);

            string filePath = Path.Combine(screenName, className + ".cs");
            CreateNewFile(fileContentTemplate, classContent, Path.Combine(projectPath, filePath));
            return new Tuple<string, string> (filePath, guid);
        }

        #region override-able functions
        /// <summary>
        /// determine generate normal scripts or Ranorex scripts
        /// </summary>
        /// <param name="specUserAction"></param>
        /// <returns></returns>
        protected virtual ScriptsExpression GenScriptType(AbstractSpecUserAction specUserAction, string instanceName = INSTANCE_NAME)
        {
            specUserAction.Params.InstanceName = instanceName;
            return specUserAction.GenScripts(specUserAction.Params);
        }

        protected virtual string GetTemplateRunCsFileContent()
        {
            return FILE_TEMPLATE;
        }

        protected virtual string GetTemplateRunCsClassContent()
        {
            return CLASS_REPLACE;
        }

        /// <summary>
        /// if need to modify others
        /// </summary>
        /// <param name="content"></param>
        /// <returns>[0]: content modified</returns>
        protected virtual string ModifyOtherRunCsFileContent(ref string content, string className)
        {
            content = content.Replace(CLASS_REPLACE, className + " : " + ABSTRACT_TEST_MODULE);
            return "";
        }

        protected virtual string GetTemplateUCClassContent()
        {
            return CLASS_TEMPLATE1;
        }

        protected virtual string GetDeclareStatementRepo(string classRepoName, string instanceName, 
            string name_space, string mainClassInstance)
        {
            return classRepoName + " " + instanceName + " = " + name_space + mainClassInstance + "." + mainClassInstance + ";";
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projFolderPath"></param>
        /// <param name="className"></param>
        /// <param name="repoFilePath"></param>
        /// <param name="imageCapFilePath"></param>
        /// <param name="appFilePath"></param>
        /// <param name="classRepoName"></param>
        /// <param name="name_space"></param>
        /// <param name="mainClassInstance">="Instance"</param>
        /// <param name="myLog"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        private string GenerateMainFile(Dictionary<string, string> mapFilePathAndId, string projFolderPath, string repoFilePath,
            string imageCapFilePath, string appFilePath, string classRepoName, string name_space,
            string mainClassInstance, MyLog myLog, string instanceName = INSTANCE_NAME)
        {
            string className = name_space + mainClassInstance;
            string fileContentTemplate = FILE_TEMPLATE
                .Replace(NAMESPACE_REPLACE, name_space);
            string classContent = CLASS_TEMPLATE1
                .Replace(CLASS_REPLACE, className + " : " + ABSTRACT_TEST_RUN);

            string classRawContent = "public static " + classRepoName + " " + mainClassInstance + " = new " +
                classRepoName + "(@\"" + repoFilePath + "\", @\"" + imageCapFilePath + "\");" + NEW_LINE;

            // disable by @duongtd 18/03/20, implemented new abstract class
            //classRawContent +=
            //    "#region common functions" + NEW_LINE +
            //    "// don't modify these functions" + NEW_LINE +
            //    "public static void Initialize()" + NEW_LINE +
            //    "{" + NEW_LINE +
            //        "Handler.Init(\"" + name_space + "\"" + ", @\"" + ProjectGeneration.RUNNING_TEST_FILE_NAME + "\");" + NEW_LINE +
            //    "}" + NEW_LINE +
            //    "public static void Finish()" + NEW_LINE +
            //    "{" + NEW_LINE +
            //        "Handler.Finish();" + NEW_LINE +
            //    "}" + NEW_LINE +
            //    "#endregion" + NEW_LINE +
            //    NEW_LINE +
            //    "public static void Main(string[] args)" + NEW_LINE +
            //    "{" + NEW_LINE +
            //        "Initialize();" + NEW_LINE;

            classRawContent +=
                "public override void DoRun()" + NEW_LINE +
                "{" + NEW_LINE;

            foreach (string runningFilePath in mapFilePathAndId.Keys)
            {
                int firstIndex = runningFilePath.IndexOf('\\');
                if (firstIndex < 0)
                    firstIndex = 0;
                else
                    firstIndex++;
                string classRunningName = runningFilePath.Substring(firstIndex, runningFilePath.LastIndexOf('.') - firstIndex);
                classRawContent += "new " + classRunningName + "().Run();" + NEW_LINE;
            }
            //classRawContent += "Finish();" + NEW_LINE + "}";
            classRawContent +=
                "}" + NEW_LINE +
                "public static void Main(string[] args)" + NEW_LINE + 
                "{" + NEW_LINE +
                    "new " + className + "().Run(\""+ name_space + "\", @\"testcase.ls\");" + NEW_LINE +
                "}";
            classContent = classContent.Replace(CONTENT_REPLACE, classRawContent);
            CreateNewFile(fileContentTemplate, classContent, Path.Combine(projFolderPath, (className + ".cs")));
            return className + ".cs";
        }

        /// <summary>
        /// create new file from its content, but re-format before write
        /// </summary>
        /// <param name="fileContentTemplate"></param>
        /// <param name="classContent"></param>
        /// <param name="filePath"></param>
        protected void CreateNewFile(string fileContentTemplate, string classContent, string filePath)
        {
            //var workspace = MSBuildWorkspace.Create();
            SyntaxTree tree = CSharpSyntaxTree.ParseText(fileContentTemplate);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var classRewrite = new ScriptsRewriter();
            classRewrite.ClassContent = classContent;
            SyntaxNode newRoot = classRewrite.Visit(root);
            //SyntaxNode formattedNode = Formatter.Format(newRoot, workspace);
            if (!File.Exists(filePath))
                Utils.CreateDirectoryForFilePath(filePath);
            string formatedContent = Utils.ReformatCsCode(
                //formattedNode.ToFullString());
                newRoot.ToFullString());
            File.WriteAllText(filePath, formatedContent);
        }

        /// <summary>
        /// only retrieve global statements
        /// </summary>
        /// <param name="listUcScriptsExp"></param>
        /// <returns></returns>
        protected string GetGlobalStatementsAndFunc(List<UserCodeScriptsExpression> listUcScriptsExp, 
            string classRepoName, string instanceName, string name_space, string mainClassInstance)
        {
            // "MyTestProjectDefinition elements = MyTestProjectInstance.Instance;"
            string re = GetDeclareStatementRepo(classRepoName, instanceName, name_space, mainClassInstance);
            if (listUcScriptsExp == null || listUcScriptsExp.Count == 0)
                return re;
            List<string> globalStatements = new List<string>();
            List<Tuple<string, FunctionExpression>> listClassFunction = new List<Tuple<string, FunctionExpression>>();
            foreach(UserCodeScriptsExpression ucScriptsExp in listUcScriptsExp)
            {
                if (!globalStatements.Contains(ucScriptsExp.GlobalScripts))
                {
                    if (re != "")
                        re += NEW_LINE;
                    re += ucScriptsExp.GlobalScripts;
                    globalStatements.Add(ucScriptsExp.GlobalScripts);
                }
            }
            return re;
        }

        /// <summary>
        /// only retrieve MapClassAndFuncsAddition
        /// </summary>
        /// <param name="listUcScriptsExp"></param>
        private List<string> GenerateAdditionScripts(List<UserCodeScriptsExpression> listUcScriptsExp, string projectPath, string name_space,
            ListUIElements listUIElements, Dictionary<string, string> mapAliasWithNode, MyLog myLog, string instanceName)
        {
            Dictionary<string, List<FunctionExpression>> mapClassFunc = new Dictionary<string, List<FunctionExpression>>();
            foreach(var ucScriptsExp in listUcScriptsExp)
            {
                var newMapClassFunc = ucScriptsExp.MapClassAndFuncsAddition;
                foreach(var pair in newMapClassFunc)
                {
                    if (mapClassFunc.ContainsKey(pair.Key))
                        mapClassFunc[pair.Key].AddRange(pair.Value);
                    else
                        mapClassFunc.Add(pair.Key, pair.Value);
                }
            }
            return GenerateAdditionScripts(mapClassFunc, projectPath, name_space, listUIElements, mapAliasWithNode, myLog, instanceName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapClassFunc">(classname, list of functions)</param>
        private List<string> GenerateAdditionScripts(Dictionary<string, List<FunctionExpression>> mapClassFunc, string projectPath, 
            string name_space, ListUIElements listUIElements, Dictionary<string, string> mapAliasWithNode, MyLog myLog, string instanceName)
        {
            List<string> listRelativeFilePath = new List<string>();
            string fileContentTemplate = GetTemplateRunCsFileContent()
                .Replace(NAMESPACE_REPLACE, name_space);
            foreach (var pair in mapClassFunc)
            {
                string className = pair.Key;
                List<FunctionExpression> listFunctions = pair.Value;
                string functionsScripts = "";
                foreach(FunctionExpression func in listFunctions)
                {
                    if (functionsScripts != "")
                        functionsScripts += NEW_LINE;
                    functionsScripts += GenerateFunctionExpScripts(func, listUIElements, mapAliasWithNode, myLog, instanceName);
                }
                // create new file
                string classContent = GetTemplateUCClassContent()
                    //.Replace(GLOBAL_REPLACE, "") modified by @duong 05/29
                    .Replace(CLASS_REPLACE, className)
                    .Replace(CONTENT_REPLACE, functionsScripts);
                string filePath = Path.Combine("UserCode", className + ".cs");
                listRelativeFilePath.Add(filePath);
                CreateNewFile(fileContentTemplate, classContent, Path.Combine(projectPath, filePath));
            }
            return listRelativeFilePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classesExpression"></param>
        /// <param name="projectPath"></param>
        /// <param name="name_space"></param>
        /// <param name="listUIElements"></param>
        /// <param name="mapAliasWithNode"></param>
        /// <param name="myLog"></param>
        /// <param name="instanceName"></param>
        /// <returns>a list of relative file path</returns>
        private List<string> GenerateClassExpScripts(List<ClassExpression> classesExpression, string projectPath, string name_space,
            ListUIElements listUIElements, Dictionary<string, string> mapAliasWithNode, MyLog myLog, string instanceName)
        {
            List<string> re = new List<string>();
            foreach(ClassExpression classExp in classesExpression)
            {
                re.Add(GenerateClassExpScripts(classExp, projectPath, name_space, listUIElements, mapAliasWithNode, myLog, instanceName));
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classExpression"></param>
        /// <param name="projectPath"></param>
        /// <param name="name_space"></param>
        /// <param name="listUIElements"></param>
        /// <param name="mapAliasWithNode"></param>
        /// <param name="myLog"></param>
        /// <param name="instanceName"></param>
        /// <returns>relative file path</returns>
        private string GenerateClassExpScripts(ClassExpression classExpression, string projectPath, string name_space,
            ListUIElements listUIElements, Dictionary<string, string> mapAliasWithNode, MyLog myLog, string instanceName)
        {
            if (classExpression.getName() == null || classExpression.getListFunction() == null ||
                classExpression.getListFunction().Count <= 0)
                return null;
            string functionsScripts = "";
            foreach (FunctionExpression func in classExpression.getListFunction())
            {
                if (functionsScripts != "")
                    functionsScripts += NEW_LINE;
                functionsScripts += GenerateFunctionExpScripts(func, listUIElements, mapAliasWithNode, myLog, instanceName);
            }
            // create new file
            string classContent = GetTemplateUCClassContent()
                //.Replace(GLOBAL_REPLACE, "") disabled by @duongtd 05/29
                .Replace(CLASS_REPLACE, classExpression.getName())
                .Replace(CONTENT_REPLACE, functionsScripts);
            string filePath = Path.Combine(classExpression.GetCorrectWorkspace(), classExpression.getName() + ".cs");
            string fileContentTemplate = GetTemplateRunCsFileContent()
                .Replace(NAMESPACE_REPLACE, name_space);
            CreateNewFile(fileContentTemplate, classContent, Path.Combine(projectPath, filePath));
            return filePath;
        }

        /// <summary>
        /// scripts content for a function
        /// </summary>
        /// <param name="functionExpression"></param>
        /// <param name="listUIElements"></param>
        /// <param name="mapAliasWithNode"></param>
        /// <param name="myLog"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        private string GenerateFunctionExpScripts(FunctionExpression functionExpression, ListUIElements listUIElements,
            Dictionary<string, string> mapAliasWithNode, MyLog myLog, string instanceName)
        {
            string accessibility = functionExpression.GetCorrectAccessibility();
            string summary = functionExpression.getSummary();
            List<ParameterExpression> @params = functionExpression.getParams();
            string name = functionExpression.getName();
            string returnDes = functionExpression.getReturnDescription();
            string content = functionExpression.getContent();

            string re = "";
            if (summary != null)
            {
                re += "/// <summary>" + NEW_LINE;
                string[] listLines = summary.Split('\n');
                foreach (string line in listLines)
                    re += "/// " + line + NEW_LINE;
                re += "/// </summary>";
            }
            var pair = GetParamsScripts(@params, listUIElements, mapAliasWithNode, myLog, instanceName);
            if (pair.Item2 != null && pair.Item2.Count > 0)
            {
                for (int fi = 0; fi < pair.Item2.Count; fi++)
                {
                    string paramName = pair.Item2[fi];
                    string paramDesc = @params[fi].getDescription();
                    if (paramDesc == null)
                        paramDesc = "";
                    if (re != "")
                        re += NEW_LINE;
                    re += "/// <param name=\"" + paramName + "\">" + paramDesc + "</param>";
                }
            }
            string returnType = functionExpression.GetCorrectReturnType();
            if (returnType != FunctionExpression.VOID)
            {
                if (re != "")
                    re += NEW_LINE;
                re += "/// <returns>" + (returnDes ?? "") + "</returns>";
            }
            if (re != "")
                re += NEW_LINE;
            // Ranorex need to insert annotation
            re += InsertAnnotation();
            re += accessibility + " static " + returnType + " " + name + "(";
            re += pair.Item1;
            re += ")" + NEW_LINE;
            re += "{" + NEW_LINE;
            if (content != null)
            {
                string[] listLines = content.Split('\n');
                foreach (string line in listLines)
                {
                    string line1 = line.Trim().StartsWith("//") ? line.Trim() : "// " + line.Trim();
                    re += line1 + NEW_LINE;
                }
            }
            if (returnType != FunctionExpression.VOID)
                re += "return null;" + NEW_LINE;
            re += "}" + NEW_LINE;
            return re;
        }

        /// <summary>
        /// nothing, however Ranorex need to insert annotation
        /// </summary>
        /// <returns></returns>
        protected virtual string InsertAnnotation()
        {
            return "";
        }

        /// <summary>
        /// </summary>
        /// <param name="params"></param>
        /// <param name="listUIElements"></param>
        /// <param name="mapAliasWithNode"></param>
        /// <param name="myLog"></param>
        /// <param name="instanceName"></param>
        /// <returns>{params scripts, list of param's name}</returns>
        private Tuple<string, List<string>> GetParamsScripts(List<ParameterExpression> @params, ListUIElements listUIElements,
            Dictionary<string, string> mapAliasWithNode, MyLog myLog, string instanceName)
        {
            if (@params == null || @params.Count == 0)
                return new Tuple<string, List<string>>("", null);
            string re = "";
            List<string> nameParams = new List<string>();
            for (int fi = 1; fi <= @params.Count; fi++)
            {
                ParameterExpression param = @params[fi - 1];
                if (re != "")
                    re += ", ";
                string varName = "param_" + fi;
                nameParams.Add(varName);
                re += "object " + varName;
            }
            return new Tuple<string, List<string>>(re, nameParams);
        }

        private bool IsFuncExisted(Tuple<string, FunctionExpression> pair, List<Tuple<string, FunctionExpression>> listClassFunction)
        {
            foreach (var classAndFunc in listClassFunction)
            {
                if (IsFuncsEqual(classAndFunc, pair))
                    return true;
            }
            return false;
        }
        private bool IsFuncsEqual(Tuple<string, FunctionExpression> pair1, Tuple<string, FunctionExpression> pair2)
        {
            // if className is diff
            if (pair1.Item1 != pair2.Item1)
                return false;
            // same className, compare func
            // if functionName diff
            if (pair1.Item2.getName() != pair2.Item2.getName())
                return false;
            // if no. params diff
            if (FunctionExpression.GetNoParams(pair1.Item2) !=
                FunctionExpression.GetNoParams(pair2.Item2))
                return false;
            // if return type diff
            if (FunctionExpression.IsVoidFunc(pair1.Item2) ==
                FunctionExpression.IsVoidFunc(pair2.Item2))
                return false;
            return true;
        }

        public static void LogError(string expression, string screenName, int scId, MyLog myLog, string node = null)
        {
            string log = "[Error] Expression: " + expression + " at sheet " + screenName +
                ", testcase #" + scId;
            if (node != null)
                log += " (" + node + " object)";
            myLog.Error(log);
        }
    }

    /// <summary>
    /// rewrite class content, in order to reformat codes
    /// </summary>
    public class ScriptsRewriter : CSharpSyntaxRewriter
    {
        string classContent;

        public string ClassContent
        {
            get { return classContent; }
            set
            {
                classContent = value;
            }
        }

        public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(ClassContent);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            return node.AddMembers(root.Members[0]);
        }
    }
}
