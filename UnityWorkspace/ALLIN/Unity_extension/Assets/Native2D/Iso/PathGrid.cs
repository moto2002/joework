using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathGrid {

	private int m_gridX = 0 ;
	private int m_gridZ = 0 ;

	private int m_type = 0 ;

	private float m_straightCost = 1f;
	private float m_diagCost = Mathf.Sqrt(2);

	private PathNode m_startNode = null ;
	private PathNode m_endNode = null;

	private List<List<PathNode>> m_nodes = null ; //所有的路点
	private List<PathNode> m_walkableNodes = null;//可以走的路点


	public int type{
		get { return m_type; }
	}
	public int gridX{
		get { return m_gridX; }
	}
	public int gridZ{
		get { return m_gridZ; }
	}


	public PathGrid(){}

	public PathGrid(int gridX, int gridZ){
		m_nodes = new List<List<PathNode>>();
		this.m_gridX = gridX;
		this.m_gridZ = gridZ;
		for(int i=0;i<gridX;i++){
			List<PathNode> v = new List<PathNode>();
			for(int j=0;j<gridZ;j++){
				PathNode node = new PathNode(i,j);
				v.Add(node);
			}
			m_nodes.Add(v);
		}
	}

	public void SetAllWalkable(bool value){
		for(int i=0;i<m_gridX;i++){
			for(int j=0;j<m_gridZ;j++){
				m_nodes[i][j].walkable = value;
			}
		}
	}

	public void ChangeSize(int gridX,int gridZ){
		if(m_gridX!=gridX || m_gridZ!=gridZ){
			List<List<PathNode>> nodes = new List<List<PathNode>>();
			for(int i=0;i<gridX;i++){
				List<PathNode> v = new List<PathNode>();
				for(int j=0;j<gridZ;j++){
					if (i<m_gridX && j<m_gridZ)
					{
						v[j] = m_nodes[i][j];
					}
					else
					{
						PathNode node = new PathNode(i,j);
						v[j] = node;
					}
				}
				nodes.Add(v);
			}
			//clear
			for(int i=0;i<m_gridX;i++){
				for(int j=0;j<m_gridZ;j++){
					if(i>=gridX || j>=gridZ){
						m_nodes[i][j].Destroy();
						m_nodes[i][j] = null;
					}
				}
			}
			m_nodes.Clear();
			m_gridX = gridX;
			m_gridZ = gridZ;
			m_nodes = nodes;
		}
	}

	public List<PathNode> GetNodesByWalkable( bool walkable){
		m_walkableNodes = new List<PathNode>();
		for(int i=0;i<m_gridX;i++){
			for(int j=0;j<m_gridZ;j++){
				if(m_nodes[i][j].walkable==walkable) {
					m_walkableNodes.Add( m_nodes[i][j]);
				}
			}
		}
		return m_walkableNodes;
	}

	public void InitNodeLink( PathNode node , int type){
		int startX = (int)Mathf.Max(0,node.x-1) ;
		int endX =  (int)Mathf.Max(m_gridZ - 1, node.x + 1);
		int startY = (int)Mathf.Max(0, node.y - 1);
		int endY = (int)Mathf.Max(m_gridX - 1, node.y + 1);

		foreach (PathLink link in node.links) {
			link.Destroy();
		}
		node.links.Clear();

		for (int i = startX; i < endX; i++){
			for (int j = startY; j < endY; j++){
				PathNode test = GetNode(i, j);
				if ( test == node || !test.walkable){
					continue;
				}
				if (type != 2 && i != node.x && j != node.y){
					PathNode test2 = GetNode((int)node.x, j);
					if (!test2.walkable){
						continue;
					}
					test2 = GetNode(i, (int)node.y);
					if (!test2.walkable){
						continue;
					}
				}
				float cost = m_straightCost;
				if (!((node.x == test.x) || (node.y == test.y))){
					if (type == 1){
						continue;
					}
					if (type == 2 && (node.x - test.x) * (node.y - test.y) == 1){
						continue;
					}
					if (type == 2){
						cost = m_straightCost;
					} else {
						cost = m_diagCost;
					}
				}
				PathLink link = new PathLink(test,cost);
				node.links.Add(link);
			}
		}
	}

	public bool CheckInGrid( int nodeX, int nodeZ){
		if(nodeX<0 || nodeZ<0 || nodeX>=m_gridX || nodeZ>=m_gridZ) return false;
		return true;
	}

	public PathNode GetNode(int nodeX,int nodeZ){
		return m_nodes[nodeX][nodeZ];
	}

	public void SetStartNode (int nodeX, int nodeZ){
		m_startNode = m_nodes[nodeX][nodeZ];
	}

	public PathNode GetStartNode(){
		return m_startNode;
	}

	public void SetEndNode(int nodeX, int nodeZ){
		m_endNode = m_nodes[nodeX][nodeZ];
	}

	public PathNode GetEndNode(){
		return m_endNode;
	}

	public void SetWalkable( int nodeX, int nodeZ,bool value)
	{
		m_nodes[nodeX][nodeZ].walkable=value;
	}

	public void CalculateLinks(int type){
		m_type = type;
		for (int i = 0; i < m_gridX; ++i ){
			for (int j = 0; j < m_gridZ; ++j){
				InitNodeLink( m_nodes[i][j], m_type);
			}
		}
	}

	public void Destroy(){
		m_startNode = null;
		m_endNode = null;
		if(m_nodes!=null){
			for(int i=0;i<m_gridX;i++){
				for(int j=0;j<m_gridZ;j++){
					m_nodes[i][j].Destroy();
				}
			}
			m_nodes.Clear();
		}

		if(m_walkableNodes!=null){
			foreach(PathNode node in m_walkableNodes){
				node.Destroy();
			}
			m_walkableNodes.Clear();
		}
	}
}
