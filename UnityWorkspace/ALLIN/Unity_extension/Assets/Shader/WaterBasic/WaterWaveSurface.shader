Shader "ZZL/Water/Water Wave Surface" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
//		_Detail ("Base (RGB)", 2D) = "white" {}
		[HideInInspector]_WaveY("Wave Min Y", float) = 0
		_Speed("Speed", float) = 20
		_Frequency("Frequency", float) = 0.5
		_Amplitude("Amplitude", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True" }
		LOD 200
		Lighting off
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert noforwardadd
		
		sampler2D _MainTex;
//		sampler2D _Detail;
		half _Speed;
		fixed _Frequency;
		fixed _Amplitude;
		fixed _WaveY;

		struct Input {
			float2 uv_MainTex;
//			float2 uv_Detail;
		};
		
		void vert (inout appdata_full v) {
		  if(v.vertex.y>_WaveY)
          {
          	fixed time = _Time*_Speed;
	        fixed waveValueA = sin(time+v.vertex.x*_Frequency)*_Amplitude;
	        v.vertex.y += waveValueA ;
	        v.normal = normalize(float3(v.normal.x+waveValueA,v.normal.y,v.normal.z));
          }
      	}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
