
namespace Game.AI.BehaviorTree 
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	/// <summary>
	/// 顺序节点
	/// 当执行本类型Node时，它将从begin到end迭代执行自己的Child Node
	/// 如遇到一个Child Node执行后返回False，那停止迭代，本Node向自己的Parent Node也返回False
	/// 否则所有Child Node都返回True，那本Node向自己的Parent Node返回True
	/// </summary>
	public class SequenceNode : CompositeNode
	{
		private int m_iRuningIndex;

		public SequenceNode()
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
			
			if(res == BTResult.FAILURE)
				return BTResult.FAILURE;
			
			if(res == BTResult.SUCCESS)
			{
				this.m_iRuningIndex++;
			}
			
			return BTResult.RUNNING;
		}


	}
}