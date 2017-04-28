// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "ZZL/Unlit/Outlighting" {  
    Properties {  
        _Color ("Main Color", Color) = (.5,.5,.5,1)  
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)  
        _Outline ("Outline width", Range (0.0, 0.1)) = .005
        _Strength("Outlighting Strength", Range (0,2)) = 1
        _MainTex ("Base (RGB)", 2D) = "white" { }
    }
    SubShader {  
        Tags { "Queue" = "Transparent" }
        Pass {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            
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
	        fixed _Strength;
	        
	        v2f vert(appdata v) {  
	            v2f o;  
	            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);  
	            float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);  
	            float2 offset = TransformViewToProjection(norm.xy);
	            float posZ = o.pos.z;
	            posZ = max(posZ,1);
	            o.pos.xy += offset * posZ * _Outline;
	            o.color = _OutlineColor;
	            
	            
	            float4x4 modelMatrix        = unity_ObjectToWorld;
				float3x3 modelMatrixInverse = (float3x3)unity_WorldToObject;
                float3 normalDirection = normalize(mul(v.normal, modelMatrixInverse)).xyz;
                float3 viewDirection = normalize(_WorldSpaceCameraPos - mul(modelMatrix, v.vertex).xyz);
                float strength = abs(dot(viewDirection, normalDirection));
                o.color.a = pow(strength, _Strength+1);
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