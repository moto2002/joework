Shader "Mogo/TestFX2" {
	Properties {
		_AlphaTex ("AlphaTex", 2D) = "white" {}
		_RGBTex("RGBTex",2D) = "white"{}
		_Color("Color",Color) = (1,1,1,1)
	}
	SubShader {
		LOD 200
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		

		 Pass
		 {
		
	Fog{Mode Off}
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _AlphaTex;
			sampler2D _RGBTex;
			float4 _Color;

			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float2  uv2 : TEXCOORD1;
			};

			float4 _AlphaTex_ST;
			float4 _RGBTex_ST;

			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _AlphaTex);
				o.uv2 = TRANSFORM_TEX(v.texcoord1,_RGBTex);
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				float a = tex2D(_AlphaTex,i.uv).a;
				fixed4 col = tex2D(_RGBTex,i.uv2);

				fixed4 result = col*a;
				result.a = a ;

				return result;
				
			}
			ENDCG
		 }
	} 
}
