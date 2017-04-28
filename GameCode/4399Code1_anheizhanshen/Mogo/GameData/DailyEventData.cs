namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class DailyEventData : Mogo.GameData.GameData<DailyEventData>
    {
        public static readonly string fileName = "xml/day_task";

        public int exp { get; protected set; }

        public List<int> finish { get; protected set; }

        public int gold { get; protected set; }

        public int group { get; protected set; }

        public int icon { get; protected set; }

        public List<int> level { get; protected set; }

        public int title { get; protected set; }

        public List<int> viplevel { get; protected set; }
    }
}

