Shader "Mogo/TestFX" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Layer1Tex("Layer1",2D) = "white"{}
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

			sampler2D _MainTex;
			sampler2D _Layer1Tex;
			float4 _Color;

			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float2  uv2 : TEXCOORD1;
				float2  uvStaticAlpha : TEXCOORD2;
			};

			float4 _MainTex_ST;
			float4 _Layer1Tex_ST;

			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uvStaticAlpha = v.texcoord;
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.texcoord1,_Layer1Tex);
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				float a = tex2D(_MainTex,i.uvStaticAlpha).a;
				fixed4 col0 = tex2D(_MainTex,i.uv);
				fixed4 col1 = tex2D(_Layer1Tex,i.uv2);

				fixed4 result = col0+col1*a*_Color;
				result.a = col1.a*_Color.a;

				return result;
				
			}
			ENDCG
		 }
	} 
}
