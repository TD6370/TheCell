Shader "Custom/BreezeLeavesSurfaceShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_SamplerText ("Samlper Shift (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_ScaleShift ("Scale shift", Range (-1,1)) = 0.0
		_ShiftSpeed ("Shift Speed", Range (1,100)) = 1.0
	}
	SubShader {
		
		Tags {"Queue" = "Transparent" "RenderType"="Transparent"}
		//7ZWrite Off
		//Blend SrcAlpha OneMinusSrcAlpha
		//----
		
		// inside SubShader
		//Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" "IgnoreProjector"="True" }
		// inside Pass
		//AlphaToMask On
		//----

		//Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade
		//#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _SamplerText;
		float _ScaleShift;
		float _ShiftSpeed;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float2 uv =  IN.uv_MainTex;

			//Sampling shift
			//float wind = c.x+sin(unity_DeltaTime + c.y);
			//float wind = c.x+sin(_Time + c.y);
			float wind = c.x+sin(sin(_Time) + unity_DeltaTime + c.y);
			float u_time = lerp(_Time*100.,unity_DeltaTime,1.5);
			u_time += wind;
			
	    	float u_Scale =_ScaleShift * _ShiftSpeed;//sin(mouse.x*1.)-0.2;
			//u_Scale =_ScaleShift * sin(u_time+c.x)*cos(u_time+c.y)* 1.;
			//u_Scale =_ScaleShift * sin(u_time+c.y)*cos(u_time-c.y)* .9;
			u_Scale =_ShiftSpeed * sin(u_time)*cos(u_time)* .005 ;
			//
			if(uv.x > 0.15 && uv.x < 0.2 )
				u_Scale=0.;

			float2 v_vTexcoord  = IN.uv_MainTex;
    		float3 displace = tex2D (_SamplerText, v_vTexcoord).rba;
    		

			displace.xy -= 0.5;
			//--horizontal
			//displace.x -= 0.5;

    		//displace.xy *= displace.z * u_Scale*2.;
			displace.xy *= displace.zxy * u_Scale*2.;
			
			
			//displace.xy += wind*.1;

    		c = tex2D(_MainTex, v_vTexcoord + displace.xy ) * _Color;
			

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	//FallBack "Diffuse"
}
