Shader "Joe/VertexColor" {
	
	SubShader 
	{		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
					
			struct v2f
			{
				float4 outVertex:POSITION;
				fixed4 outVertColor:COLOR;
			};
						
			v2f vert(appdata_base data)
			{	
				v2f o;
	
				//o.outVertex = data.vertex;
				o.outVertex = mul(UNITY_MATRIX_MVP,data.vertex);
				
				//改变cube立方体其中一个顶点的颜色
				if(data.vertex.x == 0.5 && data.vertex.y == 0.5 && data.vertex.z == -0.5)
				{
					o.outVertColor = fixed4(_SinTime.w,_SinTime.w,_CosTime.w,1);
					o.outVertex = o.outVertex+float4(1,1,1,1);
				}
				else
				{
					o.outVertColor = fixed4(0,1,0,1);				
				}
				
				return o;
			}
			
			float4 frag(v2f IN):COLOR
			{
				return IN.outVertColor;
			}
			
			ENDCG		
		}
	} 
	FallBack "Diffuse"
}
