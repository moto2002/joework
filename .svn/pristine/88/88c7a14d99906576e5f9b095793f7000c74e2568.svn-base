Shader "Custom/old_tv"
{
	Properties
	{
		_MainTex("main tex", 2d) = ""{}//主贴图
		_TVTex("TV tex",2d) = ""{}//屏幕内容
		_TVMask("TV mask",2d) = ""{}//过滤贴图
	}
	
	SubShader
	{
		Tags
		{
			"Queue"  = "Transparent"
		}
		
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			struct v2f
			{
				float4 vertex:POSITION;
				float4 uv:TEXCOORD0;
			};
			
			sampler2D _MainTex;
			sampler2D _TVTex;//视频内容从脚本本中设置
			sampler2D _TVMask;
			
			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
				o.uv.xy = v.texcoord.xy;
				return o;
			}
			
			half4 frag(v2f i):COLOR
			{
				//采样电视机贴图
				half4 tv = tex2D(_MainTex,i.uv.xy);
				
				//采样过滤贴图
				half4 mask = tex2D(_TVMask,i.uv.xy);
				
				//得到用过滤图的alpha值作为权重 影响过后的uv
				half2 maskuv = i.uv.xy * mask.a;
				
				//用该uv，采样屏幕内容，根据过滤图的alpha值，得到内容只在电视机屏幕区域显示的效果
				half4 tvcontent = tex2D(_TVTex,maskuv);
				tvcontent.a = mask.a;
				
				//return tvcontent;
				//return tv;
				
				half4 result = tv;
				//是屏幕上的区域
				if(mask.a==1)
				{
					//和屏幕内容颜色混合
					result = tv + tvcontent;
					result.rgb=result.rgb/2;
				}
				else
				{
					//是电视机外壳区域，什么也不做，保持原来的颜色
				}
				return result;
			}			
			
			ENDCG
		}
	}

	FallBack "Diffuse"
}