Shader "Mogo/MonsterHitShader" {
	 Properties 
	{
         _Color ("Main Color", Color) = (1,0,0)
		// _ColorMask("Mask Color",Color)= (0.83,0.043,0.043,0.78)
		 _MainTex ("Base (RGB)", 2D) = "white" { }
    }
    SubShader
	{

		Pass 
		{
            Material
			{
                Diffuse [_Color]
                Ambient [_Color]
               // Shininess [_Shininess]
                //Specular [_SpecColor]
               // Emission [_Emission]
            }

            Lighting Off

           // SeparateSpecular On

            SetTexture [_MainTex]
			{
                constantColor [_Color]
                Combine texture * constant QUAD, texture * constant
            }
		}
    }
}
