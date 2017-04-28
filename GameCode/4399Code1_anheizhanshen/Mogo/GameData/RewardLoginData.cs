namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class RewardLoginData : Mogo.GameData.GameData<RewardLoginData>
    {
        public static readonly string fileName = "xml/Reward_Login";

        public int days { get; protected set; }

        public int energy { get; protected set; }

        public int exp { get; protected set; }

        public Dictionary<int, int> extra_items { get; protected set; }

        public int gold { get; protected set; }

        public Dictionary<int, int> items1 { get; protected set; }

        public Dictionary<int, int> items2 { get; protected set; }

        public Dictionary<int, int> items3 { get; protected set; }

        public Dictionary<int, int> items4 { get; protected set; }

        public List<int> level { get; protected set; }

        public Dictionary<int, int> soldItem { get; protected set; }

        public int soldItemCost { get; protected set; }

        public int soldItemName { get; protected set; }
    }
}

