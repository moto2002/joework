Shader "Sbin/WaveAnimShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_WaveTex("Texture",2D) = "white"{}
	}
		SubShader
	{
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		sampler2D _MainTex;
	sampler2D _WaveTex;

	struct v2f {
		float4 pos:POSITION;
		float2 uv:TEXCOORD0;
	};

	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}

	fixed4 frag(v2f v) : COLOR
	{
		float2 uv = tex2D(_WaveTex, v.uv);
		uv = uv * 2 - 1;//[-1,1]
		uv *= 0.25;

		v.uv += uv;

		fixed4 col = tex2D(_MainTex, v.uv);
		return col;
	}
		ENDCG
	}
	}
}