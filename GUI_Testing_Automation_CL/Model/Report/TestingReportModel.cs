// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:19 PM 2018/2/25
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Testing_Automation
{
    public class TestingReportModel
    {
        public static string file_path;

        private string name;
        private List<TestingModuleReport> listTestModules;
        private ObservableCollection<ITestingItemReport> listRoots;

        public TestingReportModel() { }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        public List<TestingModuleReport> ListTestModules
        {
            get { return listTestModules; }
            set
            {
                listTestModules = value;
            }
        }
        public ObservableCollection<ITestingItemReport> ListRoots
        {
            get { return listRoots; }
            set
            {
                listRoots = value;
            }
        }

        public TestingReportModel(string name, List<TestingModuleReport> listTestModules)
        {
            this.name = name;
            this.listTestModules = listTestModules;
        }

        public void Convert()
        {
            listRoots = new ObservableCollection<ITestingItemReport>();
            foreach (TestingModuleReport module in listTestModules)
            {
                string relativePath = module.RelativeFilePath;
                string[] strs = relativePath.Split('\\');
                // if FeatureX\FeatureX_1
                if (strs.Length > 1)
                {
                    string[] parentsName = new string[strs.Length - 1];
                    Array.Copy(strs, 0, parentsName, 0, strs.Length - 1);
                    var root = CheckListItemContain(listRoots, parentsName[0]);
                    if (root == null)
                    {
                        root = new TestingFolderReport(parentsName[0]);
                        listRoots.Add(root);
                    }
                    AddNewChild(root, parentsName, module);
                }
                // if FeatureX_1
                else
                {
                    listRoots.Add(module);
                }
            }
        }

        public void AddNewChild(ITestingItemReport root, string[] parentsName, TestingModuleReport module)
        {
            // folder
            ITestingItemReport currentItem = root;
            for (int fi = 1; fi < parentsName.Length; fi++)
            {
                string currentName = parentsName[fi];
                var child = CheckListItemContain(currentItem.Children, currentName);
                if (child == null)
                {
                    child = new TestingFolderReport(currentName);
                    if (currentItem.Children == null)
                        currentItem.Children = new ObservableCollection<ITestingItemReport>();
                    currentItem.Children.Add(child);
                }
                currentItem = child;
            }
            // module
            if (currentItem.Children == null)
                currentItem.Children = new ObservableCollection<ITestingItemReport>();
            currentItem.Children.Add(module);
        }

        public ITestingItemReport CheckListItemContain(ObservableCollection<ITestingItemReport> listItems, string name)
        {
            if (listItems == null)
                return null;
            foreach (var item in listItems)
                if (item.Name.Equals(name))
                    return item;
            return null;
        }
    }
}
