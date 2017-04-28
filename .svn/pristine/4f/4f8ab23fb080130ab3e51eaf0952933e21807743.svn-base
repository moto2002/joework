Shader "ZZL/Unlit/Mobile Simple Color" {
	Properties {
		_Color("Color",Color)=(1,1,1,1)
		_MainTex ("Main Texture (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True"}
		LOD 100
		Lighting off
		
		Pass{
			CGPROGRAM
			
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					half2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
				};

				fixed4 _Color;
				sampler2D _MainTex;
				float4 _MainTex_ST;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}
				
				fixed4 frag (v2f i) : SV_Target
				{
					return tex2D(_MainTex, i.texcoord)*_Color;
				}
			
			ENDCG
		}
	} 
	FallBack "Mobile/Diffuse"
}
