// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @ duongtd
// created on 9:18 AM 2017/10/5
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI_Testing_Automation;

namespace TestingApplication
{
    public class ProjectGeneration:IProjectGeneration
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string DEFAULT_CS_OUTPUT_FILE = @"..\..\..\ProjectGenTemplate\ElementsDefinition.cs";
        public const string DEFAULT_CLASS_NAME = "ElementsDefinition";

        public const string DEFINITION = "Definition";
        public const string REPO = "Repo.xml";
        public const string IMAGE_CAPTURE = "ImageCapture.xml";
        public const string INSTANCE = "Instance";
        public const string RUNNING_TEST_FILE_NAME = "testcase.ls";

        public const string REPOSITORY = "Repository";
        public const string PROJECT_NAME = "MyTestProject";
        public const string PROJECT_TEMPLATE_PATH =
            //@"Resources\MyTestProject.zip";
            @"D:\Research\projects\GUI-Testing-Automation\TestingApplication\bin\Debug\Resources\MyTestProject.zip";

        public bool Generate(List<IElement> listRootElements, string folderOutPath, string projectName)
        {
            return Generate(listRootElements, folderOutPath, projectName, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRootElements"></param>
        /// <param name="folderOutPath"></param>
        /// <param name="projectName"></param>
        /// <param name="specExcelFilePath"></param>
        /// <returns></returns>
        public bool Generate(List<IElement> listRootElements, string folderOutPath, string projectName,
            string specExcelFilePath, string appPath)
        {
            MyLog myLog = new MyLog();
            List<SpecScreen> specScreens = null;
            if (specExcelFilePath != null && appPath != null)
            {
                ExcelSpecificationParser excelSpecificationParser = new ExcelSpecificationParser();
                specScreens = excelSpecificationParser.ParseWithRootElements(
                   specExcelFilePath, listRootElements, myLog);

                UserActionSpecAnalyzer specAnalyzer = new UserActionSpecAnalyzer();
                specAnalyzer.Expand(Utils.ConvertSpecToNormal(specScreens), appPath, myLog);
            }
            return Generate(listRootElements, specScreens, folderOutPath, projectName, appPath, myLog);
        }

        public bool Generate(List<IElement> listRootElements, List<SpecScreen> screensExpanded,
            string folderOutPath, string projectName, string appPath, MyLog myLog)
        {
            string tempFolderPath = Utils.CreateTempFolder(projectName + "_" +
                DateTime.Now.ToString("yyMMdd_HHmmss"));
            // generate CSharp code
            ElementCSharpCodeGeneration elementCSharpCodeGeneration = new ElementCSharpCodeGeneration();
            string repoClassName = projectName + DEFINITION;
            FileInfo csFile = elementCSharpCodeGeneration.Generate(
                listRootElements, tempFolderPath, repoClassName, projectName);
            if (csFile == null)
            {
                myLog.Error("An error occured when generating C# codes");
                return false;
            }
            // generate .xml file(s)
            ElementXmlGeneration elementXmlGeneration = new ElementXmlGeneration();
            string repoFileName = projectName + REPO;
            string imgCapFileName = projectName + IMAGE_CAPTURE;
            List<FileInfo> listXmlFile = elementXmlGeneration.Store(listRootElements,
                Path.Combine(tempFolderPath, repoFileName),
                Path.Combine(tempFolderPath, imgCapFileName));
            if (listXmlFile == null)
            {
                myLog.Error("An error occured when generating .xml files");
                return false;
            }

            // generate scripts
            Tuple<Dictionary<string, string>, List<string>> pair = null;
            if (screensExpanded != null && screensExpanded.Count > 0)
            {
                CSharpScriptsGeneration scriptsGeneration = new CSharpScriptsGeneration();
                pair = scriptsGeneration.Generate(
                    screensExpanded,
                    tempFolderPath,
                    repoFileName,
                    imgCapFileName,
                    appPath,
                    repoClassName,
                    projectName,
                    INSTANCE,
                    myLog);
                if (pair == null)
                {
                    myLog.Error("An error occured when generating C# scripts");
                    return false;
                }
            }

            // generate visual studio solution
            List<FileInfo> listFiles = new List<FileInfo>();
            listFiles.Add(csFile);
            listFiles.AddRange(listXmlFile);

            //generate text file contain all running class
            if (pair != null)
            {
                File.WriteAllText(Path.Combine(tempFolderPath, RUNNING_TEST_FILE_NAME),
                   string.Join(CSharpScriptsGeneration.NEW_LINE, pair.Item1));
                pair.Item2.Add(RUNNING_TEST_FILE_NAME);
            }

            VsSolutionGeneration gen = new VsSolutionGeneration();
            bool vsGenCheck = gen.Generate(listFiles, projectName, folderOutPath,
                // read from user's settings
                Properties.Settings.Default.path_GUI_Testing_Automation_ref,
                Properties.Settings.Default.path_to_vstemplate_output_proj,
                tempFolderPath,
                pair?.Item1,
                pair?.Item2,
                appPath);
            if (!vsGenCheck)
            {
                myLog.Error("An error occured when generating Visual Studio project");
                return false;
            }
            return true;
        }

        public bool GenerateRanorexProject(List<IElement> listRootElements, List<SpecScreen> screensExpanded,
            string repoFilePath, string folderOutPath, string appPath, MyLog myLog)
        {
            string tempFolderPath = Utils.CreateTempFolder(PROJECT_NAME + "_" +
                DateTime.Now.ToString("yyMMdd_HHmmss"));
            ZipFile.ExtractToDirectory(PROJECT_TEMPLATE_PATH, tempFolderPath);

            string projectTempFolder = Path.Combine(tempFolderPath, PROJECT_NAME);
            
            // generate scripts
            Tuple<Dictionary<string, string>, List<string>> pair = null;
            if (screensExpanded != null && screensExpanded.Count > 0)
            {
                RanorexScriptsGeneration scriptsGeneration = new RanorexScriptsGeneration();
                pair = scriptsGeneration.Generate(
                    screensExpanded,
                    projectTempFolder,
                    null,
                    null,
                    appPath,
                    PROJECT_NAME + REPOSITORY,
                    PROJECT_NAME,
                    INSTANCE,
                    myLog,
                    "repo");
                if (pair == null)
                {
                    myLog.Error("An error occured when generating C# scripts");
                    return false;
                }
            }
            
            RanorexSolutionGeneration gen = new RanorexSolutionGeneration();
            bool rGenCheck = gen.Generate(repoFilePath, folderOutPath, tempFolderPath, projectTempFolder,
                pair?.Item1,
                pair?.Item2,
                appPath,
                myLog);
            if (!rGenCheck)
            {
                myLog.Error("An error occured when generating Ranorex project");
                return false;
            }
            return true;
        }
    }
}