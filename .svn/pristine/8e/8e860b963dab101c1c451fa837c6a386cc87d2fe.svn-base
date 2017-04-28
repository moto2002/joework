namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class SkillIdReflectData : Mogo.GameData.GameData<SkillIdReflectData>
    {
        public static readonly string fileName = "xml/SkillIdReflect";

        public static int GetReflectSkillId(int srcId)
        {
            foreach (SkillIdReflectData data in Mogo.GameData.GameData<SkillIdReflectData>.dataMap.Values)
            {
                if (data.avatarSkillId == srcId)
                {
                    return data.mercenarySkillId;
                }
            }
            return -1;
        }

        public int aiSlot { get; protected set; }

        public int avatarSkillId { get; protected set; }

        public int mercenarySkillId { get; protected set; }
    }
}

