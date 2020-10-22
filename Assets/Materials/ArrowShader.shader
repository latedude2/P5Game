Shader "CustomShaders/ArrowShader"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1) // RGBA
	}

	SubShader
	{ // could be several SubShaders for e.g. differently powered computers or such
	
		ZWrite On
		

		Pass
		{
			CGPROGRAM

			#pragma vertex vertShader //vertex shader
			#pragma fragment fragShader // fragment shader

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			float4 _Color;

			struct v2f {
				float4 pos: SV_POSITION;
				float4 color: COLOR0;
			};

			v2f vertShader(appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				half3 normalsInWorldSpace = normalize(UnityObjectToWorldNormal(v.normal));
				half diffuseAmount = max(0, dot(normalsInWorldSpace, _WorldSpaceLightPos0.xyz));
				o.color = _LightColor0 * diffuseAmount;
				return o;
			}

			fixed4 fragShader(v2f input) : SV_TARGET{
				fixed4 color = input.color * _Color;
				return color;
			}

			ENDCG
		}
	}
		// Fallback could be used here put some shader on in case of a crash
}
