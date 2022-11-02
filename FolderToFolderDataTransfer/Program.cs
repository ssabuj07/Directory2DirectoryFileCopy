using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace FolderToFolderDataTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string Source = @"D:\Z_Sabuj\Publish\vms\VMS\Upload";
                string Destination = @"E:\_DB_Backup\VMS\Uploads";
                CopyAllFiles(Source, Destination);
            }
            catch (Exception)
            {
                throw;
            }

            CreateTextFileLog("Task Completed successfully.");
        }

        private static void CopyAllFiles(string Source, string Destination)
        {
            try
            {
                // Get the subdirectories for the specified directory.
                DirectoryInfo dir = new DirectoryInfo(Source);
                DirectoryInfo[] dirs = dir.GetDirectories();

                if (!dir.Exists)
                {
                    CreateTextFileLog("Source directory does not exist or could not be found: "
                        + Source);
                }
                // If the destination directory doesn't exist, create it.
                if (!Directory.Exists(Destination))
                {
                    Directory.CreateDirectory(Destination);
                }
                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(Destination, file.Name);
                    file.CopyTo(temppath, true);
                }
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(Destination, subdir.Name);
                    CopyAllFiles(subdir.FullName, temppath);
                }
            }
            catch (Exception ex)
            {
                CreateTextFileLog(ex.ToString());
            }
        }


        private static void CreateTextFileLog(string Message)
        {
            StreamWriter SW;
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "txt_" + DateTime.Now.ToString("yyyyMMdd") + ".txt")))
            {
                SW = File.CreateText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "txt_" + DateTime.Now.ToString("yyyyMMdd") + ".txt"));
                SW.Close();
            }

            using (SW = File.AppendText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "txt_" + DateTime.Now.ToString("yyyyMMdd") + ".txt")))
            {
                SW.WriteLine(Message);
                SW.Close();
            }
        }
    }
}
