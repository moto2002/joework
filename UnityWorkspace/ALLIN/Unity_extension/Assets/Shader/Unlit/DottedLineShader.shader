Shader "ZZL/Unlit/Dotted Line"
{
    Properties
    {
    	_MainTex ("Texture", 2D) = "white" {}
		_AlphaTex ("Alpha Texture", 2D) = "white" {}
        _RepeatCount("Repeat Count", float) = 5
        _Spacing("Spacing", float) = 0.5
        _Offset("Offset", float) = 0
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _RepeatCount;
            float _Spacing;
            float _Offset;

			sampler2D _MainTex;
			sampler2D _AlphaTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;              
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.uv;
                o.uv.x = (o.uv.x + _Offset) * _RepeatCount * (1.0f + _Spacing);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.uv.x = fmod(i.uv.x, 1.0f);

                fixed4 color = tex2D (_MainTex,i.uv);;
				color.a = tex2D (_AlphaTex,i.uv).r;
                return color;
            }
            ENDCG
        }
    }
}