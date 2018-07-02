// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 10:22 PM 2018/1/23
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Media;

namespace TestingApplication
{
    public class UserActionSpecAnalyzer
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// analyze list of screens
        /// </summary>
        /// <param name="listScreens"></param>
        /// <param name="myLog"></param>
        public bool Expand(List<IScreen> listScreens, string pathToApp, MyLog myLog)
        {
            Dictionary<string, SpecScreen> map = new Dictionary<string, SpecScreen>();
            List<ClassExpression> listClassesExpression = new List<ClassExpression>();
            foreach (IScreen screen in listScreens)
            {
                if (screen == null)
                    listScreens.Remove(screen);
                else if (screen is SpecScreen specScreen)
                {
                    map.Add(screen.Name, specScreen);
                    if (specScreen is TestSpecificationScreen testSpecScreen)
                        Utils.MergeClasses2(listClassesExpression, testSpecScreen.ClassExpressions);
                }
                else
                    myLog.Warn("Not Implement for this type", logger);
            }
            foreach (SpecScreen screen in map.Values)
            {
                DoExpand(screen, pathToApp, myLog, listClassesExpression);
            }
            foreach (SpecScreen screen in map.Values)
            {
                AddPreCondition(screen, map, myLog);
            }
            return true;
        }

        public bool Expand(List<SpecScreen> listScreens, string pathToApp, MyLog myLog)
        {
            List<IScreen> temp = new List<IScreen>(listScreens);
            return Expand(temp, pathToApp, myLog);
        }

        private void DoExpand(SpecScreen screen, string pathToApp, MyLog myLog, List<ClassExpression> allClassesExp)
        {
            for (int fi = 0; fi < screen.Scenarios.Count; fi++)
            {
                IScenario tempScenario = screen.Scenarios[fi];
                if (tempScenario is SpecScenario)
                {
                    SpecScenario scenario = tempScenario as SpecScenario;
                    List<string> listActionExp = scenario.UserActionsInString;
                    List<Color> colors = scenario.Colors;
                    for (int se = 0; se < listActionExp.Count; se++)
                    {
                        string actionExp = listActionExp[se];
                        if (actionExp == null)
                        {
                            myLog.Error("NULL at (" + (fi + 1) + "," + (se + 2) + ")");
                            continue;
                        }
                        SpecNode specNode = screen.ListSpecNodes[se];
                        ScriptGenerationParams _params = null;
                        AbstractSpecUserAction specUserAction = null;
                        int lastInd = IsValidationUserCode(se, screen.ListValidationUserCode);
                        if (lastInd > se)
                        {
                            specUserAction = new ValidationUserCodeSpecUserAction();
                            _params = new ValidationUCScriptGenerationParams();
                            List<string> listExp = new List<string>();
                            for (int th = se; th <= lastInd; th++)
                            {
                                String tempActionExp = listActionExp[th];
                                AbstractSpecUserAction temp = handleUserActionExpression(
                                        tempActionExp.Trim(), specNode, colors[th], myLog);
                                if (!(temp is ValidationSpecUserAction))
                                {
                                    myLog.Warn("Expression: " + tempActionExp + " must be validation", logger);
                                    screen = null;
                                    return;
                                }
                                else
                                {
                                    if (tempActionExp != null && !tempActionExp.Trim().Equals(""))
                                        listExp.Add(tempActionExp.Trim());
                                }
                            }
                            (specUserAction as ValidationUserCodeSpecUserAction).ListExps = listExp;
                            ((ValidationUCScriptGenerationParams)_params).ListExps = listExp;
                            if (screen is TestSpecificationScreen)
                                ((ValidationUCScriptGenerationParams)_params).ClassExpressions = allClassesExp;
                            ((ValidationUCScriptGenerationParams)_params).ClassName =
                                AbstractSpecUserAction.GetFolderNameFromScreen(screen) + "_Validation";
                            ((ValidationUCScriptGenerationParams)_params).FunctionName = "Validate_" +
                                    Regex.Replace(specNode.Expression, "[^A-Za-z0-9]", "_");
                            SetAttributes(scenario, specUserAction, specNode,
                                        screen, pathToApp, colors, _params, se, myLog);
                            se = lastInd;
                        }
                        else
                        {
                            string[] listActionsEpx = splitAction(actionExp, AbstractSpecUserAction.AND);
                            foreach (string action in listActionsEpx)
                            {
                                specUserAction = handleUserActionExpression(action.Trim(), specNode, colors[se], myLog);
                                if (specUserAction != null)
                                {
                                    if (specUserAction is UserCodeSpecUserAction userCodeExpression)
                                    {
                                        _params = new UserCodeScriptGenerationParams();
                                        ((UserCodeScriptGenerationParams)_params).MapAliasWithNode = screen.MappingAliasWithNode;
                                        if (screen is TestSpecificationScreen testSpecScreen)
                                            ((UserCodeScriptGenerationParams)_params).ClassExpressions = allClassesExp;
                                    }
                                    else if (specUserAction is WaitValidateSpecUserAction waitUserAction)
                                    {
                                        _params = new WaitValidateScriptGenerationParams();
                                    }
                                    else
                                        _params = new ScriptGenerationParams();
                                    SetAttributes(scenario, specUserAction, specNode,
                                        screen, pathToApp, colors, _params, se, myLog);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SetAttributes(SpecScenario scenario, AbstractSpecUserAction specUserAction, SpecNode specNode,
            IScreen screen, string pathToApp, List<Color> colors, ScriptGenerationParams _params, int se, MyLog myLog)
        {
            // importance: must create on the same thread with UI thread
            try
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    specUserAction.BgColor = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromArgb(
                        255, colors[se].R, colors[se].G, colors[se].B));
                });
            }
            catch (Exception)
            {
                specUserAction.BgColor = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromArgb(
                        255, colors[se].R, colors[se].G, colors[se].B));
            }
            _params.Color = colors[se];
            _params.Id = scenario.Id;
            _params.ListUIElements = screen.AllUIElements;
            _params.MyLog = myLog;
            _params.ScreenName = screen.Name;
            _params.SpecNode = specNode;
            _params.PathToApp = pathToApp;
            specUserAction.Params = _params;
            specUserAction.NodeAffect = specNode;
            if (scenario.UserActions == null)
                scenario.UserActions = new List<IUserAction>();
            scenario.UserActions.Add(specUserAction);
        }

        /// <summary>
        /// add pre-conditions to each screnario
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="mapNameAndScreens"></param>
        /// <param name="myLog"></param>
        private void AddPreCondition(SpecScreen screen, Dictionary<string, SpecScreen> mapNameAndScreens, MyLog myLog)
        {
            foreach (IScenario scenario in screen.Scenarios)
            {
                if (scenario is SpecScenario specScenario)
                {
                    if (specScenario.PreConditionsExpression != null &&
                        specScenario.PreConditionsExpression.Count > 0)
                    {
                        foreach (string preConditionExp in specScenario.PreConditionsExpression)
                        {
                            if (preConditionExp == null || preConditionExp.Equals(""))
                                continue;
                            int idx = preConditionExp.LastIndexOf("_");
                            if (idx < 0)
                                myLog.Warn("Can not find the pre-condiction: " + preConditionExp);
                            else
                            {
                                string preScreen = preConditionExp.Substring(0, idx);
                                try
                                {
                                    int preScenarioIdx = Int32.Parse(preConditionExp.Substring(idx + 1));
                                    SpecScreen preConditionScreen = mapNameAndScreens[preScreen];
                                    SpecScenario preConditonScenario = getScenarioByIndex(
                                            preScenarioIdx, preConditionScreen.Scenarios);
                                    if (preConditionScreen == null ||
                                            preScenarioIdx > preConditionScreen.Scenarios.Count ||
                                            preConditonScenario == null)
                                        myLog.Warn("Can not find the pre-condiction: " + preConditionExp);
                                    else
                                        specScenario.PreConditions.Add(preConditonScenario);
                                }
                                catch (FormatException e)
                                {
                                    myLog.Warn("Invalid pre-condiction: " + preConditionExp);
                                }
                            }
                        }
                    }
                }
                else
                    myLog.Warn("Not implement this type");
            }
        }

        /// <summary>
        /// type expression
        /// </summary>
        /// <param name="actionEpx"></param>
        /// <param name="specNode"></param>
        /// <param name="color"></param>
        /// <param name="myLog"></param>
        /// <returns></returns>
        private AbstractSpecUserAction handleUserActionExpression(String actionEpx,
                                   SpecNode specNode,
                                   Color color,
                                   MyLog myLog)
        {
            if (actionEpx == null || actionEpx.Equals("") ||
                    actionEpx.Equals(AbstractSpecUserAction.NA) ||
                    actionEpx.Equals(AbstractSpecUserAction.DO_NOTHING) ||
                    actionEpx.Equals(AbstractSpecUserAction.EMPTY) ||
                    color.Equals(AbstractSpecUserAction.NOT_TEST_COLOR1) ||
                    color.Equals(AbstractSpecUserAction.NOT_TEST_COLOR2))// ||
//                color.Equals(AbstractSpecUserAction.NOT_TEST_COLOR3))
                return null;
            /**
             * determine if delay, wait, keyboard press
             */
            if (specNode.UIElement == null)
            {
                // delay
                if (Regex.IsMatch(specNode.Expression, AbstractSpecUserAction.DELAY_TITLE_EXPRESSION))
                {
                    DelaySpecUserAction delaySpecUserAction = new DelaySpecUserAction();
                    delaySpecUserAction.Expression = actionEpx;
                    return delaySpecUserAction;
                }
                // wait
                else if (Regex.IsMatch(specNode.Expression, AbstractSpecUserAction.PRE_WAIT + "(_\\d*)*"))
                {
                    PreWaitValidateSpecUserAction waitValidateSpecUserAction = new PreWaitValidateSpecUserAction();
                    waitValidateSpecUserAction.Expression = actionEpx;
                    return waitValidateSpecUserAction;
                }
                else if (Regex.IsMatch(specNode.Expression, AbstractSpecUserAction.POST_WAIT + "(_\\d*)*"))
                {
                    PostWaitValidateSpecUserAction waitValidateSpecUserAction = new PostWaitValidateSpecUserAction();
                    waitValidateSpecUserAction.Expression = actionEpx;
                    return waitValidateSpecUserAction;
                }
                //keyboard press
                else if (Regex.IsMatch(specNode.Expression, AbstractSpecUserAction.KEYBOARD_PRESS_REGEX))
                {
                    KeyboardSpecUserAction keyboardPressSpecUserAction = new KeyboardSpecUserAction();
                    keyboardPressSpecUserAction.Expression = actionEpx;
                    return keyboardPressSpecUserAction;
                }
                //test - not test
                else if (Regex.IsMatch(specNode.Expression, AbstractSpecUserAction.RESULT))
                {
                    return null;
                }
                // usercode
                else if (Regex.IsMatch(specNode.Expression, AbstractSpecUserAction.USER_CODE) ||
                        Regex.IsMatch(specNode.Expression, AbstractSpecUserAction.USER_CODE_WITH_VARIABLE_DECLARE))
                {
                    UserCodeSpecUserAction userCodeExpression = new UserCodeSpecUserAction();
                    userCodeExpression.Expression = actionEpx;
                    return userCodeExpression;
                }
            }
            AbstractSpecUserAction specUserAction = null;
            string attribute = specNode.Attribute;
            if (actionEpx.Equals(AbstractSpecUserAction.OPEN))
            {
                specUserAction = new OpenAppSpecUserAction();
            }
            else if (actionEpx.Equals(AbstractSpecUserAction.CLOSE))
            {
                specUserAction = new CloseAppSpecUserAction();
            }
            else if (actionEpx.StartsWith(AbstractSpecUserAction.CLICK))
            {
                specUserAction = new ClickSpecUserAction();
            }
            else if (actionEpx.StartsWith(AbstractSpecUserAction.DOUBLE_CLICK))
            {
                specUserAction = new DoubleClickSpecUserAction();
            }
            else if (actionEpx.StartsWith(AbstractSpecUserAction.RIGHT_CLICK))
            {
                specUserAction = new RightClickSpecUserAction();
            }
            else if (actionEpx.StartsWith(AbstractSpecUserAction.WAIT_EXIST) ||
                  actionEpx.StartsWith(AbstractSpecUserAction.WAIT_NOT_EXIST))
            {
                specUserAction = new WaitExistSpecUserAction();
            }
            else if (actionEpx.StartsWith(AbstractSpecUserAction.EXIST) ||
                  actionEpx.StartsWith(AbstractSpecUserAction.NOT_EXIST))
            {
                specUserAction = new CheckExistSpecUserAction();
            }
            else if (actionEpx.StartsWith(AbstractSpecUserAction.DELAY))
            {
                specUserAction = new DelaySpecUserAction();
            }
            else if (actionEpx.StartsWith(AbstractSpecUserAction.GET))
            {
                specUserAction = new GetSpecUserAction();
            }
            else if (actionEpx.StartsWith(AbstractSpecUserAction.CAPTURE))
            {
                specUserAction = new CaptureSpecUserAction();
            }
            else if (actionEpx.StartsWith(AbstractSpecUserAction.DROP))
            {
                specUserAction = new DropSpecUserAction();
            }
            //else if (actionEpx.StartsWith(AbstractSpecUserAction.COMPARE_IMAGE))
            //{
            //    specUserAction = new ImageComparativeSpecUserAction();
            //    specUserAction.Expression = actionEpx;
            //}
            else if (actionEpx.StartsWith(AbstractSpecUserAction.ATTRIBUTE_CONTAIN) ||
                  actionEpx.StartsWith(AbstractSpecUserAction.ATTRIBUTE_NOT_CONTAIN))
            {
                specUserAction = new ContainSpecUserAction();
            }

            else if (color.Equals(AbstractSpecUserAction.VALIDATION_COLOR) ||
                    color.Equals(AbstractSpecUserAction.ENVIRONMENT_COLOR))
            {
                specUserAction = new ValidationSpecUserAction();
            }
            else if (color.Equals(AbstractSpecUserAction.PROCEDURES_COLOR) ||
                  color.Equals(AbstractSpecUserAction.PRE_CONDITION_COLOR))
            {
                if (attribute != null && !attribute.Equals("") &&
                        (Regex.IsMatch(attribute, "^\\w+$") ||
                         Regex.IsMatch(attribute, ".*\\..*")))
                {
                    specUserAction = new SetSpecUserAction();
                }
                else if (attribute == null || attribute.Equals("") ||
                    Regex.IsMatch(attribute, "Cell\\s*\\((\\d+?),\\s*(\\d+?)\\)") ||
                    Regex.IsMatch(attribute, "Row\\s*\\((\\d+?)\\)"))
                {
                    specUserAction = new PressSpecUserAction();
                }
            }

            // add new case to handle here
            else
            {
                myLog.Warn("Incorrect expression: " + actionEpx);
            }
            if (specUserAction != null)
            {
                specUserAction.Expression = actionEpx;
            }
            return specUserAction;
        }

        private SpecScenario getScenarioByIndex(int index, List<IScenario> scenarioList)
        {
            foreach (SpecScenario scenario in scenarioList)
            {
                if (scenario.Id == index)
                    return scenario;
            }
            return null;
        }

        private string removeBrackets(string input)
        {
            return input.Replace("(", "").Replace(")", "");
        }

        private int IsValidationUserCode(int index, List<Tuple<int, int>> listPair)
        {
            foreach (Tuple<int, int> pair in listPair)
            {
                if (index == pair.Item1) //&& index <= pair.getValue()) {
                    return pair.Item2;
            }
            return -1;
        }
        private string[] splitAction(string action, string pivot)
        {
            return Regex.Split(action, pivot);
        }
    }
}