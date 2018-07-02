// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 3:53 PM 2018/5/26
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class RanorexScriptsGeneration : CSharpScriptsGeneration
    {
        //private static string NAMESPACE = "replace_namespace";
        private const string USERCODE_NAME = "replace_UsercodeName";
        private const string REPO = "replace_repo";
        private const string OPEN_APP_REPLACE = "#replace_open_app";
        private const string GUID = "replace_guid";
        private const string REPLACE_CLOSE_APP = "replace_close_app";
        private const string REPLACE_SCRIPT = "replace_script";
        private const string REPLACE_GLOBAL_STATEMENTS = "#replace_global_statement";

        private const string FILE_NAME = "test_script";

        private const string GUID_REPLACE = "#replace_guid";
        private const string INSTANCE_REPO_REPLACE = "#replace_repo_instance";
        private const string REPO_CLASSNAME_REPLACE = "#replace_repo_classname";

        private const string TEMP_RANOREX_FILE_CONTENT =
                "using System;" + NEW_LINE +
                "using System.Collections.Generic;" + NEW_LINE +
                "using System.Text;" + NEW_LINE +
                "using System.Text.RegularExpressions;" + NEW_LINE +
                "using System.Drawing;" + NEW_LINE +
                "using System.Threading;" + NEW_LINE +
                "using WinForms = System.Windows.Forms;" + NEW_LINE +
                NEW_LINE +
                "using Ranorex;" + NEW_LINE +
                "using Ranorex.Core;" + NEW_LINE +
                "using Ranorex.Core.Testing;" + NEW_LINE +
                "" + NEW_LINE +
                "namespace " + NAMESPACE_REPLACE + NEW_LINE +
                "{" + NEW_LINE +
                "}";

        public const string CLASS_RANOREX_TEMPLATE =
                "    [UserCodeCollection]" + NEW_LINE +
                "    [TestModule(\"" + GUID_REPLACE + "\", ModuleType.Recording, 1)]" + NEW_LINE +
                "    public partial class " + CLASS_REPLACE + " : ITestModule" + NEW_LINE +
                "    {" + NEW_LINE +
                //"        public static " + REPO_CLASSNAME_REPLACE + " " + INSTANCE_REPO_REPLACE + " = " + REPO_CLASSNAME_REPLACE + ".Instance;" + NEW_LINE +
                "        " + GLOBAL_REPLACE + "" + NEW_LINE +
                "        void ITestModule.Run()" + NEW_LINE +
                "        {" + NEW_LINE +
                "            Mouse.DefaultMoveTime = 300;" + NEW_LINE +
                "            Keyboard.DefaultKeyPressTime = 100;" + NEW_LINE +
                "            Delay.SpeedFactor = 1.00;" + NEW_LINE +
                             CONTENT_REPLACE + NEW_LINE +
                "        }" + NEW_LINE +
                "    }" + NEW_LINE;

        private const string CLASS_UC_RANOREX_TEMPLATE =
                "    /// <summary>" + NEW_LINE +
                "    /// Ranorex User Code collection. A collection is used to publish User Code methods to the User Code library." + NEW_LINE +
                "    /// </summary>" + NEW_LINE +
                "    [UserCodeCollection]" + NEW_LINE +
                "    public class " + CLASS_REPLACE + NEW_LINE +
                "    {" + NEW_LINE +
                "        " + CONTENT_REPLACE + NEW_LINE +
                "    }" + NEW_LINE;

        #region override functions
        protected override string GetTemplateRunCsFileContent()
        {
            return TEMP_RANOREX_FILE_CONTENT;
        }

        protected override string GetTemplateRunCsClassContent()
        {
            return CLASS_RANOREX_TEMPLATE;
        }

        protected override string GetTemplateUCClassContent()
        {
            return CLASS_UC_RANOREX_TEMPLATE;
        }
        protected override void GenerateOthers(MyLog myLog, string folderPath, string repoFilePath, string imageCapFilePath,
            string appFilePath, string classRepoName, string name_space, string mainClassInstance, string instanceName,
            Dictionary<string, string> mapFilePathAndId, List<string> otherFilePath)
        {
            // do nothing
        }

        protected override string InsertAnnotation()
        {
            return @"[UserCodeMethod]" + NEW_LINE;
        }

        protected override string GetDeclareStatementRepo(string classRepoName, string instanceName, string name_space, string mainClassInstance)
        {
            return "public static " + classRepoName + " " + instanceName + " = " + classRepoName + "." + mainClassInstance + ";";
        }
        #endregion

        /// <summary>
        /// if need to modify others
        /// </summary>
        /// <param name="content"></param>
        /// <returns>[0]: content modified</returns>
        protected override string ModifyOtherRunCsFileContent(ref string content, string className)
        {
            content = content.Replace(CLASS_REPLACE, className);
            string guid = Guid.NewGuid().ToString();
            content = content.Replace(GUID_REPLACE, guid);
            return guid;
        }

        /// <summary>
        /// choose Ranorex scripts
        /// </summary>
        /// <param name="specUserAction"></param>
        /// <returns></returns>
        protected override ScriptsExpression GenScriptType(AbstractSpecUserAction specUserAction, string instanceName)
        {
            var param = RanorexScriptGenerationParams.CloneFromNormal(specUserAction.Params);
                //specUserAction.Params.Clone();
            param.InstanceName = instanceName;
            return specUserAction.GenRanorexScripts(param);
        }
    }
}
