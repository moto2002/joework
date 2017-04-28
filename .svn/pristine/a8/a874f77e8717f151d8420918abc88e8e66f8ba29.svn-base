Shader "Mogo/UVAnimation"
 {
	Properties
	{
		_IndexX ("IndexX", Float) = 0
		_IndexY ("IndexY", Float) = 0
		_Width("Width",Float) = 1
		_Height("Height",Float) = 1
		_MainTex ("Texture", 2D) = "white" { }
		_Color ("Color",Color) = (1,1,1,1)
	}
	SubShader 
	{
		 Pass 
		 {
			ZTest Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _IndexX;
			float _IndexY;
			float _Width;
			float _Height;
			sampler2D _MainTex;
			half4 _Color;

			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};

			float4 _MainTex_ST;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = float2((v.texcoord.x + _IndexX) * _Width ,1-( v.texcoord.y + _IndexY) * _Height);
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				half4 texcol = tex2D (_MainTex, i.uv);

				return texcol *_Color;
			}	
			ENDCG
		}
	}
	Fallback "VertexLit"
} 