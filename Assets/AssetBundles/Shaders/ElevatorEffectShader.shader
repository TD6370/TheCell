Shader "Custom/Effect/ElevatorEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_IsColorMix ("IS COLOR MIX", Range (0,1)) = 1
		_IsColorMixRGB ("IS COLOR MIX RGB", Range (0,1)) = 1
		_Color ("COLOR MIX", Color) = (1,1,1,1.0)
		_ColorMixR ("COLOR MIX R", Color) = (1,1,1,1.0)
		_ColorMixG ("COLOR MIX G", Color) = (1,1,1,1.0)
		_ColorMixB ("COLOR MIX B", Color) = (1,1,1,1.0)
		_HorizontOffset ("Horizont Offset", Range (1,200)) = 125
		_VerticalOffset ("Vertical Offset", Range (1,3000)) = 800
		_LimitHide ("Limit Hide", Range (0,40)) = 0.2
		_GradientHide ("Gradient Hide", Range (0,40)) = 1.5
		_GreenAnima ("Limit Anima", Range (-50,50)) = 0.
		_IsFilled ("IS FILLED", Range (0,1)) = 1
		_SpeedFill ("Speed Fill", Range (1,20)) = 3.
	}
	SubShader
	{
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always
		
		//Cull Off ZWrite Off ZTest Off  

		//-------------
		//Tags { "Queue" = "Transparent" }
		//Cull Off ZWrite Off ZTest Less  
		//Blend SrcAlpha OneMinusSrcAlpha
		//-------------------------
		//Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		//-------------------------
		
		//Cull Off
		//LOD 200

		//Added  texture  =====================
		//Tags { "Queue" = "Transparent" }
        //Pass {
        //    Blend One One
        //    SetTexture [_MainTex] { combine texture }
        //}
		//================

		// inside SubShader
		//Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" "IgnoreProjector"="True" }
		// inside Pass
		//AlphaToMask On

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
			fixed4  _Color;
			fixed4  _ColorMixR;
			fixed4  _ColorMixG;
			fixed4  _ColorMixB;
			float _HorizontOffset;
			float _VerticalOffset;
			float _IsColorMix;
			float _IsColorMixRGB;
			float _LimitHide;
			float _GradientHide;
			float _GreenAnima;
			float _SpeedFill;
			float _IsFilled;

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

			float4 ColorMix3(in fixed4 fragColor, in float4 colorMixR, in float4 colorMixG, in float4 colorMixB)
			{
				float maxcolor = max ( max (fragColor.r, fragColor.g), fragColor.b );
				float mincolor = min ( min (fragColor.r, fragColor.g), fragColor.b );
				float saturation = ( maxcolor - mincolor ) / maxcolor;
				float3 luminance = float3 (maxcolor,maxcolor,maxcolor);
				float3 newcolor = colorMixR.rgb * fragColor.r + 
								colorMixG.rgb * fragColor.g + 
								colorMixB.rgb * fragColor.b;
				fragColor.rgb = lerp( luminance, newcolor, saturation );                   
				return fragColor;
			}

			fixed4 anima(in float2 st, in float2 screenPos, in float u_time, in float horizontX, in float vertical, in float greenAnima) 
			{
				float3 pct = float3(st.x, st.x, st.x);
				pct.b =
					frac(ceil(st.y*vertical-u_time)/ceil(st.x*horizontX+20.))
					*sin(st.y*2.)/tan(st.y)
				;
				pct.r=(pct.b-.4)*sin(st.x-.2);
				pct.g = 0.;//greenAnima;
				pct.g +=greenAnima;
				return fixed4(pct.rgb,1.0);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float2 screenPos = i.screenPos;
				float2 mouse = float2(.5,.5);
				float iTime = 1.;
				fixed4 colorMix  = _Color ;
			
				float u_time;
				//u_time = (_Time + unity_DeltaTime)*600.;
				u_time = (_Time)*1000.+1500.;

				fixed4 anima1 = anima(i.uv, screenPos, u_time, _HorizontOffset, _VerticalOffset, _GreenAnima);
				
				bool isAfterMix = false;
				bool isVerticalLimitHide = true;
				bool isLimitStart = false;
				float timeFill = _Time*_SpeedFill;
				float limitFill = sin(timeFill)*cos(timeFill)*1.3+.6;
				//limitFill = ceil(limitFill);
				if(_IsFilled == 1)
				{
					//float posY = abs(col.y);
					//float posY = abs(col.y);
					float posY = col.y;
					//posY = floor(col.y);
					//posY = round(col.y);
					isVerticalLimitHide = posY > limitFill;
					isLimitStart = posY > limitFill+.1;
					isAfterMix = posY > limitFill-.2;
				}
				if(isLimitStart){
					col.a =0.;
				} else{
					float limRGB = anima1.r + anima1.g + anima1.b;
					//limRGB = round(limRGB);
					if(limRGB < _LimitHide){
						if(anima1.a > 0. ){
							if(isVerticalLimitHide)
								col.a = (limRGB/_LimitHide)*_GradientHide;
							if(isAfterMix){
								col.r = (limRGB/_LimitHide)*_GradientHide*.5;
								col.g*=.5;
							}
						}
					}
				}
				
				if(_IsColorMix==1.)
					col = MixColor(col, colorMix );
				if(_IsColorMixRGB==1.)
					col = ColorMix3(col, _ColorMixR, _ColorMixG, _ColorMixB);

				return col;
			}
			ENDCG
		}
	}
}
