Shader "Mogo/UVTwirl" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NosieTex("Nosie",2D) = "white"{}
		_TintColor ("Main Color", Color) = (1,1,1,1)
		_DeltaTime("DeltaTime",Float)=0
		_Strenth("Strenth",Float)=1
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
			Blend SrcAlpha One
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _NosieTex;
			fixed4 _TintColor ;
			float _DeltaTime;
			float _Strenth;

			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float2  texCoord : TEXCOORD1;
			};

			float4 _MainTex_ST;
			float4 _NosieTex_ST;

			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				o.texCoord = (float2(o.pos.x,-o.pos.y)+1.0f)/2.0f;
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed2 nosie = tex2D(_NosieTex,i.texCoord+_DeltaTime*0.1f);
				fixed4 col0 = tex2D(_MainTex,i.uv+fixed2(nosie.r*_Strenth,0));

				fixed4 result = col0 * _TintColor ;

				return result;
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
				return result;
			}
			ENDCG
		 }
	} 

	
}
