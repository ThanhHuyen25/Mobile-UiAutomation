// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 11:38 AM 2018/1/21
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GUI_Testing_Automation.Ranorex;

namespace TestingApplication
{
    public abstract class AbstractSpecUserAction : AbstractUserAction
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region const 
        public const string OPEN = "Open";
        public const string CLOSE = "Close";

        //click
        public const string CLICK = "Click";
        public const string DOUBLE_CLICK = "DoubleClick";
        public const string RIGHT_CLICK = "RightClick";

        //exist
        public const string EXIST = "Exist";
        public const string NOT_EXIST = "Not Exist";

        //press
        public const string PRESS = "Press";
        public const string PRESS_MULTIPLE_KEYS = "Press multi keys";
        public const string INPUT_STRING = "Input string";
        public const string INPUT = "Input"; //Input cell (1,1) "abc"

        //wait
        public const string WAIT_EXIST = "Wait Exist";
        public const string WAIT_NOT_EXIST = "Wait NotExist";

        //delay
        public const string DELAY = "Delay";

        //attribute
        public const string GET = "Get";
        public const string SET = "Set";

        //capture
        public const string CAPTURE = "Capture";

        //drop
        public const string DROP = "Drop";

        //getValidation
        public const string ATTRIBUTE_EQUAL = "Equal";
        public const string ATTRIBUTE_CONTAIN = "Contain";
        public const string ATTRIBUTE_NOT_CONTAIN = "Not contain";

        //image
        public const string COMPARE_IMAGE = "Compare image";

        public const string NA = "N/A";
        public const string DO_NOTHING = "DoNothing";
        public const string EMPTY = "Empty";
        public const string RESULT = "Result";

        public const string NOT_TEST = "Not Test";
        public const string TEST = "Test";

        /**
         * string join multi action
         * like this: `Click and Input string "user1"`
         */
        //regex
        public const string AND = "&";

        //color
        public static readonly Color ENVIRONMENT_COLOR = Color.FromArgb(230, 245, 216);//"FFE6F5D8";
        public static readonly Color PRE_CONDITION_COLOR = Color.FromArgb(251, 208, 228);//"FFFBD0E4";
        public static readonly Color PROCEDURES_COLOR = Color.FromArgb(255, 241, 204);//"FFFFF1CC";
        public static readonly Color VALIDATION_COLOR = Color.FromArgb(201, 248, 242);//"FFC9F8F2";
        public static readonly Color NOT_TEST_COLOR1 = Color.FromArgb(165, 165, 165);//"FFA5A5A5";
        public static readonly Color NOT_TEST_COLOR2 = Color.FromArgb(166, 166, 166);//"FFA6A6A6";

        /**
         * modified by duongtd 12/10/17, consider to revert
         */
        //    string NOT_TEST_COLOR3 = "FFFFFFFF";

        public const string DELAY_TITLE_EXPRESSION = "Delay|Delay\\s*\\(Step\\)|Delay_\\d*";

        public const string LINKS = "(?i)List Of Previous TestCases|Link";
        public const string PRE_CONDITION_COMMNENT = "Previous Testcases";
        public const string BLOCK_SLASH = "//%";

        public const string PRE_WAIT = "PreWait";
        public const string POST_WAIT = "PostWait";

        public const string KEYBOARD_PRESS_REGEX = "Keys_\\d+";

        public const string USER_CODE = "UserCode(_\\d*)*";
        public const string USER_CODE_WITH_VARIABLE_DECLARE = "Variable_(.*)";

        public const string NEW_LINE = "\r\n";
        public const string TAB = "\t";
        public const string SLASH2 = "//";
        public const string SLASH3 = "///";
        public const string TEXT = "Text";
        public const string WIDTH = "Width";
        public const string HEIGHT = "Height";
        public const string BOUND_RECT = "BoundRect";

        protected const string SINGLE_QUOTE = "(?s)'.*'";
        protected const string LOCATION = "<(\\d*)?;\\s*(\\d*)?>";
        protected const string MAXIMIZE = "Maximized";
        protected const string MINIMIZE = "Minimized";
        protected const string TRUE = "True";
        protected const string FALSE = "False";
        public const string MISSING_STATEMENT = "//[ERROR] MISSING STATEMENT HERE" + NEW_LINE;
        public const string REPO = "repo";

        #endregion const

        #region attributes and methods
        protected string expression;

        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public IScriptGenerationParams Params
        {
            get { return @params; }
            set { @params = value; }
        }

        public bool IgnoreGenerateScripts
        {
            get { return ignoreGenerateScripts; }
            set
            {
                ignoreGenerateScripts = value;
            }
        }

        protected IScriptGenerationParams @params;
        protected bool ignoreGenerateScripts;

        private System.Windows.Media.Brush bgColor;
        #endregion attributes and methods

        #region common functions
        public static string GetFolderNameFromScreen(IScreen screen)
        {
            string name = null;
            if (screen is TestSpecificationScreen)
            {
                name = ((TestSpecificationScreen)screen).Name;
            }
            else
                name = screen.Name;
            return Regex.Replace(name, "[^A-Za-z0-9]", "");
        }

        protected string GetComment(IScriptGenerationParams scriptGenerationParams)
        {
            string comment = SLASH2 + "[" + GetTypeAction(scriptGenerationParams.Color) + "] ";
            if (nodeAffect is SpecNode specNode)
                comment += specNode.Expression + ": " + (expression == null ? "" : replaceReturnChar(expression));
            return comment;
        }

        protected string GetTypeAction(Color color)
        {
            if (color.Equals(ENVIRONMENT_COLOR))
                return "Environment";
            else if (color.Equals(PRE_CONDITION_COLOR))
                return "Pre-condition";
            else if (color.Equals(PROCEDURES_COLOR))
                return "Procedure";
            else if (color.Equals(VALIDATION_COLOR))
                return "Validation";
            else
                return "";
        }

        public static string replaceReturnChar(string origin)
        {
            return origin.Replace("\n", "\\n");
        }

        public string GetScriptCapture(IElement element, string instanceName)
        {
            string re = "CaptureElement.CaptureScreen(";
            re += GetScriptAccessElement(element, instanceName) + ");";
            return re;
        }

        public static string GetScriptAccessElement(IElement element, string instanceName)
        {
            string re = ElementCSharpCodeGeneration.FormatName(element.Attributes.Name);
            IElement temp = element.Parent;
            while (temp != null)
            {
                re = ElementCSharpCodeGeneration.FormatName(temp.Attributes.Name) + "." + re;
                temp = temp.Parent;
            }
            return instanceName + "." + re;
        }

        public string SoundWithTryCatch(string tryContent, string catchContent)
        {
            return "try" + NEW_LINE +
                "{" + NEW_LINE +
                tryContent + NEW_LINE +
                "}" + NEW_LINE +
                "catch (System.Exception)" + NEW_LINE +
                "{" + NEW_LINE +
                (catchContent == null || catchContent.Equals("") ? "" : (catchContent + NEW_LINE)) +
                "}";
        }

        public static string NormalizeExpression(string expression)
        {
            expression = expression.Trim();
            Regex regex = new Regex(@"'(?<value>.*)'");
            Match match = regex.Match(expression);
            if (match.Success)
            {
                return match.Groups["value"].Value;
            }
            return expression;
        }
        public string DisplayExpression
        {
            get
            {
                return GetDisplayExpression();
            }
        }
        public System.Windows.Media.Brush BgColor
        {
            get { return bgColor; }
            set
            {
                bgColor = value;
            }
        }

        public string GetDisplayExpression()
        {
            string re = "";
            if (nodeAffect is SpecNode specNode)
                re += specNode.Expression;
            else
                re += nodeAffect.UIElement.Attributes.Name;
            if (this is ValidationUserCodeSpecUserAction validationUCSpe)
                re += " [" + string.Join(",", validationUCSpe.ListExps) + "]";
            else
                re += " [" + expression + "]";
            return re;
        }

        #endregion common functions

        //full scripts
        public virtual ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string reStr = GetComment(scriptGenerationParams) + NEW_LINE;
            var re = GenRawScripts(scriptGenerationParams);
            if (re == null)
                return null;
            re.Expression = reStr + re.Expression;
            return re;
        }

        //scripts without comment or try/catch
        public abstract ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams);

        //full scripts
        public virtual ScriptsExpression GenRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string reStr = GetComment(scriptGenerationParams) + NEW_LINE;
            var re = GenRawRanorexScripts(scriptGenerationParams);
            if (re == null)
                return null;
            re.Expression = reStr + re.Expression;
            return re;
        }

        //scripts without comment or try/catch
        public abstract ScriptsExpression GenRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams);

        public override IUserAction Clone()
        {
            AbstractSpecUserAction re = (AbstractSpecUserAction)Activator.CreateInstance(this.GetType());
            CopyAttribute(this, re);
            return re;
        }

        public static void CopyAttribute(AbstractSpecUserAction userAction1, AbstractSpecUserAction userAction2)
        {
            if (userAction1.BgColor != null)
                userAction2.BgColor = userAction1.BgColor;
            if (userAction1.Expression != null)
                userAction2.Expression = userAction1.Expression;
            userAction2.IgnoreGenerateScripts = userAction1.IgnoreGenerateScripts;
            if (userAction1.NodeAffect != null)
                userAction2.NodeAffect = userAction1.NodeAffect.Clone();
            if (userAction1.Params != null)
                userAction2.Params = userAction1.Params.Clone();
        }

        public void LogError(log4net.ILog logger)
        {
            string log = "[Error] Expression: " + this.expression + " at sheet " + this.Params.ScreenName +
                ", testcase #" + this.Params.Id;
            this.Params.MyLog.Error(log);
            logger.Error(log);
        }
        public void LogError(log4net.ILog logger, string msg)
        {
            string log = "[Error] Expression: " + this.expression + " at sheet " + this.Params.ScreenName +
                ", testcase #" + this.Params.Id + NEW_LINE + msg;
            this.Params.MyLog.Error(log);
            logger.Error(log);
        }

        protected string RemoveSingleQuote(string input)
        {
            if (Regex.IsMatch(input, SINGLE_QUOTE))
                return input.Substring(1, input.Length - 2);
            return input;
        }

        #region Ranorex // start with lower case letter
        public static string getScriptAccessRanorexObject(IRanorexScriptGenerationParams _params)
        {
            SpecNode nodeAndAttribute = _params.SpecNode;
            IElement node = nodeAndAttribute.UIElement;
            string attribute = nodeAndAttribute.Attribute;
            string re = getScriptAccessRanorexElement(nodeAndAttribute, _params.InstanceName);

            if (node is AppFolderRanorexElement ||
                    node is FolderRanorexElement) {
                re += ".SelfInfo";
            } else {
                if (attribute == null || attribute.Equals(""))
                {
                    re += "Info";
                }
                else if (Regex.IsMatch(attribute, "Cell\\s*\\((\\d+?),\\s*(\\d+?)\\)(\\.\\w+)*") ||
                      Regex.IsMatch(attribute, "Row\\s*\\((\\d+?)\\)(\\.\\w+)*"))
                {
                    re += ".Element";
                }
                else
                {
                    re += "Info";
                }
            }
            return re;
        }
        
        /// <summary>
        /// getFullNodeName
        /// </summary>
        /// <param name="nodeAndAttribute"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        public static string getScriptAccessRanorexElement(SpecNode nodeAndAttribute, string instanceName)
        {
            string attribute = nodeAndAttribute.Attribute;
            string re = GetScriptAccessElement(nodeAndAttribute.UIElement, instanceName);

            //Textbox
            if (attribute == null || attribute.Equals(""))
            {
                return re;
            }
            else
            {
                /**
                 * Dg.Cell(1,1)
                 */
                Regex pattern1 = new Regex("Cell\\s*\\((\\d+?),\\s*(\\d+?)\\)(\\.\\w+)*");
                Match matcher1 = pattern1.Match(attribute);

                /**
                 * Dg.Row(1)
                 */
                Regex pattern2 = new Regex("Row\\s*\\((\\d+?)\\)(\\.\\w+)*");
                Match matcher2 = pattern2.Match(attribute);

                if (matcher1.Success)
                {
                    re += ".Rows[";
                    string arg1Str = matcher1.Groups[1].Value;
                    string arg2Str = matcher1.Groups[2].Value;
                    re += Int32.Parse(arg1Str) + "].Cells[" + Int32.Parse(arg2Str) + "]";
                }
                else if (matcher2.Success)
                {
                    re += ".Rows[";
                    String arg1Str = matcher2.Groups[1].Value;
                    re += Int32.Parse(arg1Str) + "]";
                }
            }
            return re;
        }

        protected string getRanorexScriptCapture(string objectElement, string screenName,
                                            int testcaseId, string fileName)
        {
            fileName = fileName.Replace(".", "_");
            fileName += "_\" + System.DateTime.Now.ToString(\"HHmmss\") + \"";
            String testcase = "Testcase_" + testcaseId;
            String re =
                "System.IO.Directory.CreateDirectory(Config.getPathToCaptureImage() + \"\\\\" + screenName + "\\\\" + testcase + "\");" +
                "" + NEW_LINE +
                "Ranorex.Imaging.CaptureDesktopImage(" + objectElement + ").Save(@Config.getPathToCaptureImage() + \"\\\\" +
                screenName + "\\\\" + testcase + "\\\\" + fileName + ".jpg\");";
            return re;
        }

        protected string getAppFolderObjectElement(IElement element, string instanceName)
        {
            IElement element1 = element;
            while (element1 != null &&
                    !(element1 is AppFolderRanorexElement)) {
                element1 = element1.Parent;
            }
            return GetScriptAccessElement(element1, instanceName) + ".Self.Element";
        }

        protected string getValidation(string objectElement, SpecNode nodeAndAttribute, 
            string nameAttr, string value, string screenName, int scenarioId, string imageName)
        {
            //String re = RanorexScriptGeneration.FORMAT;
            string re = "";
            re += "try" + NEW_LINE +
                  "{" + NEW_LINE +
                  "Validate.Attribute(" + objectElement + ", \"" + nameAttr + "\", \"" + value + "\");" + NEW_LINE +
                  "}" + NEW_LINE +
                  "catch (Exception e)" + NEW_LINE + 
                  "{" + NEW_LINE;
            re += getRanorexScriptCapture(
                    getAppFolderObjectElement(nodeAndAttribute.UIElement),
                    screenName, scenarioId, nodeAndAttribute.GetNormalizedName());
            re += NEW_LINE + 
                "}";
            return re;
        }

        protected string getAppFolderObjectElement(IElement node)
        {
            IElement node1 = node;
            while (node1 != null && !(node1 is AppFolderRanorexElement)) {
                node1 = node1.Parent;
            }
            return GetScriptAccessElement(node1, REPO) + ".Self.Element";
        }

        public static string getRealAttribute(SpecNode nodeAndAttribute)
        {
            string attribute = nodeAndAttribute.Attribute;
            if (attribute == null || attribute.Equals(""))
                return "Text";
            if (Regex.IsMatch(attribute, "\\w+"))
            {
                return attribute;
            }
            else if (Regex.IsMatch(attribute, "Cell\\s*\\((\\d+?),\\s*(\\d+?)\\)") ||
                  Regex.IsMatch(attribute, "Row\\s*\\((\\d+?)\\)"))
            {
                return "Text";
            }
            else
            {
                int idx = attribute.LastIndexOf(".");
                if (idx < 0)
                {
                    return null;
                }
                return attribute.Substring(attribute.LastIndexOf(".") + 1);
            }
        }

        public static bool checkValidValueExpress(string expression)
        {
            return Regex.IsMatch(expression, SINGLE_QUOTE) || Regex.IsMatch(expression, LOCATION) ||
                    expression.Equals(MAXIMIZE) || expression.Equals(MINIMIZE) ||
                    expression.Equals(TRUE) || expression.Equals(FALSE);
        }

        /**
         * remove single quote
         * or remove <>
         * or no change with Maximized
         * @param input
         * @return
         */
        public static string formatValue(string input)
        {
            if (Regex.IsMatch(input, SINGLE_QUOTE))
                return input.Substring(1, input.Length - 2);
            else if (Regex.IsMatch(input, LOCATION))
            {
                Regex pattern = new Regex(LOCATION);
                Match matcher = pattern.Match(input);
                return matcher.Groups[1] + "," + matcher.Groups[2];
            }
            else if (input.Equals(MAXIMIZE) || input.Equals(MINIMIZE) ||
                      input.Equals(TRUE) || input.Equals(FALSE))
                return input;
            logger.Warn("don't know case " + input);
            return input;
        }
        #endregion
    }

    /// <summary>
    /// params for function gen-scripts
    /// </summary>
    
}
