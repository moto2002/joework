Shader "Custom/2d_FlowLight"
{
	Properties
	{
		_MainTex("main tex",2d) = ""{}//主贴图
		_FlowTex("flow tex",2d) = ""{}//流光图
		_MaskTex("mask tex",2d) = ""{}//过滤图
		_FlowSpeed("speed", Range(0,1)) = 1.0//流光移动速度
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include"UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _FlowTex;
			sampler2D _MaskTex;
			float _FlowSpeed;

			struct v2f
			{
				float4 vertex:POSITION;
				float4 uv:TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			half4 frag(v2f IN):COLOR
			{
				//采样主贴图
				half4 c = tex2D(_MainTex,IN.uv);
				
				//流光uv移动
				half2 flow_uv = half2(IN.uv.x / 2,IN.uv.y);
				flow_uv.x += -_FlowSpeed * _Time.w;
				half4 flow = tex2D(_FlowTex, flow_uv);
				flow.rgb *= half3(1,1,0);

				//根据过滤图的alpha值，将流光效果限制在文字上
				half4 mask = tex2D(_MaskTex,IN.uv);
				if (mask.a == 1)
				{
					c.rgb /= 1.5;
					c.rgb += flow.rgb;
				}
				return c;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
