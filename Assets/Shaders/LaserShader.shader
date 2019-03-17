Shader "Custom/LaserShader"
{
	Properties
	{
		_NoiseTex("Noise Texture", 2D) = "white" {}
		[HDR]_ColorBottomDark("Color bottom dark", color) = (1,1,1,1)
		[HDR]_ColorTopDark("Color top dark", color) = (1,1,1,1)
		[HDR]_ColorBottomLight("Color bottom light", color) = (1,1,1,1)
		[HDR]_ColorTopLight("Color top light", color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog

#include "UnityCG.cginc"

			struct appdata 
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 noiseUV : TEXCOORD1;

			};

			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			fixed4 _ColorBottomDark;
			fixed4 _ColorTopDark;
			fixed4 _ColorBottomLight;
			fixed4 _ColorTopLight;

			v2f vert(appdata v) 
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.noiseUV = TRANSFORM_TEX(v.uv, _NoiseTex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//Noise
				half noise = tex2D(_NoiseTex, float2(i.noiseUV.x - _Time.x * 10, i.noiseUV.y)).x;

				// lerping between colors
				fixed4 col = lerp(lerp(_ColorBottomDark, _ColorTopDark, i.uv.x), lerp(_ColorBottomLight, _ColorTopLight, i.uv.x), noise);
				return col;
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}