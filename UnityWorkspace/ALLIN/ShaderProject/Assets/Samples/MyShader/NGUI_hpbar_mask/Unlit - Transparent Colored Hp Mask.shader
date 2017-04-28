Shader "Custom/Unlit/Transparent Colored Hp Mask"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		
		//---add---------------------------------
		_MaskTex ("Mask Alpha (A)", 2D) = "white" {}

		_WidthRate ("Sprite.width/Atlas.width", float) = 1
		_HeightRate ("Sprite.height/Atlas.height", float) = 1
		_XOffset("offsetX/Atlas.width", float) = 0
		_YOffset("offsetY/Atlas.height", float) = 0

	    //_Factor("factor",range(0,1)) = 1
		//--------------------------------------
	}
	
	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Offset -1, -1
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
				
			#include "UnityCG.cginc"
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			//---add-------
			sampler2D _MaskTex;

			float _WidthRate;
			float _HeightRate;
			float _XOffset; 
			float _YOffset; 

			//float _Factor;
			//--------------
				
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (v2f i) : COLOR
			{
			    fixed4 col;
			    col = tex2D(_MainTex, i.texcoord);
			    
			    //---------add---------------------------------
			    //过滤
				if(i.color.r<=0.1)
				{
					float2 final_uv = float2((i.texcoord.x - _XOffset) / _WidthRate, (i.texcoord.y - _YOffset) / _HeightRate);

					float curr = final_uv.x;
					final_uv.x *= 20;
					col.a = col.a * tex2D(_MaskTex, final_uv).a;

					if (curr >= i.color.g)
					{
						col.a = 0;
					}

				  /*if (curr >= _Factor)
					{
						col.a = 0;
					}*/
				}		
				//-----------------------------------------------
				return col;
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
