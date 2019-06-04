using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JohnsBugTracker.Helpers
{
    public class AttachmentHelper
    {
        public static bool IsWebFriendlyAttachment(HttpPostedFileBase file)
        {
            if (file == null) return false;

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024) return false;

            //A Try/Catch is referred to as a Try Catch block and is used for trapping exceptions
            try
            {
                var fileExt = Path.GetExtension(file.FileName);
                return fileExt.Contains(".bmp") ||
                        fileExt.Contains(".png") ||
                        fileExt.Contains(".jpg") ||
                        fileExt.Contains(".jpeg") ||
                        fileExt.Contains(".tiff") ||
                        fileExt.Contains(".pdf") ||
                        fileExt.Contains(".txt") ||
                        fileExt.Contains(".doc") ||
                        fileExt.Contains(".docx") ||
                        fileExt.Contains(".xls") ||
                        fileExt.Contains(".xlsx");
            }
            catch
            {
                return false;
            }
        }

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
    }


}