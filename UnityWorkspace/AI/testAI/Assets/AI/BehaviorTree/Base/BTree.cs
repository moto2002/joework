
namespace Game.AI.BehaviorTree 
{
	using UnityEngine;
	using System.Collections;

	/// <summary>
	///树 
	/// </summary>
	public class BTree
	{
		public string name;
		public CompositeNode root;//根节点

		public BTree()
		{
            root = new CompositeNode();
		}

		public void SetRoot(BNode node)
		{
            this.root = node as CompositeNode;
		}
		
		public void Clear()
		{
			this.root = null;
		}
		
        public void AddNode(BNode node)
        {
            this.root.childNodes.AddChildNode(node);
        }

        public void LoadConfig(string path)
        { 

        }

        //执行整棵树
        public void Run(BInput input)
        {
            this.root.RunNode(input);
        }

	}
}