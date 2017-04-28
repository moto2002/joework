Shader "Custom/RippleShaderMy"
{
	Properties
	{
		_Color("Base Color", Color) = (1,1,1,1)
		_MainTex("Base(RGB)", 2D) = "white" {}
	_DecorativeTex("DecorativeTex (RGB)", 2D) = "white"{}
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

		float4 _Color;

	sampler2D _MainTex;
	sampler2D _DecorativeTex;

	struct v2f
	{
		float4 pos:POSITION;
		float4 uv:TEXCOORD0;
	};

	struct appdata {
		float4 vertex : POSITION;
		float4 texcoord : TEXCOORD;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord;
		return o;
	}

	half4 frag(v2f i) :COLOR
	{
		/*因为需要一种扭曲效果，因此图片的边缘肯定不能是顺滑的，
		但是也不能用随机数来决定边缘的偏移左边，因为可能不是连续的，可能会出现锯齿，
		因此需要从图片中找出一个属性是平滑的改变的，图片的rgb值就是这样的，因此这里使用gb值。
		至于为什么要获得噪波贴图的x坐标轴的下一个像素和y轴坐标的下一个像素的gb值，然后减去1，可能是因为比较接近渐变值。。
		其实只取x或者y轴坐标的下一个也可以
		例如这样：float2 c = tex2D(_DecorativeTex, i.uv.xy + float2(0, _Time.x)).gb;*/

		float2 c = (tex2D(_DecorativeTex, i.uv.xy + float2(0, _Time.x)).gb + tex2D(_DecorativeTex, i.uv.xy + float2(_Time.x , 0)).gb) - 1;

		/*后面x的0.1其实可以做成变量控制扭曲效果的程度*/
		float2 ruv = float2(i.uv.x, i.uv.y) + c.xy * 0.1;
		half4 h = tex2D(_MainTex, ruv);
		return h;
	}

		ENDCG
	}
	}
}