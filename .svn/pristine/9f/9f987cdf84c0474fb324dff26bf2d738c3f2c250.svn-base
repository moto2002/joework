Shader "Custom/nx_unlit" {
	Properties 
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _LightMap ("Mask Texture", 2D) = "gray" { }
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        Pass
        {   
            // pass drawing object
            Lighting Off
            
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            //#pragma multi_compile QOFFSET_OFF QOFFSET_ON
            //#include "QOffset.cginc"
            
            uniform float4 _MainTex_ST;
            uniform sampler2D _MainTex;
            uniform float4 _LightMap_ST;
            uniform sampler2D _LightMap;

            struct v2f {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
            };
            
            struct appdata_lightmap {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            
            v2f vert(appdata_lightmap v) {
                v2f o;
				//o.pos = GetQOffsetPos(v.vertex);
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord;
                o.uv1 = v.texcoord1.xy * _LightMap_ST.xy + _LightMap_ST.zw;
                return o;
            }
            
            half4 frag(v2f i) :COLOR 
            { 
                half4 main_color = tex2D (_MainTex, _MainTex_ST.xy * i.uv.xy + _MainTex_ST.zw);
                main_color.rgb *= ( tex2D(_LightMap, i.uv1).rgb + 2 * UNITY_LIGHTMODEL_AMBIENT.rgb );
                return main_color;
            }
                    
            ENDCG
        }
        
    }
    
    Fallback Off
}