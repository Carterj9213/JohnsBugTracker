using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace JohnsBugTracker.Helpers
{
    public class AvatarUploadValidator
    {
        public static string DisplayImage(string filePath)
        {
            var fileName = filePath;

            switch (Path.GetExtension(filePath)) //switch(".txt")
            {
                case ".doc":
                    fileName = "/Images/DefaultDoc.png";
                    break;
                case ".docx":
                    fileName = "/Images/DefaultDocx.png";
                    break;
                case ".pdf":
                    fileName = "/Images/DefaultPdf.png";
                    break;
                case ".xls":
                    fileName = "/Images/DefaultXls.png";
                    break;
                case ".xlsx":
                    fileName = "/Images/DefaultXlsx.png";
                    break;
                case ".txt":
                    fileName = "/Images/DefaultTxt.png";
                    break;
                default:
                    break;
            }
            return fileName;
        }

        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
                return false;

            if (file.ContentLength > 2 * 1024 * 1024)
                return false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                           ImageFormat.Png.Equals(img.RawFormat) ||
                           ImageFormat.Gif.Equals(img.RawFormat);
                }
            }
            catch
            {
                return false;
            }

        }
    }
}