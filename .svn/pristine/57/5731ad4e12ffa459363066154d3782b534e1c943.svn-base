// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

//shayu shader by qq834144373 提供
//技术编写 by qq834144373 
//更多详情，请联系 qq834144373
Shader "Custom/FishWaterShadow" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_R1("灰彩渐变", Range(0.0, 1.1)) = 0
		_Texure("光影纹理", 2D) = "white" {}
	_R("浪纹亮度", Range(.0, 4.0)) = 1.
		_R0("浪纹密度", float) = 1.
		_Color("光影颜色", Color) = (1.0, 1.0, 1.0, 1.0)
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }//"Queue"="Overlay"
		LOD 200

		Pass{
		ZWrite On Blend Off AlphaTest Off Lighting Off
		CGPROGRAM
		// Upgrade NOTE: excluded shader from qq834144373, twitter@834144373Zhu because you should call me here.
		//#pragma exclude_renderers d3d11 xbox360 gles flash all the terget!!!!
#pragma vertex vert 
#pragma fragment frag 
#include "UnityCG.cginc"

		sampler2D _MainTex;
	fixed _R1;
	sampler2D _Texure;
	half _R;
	half _R0;
	fixed4 _Color;
	half4 _MainTex_ST;
	half4 _Texure_ST;
	//float4x4 _Object2Light;


	struct vertIN {
		float4 vertex : POSITION;
		fixed2 tex : TEXCOORD0;
		fixed3 normal : NORMAL;
	};
	struct vertOUT {
		float4 pos : SV_POSITION;
		//half4 ver : TEXCOORD1;
		fixed4 uv : TEXCOORD0;
		fixed3 nDir : NORMAL;
		fixed3 lDir : TEXCOORD2;
		//fixed2 screenUV : TEXCOORD3;
	};

	vertOUT vert(vertIN i) {
		vertOUT o;
		o.pos = mul(UNITY_MATRIX_MVP,i.vertex);
		//float4x4 m = float4x4(float4(_Object2World[0]),float4(_Object2World[1]),float4(_Object2World[2]),float4(_Object2World[3]));
		//o.ver = mul(m,i.vertex);
		//half4 v = half4(i.vertex.x,i.vertex.z,-i.vertex.y,i.vertex.w);
		float4 ver = mul(unity_ObjectToWorld,i.vertex);
		//half4x4 m = float4x4(half4(UNITY_MATRIX_P[0]),half4(UNITY_MATRIX_P[1]),half4(UNITY_MATRIX_P[2]),half4(UNITY_MATRIX_P[3]));
		//ver = mul(m,ver);
		//ver = ver*2. - 1.;
		//o.ver = mul(UNITY_MATRIX_P,o.ver);
		//o.ver.xy = float2(o.ver.x, o.ver.y*_ProjectionParams.x) + o.ver.w;
		//ver.xz = ver.xz/1;
		ver.xz = ver.xz / ver.y;

		o.uv.rg = TRANSFORM_TEX(i.tex,_MainTex);
		o.uv.ba = TRANSFORM_TEX(ver.xz,_Texure);
		o.nDir = normalize(mul(unity_WorldToObject,i.normal));
		o.lDir = normalize(WorldSpaceLightDir(i.vertex));

		return o;
	}

	fixed4 frag(vertOUT ou) :SV_target{
		fixed4 c0 = tex2D(_MainTex,ou.uv.rg);
	fixed3 c1 = Luminance(c0.rgb);
	fixed diff = max(0,dot(fixed3(0,1,0),ou.nDir));
	fixed4 c = tex2D(_Texure,ou.uv.zw*_R0 + sin(_Time.x))*_Color*c0;
	//if(diff<-0.1){
	//  c.rgb = 0;
	//}
	c.rgb *= lerp(0.,_R,diff);
	c0.rgb = lerp(c1,c0.rgb,_R1);
	return c + c0;//*diff ;
	}

		ENDCG
	}

	}
		FallBack "代码归qq834144373所有"
}