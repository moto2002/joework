using UnityEngine;
using System.Collections;
using Game.AI.BehaviorTree;

public class TestActionNode:ActionNode
{

	public override void OnEnter (BInput input)
	{
		//Debug.Log ("action 我来了");
	}

	public override BTResult Execute (BInput input)
	{
		Debug.Log ("执行了");
		return BTResult.FAILURE;
	}

	public override void OnExit (BInput input)
	{
		//Debug.Log ("action 我走了");
	}
}
