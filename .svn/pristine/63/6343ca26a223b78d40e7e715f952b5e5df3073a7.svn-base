using UnityEngine;
using System.Collections;
/// <summary>
/// sprite2D 九宫格
/// Author:zhouzhanglin
/// </summary>
[ExecuteInEditMode]
public class SpriteSlice9 : MonoBehaviour {

	public Sprite sprite;
	[SerializeField]
	private Color m_color = Color.white;
	public Vector2 size;
	public Vector2 pivot = new Vector2(0.5f,0.5f);
	[SerializeField]
	private string m_sortLayerName = "Default";
	[SerializeField]
	private int m_sortingOrder = 0;

	private Mesh m_mesh ;
	private MeshFilter m_filter;
	private MeshRenderer m_render;
	private Vector3[] m_vertex;
	private Vector2[] m_uvs;
	private Color[] m_colors;
	private int[] m_tris;

	private Sprite m_sp;
	private Vector3 m_size;

	public Color color{
		get{return m_color;}
		set{
			m_color = value;
			if(m_mesh){
				for(int i=0;i<16;++i){
					m_colors[i] = m_color;
				}
				m_mesh.colors = m_colors;
			}
		}
	}

	public string sortLayerName{
		get{return m_sortLayerName;}
		set{
			m_sortLayerName = value;
			if(m_render && !string.IsNullOrEmpty(m_sortLayerName)){
				m_render.sortingLayerName = m_sortLayerName;
			}
		}
	}

	public int sortingOrder{
		get{return m_sortingOrder;}
		set{
			m_sortingOrder = value;
			if(m_render){
				m_render.sortingOrder = m_sortingOrder;
			}
		}
	} 

	// Use this for initialization
	void Start () {
		m_sp = sprite;
		m_size = size;

		#if UNITY_EDITOR
		if(!Application.isPlaying){
			Renderer r= GetComponent<Renderer>();
			if(r is SpriteRenderer){
				DestroyImmediate(r);
			}
		}
		#endif

		m_filter = GetComponent<MeshFilter>();
		if(m_filter==null) m_filter = gameObject.AddComponent<MeshFilter>();

		m_render = GetComponent<MeshRenderer>();
		if(m_render==null) m_render = gameObject.AddComponent<MeshRenderer>();

		UpdateMesh();
	}

	#if UNITY_EDITOR
	void LateUpdate () {
		if(!Application.isPlaying){
			if(sprite)
			{
				if(sprite!=m_sp || !Vector2.Equals(m_size,size)){
					m_sp = sprite;
					m_size = size;
					UpdateMesh();
				}
				if(m_mesh){
					for(int i=0;i<16;++i){
						m_colors[i] = m_color;
					}
					m_mesh.colors = m_colors;
				}

				if(m_render && !string.IsNullOrEmpty(m_sortLayerName)){
					m_render.sortingLayerName = m_sortLayerName;
					m_render.sortingOrder = m_sortingOrder;
				}
			}
		}
	}
	#endif

	public void UpdateMesh(){
		if(m_mesh==null){
			m_mesh = new Mesh();
			m_mesh.name="Sprie9Slice";
			m_filter.sharedMesh = m_mesh;
			m_vertex = new Vector3[16];
			m_uvs = new Vector2[16];
			m_colors = new Color[16];
			m_tris = new int[54];
		}

		if(sprite){
			float w = (sprite.rect.width+size.x)*0.01f;
			float h = (sprite.rect.height+size.y)*0.01f;
			float bl = sprite.border.x*0.01f;
			float br = (sprite.rect.width-sprite.border.z+size.x)*0.01f;
			float bt = (sprite.rect.height-sprite.border.w+size.y)*0.01f;
			float bb = sprite.border.y*0.01f;
			//vertex & uv
			m_vertex[0].x = 0; 	m_uvs[0].x = sprite.rect.x/sprite.texture.width;
			m_vertex[0].y = 0; 	m_uvs[0].y = sprite.rect.y/sprite.texture.height;
			m_vertex[1].x = bl;	m_uvs[1].x = (sprite.rect.x+sprite.border.x)/sprite.texture.width;
			m_vertex[1].y = 0;	m_uvs[1].y = m_uvs[0].y;
			m_vertex[2].x = br;	m_uvs[2].x = (sprite.rect.x+sprite.rect.width-sprite.border.z)/sprite.texture.width;
			m_vertex[2].y = 0;	m_uvs[2].y = m_uvs[0].y;
			m_vertex[3].x = w;	m_uvs[3].x = (sprite.rect.x+sprite.rect.width)/sprite.texture.width;
			m_vertex[3].y = 0;	m_uvs[3].y = m_uvs[0].y;

			m_vertex[4].x = 0;	m_uvs[4].x = m_uvs[0].x;
			m_vertex[4].y = bb;	m_uvs[4].y = (sprite.rect.y+sprite.border.y)/sprite.texture.height;
			m_vertex[5].x = bl;	m_uvs[5].x = m_uvs[1].x ;
			m_vertex[5].y = bb;	m_uvs[5].y = m_uvs[4].y ;
			m_vertex[6].x = br;	m_uvs[6].x = m_uvs[2].x ;
			m_vertex[6].y = bb;	m_uvs[6].y = m_uvs[4].y ;
			m_vertex[7].x = w;	m_uvs[7].x = m_uvs[3].x ;
			m_vertex[7].y = bb;	m_uvs[7].y = m_uvs[4].y ;

			m_vertex[8].x = 0;		m_uvs[8].x = m_uvs[0].x;
			m_vertex[8].y = bt;		m_uvs[8].y = (sprite.rect.y+sprite.rect.height-sprite.border.w)/sprite.texture.height;
			m_vertex[9].x = bl;		m_uvs[9].x = m_uvs[1].x;
			m_vertex[9].y = bt;		m_uvs[9].y = m_uvs[8].y ;
			m_vertex[10].x = br;	m_uvs[10].x = m_uvs[2].x;
			m_vertex[10].y = bt;	m_uvs[10].y = m_uvs[8].y ;
			m_vertex[11].x = w; 	m_uvs[11].x = m_uvs[3].x;
			m_vertex[11].y = bt;	m_uvs[11].y = m_uvs[8].y ;

			m_vertex[12].x = 0;		m_uvs[12].x = m_uvs[0].x;
			m_vertex[12].y = h;		m_uvs[12].y = (sprite.rect.y+sprite.rect.height)/sprite.texture.height;
			m_vertex[13].x = bl;	m_uvs[13].x = m_uvs[1].x;
			m_vertex[13].y = h;		m_uvs[13].y = m_uvs[12].y;
			m_vertex[14].x = br;	m_uvs[14].x = m_uvs[2].x;
			m_vertex[14].y = h;		m_uvs[14].y = m_uvs[12].y;
			m_vertex[15].x = w;		m_uvs[15].x = m_uvs[3].x;
			m_vertex[15].y = h;		m_uvs[15].y = m_uvs[12].y;

			//colors
			for(int i=0;i<16;++i){
				m_vertex[i].x-=w*pivot.x;
				m_vertex[i].y-=h*pivot.y;
				m_colors[i] = m_color;
			}

			//triangles
			m_tris[0]=5; m_tris[1]=1; m_tris[2]=0;
			m_tris[3]=4; m_tris[4]=5; m_tris[5]=0;
			m_tris[6]=6; m_tris[7]=2; m_tris[8]=1;
			m_tris[9]=5; m_tris[10]=6; m_tris[11]=1;
			m_tris[12]=7; m_tris[13]=3; m_tris[14]=2;
			m_tris[15]=6; m_tris[16]=7; m_tris[17]=2;

			m_tris[18]=9; m_tris[19]=5; m_tris[20]=4;
			m_tris[21]=8; m_tris[22]=9; m_tris[23]=4;
			m_tris[24]=10; m_tris[25]=6; m_tris[26]=5;
			m_tris[27]=9; m_tris[28]=10; m_tris[29]=5;
			m_tris[30]=11; m_tris[31]=7; m_tris[32]=6;
			m_tris[33]=10; m_tris[34]=11; m_tris[35]=6;

			m_tris[36]=13; m_tris[37]=9; m_tris[38]=8;
			m_tris[39]=12; m_tris[40]=13; m_tris[41]=8;
			m_tris[42]=14; m_tris[43]=10; m_tris[44]=9;
			m_tris[45]=13; m_tris[46]=14; m_tris[47]=9;
			m_tris[48]=15; m_tris[49]=11; m_tris[50]=10;
			m_tris[51]=14; m_tris[52]=15; m_tris[53]=10;

			m_mesh.vertices=m_vertex;
			m_mesh.uv = m_uvs;
			m_mesh.triangles = m_tris;
			m_mesh.colors = m_colors;
			m_mesh.RecalculateBounds();
		}
	}
}
