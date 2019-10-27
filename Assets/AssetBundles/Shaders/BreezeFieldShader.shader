Shader "Custom/BreezeFieldShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags {"Queue" = "Transparent" "RenderType"="Transparent"}
		//Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard 
		//#pragma surface surf Standard fullforwardshadows alpha:fade vertex:vert
		//#pragma surface surf Lambert vertex:vert

		#pragma surface surf Lambert vertex:vert    fullforwardshadows alpha:fade
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		//struct Input {
		//	float2 uv_MainTex;
		//};
		//-----------------
		struct Input
        {
			float2 uv_MainTex;
            //float3 worldPos;
            //float3 worldNormal;
 			float3 localPos;

            //float4 color : COLOR;
            float4 vertex : SV_POSITION;
            //float2 uv : TEXCOORD0;
			//float4 screenPos : TEXCOORD1;
        };

        //void vert (inout appdata_full v, out Input o)
        //{
        //    float4 vPos = mul (UNITY_MATRIX_MV, v.vertex);
		//	o.localPos = v.vertex.xyz;
		//	o.vertex = mul (UNITY_MATRIX_P, vPos);
        //}
		void vert (inout appdata_full o) {
			

			float4 vPos = mul (UNITY_MATRIX_MV, o.vertex);
			//o.localPos = o.vertex.xyz;
			o.vertex.xyz += o.normal;
			
			float wind = o.vertex.x + sin(sin(_Time) + unity_DeltaTime + o.vertex.y);
			float u_time = lerp(_Time*100.,unity_DeltaTime,1.5);
			u_time += wind;

			//float top  = o.vertex.y*o.vertex.y;
			float top  = pow(o.vertex.y,3.);

			float offset =cos(u_time * top * 1.5)*.01;

			o.vertex.x += offset;
			//o.vertex = mul (UNITY_MATRIX_P, vPos);
		}
		//-----------------

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		//void surf (Input IN, inout SurfaceOutputStandard o) {
		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
