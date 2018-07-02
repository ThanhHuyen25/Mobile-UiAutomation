using System;
using System.Windows;
using System.Diagnostics;
using System.ComponentModel;

namespace TestingApplication
{
    public partial class ProgressDialog : Window
    {
        private IProgressResultNoti progessResultNoti;
        #region public Constructors
        //------------------------------------------------------
        //
        //  public Constructors
        //
        //------------------------------------------------------
        public ProgressDialog()
        {
            InitializeComponent();
        }

        public ProgressDialog(string title, string message, IProgressResultNoti progessResultNoti) : this()
        {
            txt_message.Text = message;
            this.Title = title;
            this.progessResultNoti = progessResultNoti;
            //this.ProgressBar_1.Width = this.Width - 20;
        }
        #endregion public Constructors

        #region public Events
        #endregion public Events

        #region private Methods
        //------------------------------------------------------
        //
        //  private Methods
        //
        //------------------------------------------------------
        void CancelClicked(object sender, RoutedEventArgs e)
        {
            progessResultNoti.OnCancel();
        }

        public void Finish()
        {
            //this.Close();
            //Button_1.Content = "OK";
            //ProgressBar_1.IsEnabled = false;
        }

        #endregion private Methods

        #region private Fields
        #endregion private Fields

        private void StackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 180)
                this.ProgressBar_1.Width = e.NewSize.Width;
        }
    }

    public interface IProgressResultNoti
    {
        void OnSuccessful();
        void OnFailure();
        void OnCancel();
    }
}
