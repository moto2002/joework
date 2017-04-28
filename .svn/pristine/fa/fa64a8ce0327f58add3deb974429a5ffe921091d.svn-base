using UnityEngine;
using System.Collections;
using Game.AI.BehaviorTree;

public class TestContionalNode :ConditionNode
{
	public override void OnEnter (BInput input)
	{
		//Debug.Log ("conditional 我来了");
	}

	public override BTResult Execute (BInput input)
	{
		TestBInput testInput = input as TestBInput;
		//Debug.Log (testInput.name + "/" + testInput.age);

		if (testInput.name == "Joe" && testInput.age == 23)
		{
			Debug.Log("相等");
			return BTResult.SUCCESS;
		}
		else 
		{
			Debug.Log("不相等");
			return BTResult.FAILURE;
		}

	}

	public override void OnExit (BInput input)
	{
		//Debug.Log ("conditional 我走了");
	}


}
