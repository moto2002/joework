using UnityEngine;
using System.Collections;

public class Node
{
    public bool canWalk;//是否可以行走
    public Vector3 worldPos;//节点的位置
    public int gridX, gridY;//地形网格的下标

    public int gCost;//离起始点的耗费
    public int hCost;//离目标点的耗费
    public int fCost { get { return gCost + hCost; } }

    public Node parent;//父对象

    public Node(bool _canWalk, Vector3 _pos, int _x, int _y)
    {
        canWalk = _canWalk;
        worldPos = _pos;
        gridX = _x;
        gridY = _y;
    }

}
