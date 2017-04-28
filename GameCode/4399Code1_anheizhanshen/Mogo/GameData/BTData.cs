namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class BTData : Mogo.GameData.GameData<BTData>
    {
        public static readonly string fileName = "xml/BT_AI";

        public float aoi { get; protected set; }

        public float callRange { get; protected set; }

        public float fightRange { get; protected set; }

        public int level { get; protected set; }

        public float patrolRange { get; protected set; }
    }
}

