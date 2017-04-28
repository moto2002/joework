public class PathLink {

	public PathNode node = null ;
	public float cost = 0;

	public PathLink(){}

	public PathLink( PathNode node ,float cost){
		this.node = node;
		this.cost = cost;
	}

	public void Destroy(){
		if(node!=null){
			node.Destroy();
			node = null;
		}
	}
}
