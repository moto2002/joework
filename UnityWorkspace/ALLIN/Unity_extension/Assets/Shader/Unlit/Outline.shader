Shader "ZZL/Unlit/Outline" {  
    Properties {  
        _Color ("Main Color", Color) = (.5,.5,.5,1)  
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)  
        _Outline ("Outline width", Range (0.0, 0.03)) = .005  
        _MainTex ("Base (RGB)", 2D) = "white" { }  
    }
    SubShader {  
        Tags { "Queue" = "Transparent" }
        Pass {//画描边.
            ZWrite Off
            CGPROGRAM 
            #pragma vertex vert
            #pragma fragment frag 
            #include "UnityCG.cginc"
            
	        struct appdata {  
	            float4 vertex : POSITION;  
	            float3 normal : NORMAL;  
	        };  
	        struct v2f {  
	            float4 pos : POSITION;  
	            fixed4 color : COLOR;
	        };  
	        fixed _Outline;  
	        fixed4 _OutlineColor;
	        v2f vert(appdata v) {  
	            v2f o;  
	            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);  
	            float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);  
	            float2 offset = TransformViewToProjection(norm.xy);  
	            o.pos.xy += offset * o.pos.z * _Outline;  
	            o.color = _OutlineColor;  
	            return o;  
	        }  
	        
            fixed4 frag(v2f i) :COLOR {
                return i.color;
            }
            ENDCG
        }
        
        Pass {
            SetTexture [_MainTex] {
                ConstantColor [_Color] 
                Combine texture * constant
            }
        }
    }  
    Fallback "Unlit/Texture"
} 