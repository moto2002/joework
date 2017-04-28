namespace Mogo.RPC
{
    using HMF;
    using Mogo.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security;

    public class DefParser : DataLoader
    {
        private const string ATTR_BASE_METHODS = "BaseMethods";
        private const string ATTR_CELL_METHODS = "CellMethods";
        private const string ATTR_CLIENT_METHODS = "ClientMethods";
        private const string ATTR_PROPERTIES = "Properties";
        private const string EL_EXPOSED = "Exposed";
        private const string EL_FLAGS = "Flags";
        private const string EL_PARENT = "Parent";
        private const string EL_TYPE = "Type";
        private Dictionary<uint, EntityDef> m_entitysByID = new Dictionary<uint, EntityDef>();
        private Dictionary<string, EntityDef> m_entitysByName = new Dictionary<string, EntityDef>();
        private static DefParser m_instance = new DefParser();
        private const string VALUE_CLIENT = "CLIENT";

        private DefParser()
        {
        }

        private bool CheckFlags(string flags)
        {
            return !(string.IsNullOrEmpty(flags) || !flags.Contains("CLIENT"));
        }

        private void GetBaseMethods(EntityDef entity, ArrayList methods)
        {
            Dictionary<ushort, EntityDefMethod> dictionary = new Dictionary<ushort, EntityDefMethod>();
            Dictionary<string, EntityDefMethod> dictionary2 = new Dictionary<string, EntityDefMethod>();
            if (methods != null)
            {
                ushort num = 0;
                foreach (SecurityElement element in methods)
                {
                    if (((element.Children != null) && (element.Children.Count != 0)) && ((element.Children[0] as SecurityElement).Tag == "Exposed"))
                    {
                        EntityDefMethod method = new EntityDefMethod {
                            FuncName = element.Tag,
                            FuncID = num,
                            ArgsType = new List<VObject>()
                        };
                        for (int i = 1; i < element.Children.Count; i++)
                        {
                            SecurityElement element2 = element.Children[i] as SecurityElement;
                            method.ArgsType.Add(TypeMapping.GetVObject(element2.Text.Trim()));
                        }
                        dictionary.Add(method.FuncID, method);
                        if (!dictionary2.ContainsKey(method.FuncName))
                        {
                            dictionary2.Add(method.FuncName, method);
                        }
                    }
                    num = (ushort) (num + 1);
                }
            }
            entity.BaseMethodsByID = dictionary;
            entity.BaseMethodsByName = dictionary2;
        }

        private void GetCellMethods(EntityDef entity, ArrayList methods)
        {
            Dictionary<ushort, EntityDefMethod> dictionary = new Dictionary<ushort, EntityDefMethod>();
            Dictionary<string, EntityDefMethod> dictionary2 = new Dictionary<string, EntityDefMethod>();
            if (methods != null)
            {
                ushort num = 0;
                foreach (SecurityElement element in methods)
                {
                    if (((element.Children != null) && (element.Children.Count != 0)) && ((element.Children[0] as SecurityElement).Tag == "Exposed"))
                    {
                        EntityDefMethod method = new EntityDefMethod {
                            FuncName = element.Tag,
                            FuncID = num,
                            ArgsType = new List<VObject>()
                        };
                        for (int i = 1; i < element.Children.Count; i++)
                        {
                            SecurityElement element2 = element.Children[i] as SecurityElement;
                            method.ArgsType.Add(TypeMapping.GetVObject(element2.Text.Trim()));
                        }
                        dictionary.Add(method.FuncID, method);
                        if (!dictionary2.ContainsKey(method.FuncName))
                        {
                            dictionary2.Add(method.FuncName, method);
                        }
                    }
                    num = (ushort) (num + 1);
                }
            }
            entity.CellMethodsByID = dictionary;
            entity.CellMethodsByName = dictionary2;
        }

        private void GetClientMethods(EntityDef entity, ArrayList methods)
        {
            Dictionary<ushort, EntityDefMethod> dictionary = new Dictionary<ushort, EntityDefMethod>();
            Dictionary<string, EntityDefMethod> dictionary2 = new Dictionary<string, EntityDefMethod>();
            if (methods != null)
            {
                ushort num = 0;
                foreach (SecurityElement element in methods)
                {
                    EntityDefMethod method = new EntityDefMethod {
                        FuncName = element.Tag,
                        FuncID = num,
                        ArgsType = new List<VObject>()
                    };
                    if (element.Children != null)
                    {
                        foreach (SecurityElement element2 in element.Children)
                        {
                            method.ArgsType.Add(TypeMapping.GetVObject(element2.Text.Trim()));
                        }
                    }
                    dictionary.Add(method.FuncID, method);
                    if (!dictionary2.ContainsKey(method.FuncName))
                    {
                        dictionary2.Add(method.FuncName, method);
                    }
                    num = (ushort) (num + 1);
                }
            }
            entity.ClientMethodsByID = dictionary;
            entity.ClientMethodsByName = dictionary2;
        }

        public EntityDef GetEntityByID(ushort id)
        {
            EntityDef def;
            this.m_entitysByID.TryGetValue(id, out def);
            return def;
        }

        public EntityDef GetEntityByName(string name)
        {
            EntityDef def;
            this.m_entitysByName.TryGetValue(name, out def);
            return def;
        }

        private void GetHmfBaseMethods(EntityDef etyDef, List<object> methods)
        {
            Dictionary<ushort, EntityDefMethod> dictionary = new Dictionary<ushort, EntityDefMethod>();
            Dictionary<string, EntityDefMethod> dictionary2 = new Dictionary<string, EntityDefMethod>();
            if (methods != null)
            {
                ushort num = 0;
                for (int i = 0; i < methods.Count; i++)
                {
                    KeyValuePair<object, object> pair = ((Dictionary<object, object>) methods[i]).First<KeyValuePair<object, object>>();
                    List<object> list = (List<object>) pair.Value;
                    if (((list != null) && (list.Count != 0)) && (((string) ((List<object>) list[0])[0]) == "Exposed"))
                    {
                        EntityDefMethod method = new EntityDefMethod {
                            FuncName = (string) pair.Key,
                            FuncID = num,
                            ArgsType = new List<VObject>()
                        };
                        for (int j = 1; j < list.Count; j++)
                        {
                            List<object> list2 = (List<object>) list[j];
                            method.ArgsType.Add(TypeMapping.GetVObject(((string) list2[1]).Trim()));
                        }
                        dictionary.Add(method.FuncID, method);
                        if (!dictionary2.ContainsKey(method.FuncName))
                        {
                            dictionary2.Add(method.FuncName, method);
                        }
                    }
                    num = (ushort) (num + 1);
                }
            }
            etyDef.BaseMethodsByID = dictionary;
            etyDef.BaseMethodsByName = dictionary2;
        }

        private void GetHmfCellMethods(EntityDef etyDef, List<object> methods)
        {
            Dictionary<ushort, EntityDefMethod> dictionary = new Dictionary<ushort, EntityDefMethod>();
            Dictionary<string, EntityDefMethod> dictionary2 = new Dictionary<string, EntityDefMethod>();
            if (methods != null)
            {
                ushort num = 0;
                for (int i = 0; i < methods.Count; i++)
                {
                    KeyValuePair<object, object> pair = ((Dictionary<object, object>) methods[i]).First<KeyValuePair<object, object>>();
                    List<object> list = (List<object>) pair.Value;
                    if (((list != null) && (list.Count != 0)) && (((string) ((List<object>) list[0])[0]) == "Exposed"))
                    {
                        EntityDefMethod method = new EntityDefMethod {
                            FuncName = (string) pair.Key,
                            FuncID = num,
                            ArgsType = new List<VObject>()
                        };
                        for (int j = 1; j < list.Count; j++)
                        {
                            List<object> list2 = (List<object>) list[j];
                            method.ArgsType.Add(TypeMapping.GetVObject(((string) list2[1]).Trim()));
                        }
                        dictionary.Add(method.FuncID, method);
                        if (!dictionary2.ContainsKey(method.FuncName))
                        {
                            dictionary2.Add(method.FuncName, method);
                        }
                    }
                    num = (ushort) (num + 1);
                }
            }
            etyDef.CellMethodsByID = dictionary;
            etyDef.CellMethodsByName = dictionary2;
        }

        private void GetHmfClientMethods(EntityDef etyDef, List<object> methods)
        {
            Dictionary<ushort, EntityDefMethod> dictionary = new Dictionary<ushort, EntityDefMethod>();
            Dictionary<string, EntityDefMethod> dictionary2 = new Dictionary<string, EntityDefMethod>();
            if (methods != null)
            {
                ushort num = 0;
                for (int i = 0; i < methods.Count; i++)
                {
                    EntityDefMethod method = new EntityDefMethod();
                    KeyValuePair<object, object> pair = ((Dictionary<object, object>) methods[i]).First<KeyValuePair<object, object>>();
                    method.FuncName = (string) pair.Key;
                    method.FuncID = num;
                    method.ArgsType = new List<VObject>();
                    if (pair.Value != null)
                    {
                        foreach (object obj2 in (List<object>) pair.Value)
                        {
                            List<object> list = (List<object>) obj2;
                            method.ArgsType.Add(TypeMapping.GetVObject(((string) list[1]).Trim()));
                        }
                    }
                    dictionary.Add(method.FuncID, method);
                    if (!dictionary2.ContainsKey(method.FuncName))
                    {
                        dictionary2.Add(method.FuncName, method);
                    }
                    num = (ushort) (num + 1);
                }
            }
            etyDef.ClientMethodsByID = dictionary;
            etyDef.ClientMethodsByName = dictionary2;
        }

        private Dictionary<ushort, EntityDefProperties> GetHmfProperties(List<object> props)
        {
            Dictionary<ushort, EntityDefProperties> dictionary = new Dictionary<ushort, EntityDefProperties>();
            if (props != null)
            {
                short num = -1;
                for (int i = 0; i < props.Count; i++)
                {
                    KeyValuePair<object, object> pair = ((Dictionary<object, object>) props[i]).First<KeyValuePair<object, object>>();
                    List<object> list = null;
                    List<object> list2 = null;
                    foreach (object obj2 in (List<object>) pair.Value)
                    {
                        List<object> list3 = (List<object>) obj2;
                        if (((string) list3[0]) == "Type")
                        {
                            list2 = list3;
                        }
                        else if (((string) list3[0]) == "Flags")
                        {
                            list = list3;
                            break;
                        }
                    }
                    num = (short) (num + 1);
                    if ((list == null) || this.CheckFlags(((string) list[1]).Trim()))
                    {
                        EntityDefProperties properties = new EntityDefProperties {
                            Name = (string) pair.Key
                        };
                        if (list2 != null)
                        {
                            properties.VType = TypeMapping.GetVObject(((string) list2[1]).Trim());
                        }
                        if (!dictionary.ContainsKey((ushort) num))
                        {
                            dictionary.Add((ushort) num, properties);
                        }
                    }
                }
            }
            return dictionary;
        }

        private Dictionary<ushort, EntityDefProperties> GetProperties(ArrayList properties)
        {
            Dictionary<ushort, EntityDefProperties> dictionary = new Dictionary<ushort, EntityDefProperties>();
            if (properties != null)
            {
                short num = -1;
                foreach (SecurityElement element in properties)
                {
                    SecurityElement element2 = null;
                    SecurityElement element3 = null;
                    foreach (SecurityElement element4 in element.Children)
                    {
                        if (element4.Tag == "Type")
                        {
                            element3 = element4;
                        }
                        else if (element4.Tag == "Flags")
                        {
                            element2 = element4;
                            break;
                        }
                    }
                    num = (short) (num + 1);
                    if ((element2 == null) || this.CheckFlags(element2.Text.Trim()))
                    {
                        EntityDefProperties properties2 = new EntityDefProperties {
                            Name = element.Tag
                        };
                        if (element3 != null)
                        {
                            properties2.VType = TypeMapping.GetVObject(element3.Text.Trim());
                        }
                        if (!dictionary.ContainsKey((ushort) num))
                        {
                            dictionary.Add((ushort) num, properties2);
                        }
                    }
                }
            }
            return dictionary;
        }

        public void InitEntityData()
        {
            this.InitEntityData(base.m_resourcePath + SystemConfig.ENTITY_DEFS_PATH);
        }

        public void InitEntityData(string xmlPath)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.InitEntityData(xmlPath, "entities" + base.m_fileExtention);
            stopwatch.Stop();
            LoggerHelper.Debug("Getting MD5", true, 0);
            EventDispatcher.TriggerEvent(VersionEvent.GetContentMD5);
            LoggerHelper.Debug("InitEntityData time: " + stopwatch.ElapsedMilliseconds, true, 0);
        }

        public void InitEntityData(string defineFilePath, string defineListFileName)
        {
            try
            {
                SecurityElement element;
                if (base.m_isUseOutterConfig)
                {
                    element = XMLParser.LoadOutter(defineFilePath + defineListFileName);
                }
                else
                {
                    element = XMLParser.Load(defineFilePath + defineListFileName);
                }
                if (element == null)
                {
                    LoggerHelper.Error("Entity file load failed: " + defineFilePath + defineListFileName, true);
                }
                ArrayList children = element.Children;
                if ((children != null) && (children.Count != 0))
                {
                    this.m_entitysByName.Clear();
                    for (int i = 0; i < children.Count; i++)
                    {
                        SecurityElement element2 = children[i] as SecurityElement;
                        this.ParseEntitiesXmlFile(defineFilePath + element2.Tag + base.m_fileExtention, element2.Tag, (ushort) (i + 1), !string.IsNullOrEmpty(element2.Text));
                    }
                    foreach (EntityDef def in this.m_entitysByName.Values)
                    {
                        foreach (EntityDef def2 in this.m_entitysByName.Values)
                        {
                            if (def.ParentName == def2.Name)
                            {
                                def.Parent = def2;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new DefineParseException(string.Format("Define parse error.\nreason: \n{0}", exception.Message), exception);
            }
        }

        private void InitHmfEntityData(string defineFilePath, string defineFileListName)
        {
            try
            {
                byte[] buffer = null;
                if (base.m_isUseOutterConfig)
                {
                    buffer = XMLParser.LoadBytes(defineFilePath + defineFileListName);
                }
                else
                {
                    buffer = XMLParser.LoadBytes(defineFilePath + defineFileListName);
                }
                if (buffer == null)
                {
                    LoggerHelper.Error("Entity file load failed: " + defineFilePath + defineFileListName, true);
                }
                Hmf hmf = new Hmf();
                MemoryStream stream = new MemoryStream(buffer);
                stream.Seek(0L, SeekOrigin.Begin);
                Dictionary<object, object> dictionary = (Dictionary<object, object>) hmf.ReadObject(stream);
                List<object> list = (List<object>) dictionary["entities"];
                if ((list != null) && (list.Count != 0))
                {
                    this.m_entitysByName.Clear();
                    for (int i = 0; i < list.Count; i++)
                    {
                        this.ParseEntitiesHmfFile(defineFilePath + ((string) list[i]) + base.m_fileExtention, (string) list[i], (ushort) (i + 1), false);
                    }
                    foreach (EntityDef def in this.m_entitysByName.Values)
                    {
                        foreach (EntityDef def2 in this.m_entitysByName.Values)
                        {
                            if (def.ParentName == def2.Name)
                            {
                                def.Parent = def2;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new DefineParseException(string.Format("Define parse error.\nreason: \n{0}", exception.Message), exception);
            }
        }

        private EntityDef ParseDef(ArrayList xmlRoot)
        {
            EntityDef entity = new EntityDef();
            foreach (SecurityElement element in xmlRoot)
            {
                if (element.Tag == "Parent")
                {
                    entity.ParentName = element.Text.Trim();
                }
                else if (element.Tag == "ClientMethods")
                {
                    this.GetClientMethods(entity, element.Children);
                }
                else if (element.Tag == "BaseMethods")
                {
                    this.GetBaseMethods(entity, element.Children);
                }
                else if (element.Tag == "CellMethods")
                {
                    this.GetCellMethods(entity, element.Children);
                }
                else if (element.Tag == "Properties")
                {
                    entity.Properties = this.GetProperties(element.Children);
                    entity.PropertiesList = entity.Properties.Values.ToList<EntityDefProperties>();
                }
            }
            return entity;
        }

        private void ParseEntitiesHmf(Dictionary<object, object> def, string etyName, ushort entityID)
        {
            if ((def != null) && (def.Count != 0))
            {
                EntityDef def2 = this.ParseHmfDef(def);
                def2.ID = entityID;
                def2.Name = etyName;
                if (!this.m_entitysByName.ContainsKey(def2.Name))
                {
                    this.m_entitysByName.Add(def2.Name, def2);
                }
                if (!this.m_entitysByID.ContainsKey(def2.ID))
                {
                    this.m_entitysByID.Add(def2.ID, def2);
                }
            }
        }

        private void ParseEntitiesHmfFile(string fileName, string etyName, ushort entityID, bool needMD5 = false)
        {
            byte[] buffer = null;
            if (base.m_isUseOutterConfig)
            {
                buffer = Utils.LoadByteFile(fileName.Replace('\\', '/'));
            }
            else
            {
                buffer = XMLParser.LoadBytes(fileName);
            }
            Hmf hmf = new Hmf();
            MemoryStream stream = new MemoryStream(buffer);
            stream.Seek(0L, SeekOrigin.Begin);
            Dictionary<object, object> def = (Dictionary<object, object>) hmf.ReadObject(stream);
            this.ParseEntitiesHmf(def, etyName, entityID);
        }

        private void ParseEntitiesXml(SecurityElement xmlDoc, string etyName, ushort entityID)
        {
            ArrayList children = xmlDoc.Children;
            if ((children != null) && (children.Count != 0))
            {
                EntityDef def = this.ParseDef(children);
                def.ID = entityID;
                def.Name = etyName;
                if (!this.m_entitysByName.ContainsKey(def.Name))
                {
                    this.m_entitysByName.Add(def.Name, def);
                }
                if (!this.m_entitysByID.ContainsKey(def.ID))
                {
                    this.m_entitysByID.Add(def.ID, def);
                }
            }
        }

        private void ParseEntitiesXmlFile(string fileName, string etyName, ushort entityID, bool needMD5 = false)
        {
            string str;
            if (base.m_isUseOutterConfig)
            {
                str = Utils.LoadFile(fileName.Replace('\\', '/'));
            }
            else
            {
                str = XMLParser.LoadText(fileName);
            }
            if (needMD5)
            {
                LoggerHelper.Debug("AddMD5Content: " + fileName, true, 0);
                EventDispatcher.TriggerEvent<string>(VersionEvent.AddMD5Content, str);
            }
            SecurityElement xmlDoc = XMLParser.LoadXML(str);
            if (xmlDoc != null)
            {
                this.ParseEntitiesXml(xmlDoc, etyName, entityID);
            }
        }

        private void ParseEntitiesXmlString(string xml, string etyName, ushort entityID)
        {
            SecurityElement xmlDoc = XMLParser.LoadXML(xml);
            this.ParseEntitiesXml(xmlDoc, etyName, entityID);
        }

        private EntityDef ParseHmfDef(Dictionary<object, object> def)
        {
            EntityDef etyDef = new EntityDef();
            foreach (KeyValuePair<object, object> pair in def)
            {
                if (((string) pair.Key) != "Parent")
                {
                    if (((string) pair.Key) == "ClientMethods")
                    {
                        this.GetHmfClientMethods(etyDef, (List<object>) pair.Value);
                    }
                    else if (((string) pair.Key) == "BaseMethods")
                    {
                        this.GetHmfBaseMethods(etyDef, (List<object>) pair.Value);
                    }
                    else if (((string) pair.Key) == "CellMethods")
                    {
                        this.GetHmfCellMethods(etyDef, (List<object>) pair.Value);
                    }
                    else if (((string) pair.Key) == "Properties")
                    {
                        etyDef.Properties = this.GetHmfProperties((List<object>) pair.Value);
                        etyDef.PropertiesList = etyDef.Properties.Values.ToList<EntityDefProperties>();
                    }
                }
            }
            return etyDef;
        }

        public Dictionary<string, EntityDef> EntitysByName
        {
            get
            {
                return this.m_entitysByName;
            }
        }

        public static DefParser Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

