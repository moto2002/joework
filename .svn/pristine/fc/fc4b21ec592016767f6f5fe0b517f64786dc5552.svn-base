Shader "Mogo/Stone" 
{
    Properties 
	{
         _Color ("Main Color", Color) = (1,1,1,1)
		 //_ColorMask("Mask Color",Color)= (0,0.86,1,0.78)
		 _MainTex ("Base (RGB)", 2D) = "white" { }
    }
    SubShader
	{
		 Tags
		 {
			"Queue" = "Transparent"
		 }
         Pass
		 {
			
			ZWrite On
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha
            Material
			{
                Diffuse [_Color]
                Ambient [_Color]
               // Shininess [_Shininess]
               // Specular [_SpecColor]
               // Emission [_Emission]
            }

            Lighting On

           // SeparateSpecular On
 
            SetTexture [_MainTex]
			{
                constantColor [_Color]
                Combine texture * Primary double,constant
            }
		 }
    }
} 