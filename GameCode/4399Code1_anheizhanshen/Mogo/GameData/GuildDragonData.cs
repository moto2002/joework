namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class GuildDragonData : Mogo.GameData.GameData<GuildDragonData>
    {
        public static readonly string fileName = "xml/GuildDragon";

        public int diamond_recharge_cost { get; set; }

        public int diamond_recharge_exp { get; set; }

        public int dragon_limit { get; set; }

        public int get_diamond_reward { get; set; }

        public int get_gold_reward { get; set; }

        public int gold_recharge_cost { get; set; }

        public int gold_recharge_exp { get; set; }

        public int gold_recharge_money { get; set; }

        public int guild_level { get; set; }

        public int player_level { get; set; }
    }
}

