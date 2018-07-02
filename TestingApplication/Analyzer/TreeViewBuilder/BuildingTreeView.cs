// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 1:56 PM 2017/10/20
//using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GUI_Testing_Automation;


namespace TestingApplication
{
    public class BuildingTreeView:IBuildingTreeView
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<IElement, ElementItemVisual> mappingElements = new Dictionary<IElement, ElementItemVisual>();
        
        public BuildingTreeView()
        {
            mappingElements = new Dictionary<IElement, ElementItemVisual>();
            listRootVisualElements = new ObservableCollection<ElementItemVisual>();
        }

        public Dictionary<IElement, ElementItemVisual> MappingElements
        {
            get { return mappingElements; }
            set { mappingElements = value; }
        }

        public ObservableCollection<ElementItemVisual> ListRootVisualElements
        {
            get { return listRootVisualElements; }
            set { listRootVisualElements = value; }
        }

        private ObservableCollection<ElementItemVisual> listRootVisualElements = new ObservableCollection<ElementItemVisual>();

        /// <summary>
        /// add data for tree view
        /// </summary>
        /// <param name="rootElement"></param>
        /// <returns></returns>
        public ObservableCollection<ElementItemVisual> PutAdapter(List<IElement> listRootIElements)
        {
            if (listRootIElements == null || listRootIElements.Count == 0)
                return null;
            listRootVisualElements = new ObservableCollection<ElementItemVisual>();
            foreach (IElement rootElement in listRootIElements)
            {
                ElementItemVisual elementItemVisual = new ElementItemVisual(rootElement);
                listRootVisualElements.Add(elementItemVisual);
                mappingElements.Add(rootElement, elementItemVisual);
                if (rootElement.Children != null)
                    foreach(IElement child in rootElement.Children)
                    {
                        AppendNewElement(child, elementItemVisual);
                    }
            }
            return listRootVisualElements;
        }

        public ObservableCollection<ElementItemVisual> PutAdapter(IElement rootElement)
        {
            if (rootElement == null)
                return null;
            listRootVisualElements = new ObservableCollection<ElementItemVisual>();
            ElementItemVisual elementItemVisual = new ElementItemVisual(rootElement);
            listRootVisualElements.Add(elementItemVisual);
            mappingElements.Add(rootElement, elementItemVisual);
            if (rootElement.Children != null)
                foreach (IElement child in rootElement.Children)
                {
                    AppendNewElement(child, elementItemVisual);
                }
            return listRootVisualElements;
        }

        public void AppendNewElement(IElement element, ElementItemVisual parentVisual)
        {
            try
            {
                ElementItemVisual newElementVisual = new ElementItemVisual(element);
                mappingElements.Add(element, newElementVisual);
                if (parentVisual.Children == null)
                    parentVisual.Children = new ObservableCollection<ElementItemVisual>();
                parentVisual.Children.Add(newElementVisual);
                newElementVisual.Parent = parentVisual;
                if (element.Children != null)
                    foreach (IElement child in element.Children)
                        AppendNewElement(child, newElementVisual);

            } catch (ArgumentException)
            {
                logger.Error("Element " + GUI_Utils.ElementToString(element) + " already existed");
            }
        }
    }
}
