Shader "Mogo/ChangeColor" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
		_ColorCtrl("Color Ctrl",float) = 0.5
	}
	SubShader {
		Tags { "Queue" = "Transparent+10" }
		LOD 200
		 Pass
		 {
		 ZWrite On
		 ZTest Off


			Blend SrcAlpha OneMinusSrcAlpha
			Lighting Off
			//Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _Color;
			float _ColorCtrl;

			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};

			float4 _MainTex_ST;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 texcol = tex2D (_MainTex, i.uv);
				fixed4 result = fixed4(1,1,1,1);

				if(texcol.r <= _ColorCtrl)
				{
					result = 2 * texcol * _Color;
				}
				else
				{
					result = 1 - 2 * ( fixed4(1,1,1,1) - texcol) * (fixed4(1,1,1,1) - _Color);
				}

				result.a = texcol.a;

				return result;
			}
			ENDCG
		 }
	} 
}
