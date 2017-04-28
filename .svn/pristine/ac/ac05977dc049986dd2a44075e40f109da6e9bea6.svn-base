Shader "Mogo/Font" {
	Properties {
		_MainTex ("Font Texture", 2D) = "white" {}
	}

	SubShader {

		Tags { "Queue"="Transparent+30" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Lighting Off  ZTest Off ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				half4 color :COLOR0;
			};

			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				half4 color : COLOR0;
			};

			sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.color = v.color;
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col = i.color;
				col.a *= tex2D(_MainTex, i.texcoord).a;
				return col;
			}
			ENDCG 
		}
	} 	
}
