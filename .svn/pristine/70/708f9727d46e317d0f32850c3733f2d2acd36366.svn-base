//只取了顶点的透明度，没有取顶点颜色来计算.
Shader "ZZL/Unlit/Vertex Transparent" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Tint Color",Color) = (1,1,1,1)
		_Brightness ("Brightness",Range(0,2)) = 0.2
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass{
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Brightness;
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				fixed4 color    : COLOR;
				half2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				half3 texcoord  : TEXCOORD0;
			};

			v2f vert(appdata_t v)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				OUT.texcoord.xy = TRANSFORM_TEX(v.texcoord,_MainTex);
				OUT.texcoord.z = v.color.a;

				return OUT;
			}

			fixed4 frag(v2f i):SV_Target
			{
				fixed4 c = tex2D(_MainTex, i.texcoord) ;
				c.rgb *=(1+_Brightness);
				c.a = i.texcoord.z;
				return c*_Color;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
