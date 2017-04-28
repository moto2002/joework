Shader "ZZL/Unlit/Painter/Scribble Shader"
{
	Properties
	{
		_SourceTex ("Source Tex", 2D) = "white" {}
		_RenderTex("Render Tex",2D) = "white" {} //用来做遮罩
		_Color("Color",Color)=(1,1,1,1)
		_Alpha("Alpha",Range(0,1))=1
		_Cutoff("Alpha cutoff",Range(0,1))=0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc("Src Factor",float)=5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDst("Dst Factor",float)=10
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True"}
		LOD 100
		Cull back
		ZTest Always
		ZWrite off
		Blend [_BlendSrc] [_BlendDst]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _SourceTex;
			float4 _SourceTex_ST;
			sampler2D _RenderTex;
			fixed4 _Color;
			fixed _Alpha;
			fixed _Cutoff;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _SourceTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_SourceTex, i.uv);
				col.rgb*=_Color.rgb;
				col.a*=_Alpha;

				fixed4 mask = tex2D (_RenderTex,i.uv );
				col.a*=mask.a;

				clip(col.a-_Cutoff);

				return col;
			}
			ENDCG
		}
	}
}
