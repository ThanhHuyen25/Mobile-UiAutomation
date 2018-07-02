using System;
using System.Windows.Automation;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

/**
 * for capture element
 **/
namespace GUI_Testing_Automation
{
    public class CaptureElement
    {
        private Rectangle screenSize = Screen.PrimaryScreen.Bounds;

        //private double heightResizeImage;
        //private double widthResizeImage;

        //public CaptureElement(Rect windowBound)
        //{
            //this.windowBound = windowBound;
        //}

        public CaptureElement() {}

        /// <summary>
        /// store screen bound
        /// </summary>
        //private Rect windowBound = new Rect(0,0,1000000,1000000);
        //public Rect WindowBound
        //{
            //get { return windowBound; }
            //set { windowBound = value; }
        //}

        /// <summary>
        /// capture screenshot of an element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="saveImageFile">indicate store into a file or not</param>
        /// <param name="pathImage">only significant when @saveImageFile = true</param>
        /// <returns> encoded string of captured image</returns>
        public static string CaptureScreen(Rect bounds, string pathImage = null)
        {
            //ensure the element visible in the form            
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return null;

            Bitmap bitmap = new Bitmap((int)bounds.Width, (int)bounds.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(new System.Drawing.Point((int)bounds.Left, (int)bounds.Top),
                System.Drawing.Point.Empty,
                new System.Drawing.Size((int)bounds.Width, (int)bounds.Height));

            if (pathImage != null)
                bitmap.Save(pathImage, ImageFormat.Jpeg);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            return GetEncodedStringBase64(byteImage);
        }

        public static bool CaptureAllScreen(string pathImage)
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                           Screen.PrimaryScreen.Bounds.Height,
                                           PixelFormat.Format32bppArgb);
            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);
            // Save the screenshot to the specified path that the user has chosen.
            bmpScreenshot.Save(pathImage, ImageFormat.Png);
            return true;
        }

        private static string GetEncodedStringBase64(byte[] Bytes)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            return Convert.ToBase64String(Bytes); //Get Base64
        }
    }
}