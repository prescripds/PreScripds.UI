using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Infrastructure
{
    public static class FileServiceProvider
    {
        public static void CreateDirectory(string directoryPath, string directoryName)
        {
            var newDirectoryPath = Path.Combine(directoryPath, directoryName);

            if (directoryPath.IsEmpty() && directoryName.IsEmpty() && !Directory.Exists(directoryPath))
                throw new Exception("The directory path does not exists.");

            Directory.CreateDirectory(newDirectoryPath);
        }
        public static void CreateDirectory(string directoryPath)
        {
            var newDirectoryPath = "{0}".ToFormat(directoryPath);

            if (directoryPath.IsEmpty() && !Directory.Exists(directoryPath))
                throw new Exception("The directory path does not exists.");

            Directory.CreateDirectory(newDirectoryPath);
        }

        public static void DeleteDirectory(string directoryPath, string directoryName)
        {
            var newDirectoryPath = "{0}{1}".ToFormat(directoryPath, directoryName);

            if (directoryPath.IsEmpty() && directoryName.IsEmpty() && !Directory.Exists(directoryPath))
                throw new Exception("The directory path does not exists.");

            Directory.Delete(newDirectoryPath);
        }

        public static string CreateFile(string rootFolder, string fileName)
        {
            var filePath = "{0}{1}".ToFormat(rootFolder, fileName);

            if (filePath.IsEmpty() && !Directory.Exists(rootFolder))
                throw new Exception("The root path does not exists.");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
                return filePath;
        }

        public static void DeleteFile(string directoryPath, string fileName)
        {
            var filePath = Path.Combine("{0}{1}".ToFormat(directoryPath, fileName));
            if (filePath.IsEmpty() && !Directory.Exists(directoryPath))
                throw new Exception("THe Directory does not exist.");
            File.Delete(filePath);
        }
        public static FileStream CreateFileStream(string rootFolder, string fileName)
        {
            var filePath = "{0}{1}".ToFormat(rootFolder, fileName);

            if (!Directory.Exists(rootFolder))
                throw new Exception("The root path does not exists.");

            return File.Create(filePath);
        }


        public static StreamReader ReadFileStream(string rootFolder, string fileName)
        {
            var filePath = "{0}{1}".ToFormat(rootFolder, fileName);

            if (filePath.IsEmpty() && !Directory.Exists(rootFolder))
                throw new Exception("The root path does not exists.");
            if (Directory.Exists(rootFolder))
            {
                var reader = new StreamReader(filePath);
                return reader;
            }
            return null;
        }

        public static string GetFileName(string absolutePath)
        {
            if (!Path.GetFileName(absolutePath).IsNotEmpty()) return null;

            var fileName = Path.GetFileName(absolutePath);
            if (fileName.IsNotEmpty())
                if (fileName != null) return fileName.ToLower();

            return null;
        }

        public static void CopyToAndDeleteCurrent(string directoryNewPath, string directoryOldPath, string directoryName)
        {
            var newDirectoryPhysPath = "{0}{1}".ToFormat(directoryNewPath, directoryName);

            var oldDirectoryPhysPath = "{0}{1}".ToFormat(directoryOldPath, directoryName);

            if (directoryNewPath.IsEmpty() && directoryName.IsEmpty() && !Directory.Exists(directoryNewPath))
                throw new Exception("The new directory path does not exists.");

            //Create new directory.
            CreateDirectory(directoryNewPath, directoryName);


            var oldDirecoryInfo = new DirectoryInfo(oldDirectoryPhysPath);

            // Copy each file into it’s new directory.
            foreach (var oldDirFil in oldDirecoryInfo.GetFiles(oldDirectoryPhysPath))
            {
                oldDirFil.MoveTo("{0}{1}".ToFormat(newDirectoryPhysPath, oldDirFil.Name));
            }

            Directory.Delete(oldDirectoryPhysPath);
        }
    }
}
