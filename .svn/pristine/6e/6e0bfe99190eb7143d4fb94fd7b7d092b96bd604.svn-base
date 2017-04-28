Shader "ZZL/Unlit/Painter/Draw Line Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaskTex ("Texture", 2D) = "white" {}
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
				float2 uv1 : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MaskTex;
			float4 _MaskTex_ST;
			fixed4 _Color;
			fixed _Alpha;
			fixed _Cutoff;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv1 = TRANSFORM_TEX(v.uv, _MaskTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb*=_Color.rgb;
				fixed alpha = _Alpha*tex2D(_MaskTex, i.uv1).a;
				col*=alpha;

				clip(col.a-_Cutoff);

				return col;
			}
			ENDCG
		}
	}
}
