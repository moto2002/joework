namespace Game.AI.BehaviorTree
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class BNodeList 
    {
        public List<BNode> nodeList;

        public BNodeList()
        {
            this.nodeList = new List<BNode>();
        }

        public void AddChildNode(BNode _node)
        {
            this.nodeList.Add(_node);
        }

        public void RemoveChildNode(BNode _node)
        {
            this.nodeList.Remove(_node);
        }

        public BNode FindChildNode(int _index)
        {
            return this.nodeList[_index];
        }

        public void Clear()
        {
            this.nodeList.Clear();
        }
    }
}
