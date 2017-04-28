namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class SkillState : Mogo.GameData.GameData<SkillState>
    {
        public static readonly string fileName = "xml/SkillState";

        public int act { get; protected set; }

        public int attackAble { get; protected set; }

        public int direction { get; protected set; }

        public int hittable { get; protected set; }

        public int immuneShift { get; protected set; }

        public int moveAble { get; protected set; }

        public float moveSpeedRate { get; protected set; }

        public int name { get; protected set; }

        public int sfx { get; protected set; }

        public int showPriority { get; protected set; }

        public int showToOther { get; protected set; }
    }
}

