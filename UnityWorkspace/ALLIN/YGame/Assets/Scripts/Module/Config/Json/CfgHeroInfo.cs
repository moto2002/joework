// Generator by Excel2Json2CSharp

using System;
using System.Collections.Generic;

namespace HuanJueGameData
{

    public class HeroInfo 
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string iconName { get; set; }
        public string modelName { get; set; }
        public double scale { get; set; } //float ?
        public int skillId1 { get; set; }
        public int skillId2 { get; set; }
    }

    public class CfgHeroInfo 
    {
        //IList ?
        public List<HeroInfo> HeroInfo { get; set; }
    }

}
