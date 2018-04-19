Shader "Custom/RotatedBlockShader" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Emissive("Emissive", Range(0, 10)) = 1

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Standard 




		sampler2D _MainTex;
		fixed4 _Color;
		half _Emissive;

		struct Input {
			float2 uv_MainTex;
		};

		
		

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color * _Emissive;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission = o.Albedo;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
