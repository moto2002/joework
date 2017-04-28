// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "ZZL/Water/Water Refract" { 
	Properties {
		_WaveScale ("Wave scale", Range (0.02,0.15)) = 0.063
		_RefrDistort ("Refraction distort", Range (0,1.5)) = 0.40
		_RefrColor ("Refraction color", COLOR)  = ( .34, .85, .92, 1)
		_BumpMap ("Normalmap ", 2D) = "bump" {} //用来产生波纹的法线贴图.
		WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
		[HideInInspector]_RefractionTex ("Internal Refraction", 2D) = "" {}  //折射用的renderTexture
	}

	Subshader { 
		Tags { "WaterMode"="Refractive" "RenderType"="Opaque" "IgnoreProjector"="True"}
		Pass {
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				fixed4 _WaveScale4;
				fixed _RefrDistort;
				half4 _WaveOffset;

				struct appdata {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
				};

				struct v2f {
					float4 pos : SV_POSITION;
					float4 ref : TEXCOORD0;
					float4 bumpuv : TEXCOORD1;
					float3 viewDir : TEXCOORD3;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
					float4 temp;
					float4 wpos = mul (unity_ObjectToWorld, v.vertex);
					temp.xyzw = wpos.xzxz * _WaveScale4 + _WaveOffset;
					o.bumpuv = temp;
					
					o.viewDir.xzy = WorldSpaceViewDir(v.vertex);
					o.ref = ComputeScreenPos(o.pos);
					return o;
				}

				sampler2D _ReflectionTex;
				sampler2D _BumpMap;
				sampler2D _RefractionTex;
				fixed4 _RefrColor;

				half4 frag( v2f i ) : COLOR
				{
					i.viewDir = normalize(i.viewDir);
					
					// combine two scrolling bumpmaps into one
					fixed3 bump1 = UnpackNormal(tex2D( _BumpMap, i.bumpuv.xy )).rgb;
					fixed3 bump2 = UnpackNormal(tex2D( _BumpMap, i.bumpuv.zw )).rgb;
					fixed3 bump = (bump1 + bump2) * 0.5;
					
					float4 uv2 = i.ref; uv2.xy -= bump * _RefrDistort;
					fixed4 refr = tex2Dproj( _RefractionTex, UNITY_PROJ_COORD(uv2) ) * _RefrColor;
					return refr;
				}
			ENDCG

		}
	}
}
