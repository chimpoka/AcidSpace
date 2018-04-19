Shader "Custom/WorldAlignTexture" {
	Properties{
		_MainTexX("Texture X", 2D) = "white" {}
		_ColorX("Color X", Color) = (1,1,1,1)
		_EmissiveX("Emissive X", Range(0, 10)) = 1

		_MainTexY("Texture Y", 2D) = "white" {}
		_ColorY("Color Y", Color) = (1,1,1,1)
		_EmissiveY("Emissive Y", Range(0, 10)) = 1

		_MainTexZ("Texture Z", 2D) = "white" {}
		_ColorZ("Color Z", Color) = (1,1,1,1)
		_EmissiveZ("Emissive Z", Range(0, 10)) = 1
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Standard 

		sampler2D _MainTexX;
		sampler2D _MainTexY;
		sampler2D _MainTexZ;
		float4 _MainTexX_ST;
		float4 _MainTexY_ST;
		float4 _MainTexZ_ST;
		float4 _ColorX;
		float4 _ColorY;
		float4 _ColorZ;
		half _EmissiveX;
		half _EmissiveY;
		half _EmissiveZ;

	struct Input {
		float3 worldNormal;
		float3 worldPos;
	};

	void surf(Input IN, inout SurfaceOutputStandard o) {

		if (abs(IN.worldNormal.y) > 0.5)
		{
			fixed4 c = tex2D(_MainTexY, IN.worldPos.xz * _MainTexY_ST.xy + _MainTexY_ST.zw) * _ColorY * _EmissiveY;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		else if (abs(IN.worldNormal.x) > 0.5)
		{
			fixed4 c = tex2D(_MainTexX, IN.worldPos.yz * _MainTexX_ST.xy + _MainTexX_ST.zw) * _ColorX * _EmissiveX;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		else
		{
			fixed4 c = tex2D(_MainTexZ, IN.worldPos.xy * _MainTexZ_ST.xy + _MainTexZ_ST.zw) * _ColorZ * _EmissiveZ;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		o.Emission = o.Albedo;
	}

	ENDCG
	}
		FallBack "Diffuse"
}