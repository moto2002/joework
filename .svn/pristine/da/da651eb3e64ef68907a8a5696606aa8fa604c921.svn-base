namespace Game.AI.BehaviorTree 
{
	
	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// 并行全部子节点
	/// 全部执行完后
	/// 若全部失败，返回成功
	/// 若有一个成功，返回失败
	/// </summary>
	public class ParallelAllFailure:ParallelNode
	{
		private int m_iRunningIndex;	//running index
		private BTResult m_eResult;		//result
		
		public ParallelAllFailure()
		{
		
		}

		public override void OnEnter (BInput input)
		{
			this.m_iRunningIndex = 0;
			this.m_eResult = BTResult.SUCCESS;
		}

		public override BTResult Execute(BInput input)
		{
			if(this.m_iRunningIndex >= this.childNodes.nodeList.Count)
			{
				return this.m_eResult;
			}
			
			BTResult result = this.childNodes.nodeList[this.m_iRunningIndex].RunNode(input);
			if(result == BTResult.SUCCESS)
			{
				this.m_eResult = BTResult.FAILURE;
			}
			if(result != BTResult.RUNNING)
				this.m_iRunningIndex++;
			
			return BTResult.RUNNING;
		}
	}

}

