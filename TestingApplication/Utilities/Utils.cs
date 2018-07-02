// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 4:18 PM 2018/1/10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using System.Windows.Automation;
using GUI_Testing_Automation;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO.Compression;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.Formatting;
//using Microsoft.CodeAnalysis.MSBuild;

namespace TestingApplication
{
    //all utilities function should be define here with static type
    public class Utils
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// copy all files and directories in @strSource folder to @strDestination folder
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strDestination"></param>
        public static void CopyDirectory(string strSource, string strDestination)
        {
            if (!Directory.Exists(strDestination))
            {
                Directory.CreateDirectory(strDestination);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(strSource);
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo tempfile in files)
            {
                tempfile.CopyTo(Path.Combine(strDestination, tempfile.Name));
            }

            DirectoryInfo[] directories = dirInfo.GetDirectories();
            foreach (DirectoryInfo tempdir in directories)
            {
                CopyDirectory(Path.Combine(strSource, tempdir.Name), Path.Combine(strDestination, tempdir.Name));
            }
        }

        public static string ReadFileContent(string path)
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// unzip @zipFilePath to @extractPath
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="extractPath"></param>
        /// <returns></returns>
        public static void ExtractToDirectory(string zipFilePath, string extractPath)
        {
            ExtractToDirectory(ZipFile.Open(zipFilePath, ZipArchiveMode.Update), extractPath, true);
        }

        public static string CreateUniqueDirAndExtract(string zipFilePath, string extractPath)
        {
            var fileInfo = new FileInfo(zipFilePath);
            string fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
            fileName += "_" + DateTime.Now.ToString("yyMMdd_HHmmss");
            string fullPath = Path.Combine(extractPath, fileName);
            logger.Debug("FullPath: " + fullPath);
            ExtractToDirectory(ZipFile.Open(zipFilePath, ZipArchiveMode.Update), fullPath, true);
            return Path.GetFullPath(new DirectoryInfo(fullPath).GetDirectories()[0].FullName);
        }

        public static void ExtractToDirectory(ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                if (file.Name == "")
                {// Assuming Empty for Directory
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="removeIfExist">indicate remove dir if it exists</param>
        /// <returns></returns>
        public static DirectoryInfo CreateDirectory(string dirPath, bool removeIfExist)
        {
            if (Directory.Exists(dirPath))
            {
                if (removeIfExist)
                    Directory.Delete(dirPath, true);
                else
                    return new DirectoryInfo(dirPath);
            }
            return Directory.CreateDirectory(dirPath);
        }

        public static DirectoryInfo CreateDirectoryForFilePath(string filePath)
        {
            return Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        public static CsprojFile SearchCsprojFile(IFile root)
        {
            List<IFile> files = SearchFile.Search(root, new CsprojSearchCondition());
            if (files == null | files.Count != 1)
            {
                logger.Error("get " + files.Count + " .csproj in project input: " + files);
                return null;
            }
            return (CsprojFile)files[0];
        }

        public static IFile SearchFilePath(IFile root, string path)
        {
            List<IFile> files = SearchFilesPath(root, path);
            if (files == null | files.Count == 0)
            {
                logger.Error("Cannot search file: " + path + " in " + root.Path +
                    "\nContinue try to search ignore case sensitive");
                List<IFile> newFiles = SearchFilesPathIgnoreCase(root, path);
                if (newFiles == null | newFiles.Count == 0)
                {
                    logger.Error("Cannot search file: " + path + " in " + root.Path +
                       "\n(regardless of ignore case sensitive");
                    return null;
                } return newFiles[0];
            }
            else
            {
                logger.Error("Get more than 1 file " + path + " in " + root.Path);
                return files[0];
            }
        }

        public static List<IFile> SearchFilesPath(IFile root, string path)
        {
            var condition = new FilePathCondition();
            condition.Path = path;
            return SearchFile.Search(root, condition);
        }

        /// <summary>
        /// search file with path, but ignore case sensitive
        /// </summary>
        /// <param name="root"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<IFile> SearchFilesPathIgnoreCase(IFile root, string path)
        {
            var condition = new FilePathCondition();
            condition.IgnoreCase = true;
            condition.Path = path;
            return SearchFile.Search(root, condition);
        }

        public static void LogInfoAutoElement(log4net.ILog logger, AutomationElement element)
        {
            logger.Info("Element: " + element.Current.Name + ", id: " + element.Current.AutomationId +
                    ", type: " + element.Current.LocalizedControlType);
        }

        /// <summary>
        /// if file with @filePath was existed, remove it
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="removeParentDirIfExist"></param>
        public static void RemoveOldFile(string filePath, bool removeParentDirIfExist = false)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            CreateDirectory(new FileInfo(filePath).Directory.FullName, removeParentDirIfExist);
        }

        /// <summary>
        /// create temp file and return it's path 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string CreateTempFile(string filePath)
        {
            string fileNameMpExtension = Path.GetFileNameWithoutExtension(filePath);
            string fileExtension = Path.GetExtension(filePath);
            if (fileNameMpExtension == null)
                return null;
            var tempFolder = Path.GetTempPath();
            string newPath = Path.Combine(tempFolder,
                fileNameMpExtension + Guid.NewGuid() + fileExtension);
            File.Copy(filePath, newPath, true);
            return newPath;
        }

        public static string CreateTempFolder(string folderName)
        {
            string re = Path.Combine(Path.GetTempPath(), folderName);
            if (Directory.Exists(re))
                Directory.Delete(re, true);
            Directory.CreateDirectory(re);
            return re;
        }

        public static string[] SplitIgnoreInside(string str, string pivot, string keyIgnore)
        {
            //sample pattern for split string by commas outide quote
            // ",(?=(?:[^']*'[^']*')*[^']*$)"
            return Regex.Split(str, pivot + "(?=(?:[^" + keyIgnore + "]*" + keyIgnore + "[^" + keyIgnore + "]*" + keyIgnore + ")*[^" + keyIgnore + "]*$)");
        }
        public static string ExceptionToString(Exception ex)
        {
            return "Message: " + ex.Message + "; StackTrace: " + ex.StackTrace;
        }

        public static List<ClassExpression> MergeClasses(List<ClassExpression> classes1, List<ClassExpression> classes2)
        {
            List<ClassExpression> re = new List<ClassExpression>();
            foreach(ClassExpression cl in classes1)
            {
                MergeClasses(re, cl);
            }
            foreach (ClassExpression cl in classes2)
            {
                MergeClasses(re, cl);
            }
            return re;
        }

        /// <summary>
        /// return class1
        /// </summary>
        /// <param name="base"></param>
        /// <param name="addition"></param>
        public static void MergeClasses2(List<ClassExpression> @base, List<ClassExpression> addition)
        {
            foreach (ClassExpression cl in addition)
            {
                MergeClasses(@base, cl);
            }
        }

        public static void MergeClasses(List<ClassExpression> classes, ClassExpression class2)
        {
            if (classes == null)
            {
                classes = new List<ClassExpression> { class2 };
                return;
            }
            if (class2 == null || class2.getName() == null)
                return;
            foreach (ClassExpression cl in classes)
            {
                // add new functions to existed class
                if (cl.getName() == class2.getName())
                {
                    var newListFunc = class2.getListFunction();
                    if (newListFunc != null && newListFunc.Count > 0)
                        foreach(var newFunc in newListFunc)
                            MergeFunctions(cl.getListFunction(), newFunc);
                    return;
                }
            }
            // add new class
            NormalizeClass(class2);
            classes.Add(class2);
        }

        public static void NormalizeClass(ClassExpression cl)
        {
            List<FunctionExpression> listFunc = cl.getListFunction();
            if (listFunc == null)
                return;
            List<FunctionExpression> newListFunc = new List<FunctionExpression>();
            foreach(FunctionExpression func in listFunc)
            {
                MergeFunctions(newListFunc, func);
            }
            cl.setListFunction(newListFunc);
        }

        public static void MergeFunctions(List<FunctionExpression> functions, FunctionExpression func2)
        {
            if (functions == null)
            {
                functions = new List<FunctionExpression> { func2 };
                return;
            }
            if (functions.Contains(func2))
                return;
            functions.Add(func2);
        }

        public static List<IScreen> ConvertSpecToNormal(List<SpecScreen> listScreens)
        {
            List<IScreen> re = new List<IScreen>();
            foreach (SpecScreen screen in listScreens)
                re.Add(screen);
            return re;
        }

        public static void MergeUcScriptsEpx(List<UserCodeScriptsExpression> listUcScriptsEpx, UserCodeScriptsExpression ucScriptsEpx)
        {
            var addition = ucScriptsEpx.MapClassAndFuncsAddition;
            foreach (UserCodeScriptsExpression existed in listUcScriptsEpx.ToList())
            {
                var @base = existed.MapClassAndFuncsAddition;
                MergeClassAndFuncPairs(@base, addition);
            }
        }

        public static void MergeUcScriptsEpx(List<UserCodeScriptsExpression> @base,
            List<UserCodeScriptsExpression> addition)
        {
            foreach (UserCodeScriptsExpression temp in addition.ToList())
            {
                MergeUcScriptsEpx(@base, temp);
            }
        }

        public static void MergeClassAndFuncPairs(Dictionary<string, List<FunctionExpression>> @base, 
            Dictionary<string, List<FunctionExpression>> addition)
        {
            foreach(var element in addition)
            {
                if (@base.ContainsKey(element.Key))
                {
                    foreach(var newFunc in element.Value.ToList())
                    {
                        if (@base[element.Key].Contains(newFunc))
                            element.Value.Remove(newFunc);
                    }
                }
            }
        }

        public static void MergeClassesExpression(List<ClassExpression> @base, List<ClassExpression> addition)
        {
            foreach (ClassExpression newClass in addition)
                MergeClassesExpression(@base, newClass);
        }

        public static void MergeClassesExpression(List<ClassExpression> @base, ClassExpression addition)
        {
            foreach(ClassExpression existed in @base.ToList())
            {
                if (existed.getName() == addition.getName())
                {
                    List<FunctionExpression> newListFuncs = addition.getListFunction();
                    if (newListFuncs != null)
                        foreach (FunctionExpression newFunc in newListFuncs.ToList())
                        {
                            if (newFunc != null)
                                MergeFunctions(existed.getListFunction(), newFunc);
                        }
                    return;
                }
            }
            if (addition != null)
                @base.Add(addition);
        }

        public static string NormalizePathCmd(string path)
        {
            if (Regex.IsMatch(path, "^\".*\"$"))
                return path;
            return "\"" + path + "\"";
        }

        public static IElement SearchIElement(string name, ListUIElements listUIElements)
        {
            if (listUIElements.Indicator == ListElementsIndicator.AllElements)
                return listUIElements.Elements.FirstOrDefault(ele => ele.Attributes.Name.Equals(name));
            foreach (IElement element in listUIElements.Elements)
            {
                IElement temp = DoSearchElementRecursive(name, element);
                if (temp != null)
                    return temp;
            }
            return null;
        }

        public static IElement DoSearchElementRecursive(string name, IElement element)
        {
            if (element.Attributes != null && element.Attributes.Name.Equals(name))
                return element;
            if (element.Children != null && element.Children.Count > 0)
                foreach(IElement child in element.Children)
                {
                    IElement e = DoSearchElementRecursive(name, child);
                    if (e != null)
                        return e;
                }
            return null;
        }

        /// <summary>
        /// re-format code
        /// </summary>
        /// <param name="csCode"></param>
        /// <returns></returns>
        public static string ReformatCsCode(string csCode)
        {
            var tree = CSharpSyntaxTree.ParseText(csCode);
            var root = tree.GetRoot().NormalizeWhitespace();
            var ret = root.ToFullString();
            return ret;
        }
    }
}
