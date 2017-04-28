namespace Mogo.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FileAccessManager
    {
        public static void DecompressFile(string fileName)
        {
            if (SystemSwitch.UseFileSystem)
            {
                Utils.DecompressToMogoFileAndDirectory(SystemConfig.ResourceFolder, fileName);
            }
            else
            {
                Utils.DecompressToDirectory(SystemConfig.ResourceFolder, fileName);
            }
        }

        public static List<string> GetFileNamesByDirectory(string path)
        {
            return (SystemSwitch.UseFileSystem ? MogoFileSystem.Instance.GetFilesByDirectory(path) : Directory.GetFiles(Path.Combine(SystemConfig.ResourceFolder, path)).ToList<string>());
        }

        public static bool IsFileExist(string fileName)
        {
            fileName = fileName.Replace('\\', '/');
            return (SystemSwitch.UseFileSystem ? MogoFileSystem.Instance.IsFileExist(fileName) : File.Exists(Path.Combine(SystemConfig.ResourceFolder, fileName)));
        }

        public static byte[] LoadBytes(string fileName)
        {
            fileName = fileName.Replace('\\', '/');
            return (SystemSwitch.UseFileSystem ? MogoFileSystem.Instance.LoadFile(fileName) : Utils.LoadByteFile(Path.Combine(SystemConfig.ResourceFolder, fileName)));
        }

        public static List<KeyValuePair<string, byte[]>> LoadFiles(List<KeyValuePair<string, string>> fileFullNames)
        {
            return (SystemSwitch.UseFileSystem ? MogoFileSystem.Instance.LoadFiles(fileFullNames) : LoadLocalFiles(fileFullNames));
        }

        private static List<KeyValuePair<string, byte[]>> LoadLocalFiles(List<KeyValuePair<string, string>> fileFullNames)
        {
            List<KeyValuePair<string, byte[]>> list = new List<KeyValuePair<string, byte[]>>();
            foreach (KeyValuePair<string, string> pair in fileFullNames)
            {
                byte[] buffer = Utils.LoadByteFile(Path.Combine(SystemConfig.ResourceFolder, pair.Value));
                list.Add(new KeyValuePair<string, byte[]>(pair.Key, buffer));
            }
            return list;
        }

        public static string LoadText(string fileName)
        {
            fileName = fileName.Replace('\\', '/');
            return (SystemSwitch.UseFileSystem ? MogoFileSystem.Instance.LoadTextFile(fileName) : Utils.LoadFile(Path.Combine(SystemConfig.ResourceFolder, fileName)));
        }
    }
}

