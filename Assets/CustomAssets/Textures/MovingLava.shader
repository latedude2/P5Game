Shader "Unlit/MovingLava"{
	Properties
	{
		_CellColor("Cell Color", Color) = (1,1,1,1) //R,G,B,A - white
		_SecondaryColor("Secondary Color", Color) = (1,1,1,1) //R,G,B,A - white
		_CellAmount ("Cell Amount", Range(1, 20)) = 1
		_Shininess("Shininess", Float) = 3
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque"}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" // for _LightColor0

			fixed4 _CellColor;
			fixed4 _SecondaryColor;
			float _CellAmount;
			float _Shininess;
			
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 col : COLOR0; // diffuse lighting color
				float3 normal : NORMAL;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				float _MaxDisplacement = 1;


				o.vertex = v.vertex;
				o.vertex.y += sin(0.5 * (v.vertex.x + _Time.w)) / 10;
				o.vertex.y += cos(0.5 * (v.vertex.z + _Time.w)) / 9;

				o.vertex = UnityObjectToClipPos(o.vertex);

				o.uv = v.texcoord;
				

				// get vertex normal in world space
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);

				float4x4 modelMatrix = unity_ObjectToWorld;
				float3 viewport = normalize(_WorldSpaceCameraPos - mul(modelMatrix, v.vertex).xyz);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				float3 reflectionVector = reflect(-lightDirection, worldNormal);
				// standard diffuse (Lambert) lighting
				fixed4 nl = max(0, dot(worldNormal, lightDirection));
				// factor in the light color
				fixed4 diffuse = nl * _LightColor0;
				// find specular dot product
				fixed4 specular = pow(max(0.0, dot(reflectionVector, viewport)), _Shininess) * _LightColor0;
				o.col = specular + diffuse;
				return o;
			}

			float2 random2(float2 p)
			{
				//return frac(sin(float2(dot(p,float2(117.12,341.7)),dot(p,float2(269.5,123.3))))*43458.5453);
				return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3))))*43758.5453);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//fixed4 col = fixed4(0,0,0,1);
				fixed4 col = fixed4(0,0,0,1);

				float2 uv = i.uv;
				uv *= _CellAmount; //Scaling cells
				float2 iuv = floor(uv); //int values, no floating point
				float2 fuv = frac(uv); //only the fractional part
				float minDist = 1.0;

				for (int y = -1; y <= 1; y++)
				{
					for (int x = -1; x <= 1; x++)
					{
						// Position of neighbour on the grid
						float2 neighbour = float2(float(x), float(y));
						// Random position from current + neighbour place in the grid
						float2 pointv = random2(iuv + neighbour);
						// Move the point with time
						pointv = 0.5 + 0.5*sin(_Time.z + 6.2236*pointv);//each point moves in a certain way
																		// Vector between the pixel and the point
						float2 diff = neighbour + pointv - fuv;
						// Distance to the point
						float dist = length(diff);
						// Keep the closer distance
						minDist = min(minDist, dist);
					}
				}
				// Draw the min distance (distance field)
				col.r += minDist * minDist; // squared it to to make edges look sharper
				col *= i.col;
				
				return col;
			}
		ENDCG
		}
	}
}

