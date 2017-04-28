//树的简单摆动，通过顶点控制.
Shader "ZZL/Env/Tree Swing Simple"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WaveSpeed("Wave Speed",float)=1
		_WaveX("Wave X",Range(0,1))=0.1
		_WaveZ("Wave Z",Range(0,1))=0
		_HeightChange("Height Change",Range(0,0.05))=0.01
		[MaterialToggle] UseVertexColor("Use Vertex Color",float)=0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True"}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#pragma multi_compile _ USEVERTEXCOLOR_ON
			
			#include "UnityCG.cginc"
			
			struct appdata
			{
				float4 vertex : POSITION;
				half2 uv : TEXCOORD0;
				#ifdef USEVERTEXCOLOR_ON
				fixed4 color:COLOR;
				#endif
			};

			struct v2f
			{
				half2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			half _WaveSpeed;
			fixed _WaveX;
			fixed _WaveZ;
			fixed _HeightChange;
			
			v2f vert (appdata v)
			{
				v2f o;
				half pan = sin(_Time.y*_WaveSpeed);
				#ifdef USEVERTEXCOLOR_ON
				pan*=v.color.a*10;
				#else
				pan*=v.vertex.y;
				#endif
				v.vertex.x += pan*_WaveX;
				v.vertex.z += pan*_WaveZ;
				v.vertex.y -= abs(pan)*_HeightChange;

				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
				o.uv = v.uv;
				
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);				
				return col;
			}
			ENDCG
		}
	}
}
