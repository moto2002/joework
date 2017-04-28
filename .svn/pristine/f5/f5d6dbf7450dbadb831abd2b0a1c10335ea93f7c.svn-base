// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

//结合SpriteMask.cs使用
Shader "ZZL/Native2D/Sprite Mask Soft Clip"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _StencilVal ("Stencil Ref",Int) = 1

		_ClipSoftX("Clip Soft X",Range(1,200))=20
		_ClipSoftY("Clip Soft Y",Range(1,200))=20
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode",float)=0
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
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;

				return color;
			}

			float _ClipSoftX;
			float _ClipSoftY;


			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 color = SampleSpriteTexture (IN.texcoord) * IN.color;
				color.rgb *= color.a;

				float2 factor = float2(0.0,0.0);
				float2 tempXY = (IN.worldPosition.xy - _ClipRect.xy)/float2(_ClipSoftX*0.01,_ClipSoftY*0.01)*step(_ClipRect.xy, IN.worldPosition.xy);
				factor = max(factor,tempXY);
				float2 tempZW = (_ClipRect.zw-IN.worldPosition.xy)/float2(_ClipSoftX*0.01,_ClipSoftY*0.01)*step(IN.worldPosition.xy,_ClipRect.zw);
				factor = min(factor,tempZW);
				fixed alpha= clamp(min(factor.x,factor.y),0.0,1.0);

				#ifdef MASKINVERT_ON
				color*=1-alpha;
				#else
				color*=alpha;
				#endif

				clip (color.a - 0.001);

				return color;
			}
		ENDCG
		}
	}
}
