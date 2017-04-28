namespace Mogo.RPC
{
    using System;

    public class EntityDefProperties
    {
        private bool m_bSaveDb;
        private string m_defaultValue;
        private string m_name;
        private VObject m_vType;

        public override string ToString()
        {
            return this.Name;
        }

        public bool BSaveDb
        {
            get
            {
                return this.m_bSaveDb;
            }
            set
            {
                this.m_bSaveDb = value;
            }
        }

        public string DefaultValue
        {
            get
            {
                return this.m_defaultValue;
            }
            set
            {
                this.m_defaultValue = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                this.m_name = value;
            }
        }

        public VObject VType
        {
            get
            {
                return this.m_vType;
            }
            set
            {
                this.m_vType = value;
            }
        }
    }
}

