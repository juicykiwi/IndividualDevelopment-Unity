Shader "UI/UnlitAlphaMask/MaskCircle" {
	Properties {
		_Color ("Color", Color) = (0.0, 0.0, 0.0, 0.0)
		
		_IsUseRateForPos ("Is use rate for pos", Float) = 0.0
		_CutScreenPos ("Cut screen pos", Vector) = (0.0, 0.0, 0.0, 0.0)
		
		_CutRadius ("Cut radius", Float) = 0.0
		_GradientRadius ("Gradient radius", Float) = 0.0	
	}
	
	SubShader {
		
		LOD 200
		
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"
			
			float4 _Color;
			float _IsUseRateForPos;
			float4 _CutScreenPos;
			float _CutRadius;
			float _GradientRadius;
			
			struct appdata_t
			{
				float4 vertexPos : POSITION;
				float4 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct VertOut
			{
				float4 svPos : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
			
			VertOut vert (appdata_t v)
			{
				VertOut o;
				
				// svPos
				o.svPos = mul(UNITY_MATRIX_MVP, v.vertexPos);
			
				// texcoord
				o.texcoord =  v.texcoord;
				
				// color
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (VertOut IN) : SV_Target
			{
				fixed4 retColor = _Color;
				
				float x = 0;
				float y = 0;
				
				if (_IsUseRateForPos != 0.0)
				{
					x = IN.texcoord.x;
					y = IN.texcoord.y;
				}
				else
				{
					x = _ScreenParams.x * IN.texcoord.x;
					y = _ScreenParams.y * IN.texcoord.y;
				}
				
				float2 distanceValue = float2((x - _CutScreenPos.x), (y - _CutScreenPos.y));
				float lengthValue = length(distanceValue);
				
				if (lengthValue <= _CutRadius)
				{
					retColor.a = 0.0;
				}
				else if (lengthValue <= (_CutRadius + _GradientRadius))
				{
					float gradientRate = (lengthValue - _CutRadius) / _GradientRadius;
					retColor.a = retColor.a * gradientRate;
				}
				
				return retColor;
			}
			
			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
