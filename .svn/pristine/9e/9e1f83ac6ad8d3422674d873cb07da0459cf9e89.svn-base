Shader "Mogo/FlashLight" 
{
    Properties 
	{
		 _MainTex ("Base (RGB)", 2D) = "white" { }
		 _AlphaTex("AlphaTex",2D) = "white"{}
		 _LightTex("LightTex",2D) = "white"{}
		 _LightFactor("LightFactor",Float) = 1
		 _AlphaFactor("AlphaFactor",Float) = 1
		 }
    SubShader
	{

		 Tags
		 {
			"Queue" = "Transparent+8"
		 }
         Pass
		 {
			Lighting Off
			ZTest Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _LightTex;
			sampler2D _AlphaTex;
			float _LightFactor;
			half4 _Color;

			float _AlphaFactor;
		
			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float2  uv1 : TEXCOORD1;
			};

			half4 _MainTex_ST;
			half4 _LightTex_ST;
			half4 _AlphaTex_ST;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				o.uv1 = TRANSFORM_TEX(v.texcoord,_LightTex);
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				half4 texcol = tex2D (_MainTex, i.uv);
				half4 lightcol = tex2D(_LightTex,i.uv1);

				half4 result = texcol + lightcol* _LightFactor;
				result.a = tex2D(_AlphaTex,i.uv).r * _AlphaFactor;

				return result;
			}
			ENDCG
		 }
    }
} 