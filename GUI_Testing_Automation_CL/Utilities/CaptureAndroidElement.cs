using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI_Testing_Automation.Utilities
{
    public class CaptureAndroidElement
    {
        public CaptureAndroidElement() { }

        public static string CaptureElement(Bitmap source, Rect section)
        {
            Bitmap bitmap = new Bitmap((int)section.Width, (int)section.Height);

            Graphics g = Graphics.FromImage(bitmap);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            Rectangle tmp = new Rectangle();
            tmp.Y = (int)section.Y;
            tmp.X = (int)section.X;
            tmp.Width = (int)section.Width;
            tmp.Height = (int)section.Height;
            g.DrawImage(source, 0, 0, tmp, GraphicsUnit.Pixel);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            return GetEncodedStringBase64(byteImage);
        }

        private static string GetEncodedStringBase64(byte[] Bytes)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            return Convert.ToBase64String(Bytes); //Get Base64
        }
    }
}
