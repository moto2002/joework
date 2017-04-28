namespace Mogo.RPC
{
    using System;
    using System.Collections.Generic;

    public class EntityDefMethod
    {
        private List<VObject> m_argsType;
        private ushort m_funcID;
        private string m_funcName;

        public override string ToString()
        {
            return this.FuncName;
        }

        public List<VObject> ArgsType
        {
            get
            {
                return this.m_argsType;
            }
            set
            {
                this.m_argsType = value;
            }
        }

        public ushort FuncID
        {
            get
            {
                return this.m_funcID;
            }
            set
            {
                this.m_funcID = value;
            }
        }

        public string FuncName
        {
            get
            {
                return this.m_funcName;
            }
            set
            {
                this.m_funcName = value;
            }
        }
    }
}

