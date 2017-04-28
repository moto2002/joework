Shader "ZZL/Native2D/Default Outline"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_OutLine ("Outline", Range(0,1.2)) = 0
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

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			fixed _OutLine;
			fixed4 _OutlineColor;
			sampler2D _MainTex;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
			    if(_OutLine>0)
				{
			    	_OutLine *= 0.01;
					fixed4 TempColor = tex2D(_MainTex, IN.texcoord+float2(_OutLine,0.0)) + tex2D(_MainTex, IN.texcoord-float2(_OutLine,0.0));
					TempColor = TempColor + tex2D(_MainTex, IN.texcoord+float2(0.0,_OutLine)) + tex2D(_MainTex, IN.texcoord-float2(0.0,_OutLine));
					if(TempColor.a > 0.1){
					   TempColor.a = 1;
					}
					fixed4 AlphaColor = (0,0,0,TempColor.a);
					fixed4 mainColor = AlphaColor * _OutlineColor;
					fixed4 addcolor = tex2D(_MainTex, IN.texcoord) * IN.color;
		  
		            if(addcolor.a > 0.95){
		               mainColor = addcolor;
		            }
		            return mainColor;
				}
				else
				{
					fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
					c.rgb *= c.a;
					return c;
				}
			}
		ENDCG
		}
	}
}
