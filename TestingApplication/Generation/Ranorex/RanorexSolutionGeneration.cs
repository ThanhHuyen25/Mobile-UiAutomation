// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 3:03 PM 2018/5/29
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class RanorexSolutionGeneration
    {
        public bool Generate(string repoFilePath, string folderOutputPath, string tempSolutionFolder, string tempProjectFolder,
            Dictionary<string, string> mapRunningTestFilesRelativePathAndIds, 
            List<string> otherFilesRelativePath, string AUTPath, MyLog myLog)
        {
            // copy and replace repo file
            File.Copy(repoFilePath, Path.Combine(tempProjectFolder, @"MyTestProjectRepository.rxrep"), true);
            //File.Delete(repoFilePath);

            // modify temp solution
            ModifyRanorexProjectProperties modifyRanorexProject = new ModifyRanorexProjectProperties();
            modifyRanorexProject.Modify(tempProjectFolder, mapRunningTestFilesRelativePathAndIds, otherFilesRelativePath, myLog);

            // copy to destination folder
            string folderSln = Path.Combine(folderOutputPath, new DirectoryInfo(tempSolutionFolder).Name);
            Directory.CreateDirectory(folderSln);
            Utils.CopyDirectory(tempSolutionFolder, folderSln);
            return true;
        }
    }
}
