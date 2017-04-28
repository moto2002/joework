using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode {

	//A*计算时需要的值
	public float x=0,y=0,f=0,g=0,h=0;

	//是否可用
	public bool walkable=false;

	//父结点
	public PathNode parent;

	//为寻路预先计算，优化寻路速度
	public List<PathLink> links = null;

	//寻路方式
	public int version = 1;

	public PathNode(){}

	/// <summary>
	/// 初始化
	/// </summary>
	/// <param name="nodeX">Node x.</param>
	/// <param name="nodeY">Node y.</param>
	public PathNode(int nodeX,int nodeY){
		this.x = nodeX;
		this.y = nodeY;
	}

	public void Destroy(){
		if(links!=null){
			links.Clear();
			links = null;
		}
	}
}

