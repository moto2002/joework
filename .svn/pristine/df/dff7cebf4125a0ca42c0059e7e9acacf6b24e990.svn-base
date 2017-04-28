Shader "mask shader"
{
   Properties
   {
      _MainTex ("Base (RGB)", 2D) = "white" {}//主纹理
      _Mask ("Culling Mask", 2D) = "white" {}//遮罩图
      _Cutoff ("Alpha cutoff", Range (0,1)) = 0.1//用来调整边缘的透明度
   }
   SubShader
   {
      Tags {"Queue"="Transparent"}
      Lighting Off
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      AlphaTest GEqual [_Cutoff]
      Pass
      {
         
         SetTexture [_Mask] {combine texture}
         SetTexture [_MainTex] {combine texture,texture-previous}
      }
   }
}