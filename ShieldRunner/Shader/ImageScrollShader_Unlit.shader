Shader "Custom/ImageScrollShader_Unlit" {
	Properties {
		_MainTint ("Diffuse Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MoveValueX ("X Move Value Speed", Range(0, 10)) = 0
		_MoveValueY ("Y Move Value Speed", Range(0, 10)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Unlit

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		fixed4 _MainTint;
		sampler2D _MainTex;
		fixed _MoveValueX;
		fixed _MoveValueY;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed2 scrolledUV = IN.uv_MainTex;
			
			scrolledUV += fixed2(_MoveValueX, _MoveValueY);
			scrolledUV.x = fmod(scrolledUV.x, 1);
			scrolledUV.y = fmod(scrolledUV.y, 1);
		
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, scrolledUV) * _MainTint;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		
		inline fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c = fixed4(1, 1, 1, 1);
			c.rgb = c * s.Albedo;
			c.a = s.Alpha;
			return c;
		}
		
		ENDCG
	} 
	FallBack "Diffuse"
}
