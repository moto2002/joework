namespace Mogo.RPC
{
    using System;
    using System.Collections.Generic;

    public class EntityDef
    {
        private Dictionary<ushort, EntityDefMethod> m_baseMethodsByID;
        private Dictionary<string, EntityDefMethod> m_baseMethodsByName;
        private bool m_bHasCellClient;
        private Dictionary<ushort, EntityDefMethod> m_cellMethodsByID;
        private Dictionary<string, EntityDefMethod> m_cellMethodsByName;
        private Dictionary<ushort, EntityDefMethod> m_clientMethodsByID;
        private Dictionary<string, EntityDefMethod> m_clientMethodsByName;
        private ushort m_id;
        private string m_name;
        private EntityDef m_parent;
        private string m_parentName;
        private Dictionary<ushort, EntityDefProperties> m_properties;
        private List<EntityDefProperties> m_propertiesList;
        private string m_strUniqueIndex;

        public override string ToString()
        {
            return this.Name;
        }

        public EntityDefMethod TryGetBaseMethod(string name)
        {
            return this.TryGetBaseMethod(name, this);
        }

        public EntityDefMethod TryGetBaseMethod(ushort id)
        {
            return this.TryGetBaseMethod(id, this);
        }

        public EntityDefMethod TryGetBaseMethod(string name, EntityDef entity)
        {
            EntityDefMethod method = null;
            if (entity != null)
            {
                entity.BaseMethodsByName.TryGetValue(name, out method);
                if (method == null)
                {
                    return this.TryGetBaseMethod(name, entity.Parent);
                }
            }
            return method;
        }

        public EntityDefMethod TryGetBaseMethod(ushort id, EntityDef entity)
        {
            EntityDefMethod method = null;
            if (entity != null)
            {
                entity.BaseMethodsByID.TryGetValue(id, out method);
                if (method == null)
                {
                    return this.TryGetBaseMethod(id, entity.Parent);
                }
            }
            return method;
        }

        public EntityDefMethod TryGetCellMethod(string name)
        {
            return this.TryGetCellMethod(name, this);
        }

        public EntityDefMethod TryGetCellMethod(ushort id)
        {
            return this.TryGetCellMethod(id, this);
        }

        public EntityDefMethod TryGetCellMethod(string name, EntityDef entity)
        {
            EntityDefMethod method = null;
            if (entity != null)
            {
                entity.CellMethodsByName.TryGetValue(name, out method);
                if (method == null)
                {
                    return this.TryGetCellMethod(name, entity.Parent);
                }
            }
            return method;
        }

        public EntityDefMethod TryGetCellMethod(ushort id, EntityDef entity)
        {
            EntityDefMethod method = null;
            if (entity != null)
            {
                entity.CellMethodsByID.TryGetValue(id, out method);
                if (method == null)
                {
                    return this.TryGetCellMethod(id, entity.Parent);
                }
            }
            return method;
        }

        public EntityDefMethod TryGetClientMethod(string name)
        {
            return this.TryGetClientMethod(name, this);
        }

        public EntityDefMethod TryGetClientMethod(ushort id)
        {
            return this.TryGetClientMethod(id, this);
        }

        public EntityDefMethod TryGetClientMethod(string name, EntityDef entity)
        {
            EntityDefMethod method = null;
            if (entity != null)
            {
                entity.ClientMethodsByName.TryGetValue(name, out method);
                if (method == null)
                {
                    return this.TryGetClientMethod(name, entity.Parent);
                }
            }
            return method;
        }

        public EntityDefMethod TryGetClientMethod(ushort id, EntityDef entity)
        {
            EntityDefMethod method = null;
            if (entity != null)
            {
                entity.ClientMethodsByID.TryGetValue(id, out method);
                if (method == null)
                {
                    return this.TryGetClientMethod(id, entity.Parent);
                }
            }
            return method;
        }

        public Dictionary<ushort, EntityDefMethod> BaseMethodsByID
        {
            get
            {
                return this.m_baseMethodsByID;
            }
            set
            {
                this.m_baseMethodsByID = value;
            }
        }

        public Dictionary<string, EntityDefMethod> BaseMethodsByName
        {
            get
            {
                return this.m_baseMethodsByName;
            }
            set
            {
                this.m_baseMethodsByName = value;
            }
        }

        public bool BHasCellClient
        {
            get
            {
                return this.m_bHasCellClient;
            }
            set
            {
                this.m_bHasCellClient = value;
            }
        }

        public Dictionary<ushort, EntityDefMethod> CellMethodsByID
        {
            get
            {
                return this.m_cellMethodsByID;
            }
            set
            {
                this.m_cellMethodsByID = value;
            }
        }

        public Dictionary<string, EntityDefMethod> CellMethodsByName
        {
            get
            {
                return this.m_cellMethodsByName;
            }
            set
            {
                this.m_cellMethodsByName = value;
            }
        }

        public Dictionary<ushort, EntityDefMethod> ClientMethodsByID
        {
            get
            {
                return this.m_clientMethodsByID;
            }
            set
            {
                this.m_clientMethodsByID = value;
            }
        }

        public Dictionary<string, EntityDefMethod> ClientMethodsByName
        {
            get
            {
                return this.m_clientMethodsByName;
            }
            set
            {
                this.m_clientMethodsByName = value;
            }
        }

        public ushort ID
        {
            get
            {
                return this.m_id;
            }
            set
            {
                this.m_id = value;
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

        public EntityDef Parent
        {
            get
            {
                return this.m_parent;
            }
            set
            {
                this.m_parent = value;
            }
        }

        public string ParentName
        {
            get
            {
                return this.m_parentName;
            }
            set
            {
                this.m_parentName = value;
            }
        }

        public Dictionary<ushort, EntityDefProperties> Properties
        {
            get
            {
                return this.m_properties;
            }
            set
            {
                this.m_properties = value;
            }
        }

        public List<EntityDefProperties> PropertiesList
        {
            get
            {
                return this.m_propertiesList;
            }
            set
            {
                this.m_propertiesList = value;
            }
        }

        public string StrUniqueIndex
        {
            get
            {
                return this.m_strUniqueIndex;
            }
            set
            {
                this.m_strUniqueIndex = value;
            }
        }
    }
}

