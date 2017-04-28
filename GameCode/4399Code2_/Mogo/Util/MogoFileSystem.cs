namespace Mogo.Util
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using UnityEngine;

    public class MogoFileSystem
    {
        private BitCryto m_bitCryto;
        private List<IndexInfo> m_deletedIndexes = new List<IndexInfo>();
        private bool m_encodeData = true;
        private string m_fileFullName;
        private Dictionary<string, IndexInfo> m_fileIndexes = new Dictionary<string, IndexInfo>();
        private FileStream m_fileStream;
        private static MogoFileSystem m_instance = new MogoFileSystem();
        private static readonly object m_locker = new object();
        private Mogo.Util.PackageInfo m_packageInfo = new Mogo.Util.PackageInfo();
        private const int m_pageSize = 0x200;

        private MogoFileSystem()
        {
            List<short> list = new List<short>();
            foreach (byte num in this.m_number)
            {
                list.Add(num);
            }
            this.m_bitCryto = new BitCryto(list.ToArray());
        }

        private void BackupIndexInfo(byte[] indexData)
        {
            string path = Path.Combine(Path.GetDirectoryName(this.m_fileFullName), this.BACK_UP).Replace(@"\", "/");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream stream = File.Create(path))
            {
                stream.Write(indexData, 0, indexData.Length);
                stream.Flush();
                this.SavePackageInfo(stream);
            }
        }

        public IndexInfo BeginSaveFile(string fileFullName, long fileSize)
        {
            IndexInfo info;
            lock (m_locker)
            {
                fileFullName = fileFullName.PathNormalize();
                if (this.m_fileIndexes.ContainsKey(fileFullName))
                {
                    IndexInfo info2 = this.m_fileIndexes[fileFullName];
                    if (fileSize > info2.PageLength)
                    {
                        this.DeleteFile(fileFullName);
                        info = new IndexInfo {
                            Id = fileFullName,
                            FileName = info2.FileName,
                            Path = info2.Path,
                            Offset = this.m_packageInfo.IndexOffset,
                            FileState = FileState.New
                        };
                        this.m_fileStream.Position = this.m_packageInfo.IndexOffset;
                        this.m_fileIndexes[fileFullName] = info;
                        return info;
                    }
                    info = info2;
                    info.Length = 0;
                    info.FileState = FileState.Modify;
                    this.m_fileStream.Position = info.Offset;
                    return info;
                }
                info = new IndexInfo {
                    Id = fileFullName,
                    FileName = Path.GetFileName(fileFullName),
                    Path = Path.GetDirectoryName(fileFullName),
                    Offset = this.m_packageInfo.IndexOffset,
                    FileState = FileState.New
                };
                this.m_fileStream.Position = this.m_packageInfo.IndexOffset;
                this.m_fileIndexes[fileFullName] = info;
            }
            return info;
        }

        private bool CheckBackUpIndex()
        {
            string path = Path.Combine(Path.GetDirectoryName(this.m_fileFullName), this.BACK_UP).Replace(@"\", "/");
            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                long length = info.Length;
                FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                this.m_packageInfo = this.GetPackageInfo(fileStream, length);
                if ((this.m_packageInfo != null) && (this.m_packageInfo.IndexOffset != 0))
                {
                    int count = ((int) length) - Mogo.Util.PackageInfo.GetPackageSize();
                    byte[] buffer = new byte[count];
                    fileStream.Position = 0L;
                    fileStream.Read(buffer, 0, count);
                    this.LoadIndexInfo(buffer, false);
                }
                return true;
            }
            return false;
        }

        public void CleanBackUpIndex()
        {
            string path = Path.Combine(Path.GetDirectoryName(this.m_fileFullName), this.BACK_UP).Replace(@"\", "/");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void Clear()
        {
            this.m_fileIndexes.Clear();
            this.m_deletedIndexes.Clear();
            this.m_packageInfo.IndexOffset = 0;
            this.Close();
            Caching.CleanCache();
        }

        public void Close()
        {
            if (this.m_fileStream != null)
            {
                this.m_fileStream.Close();
                this.m_fileStream.Dispose();
                this.m_fileStream = null;
            }
        }

        public void DeleteFile(string fileFullName)
        {
            lock (m_locker)
            {
                fileFullName = fileFullName.PathNormalize();
                if (this.m_fileIndexes.ContainsKey(fileFullName))
                {
                    IndexInfo item = this.m_fileIndexes[fileFullName];
                    item.Deleted = true;
                    item.Id = "";
                    this.m_fileIndexes.Remove(fileFullName);
                    this.m_deletedIndexes.Add(item);
                }
            }
        }

        private byte[] DoLoadFile(string fileFullName)
        {
            IndexInfo info = this.m_fileIndexes[fileFullName];
            byte[] buffer = new byte[info.Length];
            this.m_fileStream.Position = info.Offset;
            this.m_fileStream.Read(buffer, 0, buffer.Length);
            if (this.m_encodeData)
            {
                this.m_bitCryto.Reset();
                int length = buffer.Length;
                for (int i = 0; i < length; i++)
                {
                    buffer[i] = this.m_bitCryto.Decode(buffer[i]);
                }
            }
            return buffer;
        }

        public void EndSaveFile(IndexInfo info)
        {
            lock (m_locker)
            {
                uint num = info.Length % 0x200;
                if (num != 0)
                {
                    uint num2 = 0x200 - num;
                    byte[] buffer = new byte[num2];
                    this.m_fileStream.Write(buffer, 0, buffer.Length);
                    info.PageLength = info.Length + num2;
                }
                else
                {
                    info.PageLength = info.Length;
                }
                this.m_fileStream.Flush();
                if (info.FileState == FileState.New)
                {
                    this.m_packageInfo.IndexOffset = (uint) this.m_fileStream.Position;
                }
            }
        }

        public void GetAndBackUpIndexInfo()
        {
            FileInfo info = new FileInfo(this.m_fileFullName);
            long length = info.Length;
            this.GetIndexInfo(length, true);
        }

        public List<string> GetFilesByDirectory(string path)
        {
            List<string> list = new List<string>();
            path = path.PathNormalize();
            foreach (KeyValuePair<string, IndexInfo> pair in this.m_fileIndexes)
            {
                if (pair.Value.Path.PathNormalize() == path)
                {
                    list.Add(pair.Value.Path);
                }
            }
            return list;
        }

        private void GetIndexInfo(long fileSize)
        {
            if (!this.CheckBackUpIndex())
            {
                this.GetIndexInfo(fileSize, false);
            }
        }

        private void GetIndexInfo(long fileSize, bool needBackUpIndex)
        {
            lock (m_locker)
            {
                this.m_packageInfo = this.GetPackageInfo(this.m_fileStream, fileSize);
                if ((this.m_packageInfo != null) && (this.m_packageInfo.IndexOffset != 0))
                {
                    int count = (int) ((fileSize - Mogo.Util.PackageInfo.GetPackageSize()) - this.m_packageInfo.IndexOffset);
                    byte[] buffer = new byte[count];
                    this.m_fileStream.Position = this.m_packageInfo.IndexOffset;
                    this.m_fileStream.Read(buffer, 0, count);
                    this.LoadIndexInfo(buffer, needBackUpIndex);
                }
            }
        }

        private Mogo.Util.PackageInfo GetPackageInfo(FileStream fileStream, long fileSize)
        {
            Mogo.Util.PackageInfo info = new Mogo.Util.PackageInfo();
            if (fileSize < Mogo.Util.PackageInfo.GetPackageSize())
            {
                return new Mogo.Util.PackageInfo();
            }
            fileStream.Position = fileSize - Mogo.Util.PackageInfo.GetPackageSize();
            byte[] buffer = new byte[Mogo.Util.PackageInfo.GetPackageSize()];
            fileStream.Read(buffer, 0, Mogo.Util.PackageInfo.GetPackageSize());
            int index = 0;
            info.IndexOffset = EncodeDecoder.DecodeUInt32(buffer, ref index);
            return info;
        }

        public void Init()
        {
            lock (m_locker)
            {
                if (this.m_fileFullName == null)
                {
                    this.m_fileFullName = SystemConfig.ResourceFolder + FILE_NAME;
                }
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                this.Init(this.m_fileFullName);
                stopwatch.Stop();
                LoggerHelper.Debug(string.Concat(new object[] { "file count: ", this.m_fileIndexes.Count, " init time: ", stopwatch.ElapsedMilliseconds }), true, 0);
                this.Close();
            }
        }

        public void Init(string fileName)
        {
            if (this.m_fileStream == null)
            {
                if (!File.Exists(fileName))
                {
                    string directoryName = Path.GetDirectoryName(fileName);
                    if (!(string.IsNullOrEmpty(directoryName) || Directory.Exists(directoryName)))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    this.m_fileStream = File.Create(fileName);
                }
                else
                {
                    FileInfo info = new FileInfo(fileName);
                    long length = info.Length;
                    this.m_fileStream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    this.GetIndexInfo(length);
                }
            }
        }

        public bool IsFileExist(string fileFullName)
        {
            fileFullName = fileFullName.PathNormalize();
            return this.m_fileIndexes.ContainsKey(fileFullName);
        }

        public byte[] LoadFile(string fileFullName)
        {
            this.Open();
            lock (m_locker)
            {
                fileFullName = fileFullName.PathNormalize();
                if ((this.m_fileStream != null) && this.m_fileIndexes.ContainsKey(fileFullName))
                {
                    return this.DoLoadFile(fileFullName);
                }
                return null;
            }
        }

        public List<KeyValuePair<string, byte[]>> LoadFiles(List<KeyValuePair<string, string>> fileFullNames)
        {
            Action action = null;
            this.Open();
            List<KeyValuePair<string, byte[]>> list = new List<KeyValuePair<string, byte[]>>();
            lock (m_locker)
            {
                foreach (KeyValuePair<string, string> pair in fileFullNames)
                {
                    string key = pair.Value.PathNormalize();
                    if ((this.m_fileStream != null) && this.m_fileIndexes.ContainsKey(key))
                    {
                        byte[] buffer = this.DoLoadFile(key);
                        list.Add(new KeyValuePair<string, byte[]>(pair.Key, buffer));
                    }
                    else
                    {
                        if (action == null)
                        {
                            action = delegate {
                                LoggerHelper.Error("File not exist in MogoFileSystem: " + this.FileFullName, true);
                            };
                        }
                        DriverLib.Invoke(action);
                    }
                }
            }
            return list;
        }

        private void LoadIndexInfo(byte[] indexData, bool needBackUpIndex)
        {
            if (indexData.Length > 0)
            {
                if (needBackUpIndex)
                {
                    this.BackupIndexInfo(indexData);
                }
                indexData = DESCrypto.Decrypt(indexData, this.m_number);
            }
            this.m_deletedIndexes.Clear();
            this.m_fileIndexes.Clear();
            int offset = 0;
            while (offset < indexData.Length)
            {
                IndexInfo item = IndexInfo.Decode(indexData, ref offset);
                if (item.Deleted)
                {
                    this.m_deletedIndexes.Add(item);
                }
                else
                {
                    this.m_fileIndexes[item.Id.PathNormalize()] = item;
                }
            }
            this.m_fileStream.Position = this.m_packageInfo.IndexOffset;
        }

        public string LoadTextFile(string fileFullName)
        {
            this.Open();
            byte[] bytes = this.LoadFile(fileFullName);
            if (bytes != null)
            {
                return Encoding.UTF8.GetString(bytes);
            }
            return string.Empty;
        }

        public void Open()
        {
            if (this.m_fileStream == null)
            {
                this.m_fileStream = File.Open(this.m_fileFullName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
        }

        public void SaveFile(string fileFullName)
        {
            if (File.Exists(fileFullName))
            {
                FileInfo info = new FileInfo(fileFullName);
                IndexInfo info2 = this.BeginSaveFile(fileFullName, info.Length);
                byte[] buffer = new byte[0x800];
                using (FileStream stream = File.OpenRead(fileFullName))
                {
                    int num;
                    while ((num = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        this.WriteFile(info2, buffer, 0, num);
                    }
                }
                this.EndSaveFile(info2);
            }
        }

        public void SaveIndexInfo()
        {
            lock (m_locker)
            {
                uint position = (uint) this.m_fileStream.Position;
                List<byte> list = new List<byte>();
                foreach (KeyValuePair<string, IndexInfo> pair in this.m_fileIndexes)
                {
                    list.AddRange(pair.Value.GetEncodeData());
                }
                foreach (IndexInfo info in this.m_deletedIndexes)
                {
                    list.AddRange(info.GetEncodeData());
                }
                byte[] toEncrypt = list.ToArray();
                if (toEncrypt.Length > 0)
                {
                    toEncrypt = DESCrypto.Encrypt(toEncrypt, this.m_number);
                }
                this.m_fileStream.Position = this.m_packageInfo.IndexOffset;
                this.m_fileStream.Write(toEncrypt, 0, toEncrypt.Length);
                this.m_fileStream.Flush();
                this.SavePackageInfo(this.m_fileStream);
                this.m_fileStream.Position = position;
            }
        }

        private void SavePackageInfo(FileStream fileStream)
        {
            byte[] buffer = EncodeDecoder.EncodeUInt32(this.m_packageInfo.IndexOffset);
            fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Flush();
        }

        public void WriteFile(IndexInfo info, byte[] buffer, int offset, int count)
        {
            info.Length += (uint) count;
            if (this.m_encodeData)
            {
                this.m_bitCryto.Reset();
                int length = buffer.Length;
                for (int i = 0; i < length; i++)
                {
                    buffer[i] = this.m_bitCryto.Encode(buffer[i]);
                }
            }
            this.m_fileStream.Write(buffer, offset, count);
        }

        private string BACK_UP
        {
            get
            {
                return "pkg_i_bak";
            }
        }

        public List<IndexInfo> DeletedIndexes
        {
            get
            {
                return this.m_deletedIndexes;
            }
        }

        public static string FILE_NAME
        {
            get
            {
                return "pkg";
            }
        }

        public string FileFullName
        {
            get
            {
                return this.m_fileFullName;
            }
            set
            {
                this.m_fileFullName = value;
            }
        }

        public Dictionary<string, IndexInfo> FileIndexs
        {
            get
            {
                return this.m_fileIndexes;
            }
        }

        public static MogoFileSystem Instance
        {
            get
            {
                return m_instance;
            }
        }

        public bool IsClosed
        {
            get
            {
                return (this.m_fileStream == null);
            }
        }

        private byte[] m_number
        {
            get
            {
                return Utils.GetIndexNumber();
            }
        }
    }
}

