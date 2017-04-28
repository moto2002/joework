namespace Game.AI.BehaviorTree 
{
	using UnityEngine;
	using System.Collections;

	//这个节点还是有点问题
	/// <summary>
	/// 顺序执行
	/// 直到连续成功N次就返回成功,否则失败
	/// 注意：是连续
	/// </summary>
	public class IteratorNode : CompositeNode
	{
		public int Num;//需要成功的次数
		
		private int m_iRunningIndex;//正在执行节点下标
		private int m_iRunningNum;//成功的个数
		
		public IteratorNode()
		{
		}

		public override void OnEnter (BInput input)
		{
			this.m_iRunningIndex = 0;
			this.m_iRunningNum = 0;
		}

		public override BTResult Execute (BInput input)
		{
			if(this.m_iRunningIndex >= this.childNodes.nodeList.Count)
			{
				return BTResult.FAILURE;
			}
			
			BTResult res = this.childNodes.nodeList[this.m_iRunningIndex].RunNode(input);
			
			if(res == BTResult.FAILURE)
				return BTResult.FAILURE;
			
			if(res == BTResult.SUCCESS)
			{
				this.m_iRunningIndex++; 
				this.m_iRunningNum++;
			}
			
			if(this.m_iRunningNum >= this.Num)
				return BTResult.SUCCESS;
			
			return BTResult.RUNNING;
		}

	}
}

