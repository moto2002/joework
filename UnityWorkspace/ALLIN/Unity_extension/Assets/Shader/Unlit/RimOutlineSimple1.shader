// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "ZZL/Unlit/Rim Outline Simple1" {
	Properties {
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_RimColor ("Rim Color", Color) = (1,1,1,1)
	    _RimPower ("Rim Power", Range(0.0,100.0)) = 2.0
	}
	SubShader {
	
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True"}
		LOD 100
		Lighting off
		Pass
		{
			CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				
				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _RimColor;
				half _RimPower;
				
				struct vInput {
                	float4 vertex : POSITION;
                	float4 normal : NORMAL;
					half2 texcoord : TEXCOORD0;
	            };

	            struct v2f {
					half2 texcoord : TEXCOORD0;
	                float4 position : SV_POSITION;
	                float3 viewDir:TEXCOORD1;
	                float3 worldNormal:TEXCOORD2;
	            };

	            v2f vert(vInput i) {
	                v2f o;

	                float4x4 modelMatrix        = unity_ObjectToWorld;
	                float4x4 modelMatrixInverse = unity_WorldToObject;

	                float3 normalDirection = normalize(mul(i.normal, modelMatrixInverse)).xyz;
	                float3 viewDirection   = normalize(_WorldSpaceCameraPos - mul(modelMatrix, i.vertex).xyz);

					o.texcoord = TRANSFORM_TEX(i.texcoord, _MainTex);
	                o.position = mul(UNITY_MATRIX_MVP, i.vertex);
	                o.viewDir = viewDirection;
	                o.worldNormal = normalDirection;
	                
	                return o;
	            }
				
				fixed4 frag (v2f IN) : COLOR
				{
				    fixed4 col = tex2D(_MainTex,IN.texcoord);
					half rim = 1.0 - saturate(dot (normalize(IN.viewDir), IN.worldNormal));
				    col.rgb += _RimColor.rgb * pow (rim, _RimPower)*_RimColor.a;
				    return col;
				}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
