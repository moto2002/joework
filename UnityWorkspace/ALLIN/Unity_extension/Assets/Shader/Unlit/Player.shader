Shader "ZZL/Unlit/Player"
{
	Properties {  
        _NotVisibleColor ("NotVisibleColor (RGB)", Color) = (0.3,0.3,0.3,1)  
        _MainTex ("Base (RGB)", 2D) = "white" {}  
    }  
    SubShader {  
        Tags { "Queue" = "Geometry+500" "RenderType"="Opaque" }  
        LOD 200  
  
        Pass {
            ZTest Greater
            ZWrite Off
            Blend One OneMinusSrcAlpha
            SetTexture [_MainTex] { 
            	ConstantColor [_NotVisibleColor]
            	Combine texture*constant
            }
        }  
  
        Pass {  
            SetTexture [_MainTex] { combine texture }   
        }  
  
    }   
    FallBack "Unlit/Texture"
}
