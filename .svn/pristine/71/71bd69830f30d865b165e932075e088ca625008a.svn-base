//using System;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class NPC_Control : MonoBehaviour
//{
//    public GameObject player;
//    public Transform[] path;
//    private FSM_Manager fsm;

//    public void Start()
//    {

//        MakeFSM();
//    }
//    private void MakeFSM()                                                                    
//    {
//        FollowPathState follow = new FollowPathState(path);
//        follow.AddTransition(Transition.SeePlayerTrans, StateID.ChaseingplayerId);

//        ChasePlayerState chase = new ChasePlayerState();
//        chase.AddTransition(Transition.LosePlayerTrans, StateID.FollowPathId);

//        fsm = new FSM_Manager();
//        fsm.AddFsmState(follow);
//        fsm.AddFsmState(chase);
//    }

//    public void FixedUpdate()
//    {   
//        //根据当前是什么状态,做什么样的行为
//        fsm.CurrentState.Reason(player, gameObject);
//        fsm.CurrentState.Act(player, gameObject);
//    }

//    // The NPC has two states: FollowPath and ChasePlayer                                                               transition
//    // If it's on the first state and SawPlayer transition is fired, it changes to ChasePlayer              FollowPath------------->ChasePlayer
//    // If it's on ChasePlayerState and LostPlayer transition is fired, it returns to FollowPath                       <-------------
//    //                                                                                                                   transition
//    public void SetTransition(Transition t)
//    {
//        //改变状态
//        fsm.ChangeFsmState(t);
//    }
//}

///// <summary>
///// NPC都有哪些状态,继承自FSM_State
///// </summary>
//public class FollowPathState : FSM_State
//{
//    private int currentWayPoint;
//    private Transform[] waypoints;

//    public FollowPathState(Transform[] wp)
//    {
//        waypoints = wp;
//        currentWayPoint = 0;
//        ID = StateID.FollowPathId;
//    }

//    public override void Reason(GameObject player, GameObject npc)
//    {
//        // If the Player passes less than 15 meters away in front of the NPC
//        RaycastHit hit;
//        if (Physics.Raycast(npc.transform.position, npc.transform.forward, out hit, 15F))
//        {
//            if (hit.transform.gameObject.tag == "Player")
//                npc.GetComponent<NPC_Control>().SetTransition(Transition.SeePlayerTrans);
//        }
//    }

//    public override void Act(GameObject player, GameObject npc)
//    {
//        // Follow the path of waypoints
//        // Find the direction of the current way point 
//        Vector3 vel = npc.rigidbody.velocity;
//        Vector3 moveDir = waypoints[currentWayPoint].position - npc.transform.position;

//        if (moveDir.magnitude < 1)
//        {
//            currentWayPoint++;
//            if (currentWayPoint >= waypoints.Length)
//            {
//                currentWayPoint = 0;
//            }
//        }
//        else
//        {
//            vel = moveDir.normalized * 10;

//            // Rotate towards the waypoint
//            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
//                                                      Quaternion.LookRotation(moveDir),
//                                                      5 * Time.deltaTime);
//            npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);

//        }

//        // Apply the Velocity
//        npc.rigidbody.velocity = vel;
//    }

//} // FollowPathState
//public class ChasePlayerState : FSM_State
//{
//    public ChasePlayerState()
//    {
//        ID = StateID.ChaseingplayerId;
//    }

//    public override void Reason(GameObject player, GameObject npc)
//    {
//        // If the player has gone 30 meters away from the NPC, fire LostPlayer transition
//        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 30)
//            npc.GetComponent<NPC_Control>().SetTransition(Transition.LosePlayerTrans);
//    }

//    public override void Act(GameObject player, GameObject npc)
//    {
//        // Follow the path of waypoints
//        // Find the direction of the player 		
//        Vector3 vel = npc.rigidbody.velocity;
//        Vector3 moveDir = player.transform.position - npc.transform.position;

//        // Rotate towards the waypoint
//        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
//                                                  Quaternion.LookRotation(moveDir),
//                                                  5 * Time.deltaTime);
//        npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);

//        vel = moveDir.normalized * 10;

//        // Apply the new Velocity
//        npc.rigidbody.velocity = vel;
//    }

//} // ChasePlayerState