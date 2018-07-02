// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:37 PM 2017/11/10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingApplication
{
    /// <summary>
    /// represent for analyzer .csproj file
    /// </summary>
    public class CsprojFileAnalyzer : AbstractFileAnalyzer
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        const int CHAR_ARR_MAX_SIZE = 100;
       
        public override bool Process()
        {
            return true;
        }

        public CsprojFile ParseFileContent(string filePath)
        {
            string fileContent = Utils.ReadFileContent(filePath);
            CsprojFile re = null;
            List<String> projectTypeGuids = FindProjectTypeGuids(fileContent);
            if (projectTypeGuids != null && projectTypeGuids.Contains(CsprojFile.WPF_PROJECT_TYPE_GUID))
            {
                re = new CsprojWpfFile(filePath);
                ((CsprojWpfFile)re).ProjectTypeGuids = projectTypeGuids;
                //ParseFileContentWpf((CsprojWpfFile)re, fileContent);
            } else
            {
                re = new CsprojWfaFile(filePath);
                //ParseFileContentWfa((CsprojWfaFile)re, fileContent);
            }
            return re;
        }

        public void ParseFileContentWpf(CsprojWpfFile csProjWpfFile, string fileContent)
        {
            string regex = "<Page\\s+Include\\s*=\\s*\"(.+)?\">";
            foreach (Match match in Regex.Matches(fileContent, regex))
            {
                int s = match.Groups.Count;
                string windowRelative = match.Groups[s - 1].Value;
                logger.Debug("windowRelative: " + windowRelative);
                string windowStr = windowRelative.Contains("\\") ?
                    windowRelative.Substring(windowRelative.LastIndexOf("\\") + 1) : windowRelative;
                string regex2 = "<Compile\\s+Include\\s*=\\s*\"(.+)?\">\\s*\n+" +
                                "\\s*<DependentUpon>" + windowStr + "</DependentUpon>";
                MatchCollection matchCollection = Regex.Matches(fileContent, regex2);
                if (matchCollection.Count == 0)
                {
                    logger.Error("Found Page, but not found corresponding .cs file: " + windowRelative);
                    continue;
                }
                string relativeCsFile = matchCollection[0].Groups[1].Value;
                if (csProjWpfFile.MapXamlCsFilesPath == null)
                    csProjWpfFile.MapXamlCsFilesPath = new Dictionary<string, string>();
                csProjWpfFile.MapXamlCsFilesPath.Add(
                    Path.Combine(Directory.GetParent(csProjWpfFile.Path).FullName, windowRelative),
                    Path.Combine(Directory.GetParent(csProjWpfFile.Path).FullName, relativeCsFile));
            }
            //string relativePath = FindApplicationDefinition(fileContent);
            //csProjWpfFile.ApplicationDefinion =
                //Path.Combine(Directory.GetParent(csProjWpfFile.Path).FullName, relativePath);
        }

        public void ParseFileContentWfa(CsprojWfaFile csProjWfaFile, string fileContent)
        {
            List<FormWfa> listForms = new List<FormWfa>();
            string regex = "<Compile\\s+Include\\s*=\\s*\"(.+)?\">\\s*\n+" +
                "\\s*<SubType>\\s*Form\\s*</SubType>\\s*";
            foreach (Match match in Regex.Matches(fileContent, regex))
            {
                int s = match.Groups.Count;
                string formRelative = match.Groups[s - 1].Value;
                logger.Debug("formRelative: " + formRelative);
                //string formStr = formRelative.Contains("\\") ?
                    //formRelative.Substring(formRelative.LastIndexOf("\\") + 1) : formRelative;
                if (csProjWfaFile.ListCsFilesScreen == null)
                    csProjWfaFile.ListCsFilesScreen = new List<string>();
                csProjWfaFile.ListCsFilesScreen.Add(
                    Path.Combine(new FileInfo(csProjWfaFile.Path).Directory.FullName, formRelative));
            }
        }

        /// <summary>
        /// find app.xaml
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns>relative path</returns>
        public string FindApplicationDefinition(string fileContent)
        {
            string regex = "<ApplicationDefinition\\s+Include\\s*=\\s*\"((\\w+|\\.|\\\\)+)?\".*";
            MatchCollection matches = Regex.Matches(fileContent, regex);
            if (matches.Count == 0)
                return null;
            if (matches.Count > 1)
                logger.Error("found more than one (" + matches.Count + ") ApplicationDefinition in .csproj file");
            return matches[0].Groups[1].Value;
        }

        /// <summary>
        /// find project type guid(s)
        /// </summary>
        /// <returns></returns>
        /// @author: @NguyenNm
        public List<string> FindProjectTypeGuids(string fileContent)
        {
            List<string> ptgString = null;
            string input = fileContent;
            string regex = "<ProjectTypeGuids>{(.*)?};{(.*)?}</ProjectTypeGuids>";

            foreach (Match matches in Regex.Matches(input, regex))
            {
                if (ptgString == null)
                    ptgString = new List<string>();
                string temp = matches.Groups[0].Value;
                ptgString.Add(temp);
                temp = matches.Groups[1].Value;
                ptgString.Add(temp);
            }
            return ptgString;
        }
    }
}
