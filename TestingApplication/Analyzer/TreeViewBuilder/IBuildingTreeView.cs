// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 1:56 PM 2017/10/20
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GUI_Testing_Automation;


/**
 * building a tree view for all ui-elements
 **/
namespace TestingApplication
{
    public interface IBuildingTreeView
    {
        ObservableCollection<ElementItemVisual> PutAdapter(List<IElement> listRootElements);
        ObservableCollection<ElementItemVisual> PutAdapter(IElement rootElement);
    }
}
