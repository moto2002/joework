//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HuanJueData
{
    using ProtoBuf;
    using System.Collections.Generic;
    
    
    [ProtoContract()]
    public class SkillInfo
    {
        
        private int m_skillId;
        
        private int m_skillLevel;
        
        private string m_skillName;
        
        private string m_skillIcon;
        
        private int m_type;
        
        private float m_distance;
        
        private int m_range;
        
        /// <summary>
        /// 技能Id
        /// </summary>
        [ProtoMember(1)]
        public int SkillId
        {
            get
            {
                return this.m_skillId;
            }
            set
            {
                this.m_skillId = value;
            }
        }
        
        /// <summary>
        /// 技能等级
        /// </summary>
        [ProtoMember(2)]
        public int SkillLevel
        {
            get
            {
                return this.m_skillLevel;
            }
            set
            {
                this.m_skillLevel = value;
            }
        }
        
        /// <summary>
        /// 名称
        /// </summary>
        [ProtoMember(3)]
        public string SkillName
        {
            get
            {
                return this.m_skillName;
            }
            set
            {
                this.m_skillName = value;
            }
        }
        
        /// <summary>
        /// 技能ICON
        /// </summary>
        [ProtoMember(4)]
        public string SkillIcon
        {
            get
            {
                return this.m_skillIcon;
            }
            set
            {
                this.m_skillIcon = value;
            }
        }
        
        /// <summary>
        /// 类型
        /// </summary>
        [ProtoMember(5)]
        public int Type
        {
            get
            {
                return this.m_type;
            }
            set
            {
                this.m_type = value;
            }
        }
        
        /// <summary>
        /// 距离
        /// </summary>
        [ProtoMember(6)]
        public float Distance
        {
            get
            {
                return this.m_distance;
            }
            set
            {
                this.m_distance = value;
            }
        }
        
        /// <summary>
        /// 范围
        /// </summary>
        [ProtoMember(7)]
        public int Range
        {
            get
            {
                return this.m_range;
            }
            set
            {
                this.m_range = value;
            }
        }
        
        /// <summary>
        /// 主键
        /// </summary>
        public string MainKey
        {
            get
            {
                return this.m_skillId.ToString() + "_" + this.m_skillLevel.ToString();
            }
        }
    }
}
