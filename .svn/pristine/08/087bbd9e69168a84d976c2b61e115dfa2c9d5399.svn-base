Shader "Mogo/Dission" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NosieTex("Nosie",2D) = "white"{}
		_NosieOffset("NosieOffset",Float) = 0
		_DissionColor("DissonColor",Color) = (1,1,1,1)
		_UVOffset("UIOffset",Float) = 0
	}
	SubShader {
		LOD 200
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		

		 Pass
		 {
		
	Fog{Mode Off}
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _NosieTex;
			float _NosieOffset;
			float _UVOffset;
			float4 _DissionColor;

			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};

			float4 _MainTex_ST;
			float4 _NosieTex_ST;

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
				fixed4 nosieCol = tex2D(_NosieTex,i.uv+_UVOffset);

				float data = nosieCol.r;//(nosieCol.r+nosieCol.b+nosieCol.g)*0.33f;

				if(data >= _NosieOffset)
				{
					return fixed4(0,0,0,0);
				}
				else if(data > abs(_NosieOffset - 0.015f))
				{
					return _DissionColor;
				}
				else
				{
					return texcol;
				}
				
			}
			ENDCG
		 }
	} 
}
