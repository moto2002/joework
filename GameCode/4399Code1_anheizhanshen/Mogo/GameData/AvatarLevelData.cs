namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class AvatarLevelData : Mogo.GameData.GameData<AvatarLevelData>
    {
        public static readonly string fileName = "xml/AvatarLevel";

        public static int GetExpStandard(int level)
        {
            if (Mogo.GameData.GameData<AvatarLevelData>.dataMap.ContainsKey(level))
            {
                return Mogo.GameData.GameData<AvatarLevelData>.dataMap[level].expStandard;
            }
            return 0;
        }

        public static int GetGoldStandard(int level)
        {
            if (Mogo.GameData.GameData<AvatarLevelData>.dataMap.ContainsKey(level))
            {
                return Mogo.GameData.GameData<AvatarLevelData>.dataMap[level].goldStandard;
            }
            return 0;
        }

        public int expStandard { get; protected set; }

        public int goldStandard { get; protected set; }

        public int maxEnergy { get; protected set; }

        public int nextLevelExp { get; protected set; }
    }
}

