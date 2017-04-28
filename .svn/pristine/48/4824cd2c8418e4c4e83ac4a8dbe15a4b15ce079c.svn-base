namespace Game.AI.BehaviorTree 
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	
	public class BTreeMgr 
	{
		private static BTreeMgr instance;
		public static BTreeMgr GetInstance()
		{
			if (instance == null)
				instance = new BTreeMgr ();
			return instance;
		}

		public Dictionary<string,BTree> m_mapTree = new Dictionary<string, BTree>();

		public void Add(BTree tree)
		{
			if (this.m_mapTree.ContainsKey (tree.name))
			{
				Debug.LogError("The tree id is exist.");
			} 
			else 
			{
				this.m_mapTree.Add(tree.name,tree);
			}
		}

		public void Remove(BTree tree)
		{
			if(tree == null ) return;
			if( this.m_mapTree.ContainsKey(tree.name))
				this.m_mapTree.Remove(tree.name);
			else
				Debug.LogError("The tree id is not exist.");
			return;
		}

		public BTree GetTree(string name)
		{
			if( this.m_mapTree.ContainsKey(name))
				return this.m_mapTree[name];
			return null;
		}

		public List<BTree> GetTreeValues()
		{
			List<BTree> lst = new List<BTree>(m_mapTree.Values);
			return lst;
		}

		public List<string> GetTreeKeys()
		{
			return new List<string>(m_mapTree.Keys);
		}

        public void LoadConfigCreateBehaviorTree(string s)
        { 
            //遍历目录下的所有AI配置文件
            //解析所有的配置，加到字典里面存储
        }
	}
}

