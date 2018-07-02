// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 8:31 PM 2018/5/27
using stdole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class ModifyRanorexProjectProperties
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string CSPROJ_ADD = "#Compile_add_here";
        private const string RXTST_ADD_1 = "#testcase_add_here_1";
        private const string RXTST_ADD_2 = "#testcase_add_here_2";
        private const string RXTST_ADD_3 = "#testcase_add_here_3";

        private const string TAB = "    ";
        private const string TAB3 = TAB + TAB + TAB;
        private const string TAB5 = TAB + TAB + TAB + TAB + TAB;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectFolderPath">MyTestProject_180529_163316\MyTestProject</param>
        /// <param name="mapFilesAndIds"></param>
        /// <param name="myLog"></param>
        /// <returns></returns>
        public bool Modify(string projectFolderPath, Dictionary<string, string> mapFilesAndIds,
            List<string> otherFilesRelativePath, MyLog myLog)
        {
            List<DirectoryInfo> listFolders = new List<DirectoryInfo>();
            List<string> listTestIds = new List<string>();
            List<FileInfo> listCsFiles = new List<FileInfo>();
            foreach (KeyValuePair<string, string> pair in mapFilesAndIds)
            {
                string filePath = pair.Key;
                string id = pair.Value;
                FileInfo file = new FileInfo(Path.Combine(projectFolderPath, filePath));
                listCsFiles.Add(file);
                DirectoryInfo folder = file.Directory;
                if (listFolders.FirstOrDefault(ele => ele.FullName.Equals(folder.FullName)) == null)
                    listFolders.Add(folder);
                listTestIds.Add(id);
            }

            string csProjFilePath = SearchCsProjFilePath(projectFolderPath);
            string rxtstFilePath = SearchRxtstFilePath(projectFolderPath);

            if (csProjFilePath == null || rxtstFilePath == null)
            {
                myLog.Error("An error occured when generating project, exit!");
                return false;
            }
            ModifyCsProjNode(csProjFilePath, mapFilesAndIds, otherFilesRelativePath);
            ModifyRxtstNode(rxtstFilePath, listFolders, listCsFiles, listTestIds);
            return true;
        }

        protected string SearchCsProjFilePath(string slnFolderPath)
        {
            var listFiles = new DirectoryInfo(slnFolderPath).GetFiles("*.csproj");
            if (listFiles == null || listFiles.Length != 1)
            {
                logger.Error("Not found .csproj file");
               return null;
            }
            return listFiles[0].FullName;
        }

        protected string SearchRxtstFilePath(string slnFolderPath)
        {
            var listFiles = new DirectoryInfo(slnFolderPath).GetFiles("*.rxtst");
            if (listFiles == null || listFiles.Length != 1)
            {
                logger.Error("Not found .rxtst file");
                return null;
            }
            return listFiles[0].FullName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="csprojFilePath">absolute path</param>
        /// <param name="listCsFiles">relative paths</param>
        private void ModifyCsProjNode(string csprojFilePath, Dictionary<string, string> mapListCsFilesAndIds,
            List<string> otherFilesRelativePath)
        {
            string replace = "";
            foreach (string csFilePath in mapListCsFilesAndIds.Keys) {
                //string csFilePath1 = !csFilePath.EndsWith(".cs") ? csFilePath :
                    //csFilePath.Substring(0, csFilePath.Length - ".cs".Length);
                replace += TAB + "<Compile Include=\"" + csFilePath + "\">\n";
                replace += TAB + TAB + "<id>" + Guid.NewGuid().ToString() + "</id>\n";
                replace += TAB + "</Compile>\n";
            }
            foreach (string otherFilePath in otherFilesRelativePath)
            {
                replace += TAB + "<Compile Include=\"" + otherFilePath + "\">\n";
                replace += TAB + TAB + "<id>" + Guid.NewGuid().ToString() + "</id>\n";
                replace += TAB + "</Compile>\n";
            }

            // replace
            string fileContent = File.ReadAllText(csprojFilePath);
            fileContent = fileContent.Replace(CSPROJ_ADD, replace);
            File.WriteAllText(csprojFilePath, fileContent);
        }

        private void ModifyRxtstNode(string rxtstFilePath, List<DirectoryInfo> listFolders,
                                 List<FileInfo> listCsFiles, List<string> listTestId6)
        {
            string[] foldersId = new string[listFolders.Count];
            List<string> id1s = new List<string>();
            List<string> id2s = new List<string>();
            string replace1 = "";
            string replace2 = "";
            string replace3 = "";

            for (int fi = 0; fi < listFolders.Count; fi++ ) {
                DirectoryInfo folder = listFolders[fi];
                string folderName = folder.Name;
                foldersId[fi] = Guid.NewGuid().ToString();
                //<flatlistofchildren>
                replace1 +=
                            TAB3 + "<smartfoldernode\n" +
                            TAB3 + "id=\"" + foldersId[fi] + "\"\n" +
                            TAB3 + "name=\"" + folderName + "\">\n" +
                            TAB3 + "</smartfoldernode>\n";

                //<childhierarchy>
                replace2 +=
                        TAB5 + "<smartfoldernode\n" +
                        TAB5 + "id=\"" + foldersId[fi] + "\"\n" +
                        TAB5 + "name=\"" + folderName + "\">\n";
                //declare all testcase and testmodule (cs file) in this folder
                foreach (FileInfo file in folder.GetFiles()) {
                    string csFileName = file.Name;
                    string fileNameWithoutExtension = csFileName.Substring(0, csFileName.LastIndexOf("."));
                    string id1 = Guid.NewGuid().ToString();
                    string id2 = Guid.NewGuid().ToString();
                    id1s.Add(id1);
                    id2s.Add(id2);

                    replace2 += TAB5 + TAB + "<testcase\n" +
                                TAB5 + TAB + "name=\"" + fileNameWithoutExtension + "\"\n" +
                                TAB5 + TAB + "id=\"" + id1 + "\">\n";
                    replace2 +=
                                TAB5 + TAB + TAB + "<testmodule\n" +
                                TAB5 + TAB + TAB + "name=\"" + fileNameWithoutExtension + "\"\n" +
                                TAB5 + TAB + TAB + "id=\"" + id2 + "\"/>\n";
                    replace2 += TAB5 + TAB + "</testcase>\n";
                }
                replace2 += TAB5 + "</smartfoldernode>\n";

                replace3 += TAB3 + "<testcontainer\n" +
                        TAB3 + TAB + "id=\"" + foldersId[fi] + "\">\n" +
                        TAB3 + "</testcontainer>\n";
            }

            for (int fi = 0; fi<listCsFiles.Count; fi++) {
                string csFileName = listCsFiles[fi].Name;
                string testId = listTestId6[fi];
                string fileNameWithoutExtension = csFileName.Substring(0, csFileName.LastIndexOf("."));
                string id1 = id1s[fi];
                string id2 = id2s[fi];

                replace1 += TAB3 + "<testcase\n";
                replace1 += TAB3 + "name=\"" + fileNameWithoutExtension + "\"\n";
                replace1 += TAB3 + "id=\"" + id1 + "\">\n";
                replace1 += TAB3 + "</testcase>" + "\n";
                replace1 += TAB3 + "<testmodule" + "\n";
                replace1 += TAB3 + "name=\"" + fileNameWithoutExtension + "\"\n";
                replace1 += TAB3 + "id=\"" + id2 + "\"\n";
                replace1 += TAB3 + "ref=\"" + testId + "\"\n";
                replace1 += TAB3 + "type=\"Recording\"/>" + "\n";

                replace3 += TAB3 + "<testcontainer\n";
                replace3 += TAB3 + TAB + "id=\"" + id1 + "\">\n";
                replace3 += TAB3 + "</testcontainer>\n";
            }

            /**
             * replace
             */
            string fileContent = File.ReadAllText(rxtstFilePath);
            fileContent = fileContent.Replace(RXTST_ADD_1, replace1);
            fileContent = fileContent.Replace(RXTST_ADD_2, replace2);
            fileContent = fileContent.Replace(RXTST_ADD_3, replace3);
            File.WriteAllText(rxtstFilePath, fileContent);
        }
    }
}
