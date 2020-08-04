Shader "Custom/DestroyBurningShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_SamplerText ("Background texture (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Red ("Red", Range (-1,1)) = -0.5
		_Green ("Green", Range (-1,1)) = 0.1
		_Blue ("Blue", Range (-5,5)) = 0.
		_F1 ("F1", Range (-1,1)) = 0.
		_F2 ("F2", Range (-1,1)) = 0.
	}
	SubShader {
		//Tags { "RenderType"="Opaque" }
		Tags {"Queue" = "Transparent" "RenderType"="Transparent"}
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _SamplerText;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Red; 
		float _Green;
		float _Blue;
		float _F1;
		float _F2;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		//------------------
		//fixed3 TextureSource(in float2 uv)
		//{
		//	return tex2D(iChannel0, uv).rgb;;
		//}

		fixed3 TextureTarget(in float2 uv)
		{
			//return texture(iChannel1, uv).rrr;
			return tex2D(_SamplerText, uv).rgb;
		}

		float Hash(in float2 p)
		{
			float3 p2 = float3(p.xy,1.0);
			return frac(sin(dot(p2,float3(37.1,61.7, 12.4)))*3758.5453123);
		}

		float noise(in float2 p)
		{
			float2 i = floor(p);
			float2 f = frac(p);
			f *= f * (3.0-2.0*f);

			return lerp(lerp(Hash(i + float2(0.,0.)), Hash(i + float2(1.,0.)),f.x),
				lerp(Hash(i + float2(0.,1.)), Hash(i + float2(1.,1.)),f.x),
				f.y);
		}

		float fbm(in float2 p) 
		{
			float v = 0.0;
			v += noise(p*1.)*.5;
			v += noise(p*2.)*.25;
			v += noise(p*4.)*.125;
			return v;
		}

		fixed3 Burning(in fixed3 src, in float2 uv)
		//float3 Burning(in fixed3 src, in float2 uv)
		{
			//float2 uv = (fragCoord - iResolution.xy*.5)/iResolution.y;
			//fixed3 src = TextureSource(uv);
			fixed3 tgt = TextureTarget(uv);
			fixed3 col = src;
			//float3 col = src;
	
			//uv.x -= 1.5;
			uv.x -= 2.;
			uv.y -= .5;
	
			float u_time = (_Time*20);

			//float ctime = mod(iTime*.5,2.5);
			//float ctime = mod(iTime*.9,2.5);
			float ctime = fmod(u_time*.9,2.5);
			//float ctime = fmod(u_time*.9,2.5);
	
			// burn
			//float d = uv.x+uv.y*0.5 + 0.5*fbm(uv*15.1) + ctime*1.3;
			//float d = cos(uv.y)*sin(uv.x)*2. - 0.5*fbm(uv*15.1) + ctime*1.3;
			//float figureMove = cos(uv.y)*sin(uv.x)*2.;
			float figureMove = cos(uv.y)*sin(uv.x)*1.5;
			float noiseRadius =  0.5*fbm(uv*15.1);
    
			float f1 = _F1; //-.5
			float f2 = _F2;
			float f_r = .7 + _Red; //1.5
			float f_g = .0 + _Green; //0.5
			float f_b = 5. + _Blue; //25.
    
			float d =  figureMove - noiseRadius + ctime*1.1;
			if (d >0.35) col = clamp(col-(d-0.35)*10.,0.0,1.0);
			if (d >0.47) {
				if (d < 0.5 ) 
					//col += (d-0.4)*33.0*0.5*(f1+noise(100.*uv+vec2(-ctime*2.,f2)))*vec3(f_r,f_g,f_b);
					col += (d-0.4)*33.0*0.5*(f1+noise(100.*uv+float2(-ctime*2.,f2)))*float3(f_r,f_g,f_b);
				else 
					col.rgb = 0.;
			}
			return col;
		}

		//----------------------
		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			//-------------------------
			fixed3 src = c.rgb;
			float2 uv = IN.uv_MainTex;
			fixed3 bur = Burning(src, uv);
			o.Albedo = bur;
			
			//-------------------------
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			
			if (bur.r == 0. && bur.g == 0. && bur.b == 0.)
				o.Alpha = 0.;
			else
				o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
