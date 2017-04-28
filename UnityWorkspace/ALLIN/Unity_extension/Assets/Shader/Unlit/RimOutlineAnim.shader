// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "ZZL/Unlit/RimOutlineAnim" {
	Properties {
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_RimColor ("Rim Color", Color) = (1,1,1,1)
	    _RimPower ("Rim Power", Range(0.0,100.0)) = 2.0 //内发光强度.
	    _RimSequence ("Rim Sequence",float)=30 //动画频率.
	    _RimScope ("Rim Scope",float)=3 //发光范围.
	    _AdjustRim("Adjust Aim",Range(0,1))=0
	}
	SubShader {
	
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True"}
		LOD 100

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
				half _RimSequence;
				half _RimScope;
				fixed _AdjustRim;
				
				struct vInput {
                	float4 vertex : POSITION;
                	float4 normal : NORMAL;
					half2 texcoord : TEXCOORD0;
	            };

	            struct v2f {
					half2 texcoord : TEXCOORD0;
	                float4 position : SV_POSITION;
	                fixed3 emssion:COLOR;
	            };
				
	            v2f vert(vInput i) {
	                v2f o;

	                float4x4 modelMatrix        = unity_ObjectToWorld;
	                float4x4 modelMatrixInverse = unity_WorldToObject;

	                float3 normalDirection = normalize(mul(i.normal, modelMatrixInverse)).xyz;
	                float3 viewDirection   = normalize(_WorldSpaceCameraPos - mul(modelMatrix, i.vertex).xyz);

					o.texcoord = TRANSFORM_TEX(i.texcoord, _MainTex);
	                o.position = mul(UNITY_MATRIX_MVP, i.vertex);
	                
	                //内发光计算.
	                fixed seq = abs(sin(_Time*_RimSequence));
	                float rimx = _RimPower- seq*_RimScope;
	                float rimy = 1.0 - saturate(dot (normalize(viewDirection), normalDirection));
	                half a = (1-seq)*_RimColor.a+_AdjustRim ;
	                o.emssion = _RimColor.rgb * pow (rimy, rimx)*a;

	                return o;
	            }
				
				fixed4 frag (v2f IN) : COLOR
				{
				    fixed4 col = tex2D(_MainTex,IN.texcoord);
				    col.rgb =  col.rgb+IN.emssion;
				    return col;
				}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
