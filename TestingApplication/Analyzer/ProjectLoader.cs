// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 5:38 PM 2017/11/22
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class ProjectLoader
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string BIN_FOLDER = "bin";
        public const string OBJ_FOLDER = "obj";
        public const string PROPERTIES_FOLDER = "Properties";
        public const string CONFIG_FILE_EXTENSION = ".config";
        public const string XAML_FILE_EXTENSION = ".xaml";
        public const string XAML_CS_FILE_EXTENSION = ".xaml.cs";
        public const string CS_FILE_EXTENSION = ".cs";
        public const string CSPROJ_FILE_EXTENSION = ".csproj";

        public static AbstractProject Load(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            Folder folderProj = new Folder(path, null);
            foreach (DirectoryInfo childrenDir in directoryInfo.GetDirectories())
                LoadDir(childrenDir, folderProj);
            foreach (FileInfo childFile in directoryInfo.GetFiles())
                LoadDir(childFile, folderProj);

            return HandleProject(folderProj);
        }

        private static void LoadDir(DirectoryInfo directoryInfo, IFile parentFile)
        {
            if (CheckIgnore(directoryInfo))
            {
                new UndefinedFile(directoryInfo.FullName, parentFile);
            }
            else
            {
                Folder currentFolder = new Folder(directoryInfo.FullName, parentFile);
                DirectoryInfo[] directories = directoryInfo.GetDirectories();
                foreach (DirectoryInfo directory in directories)
                {
                    Folder newFolder = new Folder(directory.FullName, currentFolder);
                    LoadDir(directory, newFolder);
                }
                FileInfo[] files = directoryInfo.GetFiles();
                foreach (FileInfo file in files)
                {
                    LoadDir(file, currentFolder);
                }
            }
        }

        private static bool CheckIgnore(DirectoryInfo directoryInfo)
        {
            switch (directoryInfo.Name)
            {
                case BIN_FOLDER:
                    return true;
                case OBJ_FOLDER:
                    return true;
                case PROPERTIES_FOLDER:
                    return true;
            }
            return false;
        }

        private static void LoadDir(FileInfo file, IFile parent)
        {
            string fileName = file.Name;
            if (fileName.EndsWith(CS_FILE_EXTENSION))
                new CsFile(file.FullName, parent);
            else if (fileName.EndsWith(XAML_FILE_EXTENSION))
                new XamlFile(file.FullName, parent);
            else if (fileName.EndsWith(CSPROJ_FILE_EXTENSION))
            {
                HandleCsProjFile(file.FullName, parent);
            }
            else if (fileName.EndsWith(CONFIG_FILE_EXTENSION))
                new ConfigFile(file.FullName, parent);
            //add new interested file type here

            else
                new UndefinedFile(file.FullName, parent);
        }

        private static AbstractProject HandleProject(Folder projFolder)
        {
            CsprojFile csprojFile = Utils.SearchCsprojFile(projFolder);

            // determine Wpf project or Windows Form Application
            AbstractProject inputProj;
            if (csprojFile is CsprojWpfFile)
                inputProj = new WpfProject();
            else
                inputProj = new WfaProject();
            inputProj.RootFolder = projFolder;
            return inputProj;
        }

        public static void HandleCsProjFile(string filePath, IFile parent)
        {
            CsprojFileAnalyzer csprojFileAnalyzer = new CsprojFileAnalyzer();
            CsprojFile csProjFile = csprojFileAnalyzer.ParseFileContent(filePath);

            csProjFile.Children = new List<IFile>();
            csProjFile.Parent = parent;
            if (parent != null)
            {
                if (parent.Children == null)
                    parent.Children = new List<IFile>();
                parent.Children.Add(csProjFile);
            }
        }
    }
}
