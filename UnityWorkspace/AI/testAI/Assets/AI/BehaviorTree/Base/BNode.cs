namespace Game.AI.BehaviorTree
{
	using UnityEngine;
	using System.Collections;
	
	public enum BTResult
	{ 
		NONE,
		FAILURE,
		RUNNING,
		SUCCESS
	}


	/// <summary>
	///所有节点的基类 
	/// </summary>
	public class BNode 
	{
		//public BNode nodeParent;
		private BTResult m_eState;

		public BNode()
		{
			//this.nodeParent = null;
			this.m_eState = BTResult.NONE;
		}

        //public void SetNodeParent(BNode _nodeParent)
        //{
        //    this.nodeParent = _nodeParent;
        //}

        public virtual void Clear()
        {
            //this.nodeParent = null;
        }
		

		public BTResult RunNode(BInput input)
		{
			if(this.m_eState == BTResult.NONE)
			{
				this.OnEnter(input);
				this.m_eState = BTResult.RUNNING;
			}
			BTResult res = this.Execute(input);
			if(res != BTResult.RUNNING)
			{
				this.OnExit(input);
				this.m_eState = BTResult.NONE;
			}
			return res;
		}
		
		//enter
		public virtual void OnEnter(BInput input)
		{
			//
		}

		//exit
		public virtual void OnExit(BInput input)
		{
			//
		}

        // 节点执行
		public virtual BTResult Execute(BInput input) 
        {
            return BTResult.FAILURE;
        }
	}





}

