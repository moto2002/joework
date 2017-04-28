
namespace Game.AI.BehaviorTree
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	/// <summary>
	/// 选择节点
	/// 当执行本类型Node时，它将从begin到end迭代执行自己的Child Node
	/// 如遇到一个Child Node执行后返回True，那停止迭代，本Node向自己的Parent Node也返回True
	/// 否则所有Child Node都返回False，那本Node向自己的Parent Node返回False
	/// </summary>
	public class SelectorNode : CompositeNode
	{
		private int m_iRuningIndex;	//runing index
		
		public SelectorNode()
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
				return BTResult.FAILURE;
			}
			
			BNode node = this.childNodes.nodeList[this.m_iRuningIndex];
			
			BTResult res = node.RunNode(input);
			
			if(res == BTResult.SUCCESS)
				return BTResult.SUCCESS;
			
			if(res == BTResult.FAILURE)
			{
				this.m_iRuningIndex++;
			}
			return BTResult.RUNNING;
		}
	}
}