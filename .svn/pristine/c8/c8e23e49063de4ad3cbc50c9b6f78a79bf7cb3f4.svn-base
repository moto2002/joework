Shader "Custom/Unlit/Transparent Colored Grey Mask Flow"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		
		//---add---------------------------------
		_MaskTex ("Mask Alpha (A)", 2D) = "white" {}
		_IfMask("Open mask if larger than 0.5", Range(0,1)) = 0
		_WidthRate ("Sprite.width/Atlas.width", float) = 1
		_HeightRate ("Sprite.height/Atlas.height", float) = 1
		_XOffset("offsetX/Atlas.width", float) = 0
		_YOffset("offsetY/Atlas.height", float) = 0
		_FlowTex("flow tex",2D) = ""{}
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
			float _IfMask; 
			float _WidthRate;
			float _HeightRate;
			float _XOffset; 
			float _YOffset; 
			sampler2D _FlowTex;
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
				if(_IfMask>0.5)
				{
					col.a = col.a * tex2D(_MaskTex, float2((i.texcoord.x-_XOffset)/_WidthRate, (i.texcoord.y-_YOffset)/_HeightRate)).a; 
				}		
				//变灰
				if(i.color.r<=0.1)
				{
					float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));  
					col.rgb = float3(grey, grey, grey);  
				}
				//流光
				if(i.color.g<=0.1)
				{
					float2 flow_uv = float2((i.texcoord.x-_XOffset)/_WidthRate, (i.texcoord.y-_YOffset)/_HeightRate);
					flow_uv.x/=2;
					flow_uv.x-= _Time.y *2;
					half flow = tex2D(_FlowTex,flow_uv).a;	
					col.rgb+= half3(flow,flow,flow);
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
