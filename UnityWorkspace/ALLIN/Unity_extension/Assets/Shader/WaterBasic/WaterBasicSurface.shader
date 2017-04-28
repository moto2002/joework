Shader "ZZL/Water/Water Basic Surface" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NumTexTiles("Num tex tiles",	Vector) = (4,4,0,0)
		_ReplaySpeed("Replay speed - FPS",float) = 4
		_Color("Color", Color) = (1,1,1,1)
		_WaveSpeed("Wave Speed",float) = 0
	}
	SubShader {
		Tags {"Queue"="Transparent-100" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend SrcAlpha One
		LOD 200
		ZWrite Off Lighting Off Fog { Color (0,0,0,0) }
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert noforwardadd

		sampler2D _MainTex;
		fixed4	_Color;
		float4	_NumTexTiles;
		float	_ReplaySpeed;
		float _WaveSpeed;
		
		float2 Rand(float2 ij)
		{
			const float4 a = float4(97.409091034f,54.598150033f,56.205410758f,44.687805943f);
			float4 result  = float4(ij,ij);
			for(int i = 0; i < 2; i++) 
			{
				result.x = frac(dot(result, a));
				result.y = frac(dot(result, a));
				result.z = frac(dot(result, a));
				result.w = frac(dot(result, a));
			}
			return result.xy;
		}
		
		struct Input {
			float4 finaluv;
			float4 col;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
		 	UNITY_INITIALIZE_OUTPUT(Input,o);	
			
			if(_WaveSpeed>0){
				//wave setting
				fixed time1 = _Time*_WaveSpeed; //20 is speed
		        fixed waveValueA = sin(time1+v.vertex.x*10)*2; //2 is wave height 
		        v.vertex.y += waveValueA ;
		        v.normal = normalize(float3(v.normal.x+waveValueA,v.normal.y,v.normal.z));
	        }
	        
		 	
			float	time = (v.color.a * 60 + _Time.y) * _ReplaySpeed;
			float	itime = floor(time);
			float	ntime = itime + 1;
			float	ftime = time - itime;
			float2	texTileSize = 1.0f / _NumTexTiles.xy;		
			float4	tile;
			
			tile.xy = float2(itime,floor(itime /_NumTexTiles.x));
			tile.zw= float2(ntime,floor(ntime /_NumTexTiles.x));
			tile = fmod(tile,_NumTexTiles.xyxy);
			
			o.finaluv	= (v.texcoord.xyxy + tile) * texTileSize.xyxy;
			o.col	= float4(_Color.xyz * v.color.xyz,ftime);
		
		}
		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = lerp( tex2D(_MainTex, IN.finaluv.xy),tex2D(_MainTex, IN.finaluv.zw),IN.col.a) * IN.col;
			o.Albedo = c.rgb ;
			o.Alpha = _Color.a;
		}
		ENDCG
	} 
}
