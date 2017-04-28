namespace Mogo.GameData
{
    using HMF;
    using Mogo.Util;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class GameDataControler : DataLoader
    {
        private List<Type> m_defaultData = new List<Type> { typeof(GlobalData), typeof(MapData), typeof(LanguageData), typeof(UIMapData), typeof(SoundData), typeof(InstanceLevelGridPosData), typeof(MapUIMappingData) };
        private static GameDataControler m_instance = new GameDataControler();

        public object FormatData(string fileName, Type dicType, Type type)
        {
            if (SystemSwitch.UseHmf)
            {
                return this.FormatHmfData(base.m_resourcePath + fileName + base.m_fileExtention, dicType, type);
            }
            return this.FormatXMLData(base.m_resourcePath + fileName + base.m_fileExtention, dicType, type);
        }

        private object FormatHmfData(string fileName, Type dicType, Type type)
        {
            object obj2 = null;
            try
            {
                obj2 = obj2 = dicType.GetConstructor(Type.EmptyTypes).Invoke(null);
                MemoryStream stream = new MemoryStream(XMLParser.LoadBytes(fileName));
                stream.Seek(0L, SeekOrigin.Begin);
                Hmf hmf = new Hmf();
                Dictionary<object, object> dictionary = (Dictionary<object, object>) hmf.ReadObject(stream);
                PropertyInfo[] properties = type.GetProperties();
                foreach (KeyValuePair<object, object> pair in dictionary)
                {
                    object obj3 = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                    foreach (PropertyInfo info in properties)
                    {
                        if (info.Name == "id")
                        {
                            object obj4 = Utils.GetValue((string) pair.Key, info.PropertyType);
                            info.SetValue(obj3, obj4, null);
                        }
                        else
                        {
                            Dictionary<object, object> dictionary2 = (Dictionary<object, object>) pair.Value;
                            if (dictionary2.ContainsKey(info.Name))
                            {
                                object obj5 = Utils.GetValue((string) dictionary2[info.Name], info.PropertyType);
                                info.SetValue(obj3, obj5, null);
                            }
                        }
                    }
                    object obj6 = Utils.GetValue((string) pair.Key, typeof(int));
                    dicType.GetMethod("Add").Invoke(obj2, new object[] { obj6, obj3 });
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("FormatData Error: " + fileName + "  " + exception.Message, true);
            }
            return obj2;
        }

        private object FormatXMLData(string fileName, Type dicType, Type type)
        {
            object obj2 = null;
            try
            {
                Dictionary<int, Dictionary<string, string>> dictionary;
                obj2 = dicType.GetConstructor(Type.EmptyTypes).Invoke(null);
                if (!XMLParser.LoadIntMap(fileName, base.m_isUseOutterConfig, out dictionary))
                {
                    return obj2;
                }
                PropertyInfo[] properties = type.GetProperties();
                foreach (KeyValuePair<int, Dictionary<string, string>> pair in dictionary)
                {
                    object obj3 = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                    foreach (PropertyInfo info in properties)
                    {
                        if (info.Name == "id")
                        {
                            info.SetValue(obj3, pair.Key, null);
                        }
                        else if (pair.Value.ContainsKey(info.Name))
                        {
                            object obj4 = Utils.GetValue(pair.Value[info.Name], info.PropertyType);
                            info.SetValue(obj3, obj4, null);
                        }
                    }
                    dicType.GetMethod("Add").Invoke(obj2, new object[] { pair.Key, obj3 });
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("FormatData Error: " + fileName + "  " + exception.Message, true);
            }
            return obj2;
        }

        public static void Init(Action<int, int> progress = null, Action finished = null)
        {
            Action action;
            Action action2 = null;
            Action action3 = null;
            if (SystemSwitch.UseHmf)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                m_instance.LoadData(m_instance.m_defaultData, new Func<string, Type, Type, object>(m_instance.FormatHmfData), null);
                stopwatch.Stop();
                LoggerHelper.Info("InitSynHmfData time: " + stopwatch.ElapsedMilliseconds, true);
                if (DataLoader.m_isPreloadData)
                {
                    if (action2 == null)
                    {
                        action2 = () => m_instance.InitAsynData(new Func<string, Type, Type, object>(m_instance.FormatHmfData), progress, finished);
                    }
                    action = action2;
                    if (SystemSwitch.ReleaseMode)
                    {
                        action.BeginInvoke(null, null);
                    }
                    else
                    {
                        action();
                    }
                }
                else
                {
                    finished();
                }
            }
            else
            {
                m_instance.LoadData(m_instance.m_defaultData, new Func<string, Type, Type, object>(m_instance.FormatXMLData), null);
                if (DataLoader.m_isPreloadData)
                {
                    if (action3 == null)
                    {
                        action3 = () => m_instance.InitAsynData(new Func<string, Type, Type, object>(m_instance.FormatXMLData), progress, finished);
                    }
                    action = action3;
                    if (SystemSwitch.ReleaseMode)
                    {
                        action.BeginInvoke(null, null);
                    }
                    else
                    {
                        action();
                    }
                }
                else
                {
                    finished();
                }
            }
        }

        private void InitAsynData(Func<string, Type, Type, object> formatData, Action<int, int> progress, Action finished)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                List<Type> gameDataType = new List<Type>();
                Type[] types = typeof(GameDataControler).Assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.Namespace == "Mogo.GameData")
                    {
                        for (Type type2 = type.BaseType; type2 != null; type2 = type2.BaseType)
                        {
                            if ((type2 == typeof(Mogo.GameData.GameData)) || (type2.IsGenericType && (type2.GetGenericTypeDefinition() == typeof(Mogo.GameData.GameData<>))))
                            {
                                if (!this.m_defaultData.Contains(type))
                                {
                                    gameDataType.Add(type);
                                }
                                break;
                            }
                        }
                    }
                }
                this.LoadData(gameDataType, formatData, progress);
                stopwatch.Stop();
                LoggerHelper.Debug("Asyn GameData init time: " + stopwatch.ElapsedMilliseconds, true, 0);
                GC.Collect();
                if (finished != null)
                {
                    finished();
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("InitData Error: " + exception.Message, true);
            }
        }

        private void LoadData(List<Type> gameDataType, Func<string, Type, Type, object> formatData, Action<int, int> progress)
        {
            int count = gameDataType.Count;
            int num2 = 1;
            foreach (Type type in gameDataType)
            {
                PropertyInfo property = type.GetProperty("dataMap", ~BindingFlags.DeclaredOnly);
                FieldInfo field = type.GetField("fileName");
                if ((property != null) && (field != null))
                {
                    string str = field.GetValue(null) as string;
                    object obj2 = formatData(base.m_resourcePath + str + base.m_fileExtention, property.PropertyType, type);
                    property.GetSetMethod().Invoke(null, new object[] { obj2 });
                }
                if (progress != null)
                {
                    progress(num2, count);
                }
                num2++;
            }
        }

        public static GameDataControler Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

