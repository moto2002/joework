namespace Mogo.GameData
{
    using Mogo.Util;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public abstract class GameData
    {
        protected GameData()
        {
        }

        protected static Dictionary<int, T> GetDataMap<T>()
        {
            Dictionary<int, T> dictionary;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Type type = typeof(T);
            FieldInfo field = type.GetField("fileName");
            if (field != null)
            {
                string fileName = field.GetValue(null) as string;
                dictionary = GameDataControler.Instance.FormatData(fileName, typeof(Dictionary<int, T>), type) as Dictionary<int, T>;
            }
            else
            {
                dictionary = new Dictionary<int, T>();
            }
            stopwatch.Stop();
            LoggerHelper.Info(type + " time: " + stopwatch.ElapsedMilliseconds, true);
            return dictionary;
        }

        public int id { get; protected set; }
    }
}

