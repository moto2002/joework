Shader "Mogo/CameraLighting" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	//	_LightMap ("Base (RGB)",2D) = "white" {}
		_Controll("Control",Float) = 0.5
		_LastTime("Time",Float) = 0
		_CenterX("CenterX",Float) = 0
		_CenterY("CenterY",Float) = 0
		_Color("FadeColor",Color) = (1,1,1,1)
		}
	SubShader {
		 Pass
		 {
			ZTest Off
			ZWrite Off
			Lighting Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
		//	sampler2D _LightMap;
			float _Controll;
			float _LastTime;
			float _CenterX;
			float _CenterY;
			float4 _Color;
 
			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float4  poscolor : TEXCOORD1;
			};

			float4 _MainTex_ST;
		//	float4 _LightMap_ST;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				o.poscolor = o.pos;
				
				return o;
			}

			float4 frag (v2f i) : COLOR
			{
				_Controll = 1.0 -  _Controll  / (((i.poscolor.x - _CenterX) * (i.poscolor.x - _CenterX)+ (i.poscolor.y - _CenterY) * (i.poscolor.y-_CenterY)) + _Controll);
				half4 mainTex = tex2D(_MainTex,i.uv);
			//	half4 lightTex = tex2D(_LightMap,i.uv);
				half4 lightTex = _Color;

				return mainTex * _Controll+ lightTex * (1.0 - _Controll);
				//return i.poscolor;
				//return float4(_Controll,_Controll,_Controll,1.0);
			}
			ENDCG
		}
	}
}
