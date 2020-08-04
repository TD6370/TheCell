Shader "Custom/ElectricShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows
		#pragma surface surf Lambert vertex:vert    fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		//-----------------
		struct Input
        {
			float2 uv_MainTex;
 			float3 localPos;
            //float4 color : COLOR;
            float4 Pos : SV_POSITION;
            float2 texCoord : TEXCOORD0;
			//float4 screenPos : TEXCOORD1;
        };
		
		void vert (inout appdata_full o) {
			float4 vPos = mul (UNITY_MATRIX_MV, o.Pos);
			vPos.xy=ign(vPos.xy);
			o.Pos = float4(vPos.xy, 0, 1);
			o.texCoord  = Pos.xy;
		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		//--------
		/*
		float4 color: register(c1);
		float glowStrength: register(c2);
		float height: register(c3);
		float glowFallOff: register(c4);
		float speed: register(c5);
		float sampleDist: register(c6);
		float ambientGlow: register(c7);
		float ambientGlowHeightScale: register(c8);
		float vertNoise: register(c9);
		float time_0_X: register(c0);
		sampler Noise: register(s0);

		float4 PS_Electricity(float2 texCoord: TEXCOORD) : COLOR 
		{
		   float2 t = float2(speed * time_0_X * 0.5871 - vertNoise * abs(texCoord.y), speed * time_0_X);

		   // Sample at three positions for some horizontal blur
		   // The shader should blur fine by itself in vertical direction
		   float xs0 = texCoord.x - sampleDist;
		   float xs1 = texCoord.x;
		   float xs2 = texCoord.x + sampleDist;

		   // Noise for the three samples
		   float noise0 = tex3D(Noise, float3(xs0, t));
		   float noise1 = tex3D(Noise, float3(xs1, t));
		   float noise2 = tex3D(Noise, float3(xs2, t));

		   // The position of the flash
		   float mid0 = height * (noise0 * 2 - 1) * (1 - xs0 * xs0);
		   float mid1 = height * (noise1 * 2 - 1) * (1 - xs1 * xs1);
		   float mid2 = height * (noise2 * 2 - 1) * (1 - xs2 * xs2);

		   // Distance to flash
		   float dist0 = abs(texCoord.y - mid0);
		   float dist1 = abs(texCoord.y - mid1);
		   float dist2 = abs(texCoord.y - mid2);

		   // Glow according to distance to flash
		   float glow = 1.0 - pow(0.25 * (dist0 + 2 * dist1 + dist2), glowFallOff);

		   // Add some ambient glow to get some power in the air feeling
		   float ambGlow = ambientGlow * (1 - xs1 * xs1) * (1 - abs(ambientGlowHeightScale * texCoord.y));

		   return (glowStrength * glow * glow + ambGlow) * color;
		}
		*/
		//----------

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		//----------

		ENDCG
	}
	FallBack "Diffuse"
}
