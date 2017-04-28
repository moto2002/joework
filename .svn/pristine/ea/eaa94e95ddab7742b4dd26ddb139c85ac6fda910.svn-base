Shader "ZZL/Native2D/Diffuse Outline"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_OutLine ("Outline", Range(0,2)) = 0
		_OutlineColor("Outline Color", Color) = (1.0,1.0,1.0,1.0)
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

		Cull [_CullMode]
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma multi_compile _ PIXELSNAP_ON

		sampler2D _MainTex;
		fixed4 _Color;
		fixed _OutLine;
		fixed4 _OutlineColor;

		struct Input
		{
			float2 uv_MainTex;
			fixed4 color;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color * _Color;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			if(_OutLine>0)
			{
		    	_OutLine *= 0.01;
				fixed4 TempColor = tex2D(_MainTex, IN.uv_MainTex+float2(_OutLine,0.0)) + tex2D(_MainTex, IN.uv_MainTex-float2(_OutLine,0.0));
				TempColor = TempColor + tex2D(_MainTex, IN.uv_MainTex+float2(0.0,_OutLine)) + tex2D(_MainTex, IN.uv_MainTex-float2(0.0,_OutLine));
				if(TempColor.a > 0.1){
				   TempColor.a = 1;
				}
				fixed4 AlphaColor = (0,0,0,TempColor.a);
				fixed4 mainColor = AlphaColor * _OutlineColor;
				fixed4 addcolor = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
	  
	            if(addcolor.a > 0.95){
	               mainColor = addcolor;
	            }
	            o.Albedo = mainColor.rgb;
	            o.Alpha = mainColor.a;
			}
			else
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
				o.Albedo = c.rgb * c.a;
				o.Alpha = c.a;
			}
		}
		ENDCG
	}

}
