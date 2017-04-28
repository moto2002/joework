
namespace Game.AI.BehaviorTree 
{
	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// 并行节点
	/// 顺序执行节点无论失败成功,直到结束返回成功
	/// </summary>
	public class ParallelNode : CompositeNode
	{
		private int m_iRuningIndex;	//runting index
		
		public ParallelNode()
		{

		}

		public override void OnEnter (BInput input)
		{
			this.m_iRuningIndex = 0;
		}

		public override BTResult Execute(BInput input)
		{
			if(this.m_iRuningIndex >= this.childNodes.nodeList.Count)
			{
				return BTResult.SUCCESS;
			}
			
			BNode node = this.childNodes.nodeList[this.m_iRuningIndex];
			
			BTResult res = node.RunNode(input);
			
			if(res != BTResult.RUNNING)
			{
				this.m_iRuningIndex++;
			}
			
			return BTResult.RUNNING;
		}
	}
}