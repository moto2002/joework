// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Mogo/DiamondShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
		_EmiColor ("Emive Color", Color) = (1,1,1,1)
	}
	SubShader 
	{
         Pass
		 {
			//ZWrite Off
			//Blend SrcAlpha OneMinusSrcAlpha
			Lighting Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _Color;
			float4 _EmiColor;

			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float4  color : TEXCOORD1;
			};

			float4 _MainTex_ST;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				//float4 wpos = mul(_Object2World,v.vertex);
				float3 toView = normalize(WorldSpaceViewDir(v.vertex));
				float3 wnormal = normalize(mul(transpose((float3x3)unity_WorldToObject),v.normal)) ;

				o.color = _Color * dot(toView,wnormal) + _EmiColor;
				
				return o;
			}

			float4 frag (v2f i) : COLOR
			{
				//half4 texcol = tex2D (_MainTex, i.uv);
				//return  _ColorMask;
				return float4(i.color.rgb,1) * tex2D(_MainTex,i.uv);
			}
			ENDCG
		}
	} 
}
