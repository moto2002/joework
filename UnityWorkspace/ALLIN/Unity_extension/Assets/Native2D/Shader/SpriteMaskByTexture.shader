﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

//结合SpriteMask.cs使用
Shader "ZZL/Native2D/Sprite Mask By Texture"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _StencilVal ("Stencil Ref",Int) = 1

		_MaskTex("Mask Texture",2D)= "white" {}
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode",float)=0
		_ClipRect("Clip Rect",Vector) = (0,0,100,100)
		[MaterialToggle] MaskInvert ("Mask Invert", Float) = 0
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

			Stencil
			{
				Ref [_StencilVal]
				Comp always
				Pass replace
			}

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ MASKINVERT_ON
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
				float4 worldPosition : TEXCOORD1;
			};
			
			fixed4 _Color;
			float4 _ClipRect;
			half _Size = 1;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = mul(unity_ObjectToWorld,IN.vertex);
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			sampler2D _MaskTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 color = SampleSpriteTexture (IN.texcoord) * IN.color;
				color.rgb *= color.a;

				fixed4 mask = tex2D (_MaskTex,(IN.worldPosition.xy - _ClipRect.xy)/_ClipRect.zw );
				#ifdef MASKINVERT_ON
				color*=1-mask.a;
				#else
				color*=mask.a;
				#endif

				clip (color.a - 0.001);

				return color;
			}
		ENDCG
		}
	}
}
