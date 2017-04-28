namespace Game.AI.BehaviorTree 
{
	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// 随机选择一个节点执行,
	/// 返回该节点的结果
	/// </summary>
	public class RandomNode:CompositeNode
	{
		private int m_iRunningIndex;
		
		public RandomNode()
		{
		}
		
		public override void OnEnter (BInput input)
		{
			this.m_iRunningIndex = Random.Range(0,this.childNodes.nodeList.Count);
			base.OnEnter (input);
		}
	
		public override BTResult Execute (BInput input)
		{
			return this.childNodes.nodeList[this.m_iRunningIndex].RunNode(input);
		}
	}
}


