Shader "ZZL/Water/Water Wave" {
	Properties {
		_Color("Main Color",Color)=(1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector]_WaveY("Wave Min Y", float) = 0
		_Speed("Speed", float) = 20
		_Frequency("Frequency", float) = 0.5
		_Amplitude("Amplitude", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True" }
		LOD 100
		Lighting off
		
		Pass{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members normal)
#pragma exclude_renderers d3d11 xbox360

			#pragma fragment frag 
			#pragma vertex vert
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _MainTex_ST;
			half _Speed;
			fixed _Frequency;
			fixed _Amplitude;
			fixed _WaveY;
			fixed4 _Color;

			struct v2f {
				float2 uv:TEXCOORD0;
				float4 pos : SV_POSITION;
				float3 normal:NORMAL;
			};
			
			v2f vert (appdata_base v) {
			  v2f o;
			  o.uv = v.texcoord;
			  
			  if(v.vertex.y>_WaveY)
	          {
	          	fixed time = _Time*_Speed;
		        fixed waveValueA = sin(time+v.vertex.x*_Frequency)*_Amplitude;
		        v.vertex.y += waveValueA ;
		        o.normal = normalize(float3(v.normal.x+waveValueA,v.normal.y,v.normal.z));
	          }
			  o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	          return o;
	      	}

			fixed4 frag(v2f IN):SV_Target
			{
				fixed4 c = tex2D(_MainTex, IN.uv)*_Color;
				return c;
			}
			ENDCG
		}
	} 
}
