Shader "Custom/Default"
{
	Properties 
	{
		_MainTex("main tex",2D) = ""{}
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
			};

			sampler2D _MainTex;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			
			half4 frag(v2f IN):COLOR
			{
				half4 c = tex2D(_MainTex,IN.uv);
				return c;
			}
			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
