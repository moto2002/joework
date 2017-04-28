// Upgrade NOTE: replaced 'glstate.matrix.modelview[0]' with 'UNITY_MATRIX_MV'
// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'

Shader "ML/Rrefraction" {
Properties {
    _NoiseTex ("Noise Texture (RG)", 2D) = "white" {}
    strength("strength", Range(-1, 1)) = 1
    transparency("transparency", Range(0, 1)) = 1
}

Category {
    Tags { "Queue" = "Transparent+10" }
    SubShader {
        GrabPass {
	        Name "BASE"
            Tags { "LightMode" = "Always" }
        }
       
        Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			Fog { Mode off }
	        Lighting Off
		    Cull Off
			ZWrite On
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaTest Greater 0
         
         
CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members distortion)
#pragma exclude_renderers d3d11 xbox360
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members distortion)
#pragma exclude_renderers xbox360
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma fragmentoption ARB_fog_exp2
#include "UnityCG.cginc"

sampler2D _GrabTexture : register(s0);
float4 _NoiseTex_ST;
sampler2D _NoiseTex;
float strength;
float transparency;

struct data {
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 texcoord : TEXCOORD0;
};

struct v2f {
    float4 position : POSITION;
    float4 screenPos : TEXCOORD0;
    float2 uvmain : TEXCOORD2;
	float2 distortion : TEXCOORD1;
};

v2f vert(data i){
    v2f o;
    o.position = mul(UNITY_MATRIX_MVP, i.vertex);      // compute transformed vertex position
    o.uvmain = TRANSFORM_TEX(i.texcoord, _NoiseTex);   // compute the texcoords of the noise

	float viewAngle = dot(normalize(ObjSpaceViewDir(i.vertex)),
						 i.normal);
	o.distortion = viewAngle * viewAngle;	// square viewAngle to make the effect fall off stronger
	float depth = -mul( UNITY_MATRIX_MV, i.vertex ).z;	// compute vertex depth
	o.distortion /= 1+depth;		// scale effect with vertex depth
	o.distortion *= strength;	// multiply with user controlled strength
	o.screenPos = o.position;   // pass the position to the pixel shader
	return o;
}

half4 frag( v2f i ) : COLOR
{   
    // compute the texture coordinates
    float2 screenPos = i.screenPos.xy / i.screenPos.w;   // screenpos ranges from -1 to 1
    screenPos.x = (screenPos.x + 1) * 0.5;   // I need 0 to 1
    screenPos.y = (screenPos.y + 1) * 0.5;   // I need 0 to 1

    half4 offsetColor1 = tex2D(_NoiseTex, i.uvmain);

    screenPos.x += ((offsetColor1.r)) * i.distortion;
    screenPos.y += ((offsetColor1.g)) * i.distortion;
    
	screenPos.y = 1 - screenPos.y;
    half4 col = tex2D( _GrabTexture, screenPos );
    col.a = transparency;
	return col;
}

ENDCG
        }
    }
}

}