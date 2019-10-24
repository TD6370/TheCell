Shader "Hidden/NewImageEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ColorMix ("COLOR MIX", Color) = (1,1,1,1.0)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		LOD 200

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.vertex = mul (UNITY_MATRIX_MVP, v.vertex);       
				o.uv = v.uv;
			    o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			sampler2D _MainTex;
			fixed4  _ColorMix;

			float4  MixColor(in fixed4 frag, in float4  vColour)
			{
				float maxcolor = max ( max ( frag.r, frag.g ), frag.b );
				float mincolor = min ( min ( frag.r, frag.g ), frag.b );
				float4  luminance = float4  (maxcolor,maxcolor,maxcolor, maxcolor );
				float saturation = ( maxcolor - mincolor ) / maxcolor;
    
				frag.rgb = lerp ( luminance, vColour.rgb * luminance, saturation );
				frag.a *= vColour.a;
				return frag;
			}



			fixed4 frag (v2f i) : SV_Target
			//fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				
				// just invert the colors
				//col.rgb = 1 - col.rgb;
				//return col;
				//----------------
				float2 mouse = float2(.5,.5);
				float iTime = 1.;
				//float4 colorMix = float4(1.2+mouse.x*5.,0.2+mouse.y*5.,3.2*sin(iTime),1.);
				float4 colorMix =_ColorMix;
				
				
				col = MixColor(col, colorMix);
				
				//----------------
				/*
				// screenPos.xy will contain pixel integer coordinates.
                // use them to implement a checkerboard pattern that skips rendering
                // 4x4 blocks of pixels

                // checker value will be negative for 4x4 blocks of pixels
                // in a checkerboard pattern
				//float2 screenPos = i.uv;
				float2 screenPos = i.screenPos;
                screenPos.xy = floor(screenPos.xy * 0.25) * 0.5;
                float checker = -frac(screenPos.r + screenPos.g);

                // clip HLSL instruction stops rendering a pixel if value is negative
                clip(checker);

                // for pixels that were kept, read the texture and output it
               	*/
				//----------------

				//o.Albedo = _Color.rgb;
				//o.Emission = _Color.rgb; // * _Color.a;
				//o.Alpha = _Color.a;

				return col;
			}
			ENDCG
		}
	}
}
