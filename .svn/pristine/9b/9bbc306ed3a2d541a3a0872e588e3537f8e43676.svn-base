using UnityEngine;
using System.Collections;
using Game.AI.BehaviorTree;

public class TestBehaviorTree : MonoBehaviour 
{

	BTree mbt;

	void Start ()
    {
        #region 手动化


		#region Selector
//		mbt = new BTree();
//				SelectorNode sel = new SelectorNode();
//				mbt.SetRoot(sel);
//		
//				TestContionalNode1 tCon1 = new TestContionalNode1();
//				TestActionNode1 tAct1 = new TestActionNode1();
//				sel.childNodes.AddChildNode(tCon1);
//				sel.childNodes.AddChildNode(tAct1);
		#endregion

		#region Sequence
//		mbt = new BTree();
//		SequenceNode seq = new SequenceNode();
//		mbt.SetRoot(seq);
//			TestContionalNode1 tCon1 = new TestContionalNode1();
//			TestActionNode1 tAct1 = new TestActionNode1();
//			seq.childNodes.AddChildNode(tCon1);
//			seq.childNodes.AddChildNode(tAct1);
		#endregion

		#region 复杂点的树
		mbt = new BTree();
		SelectorNode selNode = new SelectorNode();
		mbt.SetRoot(selNode);
			
			SequenceNode sqnd = new SequenceNode();
				TestContionalNode tCon = new TestContionalNode();
				TestActionNode tAct = new TestActionNode();
				sqnd.childNodes.AddChildNode(tCon);
				sqnd.childNodes.AddChildNode(tAct);
	
			SequenceNode seld = new SequenceNode();
				TestContionalNode1 tCon1 = new TestContionalNode1();
				TestActionNode1 tAct1 = new TestActionNode1();
				seld.childNodes.AddChildNode(tCon1);
				seld.childNodes.AddChildNode(tAct1);

		//selNode.childNodes.AddChildNode(sqnd);
		//selNode.childNodes.AddChildNode(seld);
		mbt.AddNode(sqnd);
		mbt.AddNode(seld);
		#endregion
          

        #endregion


        #region 自动化
        // this.mbt = BTreeMgr.GetInstance().GetTree("test1");
        #endregion
       

    }	
	
	void Update () 
	{
		mbt.Run (new TestBInput());
	}
}
