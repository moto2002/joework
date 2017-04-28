Shader "Custom/OccTransVF"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_RimColor("RimColor",Color) = (0,1,1,1)
		_RimPower("Rim Power", Range(0.1,8.0)) = 1.0
	}

		SubShader
	{

		LOD 300
		Tags{ "Queue" = "Geometry+500" "RenderType" = "Opaque" }
		Pass
	{
		Blend SrcAlpha One
		ZWrite off
		Lighting off

		ztest greater

		CGPROGRAM
#pragma vertex vert  
#pragma fragment frag  
#include "UnityCG.cginc"  

		float4 _RimColor;
	float _RimPower;

	struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float4 color:COLOR;
		float4 normal:NORMAL;
	};

	struct v2f {
		float4  pos : SV_POSITION;
		float4  color:COLOR;
	};
	v2f vert(appdata_t v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
		float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
		float rim = 1 - saturate(dot(viewDir,v.normal));
		o.color = _RimColor*pow(rim,_RimPower);
		return o;
	}
	float4 frag(v2f i) : COLOR
	{
		return i.color;
	}
		ENDCG
	}
		pass
	{
		ZWrite on
			ZTest less

			CGPROGRAM
#pragma vertex vert    
#pragma fragment frag    
			sampler2D _MainTex;
		float4 _MainTex_ST;

		struct appdata {
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f {
			float4 pos : POSITION;
			float2 uv : TEXCOORD0;
		};

		v2f vert(appdata v)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
			o.uv = v.texcoord;
			return o;
		}

		float4 frag(v2f i) : COLOR
		{
			float4 texCol = tex2D(_MainTex, i.uv);
			return texCol;
		}
			ENDCG
	}
	}
		FallBack "Diffuse"
}