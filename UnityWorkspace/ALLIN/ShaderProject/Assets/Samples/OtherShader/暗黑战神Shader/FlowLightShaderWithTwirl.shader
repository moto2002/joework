// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'

Shader "Mogo/FlowLightShaderWithTwirl" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AlphaTex("AlphaTex",2D) = "white"{}
		_Layer1Tex("Layer1",2D) = "white"{}
		_NosieTex("Nosie",2D) = "white"{}
		_Color ("Main Color", Color) = (1,1,1,1)
		_CtrlColor("CtrlColor",Color) = (1,1,1,1)
		_HitColor("Hit Color",Color) = (0,0,0,0)
		_AddColor("AddColor",Color) = (1,1,1,1)
		_HighLight("High Light",Float) = 1
		_DeltaTime("DeltaTime",Float)=0
		_Strenth("Strenth",Float)=1
		_LightStrenth("LightStrenth",Float)=1
		_LightDirX("LightDirX",Float) = 0
		_LightDirY("LightDirY",Float) = 0
		_LightDirZ("LightDirZ",Float) = 0
		_DotPower("DotPower",Float) = 0
		_DotStrenth("DotStrenth",Float) = 0
	}

	SubShader {
		LOD 205
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
			sampler2D _NosieTex;
			sampler2D _AlphaTex;
			fixed4 _Color;
			fixed4 _CtrlColor;
			fixed4 _HitColor;
			fixed4 _AddColor;
			float _HighLight;
			float _DeltaTime;
			float _Strenth;
			float _LightStrenth;
			float _LightDirX;
			float _LightDirY;
			float _LightDirZ;
			float _DotPower;
			float _DotStrenth;

			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float2	uv2 : TEXCOORD1;
				float2	uvStaticAlpha : TEXCOORD2;
				float2  texCoord : TEXCOORD3;
				float	VDotN : TEXCOORD4;
			};

			float4 _MainTex_ST;
			float4 _Layer1Tex_ST;
			float4 _NosieTex_ST;
			float4 _AlphaTex_ST;

			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uvStaticAlpha = v.texcoord;
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.texcoord,_Layer1Tex);
				//o.texCoord = (float2(o.pos.x,-o.pos.y)+1.0f)/2.0f;
				o.texCoord = o.uv;

				float3 objSpaceCameraPos = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos.xyz+ float3(_LightDirX,_LightDirY,_LightDirZ), 1)).xyz * 1.0;
				
				float3 lightDir = objSpaceCameraPos - v.vertex.xyz;

				o.VDotN = saturate(dot(v.normal,normalize(lightDir)));
				//o.VDotN = float4(normalize(WorldSpaceViewDir(v.vertex)).xyz,1);
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed2 nosie = tex2D(_NosieTex,i.texCoord+_DeltaTime*0.1f);
				float a = tex2D(_AlphaTex,i.uvStaticAlpha).b;
				float b = tex2D(_AlphaTex,i.uvStaticAlpha).g;
				fixed4 col0 = tex2D(_MainTex,i.uv);
				fixed4 col1 = tex2D(_Layer1Tex,i.uv2+fixed2(nosie.r*_Strenth,0));
				fixed4 result = 0;
				if(_DotPower == 0)
				{
				result= (col0 * _Color+ col1 * a *_CtrlColor*_LightStrenth)*(1-b)+
				b*((_AddColor*(col0 * _Color+ col1 * a *_LightStrenth)+_CtrlColor*a));
				}
				else
				{
				result = (col0 * _Color+ col1 * a *_CtrlColor*_LightStrenth)*(1-b)+
				b*((_AddColor*(col0 * _Color+ col1 * a *_LightStrenth)+_CtrlColor*a)+pow(i.VDotN,_DotPower)*_DotStrenth);
				}
				result.a = col1.a * _CtrlColor.a * _Color.a;

				return fixed4(result.rgb*_HighLight,result.a);
				//return fixed4(i.VDotN,i.VDotN,i.VDotN,1);
			
				
			}
			ENDCG
		 }
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
			//Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			fixed4 _Color;
			fixed4 _HitColor;

			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};

			float4 _MainTex_ST;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 result = tex2D(_MainTex,i.uv);
				result.a = tex2D(_AlphaTex,i.uv);
				return result;
			}
			ENDCG
		 }
	} 

	
}
