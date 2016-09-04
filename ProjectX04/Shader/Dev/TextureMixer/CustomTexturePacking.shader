Shader "Custom/CustomTexturePacking" {
	Properties {
		_MainTint ("Diffuse Tint", Color) = (1, 1, 1, 1)
		_ColorA ("Terrain Color A", Color) = (1, 1, 1, 1)
		_ColorB ("Terrain Color B", Color) = (1, 1, 1, 1)
		
		_RTexture ("Red Channel Texture", 2D) = "" {}
		_GTexture ("Green Channel Texture", 2D) = "" {}
		_BTexture ("Blue Channel Texture", 2D) = "" {}
//		_ATexture ("Alpha Channel Texture", 2D) = "" {}
		
		_BlendTex ("Blend Texture", 2D) = "" {}
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
		float4 _ColorA;
		float4 _ColorB;

		sampler2D _RTexture;
		sampler2D _GTexture;
		sampler2D _BTexture;
//		sampler2D _ATexture;
		sampler2D _BlendTex;

		struct Input {
			float2 uv_RTexture;
			float2 uv_GTexture;
			float2 uv_BTexture;
//			float2 uv_ATexture;
			float2 uv_BlendTex;
		};
		
		float4 CustomLerp(float4 a, float4 b, float blendValue)
		{
			return (1 - blendValue) * a + blendValue * b;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			// 기본 샘플 코드로는 텍스쳐 사용이 많아 다음 에러가 발생한다.
			// Too many texture interpolators would be used for ForwardBase pass (11 out of max 10)
			// a. : 우선 에러 없이 사용하기 위해 lerp() 함수 하나 사용을 주석처리한다.
		
			// 혼합 텍스처로부터 픽셀 데이터를 얻어온다.
			// 텍스처가 R, G, B 그리고 A 또는 X, Y, z 그리고 W를 리턴하기 때문에
			float4 blendData = tex2D(_BlendTex, IN.uv_BlendTex);
			
			// 혼합할 텍스처로부터 데이터를 얻어온다.
			float4 rTexData = tex2D(_RTexture, IN.uv_RTexture);
			float4 gTexData = tex2D(_GTexture, IN.uv_GTexture);
			float4 bTexData = tex2D(_BTexture, IN.uv_BTexture);
//			float4 aTexData = tex2D(_ATexture, IN.uv_ATexture);
			
			// 이제 새 RGBA 값을 구성해 다른 혼합된 텍스처를 함께 더해야 한다.
			// lerp(a, b, blend f)
			//  a,b  텍스처를 c값을 이용하여 혼합한다.
			//  선형보간(linear interpolation)을 포함한다.
			//  (1 - f) * a + f * b
			//  a,b : 스칼라, 벡터
			//  f : a,b 와 같은 타입의 스칼라 또는 벡터
			float4 finalColor;
			finalColor = lerp(rTexData, gTexData, blendData.g);
			finalColor = CustomLerp(finalColor, bTexData, blendData.b);
			// a. : finalColor = CustomLerp(finalColor, aTexData, blendData.a);
			finalColor.a = 1.0;
			
			// 터레인에 색조를 더한다.
			float4 terrainLayers = lerp(_ColorA, _ColorB, blendData.r);
			finalColor *= terrainLayers;
			finalColor = saturate(finalColor);
			o.Albedo = finalColor.rgb * _MainTint.rgb;
			o.Alpha = finalColor.a;
		}
		
		ENDCG
	} 
	FallBack "Diffuse"
}
