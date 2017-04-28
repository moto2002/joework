Shader "Custom/3d_FlowLight" 
{
	Properties 
	{
		_MainTex("main tex",2D) = ""{}
		_FlowTex("flow tex",2D) = ""{}
		_FlowColor("flow color",Color) = (1,1,1,1)
		_FlowSpeed("flow speed",range(0,1)) = 0.1
	}

	SubShader
	{
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
				float3 normal:NORMAL;
				float3 tangent:TANGENT;
			};

			sampler2D _MainTex;
			sampler2D _FlowTex;
			half4 _FlowColor;
			float _FlowSpeed;

			v2f vert(appdata_tan v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
				o.uv = v.texcoord;
				o.normal = v.normal;
				o.tangent = v.tangent;
				return o;
			}
			
			half4 frag(v2f IN):COLOR
			{
				half4 c = tex2D(_MainTex,IN.uv);
				float2 flow_uv = IN.normal.xy;
				//float2 flow_uv = IN.tangent.xy;
				//float2 flow_uv = _ScreenParams.xy;
				flow_uv.x /= 2;
				flow_uv.x += _Time.w * _FlowSpeed;
				half4 flow = tex2D(_FlowTex, flow_uv);
				flow.rgb *= _FlowColor;
				c.rgb += flow.rgb;
				return c;
			}

			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
