Shader "Custom/TextureMixerBlend" {
	Properties {
		_MainTint ("Main Tint", Color) = (1, 1, 1, 1)
		_AddColor ("Add Color", Color) = (1, 1, 1, 1)
		
		_MaterialTexture01 ("Material Texture 01", 2D) = "" {}
		_MaterialTexture02 ("Material Texture 02", 2D) = "" {}
		_MaterialTexture03 ("Material Texture 03", 2D) = "" {}
		
		_MixTexture ("Mix Texture", 2D) = "" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float4 _MainTint;
		float4 _AddColor;
		
		sampler2D _MaterialTexture01;
		sampler2D _MaterialTexture02;
		sampler2D _MaterialTexture03;
		
		sampler2D _MixTexture;

		struct Input {
			float2 uv_MaterialTexture01;
			float2 uv_MaterialTexture02;
			float2 uv_MaterialTexture03;
			
			float2 uv_MixTexture;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			// 기본 샘플 코드로는 텍스쳐 사용이 많아 다음 에러가 발생한다.
			// Too many texture interpolators would be used for ForwardBase pass (11 out of max 10)
			// a. : 우선 에러 없이 사용하기 위해 lerp() 함수 하나 사용을 주석처리한다.
		
			// 혼합할 텍스처로부터 데이터를 얻어온다.
			float4 materialTexData01 = tex2D(_MaterialTexture01, IN.uv_MaterialTexture01);
			float4 materialTexData02 = tex2D(_MaterialTexture02, IN.uv_MaterialTexture02);
			float4 materialTexData03 = tex2D(_MaterialTexture03, IN.uv_MaterialTexture03);
			
			float4 mixTexData = tex2D(_MixTexture, IN.uv_MixTexture);
			
			// 이제 새 RGBA 값을 구성해 다른 혼합된 텍스처를 함께 더해야 한다.
			// lerp(a, b, blend f)
			//  a,b  텍스처를 c값을 이용하여 혼합한다.
			//  선형보간(linear interpolation)을 포함한다.
			//  (1 - f) * a + f * b
			//  a,b : 스칼라, 벡터
			//  f : a,b 와 같은 타입의 스칼라 또는 벡터
			float4 finalColor;
			finalColor = lerp(materialTexData01, materialTexData02, 0.5f);
			finalColor = lerp(finalColor, materialTexData02, 0.5f);
			finalColor = lerp(finalColor, materialTexData03, 0.5f);
			finalColor.a = 1.0;
			
			// 터레인에 색조를 더한다.
			finalColor *= _AddColor;
			finalColor = saturate(finalColor);
			o.Albedo = finalColor.rgb * _MainTint.rgb;
			o.Alpha = finalColor.a;
			
			mixTexData.rgb = o.Albedo;
			mixTexData.a = o.Alpha;
			
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
