Shader "ZZL/Env/Grass Wave"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WaveDistance("Wave Distance",Range(0,1))=0.25
		_WindSpeed("Wind Speed",Range(0,2))=0.4
		_Cutoff("alpha CutOff",Range(0,1)) = 0.5
	}
	SubShader
	{
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparent"}
		
		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			half _WaveDistance;
			fixed _WindSpeed;
			fixed _Cutoff;
			
			float4 GrassRock(float4 pos, half2 uv1,float vy)
			{
				float windx = 0;
				windx = sin(_Time.y * _WindSpeed)*_WaveDistance;
				pos.x += windx*vy;
				return pos;
			}

			struct appdata
			{
				float4 vertex : POSITION;
				half2 uv : TEXCOORD0;
			};

			struct v2f
			{
				half2 uv:TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.pos = GrassRock(o.pos, v.uv,v.vertex.y);
				o.uv = TRANSFORM_TEX(v.uv,_MainTex);
				return o;
			}
			
			fixed4 frag (v2f i):SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				clip(col.a-_Cutoff);
				return col;
			}
			ENDCG
		}
	}
}
