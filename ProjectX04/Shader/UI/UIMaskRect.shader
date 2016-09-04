Shader "UI/UnlitAlphaMask/MaskRect" {
	Properties {
		_Color ("Color", Color) = (0.0, 0.0, 0.0, 0.0)
		
		_IsUseRateForPos ("Is use rate for pos", Float) = 0.0
		_CutScreenPos ("Cut screen pos", Vector) = (0.0, 0.0, 0.0, 0.0)
		
		_CutWidth ("cutWidth", Float) = 0.0
		_CutHeight ("cutHeight", Float) = 0.0	
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
			float _CutWidth;
			float _CutHeight;
			
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

				if (x >= _CutScreenPos.x - (_CutWidth * 0.5) &&
					x <= _CutScreenPos.x + (_CutWidth * 0.5) &&
					y >= _CutScreenPos.y - (_CutHeight * 0.5) &&
					y <= _CutScreenPos.y + (_CutHeight * 0.5))
				{
					retColor.a = 0.0;
				}
				
				return retColor;
			}
			
			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
