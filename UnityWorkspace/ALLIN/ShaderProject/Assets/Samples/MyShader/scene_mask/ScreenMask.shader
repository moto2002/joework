Shader "Custom/ScreenMask" {
Properties {
	_MaskPos ("MaskPos ", VECTOR) = (0.5,0.5,0,5)
}

SubShader {
	Tags { "RenderType"="Transparent"  "Queue" = "Transparent+100"}
	
	Pass {
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			float4 _MaskPos;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex.xy = v.vertex.xy * 2;
				o.vertex.z = 0;
				o.vertex.w = 1;
				o.texcoord = v.texcoord;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,1);
				float2 dis = i.texcoord.xy - _MaskPos.xy;
				dis *= _MaskPos.w;
				dis.x *= _ScreenParams.x * (_ScreenParams.w - 1);
				col.a = clamp(dot(dis,dis)-0.5f, 0, 0.8f);
				//col.a = saturate(col.a);
				return col;
			}
		ENDCG
	}
}

}
