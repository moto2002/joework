using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class testAI : MonoBehaviour
{
    private GameObject target;
    private FSM_Manager fsm;

    FollowPathState follow;
    ChasePlayerState chase;

    float timeStep;
    float time;

    public FindPath myAxing = new FindPath();

    public void Start()
    {
        target = GameObject.Find("EndPoint");
        myAxing.mapGrid = GameObject.Find("ShowGrid").GetComponent<Grid>();

        MakeFSM();
    }
    public void MakeFSM()
    {
        //List<Node> tempPath = GetComponent<FindPath>().GetFinalPath(this.gameObject, target);
        List<Node> tempPath = myAxing.GetFinalPath(this.gameObject, target);
        follow = new FollowPathState();
        follow.SetWayPoints(tempPath);
        follow.AddTransition(Transition.SeePlayerTrans, StateID.ChaseingplayerId);

        chase = new ChasePlayerState();
        chase.AddTransition(Transition.LosePlayerTrans, StateID.FollowPathId);

        fsm = new FSM_Manager();
        fsm.AddFsmState(follow);
        fsm.AddFsmState(chase);

        //SetTransition(Transition.LosePlayerTrans);
    }

    public void FixedUpdate()
    {
        fsm.CurrentState.Reason(target, this.gameObject);
        fsm.CurrentState.Act(target, this.gameObject);
    }
    //public void LateUpdate()
    //{
    //    fsm.CurrentState.Reason(target, this.gameObject);
    //    fsm.CurrentState.Act(target, this.gameObject);
    //}
    public void SetTransition(Transition t)
    {
        fsm.ChangeFsmState(t);
    }
}


