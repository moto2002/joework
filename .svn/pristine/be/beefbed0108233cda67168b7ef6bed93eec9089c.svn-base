Shader"Custom/Water3"
{
	Properties
	{
		_MainTex("main texture",2D) = "white" {}
		_SecondTex("sec texture",2D) = "white"{}
	}

		SubShader
	{
		pass {
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4 _MainTex_ST;

		sampler2D _SecondTex;

		uniform float times;

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		v2f vert(appdata_base v)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
			o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
			return o;
		}

		float4 frag(v2f i) :COLOR
		{
			//==================================================================================
			//波动效果

			//float times = _Time.w;
			float4 color = float4(0, 0, 0, 0);
			float2 bgUV = i.uv;
			//mark #1
			//当前像素点显示的纹理上的UV坐标
			//是根据原始显示的纹理UV计偏移
			//其实就是纹理上的点做园周运动
			//原理：像素点x ,uv为(x,y)
			//修改像素点显示的纹理
			//uv => 新的uv为(x*(1+ sin(x * Increment)),y*(1+cos(y* Increment)));


			bgUV.x += sin(times / 500.f + bgUV.x * 15) * 0.005;
			bgUV.y += cos(times / 500.f + bgUV.y * 15) * 0.005;
			color = tex2D(_MainTex, bgUV);

			//==================================================================================
			//原理同上
			//水底的光效果
			float4 shading = float4(0, 0, 0, 0);
			float2 lightUv = i.uv;

			//同上 mark #1
			lightUv.x += cos(times / 100.f + lightUv.x * 50) * 0.01;
			lightUv.y += sin(times / 100.f + lightUv.y * 50) * 0.01;
			shading = tex2D(_SecondTex, lightUv);

			//让一张白图呈沅的颜色
			shading.r *= 0.7f;
			shading.g *= 0.59f;
			shading.b *= 0.11f;
			//==================================================================================

			return color + shading  * 0.3;
		}
			ENDCG
	}
	}
















}