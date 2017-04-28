Shader "ZZL/Unlit/Mobile Simple Bump" {
	Properties 
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _NormalMap("NormalMap",2D) = "Bump"{}
    }
    SubShader 
    {
    	Tags { "RenderType"="Transparent" "IgnoreProjector"="True"}
		LOD 100
		Lighting off
         
        Pass
        {
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            sampler2D _MainTex_ST;
            sampler2D _NormalMap;
            fixed4 _LightColor0;
 
            struct VertexOutput 
            {
                float4 pos:SV_POSITION;
                fixed2 uv:TEXCOORD0;
                fixed3 lightDir:TEXCOORD1;
                fixed3 viewDir:TEXCOORD2;
            };
 
            VertexOutput vert(appdata_tan v)
            {
                VertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
                o.uv = v.texcoord.xy;
                
                fixed3 normal = v.normal;
                fixed3 tangent = v.tangent;
                float3 binormal= cross(v.normal,v.tangent.xyz) * v.tangent.w;
                float3x3 Object2TangentMatrix = float3x3(tangent,binormal,normal);
                o.lightDir = mul(Object2TangentMatrix,ObjSpaceLightDir(v.vertex));
                o.viewDir = mul(Object2TangentMatrix,ObjSpaceViewDir(v.vertex));
                return o;
            }
 
            fixed4 frag(VertexOutput input):COLOR
            {
                fixed3 lightDir = normalize(input.lightDir);
                fixed3 viewDir = normalize(input.viewDir);
                fixed4 encodedNormal = tex2D(_NormalMap,input.uv);
                fixed3 normal = float3(2.0*encodedNormal.ag - 1,0.0);
                normal.z = sqrt(1 - dot(normal,normal));
                fixed4 texColor = tex2D(_MainTex,input.uv);
                fixed3 ambient = texColor.rgb ; //* UNITY_LIGHTMODEL_AMBIENT.rgb
                fixed3 diffuseReflection = texColor.rgb * max(0,dot(normal,lightDir))* _LightColor0.rgb ;
 
                return fixed4(ambient + diffuseReflection,texColor.a);
            }
            ENDCG
        }
         
    }
	FallBack "Mobile/Diffuse"
}
