//author:zhouzhanglin
//RGB+A图片分离
Shader "ZZL/Native2D/Sprite Split Alpha"
{
	Properties
	{
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		_AlphaTex ("Alpha Texture", 2D) = "white" {}
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode",float)=0
	}
	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
	
		Lighting off
		Zwrite off
		Cull [_CullMode]
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color:COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				fixed4 color:COLOR;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _AlphaTex;
			float4 _AlphaTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv1 = TRANSFORM_TEX(v.uv, _AlphaTex);
				o.color = v.color;
				return o;
			}


			fixed4 SampleSpriteTexture (float2 uv,float2 uv1)
			{
				fixed4 color = tex2D (_MainTex, uv);
				color.a = tex2D (_AlphaTex,uv1).r;
				return color;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = SampleSpriteTexture(i.uv,i.uv1)*i.color;
				return col;
			}
			ENDCG
		}
	}
}
