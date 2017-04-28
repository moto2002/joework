using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public Node[,] grid;//网格，是Node节点的二维数组
    public Vector2 gridSize;//网格的大小
    public float nodeRadius;//节点的半径
    private float nodeDiameter;//节点的直径

    public LayerMask whatLayer;//是可走层还是不可走层

    public int gridCountX, gridCountY;//每一行、列有几个Node

    public List<List<Node>> AllPath = new List<List<Node>>();//所有人的路径

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridCountX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridCountY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

        grid = new Node[gridCountX, gridCountY];

        CreateGrid();
    }

    void CreateGrid()
    {
        //左下角
        Vector3 startPoint = this.transform.position - gridSize.x / 2 * Vector3.right - gridSize.y / 2 * Vector3.forward;

        for (int i = 0; i < gridCountX; i++)
        {
            for (int j = 0; j < gridCountY; j++)
            {
                Vector3 worldPoint = startPoint + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius *2, whatLayer);//检测半径（直径）范围内是否可行走，发射球形射线检测层
                grid[i, j] = new Node(walkable, worldPoint, i, j);
            }
        }
    }

    void OnDrawGizmos()
    {

        //画地形网格边缘
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        //画节点Node
        if (grid == null) return;
        foreach (var node in grid)
        {
            Gizmos.color = node.canWalk ? Color.white : Color.red;
            Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .1f));
        }
        //画角色
        /*  Node playerNode = GetFromPositon(player.position);
          if (playerNode != null && playerNode.canWalk)
          {
              Gizmos.color = Color.yellow;
              Gizmos.DrawCube(playerNode.worldPos, Vector3.one * (nodeDiameter - .1f));
          }*/

        //画路径
        //if (path != null)
        //{
        //    foreach (var node in path)
        //    {
        //        Gizmos.color = Color.black;
        //        Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .1f));
        //    }
        //}

        if (AllPath.Count > 0)
        {
            for (int i = 0; i < AllPath.Count; i++)
            {
                if (AllPath[i].Count > 0)
                {
                    foreach (var node in AllPath[i])
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .1f));
                    }
                }
            }
        }
    }


}
