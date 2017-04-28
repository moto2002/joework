using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public Node[,] grid,_grid;//网格，是Node节点的二维数组
	public Vector2 gridSize;//网格的大小
	private float nodeRadius ;//节点的半径
    public float nodeDiameter;//节点的直径
	
    public int gridCountX, gridCountY;//每一行、列有几个Node

    public List<List<Node>> AllPath = new List<List<Node>>();//所有人的路径

    void Start()
    {
		nodeRadius = nodeDiameter * 0.5f;
        gridCountX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridCountY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

        grid = new Node[gridCountX, gridCountY];
		_grid = new Node[gridCountX, gridCountY];
	
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
              
                grid[i, j] = new Node(false, worldPoint, i, j);
            }
        }

		CreateGrids ();
    }

	void CreateGrids()
	{
		if (grid == null) return;
		GameObject parent = new GameObject ();
		parent.name = "map";
		foreach (var node in grid)
		{
			GameObject go =  GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.transform.parent = parent.transform;
			go.transform.localPosition = node.worldPos;
			go.transform.localScale = Vector3.one * (nodeDiameter - 0.1f);
			node.cube = go;
			if(node.canWalk)
				go.GetComponent<MeshRenderer>().material.color = Color.green;
			else
				go.GetComponent<MeshRenderer>().material.color = Color.red;

		}
	}




	void OnGUI()
	{


		if (GUILayout.Button ("output")) 
		{    
			string mapInfo ="";

			for (int i = 0; i < gridCountX; i++)
			{
				for (int j = 0; j<gridCountY; j++)
				{
					_grid[i,j] = grid[i,gridCountY -1 - j];
				}
			}

			for (int j = 0; j < gridCountY; j++)
			{
				
				for (int i = 0; i < gridCountX; i++)
				{
					if(_grid[i, j].canWalk)
					{
						mapInfo+="1";
					}
					else
					{
						mapInfo+="0";
					}
				}
				mapInfo+="\n";
			}

			Debug.Log(mapInfo);
		}
		
	}
	
	
	
	void OnDrawGizmos()
	{
		
		//画地形网格边缘
		Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        //画节点Node
//        if (grid == null) return;
//        foreach (var node in grid)
//        {
//            Gizmos.color = node.canWalk ? Color.white : Color.red;
//            Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .1f));
//        }
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

//        if (AllPath.Count > 0)
//        {
//            for (int i = 0; i < AllPath.Count; i++)
//            {
//                if (AllPath[i].Count > 0)
//                {
//                    foreach (var node in AllPath[i])
//                    {
//                        Gizmos.color = Color.black;
//                        Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .1f));
//                    }
//                }
//            }
//        }
    }


}
