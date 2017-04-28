using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindPath
{
    public Grid mapGrid;
    private GameObject npc, target;
    private List<Node> FinalPath = new List<Node>();

    public List<Node> GetFinalPath(GameObject self, GameObject target)
    {
        FindingPath(self.transform.position, target.transform.position);
        return FinalPath;
    }

    public void FindingPath(Vector3 _startPos, Vector3 _endPos)
    {
        //Node startNode = mapGrid.GetFromPositon(_startPos);
        //Node endNode = mapGrid.GetFromPositon(_endPos);

        Node startNode = GetFromPositon(_startPos);
        Node endNode = GetFromPositon(_endPos);

        //开启列表
        List<Node> openSet = new List<Node>();

        //关闭列表
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            if (currentNode == endNode)
            {
                GeneratePath(startNode,endNode);
                return;
            }
               

            foreach (var node in GetNeibourhood(currentNode))
            {
                if (!node.canWalk || closeSet.Contains(node)) continue;
                int newCost = currentNode.gCost + GetNodesDistance(currentNode,node);
                if (newCost < node.gCost || !openSet.Contains(node))
                {
                    node.gCost = newCost;
                    node.hCost = GetNodesDistance(node,endNode);
                    node.parent = currentNode;
                    if (!openSet.Contains(node))
                    {
                        openSet.Add(node);
                    }
                }
            }
        }
    }
    private void GeneratePath(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();
        Node temp = endNode;
        while (temp != startNode)
        {
            path.Add(temp);
            temp = temp.parent;
        }
        path.Reverse();    
        FinalPath = path;

        mapGrid.AllPath.Add(FinalPath);
    }
    public int GetNodesDistance(Node a, Node b)
    {
        int countX = Mathf.Abs(a.gridX - b.gridX);
        int countY = Mathf.Abs(a.gridY - b.gridY);
        if (countX > countY)
        {
            return 14 * countY + 10 * (countX - countY);
        }
        else
        {
            return 14 * countX + 10 * (countY - countX);
        }
    }

    //角色在哪个节点之上
    public Node GetFromPositon(Vector3 _PlayerPos)
    {
        float percentX = (_PlayerPos.x + mapGrid.gridSize.x / 2) / mapGrid.gridSize.x;
        float percentY = (_PlayerPos.z + mapGrid.gridSize.y / 2) / mapGrid.gridSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((mapGrid.gridCountX - 1) * percentX);
        int y = Mathf.RoundToInt((mapGrid.gridCountY - 1) * percentY);

        return mapGrid.grid[x, y];
    }

    //获取节点周围的节点
    public List<Node> GetNeibourhood(Node node)
    {
        List<Node> neribourhood = new List<Node>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                int tempX = node.gridX + i;
                int tempY = node.gridY + j;
                if (tempX > 0 && tempY > 0 && tempX < mapGrid.gridCountX && tempY < mapGrid.gridCountY)
                {
                    neribourhood.Add(mapGrid.grid[tempX, tempY]);
                }
            }
        }
        return neribourhood;
    }

}
