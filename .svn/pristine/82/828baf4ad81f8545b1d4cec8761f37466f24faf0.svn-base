namespace Mogo.Util
{
    using Mono.Xml;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security;

    public class XMLParser
    {
        public static SecurityElement Load(string fileName)
        {
            string str = LoadText(fileName);
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return LoadXML(str);
        }

        public static byte[] LoadBytes(string fileName)
        {
            return (SystemSwitch.ReleaseMode ? FileAccessManager.LoadBytes(fileName) : Utils.LoadByteResource(fileName));
        }

        public static Dictionary<int, Dictionary<string, string>> LoadIntMap(SecurityElement xml, string source)
        {
            Dictionary<int, Dictionary<string, string>> dictionary = new Dictionary<int, Dictionary<string, string>>();
            int num = 0;
            foreach (SecurityElement element in xml.Children)
            {
                num++;
                if ((element.Children == null) || (element.Children.Count == 0))
                {
                    LoggerHelper.Warning(string.Concat(new object[] { "empty row in row NO.", num, " of ", source }), true);
                }
                else
                {
                    int key = int.Parse((element.Children[0] as SecurityElement).Text);
                    if (dictionary.ContainsKey(key))
                    {
                        LoggerHelper.Warning(string.Format("Key {0} already exist, in {1}.", key, source), true);
                    }
                    else
                    {
                        Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
                        dictionary.Add(key, dictionary2);
                        for (int i = 1; i < element.Children.Count; i++)
                        {
                            string tag;
                            SecurityElement element2 = element.Children[i] as SecurityElement;
                            if (element2.Tag.Length < 3)
                            {
                                tag = element2.Tag;
                            }
                            else
                            {
                                string str2 = element2.Tag.Substring(element2.Tag.Length - 2, 2);
                                if (((((str2 == "_i") || (str2 == "_s")) || ((str2 == "_f") || (str2 == "_l"))) || (str2 == "_k")) || (str2 == "_m"))
                                {
                                    tag = element2.Tag.Substring(0, element2.Tag.Length - 2);
                                }
                                else
                                {
                                    tag = element2.Tag;
                                }
                            }
                            if ((element2 != null) && !dictionary2.ContainsKey(tag))
                            {
                                if (string.IsNullOrEmpty(element2.Text))
                                {
                                    dictionary2.Add(tag, "");
                                }
                                else
                                {
                                    dictionary2.Add(tag, element2.Text.Trim());
                                }
                            }
                            else
                            {
                                LoggerHelper.Warning(string.Format("Key {0} already exist, index {1} of {2}.", element2.Tag, i, element2.ToString()), true);
                            }
                        }
                    }
                }
            }
            return dictionary;
        }

        public static bool LoadIntMap(string fileName, bool isForceOutterRecoure, out Dictionary<int, Dictionary<string, string>> map)
        {
            try
            {
                SecurityElement element;
                if (isForceOutterRecoure)
                {
                    element = LoadOutter(fileName);
                }
                else
                {
                    element = Load(fileName);
                }
                if (element == null)
                {
                    LoggerHelper.Error("File not exist: " + fileName, true);
                    map = null;
                    return false;
                }
                map = LoadIntMap(element, fileName);
                return true;
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Load Int Map Error: " + fileName + "  " + exception.Message, true);
                map = null;
                return false;
            }
        }

        public static Dictionary<string, Dictionary<string, string>> LoadMap(SecurityElement xml)
        {
            Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
            foreach (SecurityElement element in xml.Children)
            {
                string key = (element.Children[0] as SecurityElement).Text.Trim();
                if (dictionary.ContainsKey(key))
                {
                    LoggerHelper.Warning(string.Format("Key {0} already exist, in {1}.", key, xml.ToString()), true);
                }
                else
                {
                    Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
                    dictionary.Add(key, dictionary2);
                    for (int i = 1; i < element.Children.Count; i++)
                    {
                        SecurityElement element2 = element.Children[i] as SecurityElement;
                        if ((element2 != null) && !dictionary2.ContainsKey(element2.Tag))
                        {
                            if (string.IsNullOrEmpty(element2.Text))
                            {
                                dictionary2.Add(element2.Tag, "");
                            }
                            else
                            {
                                dictionary2.Add(element2.Tag, element2.Text.Trim());
                            }
                        }
                        else
                        {
                            LoggerHelper.Warning(string.Format("Key {0} already exist, index {1} of {2}.", element2.Tag, i, element2.ToString()), true);
                        }
                    }
                }
            }
            return dictionary;
        }

        public bool LoadMap(string fileName, out Dictionary<string, Dictionary<string, string>> map)
        {
            try
            {
                SecurityElement xml = Load(fileName);
                map = LoadMap(xml);
                return true;
            }
            catch (Exception exception)
            {
                LoggerHelper.Except(exception, null);
                map = null;
                return false;
            }
        }

        public Dictionary<string, Dictionary<string, string>> LoadMap(string fileName, out string key)
        {
            key = Path.GetFileNameWithoutExtension(fileName);
            return LoadMap(Load(fileName));
        }

        public static SecurityElement LoadOutter(string fileName)
        {
            string str = Utils.LoadFile(fileName.Replace('\\', '/'));
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return LoadXML(str);
        }

        public static string LoadText(string fileName)
        {
            try
            {
                return (SystemSwitch.ReleaseMode ? FileAccessManager.LoadText(fileName) : Utils.LoadResource(fileName));
            }
            catch (Exception exception)
            {
                LoggerHelper.Except(exception, null);
                return "";
            }
        }

        public static SecurityElement LoadXML(string xml)
        {
            try
            {
                SecurityParser parser = new SecurityParser();
                parser.LoadXml(xml);
                return parser.ToXml();
            }
            catch (Exception exception)
            {
                LoggerHelper.Except(exception, null);
                return null;
            }
        }

        public static void SaveBytes(string fileName, byte[] buffer)
        {
            if (!Directory.Exists(Utils.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Utils.GetDirectoryName(fileName));
            }
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(buffer);
                    writer.Flush();
                    writer.Close();
                }
                stream.Close();
            }
        }

        public static void SaveText(string fileName, string text)
        {
            if (!Directory.Exists(Utils.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Utils.GetDirectoryName(fileName));
            }
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(text);
                    writer.Flush();
                    writer.Close();
                }
                stream.Close();
            }
        }
    }
}

