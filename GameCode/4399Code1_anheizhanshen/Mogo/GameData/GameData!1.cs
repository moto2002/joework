namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;

    public abstract class GameData<T> : Mogo.GameData.GameData where T: Mogo.GameData.GameData<T>
    {
        private static Dictionary<int, T> m_dataMap;

        protected GameData()
        {
        }

        public static Dictionary<int, T> dataMap
        {
            get
            {
                if (Mogo.GameData.GameData<T>.m_dataMap == null)
                {
                    Mogo.GameData.GameData<T>.m_dataMap = Mogo.GameData.GameData.GetDataMap<T>();
                }
                return Mogo.GameData.GameData<T>.m_dataMap;
            }
            set
            {
                Mogo.GameData.GameData<T>.m_dataMap = value;
            }
        }
    }
}

