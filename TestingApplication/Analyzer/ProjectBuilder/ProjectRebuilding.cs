// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 5:38 PM 2017/11/22
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace TestingApplication
{
    /// <summary>
    /// represent for building project after modifing
    /// </summary>
    public class ProjectRebuilding
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectfilePath"> path to .csproj file</param>
        /// <returns></returns>
        public LogProcess Compile(string projectFilePath)
        {
            return Compile(projectFilePath, Properties.Settings.Default.vs_msbuild_path, Properties.Settings.Default.nuget_path);
        }

        public LogProcess Compile(string projectFilePath, string msBuildToolPath, string file2AppendLog, string nugetPath)
        {
            LogProcess logProcess = Compile(projectFilePath, msBuildToolPath, nugetPath);
            return logProcess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectFilePath"></param>
        /// <param name="msBuildToolPath"></param>
        /// <param name="nugetPath"></param>
        /// <returns></returns>
        public LogProcess Compile(string projectFilePath, string msBuildToolPath, string nugetPath)
        {
            msBuildToolPath = Utils.NormalizePathCmd(msBuildToolPath);
            nugetPath = Utils.NormalizePathCmd(nugetPath);

            LogProcess logOut = new LogProcess();

            // restore nuget packages
            Process process = new Process();
            process.StartInfo.FileName = nugetPath;
            string temp = "restore \"" +
                Path.Combine(Path.GetDirectoryName(projectFilePath), "packages.config") + "\"" +
                " -PackagesDirectory \"" + 
                Path.Combine(Path.GetDirectoryName(projectFilePath), "..\\packages") + "\"";
            process.StartInfo.Arguments = temp;
            //logger.Debug("------" + temp);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();
            //logger.Debug("----" + process.StandardOutput.ReadToEnd().Trim());

            // build project
            process = new Process();
            process.StartInfo.FileName = msBuildToolPath;
            process.StartInfo.Arguments = "\"" + projectFilePath + "\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            //* Read the output (or the error)
            string msg = process.StandardOutput.ReadToEnd().Trim();
            string errorStr = process.StandardError.ReadToEnd().Trim();
            logOut = ParseLogBuild(msg);
            logOut.appendOutput(msg);
            logOut.appendError(errorStr);
            process.WaitForExit();
            return logOut;
        }
        
        public LogProcess ParseLogBuild(string log)
        {
            string warning_pattern = @"(\d+)? Warning\(s\)";
            string error_pattern = @"(\d+)? Error\(s\)";
            LogProcess re = new LogProcess();
            var m_warning = Regex.Matches(log, warning_pattern);
            if (m_warning != null && m_warning.Count > 0)
            {
                re.Warning_count = Int32.Parse(m_warning[0].Groups[1].Value);
            }
            var m_error = Regex.Matches(log, error_pattern);
            if (m_error != null && m_error.Count > 0)
            {
                re.Error_count = Int32.Parse(m_error[0].Groups[1].Value);
            }
            return re;
        }
    }

    public class LogProcess
    {
        public const string NEW_LINE = "\r\n";

        private string output = "";
        private string error = "";
        private string warning = "";

        private int error_count = 0;
        private int warning_count = 0;
    
        public LogProcess()
        {
        }

        public LogProcess(string output, string error)
        {
            this.Output = output;
            this.Error = error;
        }

        public string Output
        {
            get { return output; }
            set { this.output = value; }
        }
        public string Error
        {
            get { return error; }
            set { this.error = value; }
        }

        public string Warning
        {
            get { return warning; }
            set { warning = value; }
        }

        public int Error_count
        {
            get { return error_count; }
            set { this.error_count = value; }
        }

        public int Warning_count
        {
            get { return warning_count; }
            set { warning_count = value; }
        }

        public void appendOutput(string addStr)
        {
            this.output += NEW_LINE + addStr;
        }

        public void appendError(string addStr)
        {
            this.error += NEW_LINE + addStr;
        }
    }
}
