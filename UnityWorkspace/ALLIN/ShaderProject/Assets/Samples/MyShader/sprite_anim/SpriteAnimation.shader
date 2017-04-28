Shader "Custom/SpriteAnimation"
{
	Properties
	{
		_MainTex("main tex" ,2D) = ""{}
		_Row("行",Int) = 1
		_Column("列",Int) = 1
		_Speed("speed",Range(0,10)) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }

		Pass
		{
			Blend One OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			int _Row;
			int _Column;
			float _Speed;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			half4 frag(v2f IN) :COLOR
			{
				float2 uv = IN.uv;

				float cellX = uv.x / _Column;
				float cellY = uv.y / _Row;

				//Sprite总数
				int count = _Row * _Column;

				//在0到count-1 范围内循环
				int SpriteIndex = fmod(_Time.w*_Speed,count);

				//当前Sprite所在行的下标
				int SpriteRowIndx = (SpriteIndex / _Column);

				//当前Sprite所在列的下标
				int SpriteColumnIndex = fmod(SpriteIndex,_Column);

				//因uv坐标左下角为（0,0），第一行为最底下一行，为了合乎我们常理，我们转换到最上面一行为第一行,eg:0,1,2-->2,1,0
				SpriteRowIndx = (_Row - 1) - fmod(SpriteRowIndx,_Row);

				//乘以1.0转为浮点数,不然加号右边，整数除以整数，还是整数（有误）
				uv.x = cellX + SpriteColumnIndex*1.0 / _Column;
				uv.y = cellY + SpriteRowIndx*1.0 / _Row;

				half4 c = tex2D(_MainTex,uv);
				return c;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}