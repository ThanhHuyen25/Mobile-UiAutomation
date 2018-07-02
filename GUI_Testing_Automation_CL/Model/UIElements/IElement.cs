using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;

/**
 * represent for an ui-element
 **/
namespace GUI_Testing_Automation
{
    public interface IElement
    {
        #region common actions
        void Touch();
        void Focus();
        bool Click(ClickOptions option = ClickOptions.CenterElement);
        string GetText();
        string GetTextPattern();
        double GetWidth();
        double GetHeight();
        Rect GetBoundingRect();
        bool SetWidth(double width);
        bool SetHeight(double height);
        bool SetSize(double width, double height);
        bool SetLocation(double x, double y);
        #endregion end common actions

        #region attributes
        List<IElement> Children { get; set; }
        IElement Parent { get; set; }
        string Id { get; set; }
        ElementAttributes Attributes { get; set; }
        string Alias { get; set; }
        #endregion end attributes
    }

    /// <summary>
    /// element's attribute
    /// </summary>
    public class ElementAttributes
    {
        private string designedName;
        public string DesignedName
        {
            get { return designedName; }
            set { designedName = value; }
        }

        private string designedId;
        public string DesignedId
        {
            get { return designedId; }
            set { designedId = value; }
        }

        private string elementType;
        public string ElementType
        {
            get { return elementType; }
            set { elementType = value; }
        }

        private IElementPath elementPath;
        public IElementPath ElementPath
        {
            get { return elementPath; }
            set { elementPath = value; }
        }

        private string imageCaptureEncoded;
        public string ImageCaptureEncoded
        {
            get { return imageCaptureEncoded; }
            set { imageCaptureEncoded = value; }
        }

        private string acceleratorKey;
        public string AcceleratorKey
        {
            get { return acceleratorKey; }
            set { acceleratorKey = value; }
        }
        private string accessKey;
        public string AccessKey
        {
            get { return accessKey; }
            set { accessKey = value; }
        }

        private string className;
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private string frameworkId;
        public string FrameworkId
        {
            get { return frameworkId; }
            set { frameworkId = value; }
        }

        private bool hasKeyboardFocus;
        public bool HasKeyboardFocus
        {
            get { return hasKeyboardFocus; }
            set { hasKeyboardFocus = value; }
        }

        private string helpText;
        public string HelpText
        {
            get { return helpText; }
            set { helpText = value; }
        }

        private bool isContentElement;
        public bool IsContentElement
        {
            get { return isContentElement; }
            set { isContentElement = value; }
        }

        private bool isControlElement;
        public bool IsControlElement
        {
            get { return isControlElement; }
            set { isControlElement = value; }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        private bool isKeyboardFocusable;
        public bool IsKeyboardFocusable
        {
            get { return isKeyboardFocusable; }
            set { isKeyboardFocusable = value; }
        }

        private bool isOffscreen;
        public bool IsOffscreen
        {
            get { return isOffscreen; }
            set { isOffscreen = value; }
        }

        private bool isPassword;
        public bool IsPassword
        {
            get { return isPassword; }
            set { isPassword = value; }
        }

        private bool isRequiredForForm;
        public bool IsRequiredForForm
        {
            get { return isRequiredForForm; }
            set { isRequiredForForm = value; }
        }

        private string itemStatus;
        public string ItemStatus
        {
            get { return itemStatus; }
            set { itemStatus = value; }
        }

        private string itemType;
        public string ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }

        private string localizedControlType;
        public string LocalizedControlType
        {
            get { return localizedControlType; }
            set { localizedControlType = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int nativeWindowHandle;
        public int NativeWindowHandle
        {
            get { return nativeWindowHandle; }
            set { nativeWindowHandle = value; }
        }

        private int processId;
        public int ProcessId
        {
            get { return processId; }
            set { processId = value; }
        }

        public Rect RectBounding
        {
            get { return rectBounding; }
            set { rectBounding = value; }
        }
        private Rect rectBounding;
    }
}