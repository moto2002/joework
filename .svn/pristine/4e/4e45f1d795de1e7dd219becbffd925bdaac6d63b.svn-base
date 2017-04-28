Shader "Custom/RippleShader"
{
	Properties
	{
		//      _Color("Base Color", Color) = (1,1,1,1)
		_MainTex("Base(RGB)", 2D) = "white" {}

	_NoiseTex("Wave Noise", 2D) = "white" {}//噪波贴图

	_Color("Tint", Color) = (1,1,1,1)

		_Indentity("Indentity", float) = 0.1//表示水波的扭曲强度

		_SpeedX("WaveSpeedX", float) = 0.08//噪波贴图延X方向的移动速度
		_SpeedY("WaveSpeedY", float) = 0.04//噪波贴图延Y方向的移动速度

		_AlphaFadeIn("AlphaFadeIn", float) = 0.0//水波的淡入位置
		_AlphaFadeOut("AlphaFadeOut", float) = 1.0//水波的淡出位置
		_TwistFadeIn("TwistFadeIn", float) = 1.0//扭曲的淡入位置
		_TwistFadeOut("TwistFadeOut", float) = 1.01//扭曲的淡出位置
		_TwistFadeInIndentity("TwistFadeInIndentity", float) = 1.0//扭曲的淡入强度
		_TwistFadeOutIndentity("TwistFadeOutIndentity", float) = 1.0//扭曲的淡出强度
	}

		SubShader
	{
		tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		//          float4 _Color;
		sampler2D _MainTex;

	sampler2D _NoiseTex;

	fixed4 _Color;

	half _Indentity;

	half _SpeedX;
	half _SpeedY;

	float _AlphaFadeIn;
	float _AlphaFadeOut;
	half _TwistFadeIn;
	half _TwistFadeOut;
	fixed _TwistFadeInIndentity;
	fixed _TwistFadeOutIndentity;

	struct v2f
	{
		float4 pos:POSITION;
		float4 uv:TEXCOORD0;
		float4 color:COLOR;
	};

	struct appdata {
		float4 vertex : POSITION;
		float4 texcoord : TEXCOORD;
		float4 color : COLOR;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord;
		o.color = v.color;
		return o;
	}

	half4 frag(v2f i) :COLOR
	{
		//对淡入强度和淡出强度的插值  
		fixed fadeT = saturate((_TwistFadeOut - i.uv.y) / (_TwistFadeOut - _TwistFadeIn));
	float2 tuv = (i.uv - float2(0.5, 0)) * fixed2(lerp(_TwistFadeOutIndentity, _TwistFadeInIndentity, fadeT), 1) + float2(0.5, 0);

	//计算噪波贴图的RG值，得到扭曲UV，
	float2 waveOffset = (tex2D(_NoiseTex, i.uv.xy + float2(0, _Time.y * _SpeedY)).rg + tex2D(_NoiseTex, i.uv.xy + float2(_Time.y * _SpeedX, 0)).rg) - 1;
	float2 ruv = float2(i.uv.x, 1 - i.uv.y) + waveOffset * _Indentity;

	//使用扭曲UV对纹理采样
	float4 c = tex2D(_MainTex, ruv);

	//对淡入Alpha和淡出Alpha的插值
	fixed fadeA = saturate((_AlphaFadeOut - ruv.y) / (_AlphaFadeOut - _AlphaFadeIn));
	c = c * _Color * i.color * fadeA;
	clip(c.a - 0.01);
	return c;
	}

		ENDCG
	}
	}
}