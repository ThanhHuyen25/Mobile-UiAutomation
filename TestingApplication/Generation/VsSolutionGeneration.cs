using EnvDTE;
using EnvDTE80;
using GUI_Testing_Automation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TestingApplication
{
    public class VsSolutionGeneration
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private delegate void ShowPopupMessage();
        /// <summary>
        /// generate new Visual Studio solution and add files
        /// </summary>
        /// <param name="files"> list of files to add </param>
        /// <param name="projName">solution name = project name</param>
        /// <param name="folderOutputPath">folder to put solution</param>
        /// <param name="path_to_GUI_Testing_Automation_ref">path to GUI_Testing_Automation reference</param>
        /// <param name="path_to_vstemplate_output_proj">path to file.vstemplate of template proj</param>
        /// <param name="AUTPath">path to app under test</param>
        public bool Generate(List<FileInfo> files, string projName, string folderOutputPath
            , string path_to_GUI_Testing_Automation_ref
            , string path_to_vstemplate_output_proj
            , string tempProjectFolder
            , Dictionary<string, string> mapRunningTestFilesRelativePathAndIds
            , List<string> otherFilesRelativePath
            , string AUTPath)
        {            
            //System.Threading.Tasks.Task.Factory.StartNew(() =>
                return RunLongProcess(files, projName, folderOutputPath
                    , path_to_GUI_Testing_Automation_ref
                    , path_to_vstemplate_output_proj
                    , tempProjectFolder
                    , new List<string>(mapRunningTestFilesRelativePathAndIds.Keys)
                    , otherFilesRelativePath
                    , AUTPath);
        }

        /// <summary>
        /// create solution and output proj in other thread
        /// solution name = project name
        /// </summary>        
        private bool RunLongProcess(List<FileInfo> files, String projName, String folderOutputPath
            , string path_to_GUI_Testing_Automation_ref
            , string path_to_vstemplate_output_proj
            , string tempProjectFolder
            , List<string> runningTestFilesRelativePath
            , List<string> otherFilesRelativePath
            , string AUTPath)
        {
            EnvDTE80.DTE2 dte2 = null;
            string folderSolution = folderOutputPath;
            string folderOutputProj = folderOutputPath + "\\" + projName;
            string solutionName = projName + ".sln";
            string outputProjName = projName;
            string pathTo_csproj = folderOutputProj + "\\" + outputProjName + ".csproj";
            //string pathToSolution = folderSolution + "\\" + solutionName;

            //get solution2 as part of visual studio
            string versionDTE = "VisualStudio.DTE.15.0";
            System.Type dteType = System.Type.GetTypeFromProgID(versionDTE, true);
            dte2 = (EnvDTE80.DTE2)System.Activator.CreateInstance(dteType, true);

            MessageFilter.Register();

            Solution2 sol = (Solution2)dte2.Solution;
            
            //delete the old folder_solution if it exists.
            if (Directory.Exists(folderSolution))
            {
                Directory.Delete(folderSolution, true);
            }
            //create a new folder_solution
            Directory.CreateDirectory(folderSolution);

            //create a solution that locates in the above solution
            sol.Create(folderSolution, solutionName);

            //create a new folder_output_project
            Directory.CreateDirectory(folderOutputProj);

            Project outputProj = sol.AddFromTemplate(path_to_vstemplate_output_proj
            , folderOutputProj, outputProjName, false);

            //save the solution
            sol.SaveAs(solutionName);
            // for kill process
            dte2.Quit();
            MessageFilter.Revoke();
            var t1 = DateTime.Now;
            while (dte2 != null && (DateTime.Now - t1).TotalSeconds < 5)
            {
                //dte2.
                try
                {
                    dte2.Quit();
                }
                catch (Exception)
                { }
                System.Threading.Thread.Sleep(1000);
            }
            ModifyProject(files, folderOutputProj, runningTestFilesRelativePath, otherFilesRelativePath,
                tempProjectFolder, pathTo_csproj, path_to_GUI_Testing_Automation_ref, projName, AUTPath);
            return true;
        }

        private void ModifyProject(List<FileInfo> files, string folderOutputProj, List<string> runningTestFilesRelativePath
            , List<string> otherFilesRelativePath, string tempProjectFolder, string pathTo_csproj, 
            string path_to_GUI_Testing_Automation_ref, string projName, string AUTPath)
        {
            //store name of files
            List<string> listNameFile = new List<string>();

            //copy already files to output proj 
            foreach (FileInfo item in files)
            {
                int indexLastSlash = item.FullName.LastIndexOf("\\");
                string nameItem = item.FullName.Substring(indexLastSlash + 1);
                string desLocation = Path.Combine(folderOutputProj, nameItem);
                File.Copy(item.FullName, desLocation, true);

                //add new name file
                listNameFile.Add(nameItem);
            }
            AddFiles(runningTestFilesRelativePath, listNameFile, folderOutputProj, tempProjectFolder);
            AddFiles(otherFilesRelativePath, listNameFile, folderOutputProj, tempProjectFolder);

            var contentFile = XDocument.Load(pathTo_csproj);
            
            //edit .csproj to add item into the output project
            XmlDocument doc = new XmlDocument();
            doc.Load(contentFile.CreateReader());

            XmlElement refGroup = doc.CreateElement("ItemGroup");
            XmlElement refTag = doc.CreateElement("Reference");
            refTag.InnerXml = "<HintPath>" + path_to_GUI_Testing_Automation_ref + "</HintPath>";
            refGroup.AppendChild(refTag);
            var attribute = doc.CreateAttribute("Include");
            attribute.Value = Path.GetFileNameWithoutExtension(path_to_GUI_Testing_Automation_ref);
            refTag.Attributes.Append(attribute);
            doc.FirstChild.AppendChild(refGroup);

            foreach (string nameFile in listNameFile)
            {
                XmlAttribute itemAttr = doc.CreateAttribute("Include");
                //string reNameFile = nameFile.Substring(1);
                itemAttr.Value = nameFile;

                XmlElement itemContent;
                if (nameFile.EndsWith(".cs"))
                {
                    itemContent = doc.CreateElement("Compile");
                }
                else if (nameFile.EndsWith(".xml") || nameFile.EndsWith(".ls"))
                {
                    itemContent = doc.CreateElement("EmbeddedResource");
                    itemContent.InnerXml = "<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>";
                }
                else
                {
                    itemContent = doc.CreateElement("Content");
                }
                itemContent.Attributes.Append(itemAttr);
                XmlElement itemGroup = doc.CreateElement("ItemGroup");
                itemGroup.AppendChild(itemContent);
                doc.FirstChild.AppendChild(itemGroup);
            }
            // add by @duongtd 18/03/16
            // define default namespace
            XmlElement propertyGroup = doc.CreateElement("PropertyGroup");
            XmlElement rootNamespace = doc.CreateElement("RootNamespace");
            rootNamespace.InnerText = projName;
            propertyGroup.AppendChild(rootNamespace);
            doc.FirstChild.AppendChild(propertyGroup);

            //save the edited .csproj file                
            string text = doc.OuterXml.Replace(" xmlns=\"\"", "");
            XDocument docText = XDocument.Parse(text);
            docText.Save(pathTo_csproj);

            // modify Config file
            string configContent = File.ReadAllText(Path.Combine(folderOutputProj, "Config.cs"));
            configContent = configContent.Replace("ReplaceNamespace", projName)
                .Replace("ReplaceAUTPath", AUTPath ?? "");
            File.WriteAllText(Path.Combine(folderOutputProj, "Config.cs"), configContent);
        }

        private void AddFiles(List<string> listRelativeFilesPath, List<string> listNameFile, string folderOutputProj, string tempProjectFolder)
        {
            // add by duongtd 180203
            if (listRelativeFilesPath != null)
            {
                listNameFile.AddRange(listRelativeFilesPath);
                foreach (string relativeFilePath in listRelativeFilesPath)
                {
                    var dir = Utils.CreateDirectoryForFilePath(Path.Combine(folderOutputProj, relativeFilePath));
                    int tempIndex = relativeFilePath.LastIndexOf(@"\");
                    if (tempIndex < 0)
                        tempIndex = 0;
                    else
                        tempIndex++;
                    File.Copy(Path.Combine(tempProjectFolder, relativeFilePath),
                        Path.Combine(dir.FullName, relativeFilePath.Substring(tempIndex)), true);
                }
            }
        }
    }

    /// <summary>
    /// Class containing the IOleMessageFilter
    /// thread error-handling functions.
    /// </summary>
    internal class MessageFilter : IOleMessageFilter
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Start the filter.
        public static void Register()
        {
            IOleMessageFilter newFilter = new MessageFilter();
            IOleMessageFilter oldFilter = null;
            CoRegisterMessageFilter(newFilter, out oldFilter);
        }

        // Done with the filter, close it.
        public static void Revoke()
        {
            IOleMessageFilter oldFilter = null;
            CoRegisterMessageFilter(null, out oldFilter);
            //logger
        }

        //
        // IOleMessageFilter functions.
        // Handle incoming thread requests.
        int IOleMessageFilter.HandleInComingCall(int dwCallType, System.IntPtr hTaskCaller, int dwTickCount, System.IntPtr lpInterfaceInfo)
        {
            return 0; //Return the flag SERVERCALL_ISHANDLED.
        }

        // Thread call was rejected, so try again.
        int IOleMessageFilter.RetryRejectedCall(System.IntPtr hTaskCallee, int dwTickCount, int dwRejectType)
        {
            if (dwRejectType == 2)
            // flag = SERVERCALL_RETRYLATER.
            {
                return 99; // Retry the thread call immediately if return >=0 & <100.
            }
            return -1; // Too busy; cancel call.
        }

        int IOleMessageFilter.MessagePending(System.IntPtr hTaskCallee, int dwTickCount, int dwPendingType)
        {
            //Return the flag PENDINGMSG_WAITDEFPROCESS.
            return 2;
        }

        // Implement the IOleMessageFilter interface.
        [DllImport("Ole32.dll")]
        private static extern int CoRegisterMessageFilter(IOleMessageFilter newFilter, out IOleMessageFilter oldFilter);
    }

    [ComImport(), Guid("00000016-0000-0000-C000-000000000046"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    interface IOleMessageFilter
    {
        [PreserveSig]
        int HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo);
        [PreserveSig]
        int RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType);
        [PreserveSig]
        int MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType);
    }
}
