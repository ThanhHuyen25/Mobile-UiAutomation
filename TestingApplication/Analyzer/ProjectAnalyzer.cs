// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:30 PM 2017/11/26
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class ProjectAnalyzer : IProjectAnalyser
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string NEW_LINE = "\r\n";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputProject"></param>
        /// <returns> path to .exe file </returns>
        public Tuple<string, LogProcess> Process(IProject inputProject)
        {
            List<IFile> listCsFiles = SearchFile.Search(inputProject.RootFolder,
                new CsFileSearchCondition());
            List<CsClass> screens = new List<CsClass>();

            List<CsFile> listCsFilesNeedToModify = new List<CsFile>();

            foreach (IFile file in listCsFiles)
            {
                CsFileAnalyzer csFileAnalyzer = new CsFileAnalyzer();
                CsFile csFile = (CsFile)file;
                csFileAnalyzer.CsFile = csFile;
                csFileAnalyzer.Process();
                foreach (CsClass csClass in csFile.Classes)
                {
                    if (csClass.BaseOnClass != null && (
                        csClass.BaseOnClass.Contains(CsClass.WINDOW_CLASS) |
                        csClass.BaseOnClass.Contains(CsClass.FORM_CLASS)))
                    {
                        screens.Add(csClass);
                        if (!listCsFilesNeedToModify.Contains(file))
                            listCsFilesNeedToModify.Add((CsFile)file);
                    }
                }
            }
            
            ModifyProject(inputProject, screens, listCsFilesNeedToModify);
            return RebuildProject(inputProject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputProject"></param>
        /// <param name="screens"></param>
        /// <param name="listCsFiles"> list of .cs file need to modify</param>
        /// <returns></returns>
        private bool ModifyProject(IProject inputProject, List<CsClass> screens, List<CsFile> listCsFiles)
        {
            ProjectModifier projectModifier = new ProjectModifier();

            //modify project in order to visible all screens at start up
            return projectModifier.ModifyListCsFile(listCsFiles, screens);
        }

        /// <summary>
        /// Rebuild project after modifing
        /// </summary>
        /// <param name="inputProject"></param>
        /// <returns> path to .exe file </returns>
        private Tuple<string, LogProcess> RebuildProject(IProject inputProject)
        {
            CsprojFile csprojFile = Utils.SearchCsprojFile(inputProject.RootFolder);

            //rebuild project
            ProjectRebuilding projectRebuilding = new ProjectRebuilding();
            LogProcess logBuilding = projectRebuilding.Compile(csprojFile.Path);
            if (logBuilding.Output != null)
                File.AppendAllText(@"..\..\Log Output\building.log", logBuilding.Output + NEW_LINE);
            if (logBuilding.Error != null)
                File.AppendAllText(@"..\..\Log Output\building.log", logBuilding.Error + NEW_LINE);
            if (logBuilding.Error_count > 0 ||
                (logBuilding.Error != null && logBuilding.Error.Trim() != ""))
            {
                logger.Error("--Rebuild project fail: " + logBuilding.Error);
                return new Tuple<string, LogProcess>(null, logBuilding);
            }

            string output = inputProject.RootFolder.Path + @"\bin\Debug";

            logger.Debug("Output path: " + output);
            ExeFile exeFile = FindExeFile(output);
            logger.Debug("Modified, Built successfully" + NEW_LINE +
                "Get .exe file path: " + exeFile.Path);

            return new Tuple<string, LogProcess>(exeFile.Path, logBuilding);
        }

        public ExeFile FindExeFile(string folderOuput)
        {
            string [] paths = Directory.GetFiles(folderOuput, "*.exe", SearchOption.AllDirectories);
            List<string> filtered = new List<string>();
            foreach (string path in paths)
                if (!path.EndsWith(".vshost.exe"))
                    filtered.Add(path);
            if (filtered.Count == 0)
                throw new Exception("Building fail, not found .exe file in output folder: " + folderOuput);
            if (filtered.Count > 1)
                logger.Error("Found more than 1 .exe file in output folder: " + folderOuput + 
                    "\nTake the first .exe file by default");
            return new ExeFile(filtered[0]);
        }
    }
}
