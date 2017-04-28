namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GameProcData : Mogo.GameData.GameData<GameProcData>
    {
        public static readonly string fileName = "xml/GameProcData";
        private static Dictionary<Mogo.GameData.ProcType, Dictionary<string, GameProcData>> m_procData;

        public string Paras { get; protected set; }

        public static Dictionary<Mogo.GameData.ProcType, Dictionary<string, GameProcData>> ProcData
        {
            get
            {
                if (m_procData == null)
                {
                    m_procData = new Dictionary<Mogo.GameData.ProcType, Dictionary<string, GameProcData>>();
                    foreach (GameProcData data in Mogo.GameData.GameData<GameProcData>.dataMap.Values)
                    {
                        if (!string.IsNullOrEmpty(data.Paras))
                        {
                            Mogo.GameData.ProcType procType = (Mogo.GameData.ProcType) data.ProcType;
                            if (!m_procData.ContainsKey(procType))
                            {
                                m_procData.Add(procType, new Dictionary<string, GameProcData>());
                            }
                            m_procData[procType][data.Paras.ToLower()] = data;
                        }
                    }
                }
                return m_procData;
            }
        }

        public byte ProcType { get; protected set; }

        public ushort Progress { get; protected set; }
    }
}

