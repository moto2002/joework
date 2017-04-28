namespace Mogo.GameData
{
    using Mogo.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PowerScoreStarData : Mogo.GameData.GameData<PowerScoreStarData>
    {
        public static readonly string fileName = "xml/PowerScoreStar";
        private static readonly int STARCOUNT = 3;

        public static float CalScoreProgressByModulus(float modulus, int id)
        {
            float num;
            float num2;
            if (modulus < 0f)
            {
                modulus = 0f;
            }
            if (modulus > 1f)
            {
                modulus = 1f;
            }
            if (CheckRowData())
            {
                if (!CheckModulus(id))
                {
                    return 0f;
                }
                num = 0f;
                num2 = 0f;
                if ((modulus >= 0f) && (modulus < Mogo.GameData.GameData<PowerScoreStarData>.dataMap[1].range[id - 1]))
                {
                    num = 0f;
                    num2 = Mogo.GameData.GameData<PowerScoreStarData>.dataMap[1].range[id - 1];
                    goto Label_01A7;
                }
                if ((modulus >= Mogo.GameData.GameData<PowerScoreStarData>.dataMap[1].range[id - 1]) && (modulus < Mogo.GameData.GameData<PowerScoreStarData>.dataMap[2].range[id - 1]))
                {
                    num = Mogo.GameData.GameData<PowerScoreStarData>.dataMap[1].range[id - 1];
                    num2 = Mogo.GameData.GameData<PowerScoreStarData>.dataMap[2].range[id - 1];
                    goto Label_01A7;
                }
                if ((modulus > Mogo.GameData.GameData<PowerScoreStarData>.dataMap[2].range[id - 1]) && (modulus <= Mogo.GameData.GameData<PowerScoreStarData>.dataMap[3].range[id - 1]))
                {
                    num = Mogo.GameData.GameData<PowerScoreStarData>.dataMap[2].range[id - 1];
                    num2 = Mogo.GameData.GameData<PowerScoreStarData>.dataMap[3].range[id - 1];
                    goto Label_01A7;
                }
            }
            return 0f;
        Label_01A7:
            return ((modulus - num) / (num2 - num));
        }

        public static int CalStarNumByModulus(float modulus, int id)
        {
            if (modulus < 0f)
            {
                modulus = 0f;
            }
            if (modulus > 1f)
            {
                modulus = 1f;
            }
            if (CheckRowData())
            {
                if (!CheckModulus(id))
                {
                    return 0;
                }
                if ((modulus >= 0f) && (modulus < Mogo.GameData.GameData<PowerScoreStarData>.dataMap[1].range[id - 1]))
                {
                    return 1;
                }
                if ((modulus >= Mogo.GameData.GameData<PowerScoreStarData>.dataMap[1].range[id - 1]) && (modulus < Mogo.GameData.GameData<PowerScoreStarData>.dataMap[2].range[id - 1]))
                {
                    return 2;
                }
                if ((modulus > Mogo.GameData.GameData<PowerScoreStarData>.dataMap[2].range[id - 1]) && (modulus <= Mogo.GameData.GameData<PowerScoreStarData>.dataMap[3].range[id - 1]))
                {
                    return 3;
                }
            }
            return 0;
        }

        private static bool CheckModulus(int id)
        {
            int num = 1;
            for (num = 1; num <= STARCOUNT; num++)
            {
                if (id > Mogo.GameData.GameData<PowerScoreStarData>.dataMap[num].range.Count)
                {
                    LoggerHelper.Error("out of bounds, id = " + id, true);
                    return false;
                }
            }
            return true;
        }

        private static bool CheckRowData()
        {
            int key = 1;
            key = 1;
            while (key <= STARCOUNT)
            {
                if (!Mogo.GameData.GameData<PowerScoreStarData>.dataMap.ContainsKey(key))
                {
                    LoggerHelper.Error("no contains key = " + key, true);
                    return false;
                }
                key++;
            }
            return ((key - 1) == STARCOUNT);
        }

        public List<float> range { get; protected set; }

        public int star { get; protected set; }
    }
}

