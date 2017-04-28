using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FollowPathState : FSM_State
{
    private int currentWayPoint;
    private List<Node> waypoints;
    CharacterController cc;

    GameObject self, target;

    public FollowPathState()
    {
        ID = StateID.FollowPathId;
    }

    public void SetWayPoints(List<Node> wp)
    {
        currentWayPoint = 0;
        waypoints = wp;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if ((npc.transform.position - player.transform.position).sqrMagnitude <= 2 * 2)
        {
            self = npc;
            target = player;
            cc = npc.GetComponent<CharacterController>();
            Debug.Log("5米以内,寻路切换为攻击");
            //npc.rigidbody.velocity = Vector3.zero;
            //停止
            cc.SimpleMove(Vector3.zero);
            npc.GetComponent<testAI>().SetTransition(Transition.SeePlayerTrans);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        Debug.Log("寻路中......");
        //List<Node> tempPath = self.GetComponent<FindPath>().GetFinalPath(self, target);
        //SetWayPoints(tempPath);


        cc = npc.GetComponent<CharacterController>();

        //Vector3 vel = npc.rigidbody.velocity;
        Vector3 vel = cc.velocity;
        Vector3 moveDir = waypoints[currentWayPoint].worldPos - npc.transform.position;

        if (moveDir.magnitude < 1)
        {
            currentWayPoint++;
            if (currentWayPoint >= waypoints.Count)
            {
                // npc.rigidbody.velocity = Vector3.zero;
                //停止
                if ((npc.transform.position - player.transform.position).sqrMagnitude <= 2 * 2)
                {
                    cc.SimpleMove(Vector3.zero);
                    return;
                }
                else
                {
                    DoBeforeEnter();
                }

                //currentWayPoint = 0;
            }
        }
        else
        {
            vel = moveDir.normalized * 3;

            // Rotate towards the waypoint
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                                      Quaternion.LookRotation(moveDir),
                                                      5 * Time.deltaTime);
            npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);

        }

        // Apply the Velocity
        //npc.rigidbody.velocity = vel;
        cc.SimpleMove(vel);

    }

    public override void DoBeforeMoveing()
    {
        Debug.Log(" before remove");
        //SetWayPoints(null);
    }
    public override void DoBeforeEnter()
    {
        Debug.Log(" before enter:重新获得寻路路径");

        // List<Node> tempPath = self.GetComponent<FindPath>().GetFinalPath(self, target);
        List<Node> tempPath = self.GetComponent<testAI>().myAxing.GetFinalPath(self, target);
        SetWayPoints(tempPath);
    }

}
public class ChasePlayerState : FSM_State
{
    float nextTime;
    float timeStep = 2f;

    public ChasePlayerState()
    {
        ID = StateID.ChaseingplayerId;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 2)
        {
            //List<Node> tempPath = npc.GetComponent<FindPath>().GetFinalPath(npc, player);
            //follow.SetWayPoints(tempPath);

            npc.GetComponent<testAI>().SetTransition(Transition.LosePlayerTrans);
        }





        //npc.GetComponent<testAI>().SetTransition(Transition.LosePlayerTrans);

    }

    public override void Act(GameObject player, GameObject npc)
    {
        if (Time.time > nextTime)
        {
            nextTime += timeStep;
            Debug.Log("攻击中");

            // npc.gameObject.SetActive(false);
        }


        #region
        // Follow the path of waypoints
        // Find the direction of the player 		
        /* Vector3 vel = npc.rigidbody.velocity;
         Vector3 moveDir = player.transform.position - npc.transform.position;

         // Rotate towards the waypoint
         npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                                   Quaternion.LookRotation(moveDir),
                                                   5 * Time.deltaTime);
         npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);

         vel = moveDir.normalized * 10;

         // Apply the new Velocity
         npc.rigidbody.velocity = vel;
         */

        #endregion
    }

}