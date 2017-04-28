Shader "Mogo/BlackWhite" 
{
    Properties 
	{
		 _MainTex ("Base (RGB)", 2D) = "white" { }
    }
    SubShader
	{

		 Tags
		 {
			"Queue" = "Transparent+10"
		 }
         Pass
		 {
			Lighting Off
			ZTest Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			half4 _Color;
		
			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};

			half4 _MainTex_ST;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				half4 texcol = tex2D (_MainTex, i.uv);
				half4 result = half4((texcol.r + texcol.g + texcol.b) * 0.33f,(texcol.r + texcol.g + texcol.b) * 0.33f,(texcol.r + texcol.g + texcol.b) * 0.33f,texcol.a);

				return result;
			}
			ENDCG
		 }
    }
} 