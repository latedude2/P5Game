Shader "Unlit/MovingWater"{
	Properties
	{
		_MainColor("Main Color", Color) = (1,1,1,1) //R,G,B,A - white
		_SecondaryColor("Secondary Color", Color) = (1,1,1,1) //R,G,B,A - white
		_CellAmount("Cell Amount", Range(2, 50)) = 0
		_Shininess("Shininess", Float) = 10
		_Speed("Cell Speed", Range(0,0.9)) = 0.3
		_Ripples("Ripple Thickness", Int) = 2
		_Offset("Movement Offset", Int) = 6
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

			fixed4 _MainColor;
			fixed4 _SecondaryColor;
			float _Speed;
			float _CellAmount;
			float _Shininess;
			int _Ripples;
			int _Offset;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1; //world position,
				float3 worldNormal : TEXCOORD2; //normal vector in world space
				float3 normal : NORMAL;
			};

			v2f vert(appdata v)
			{
				v2f o;

				o.vertex = v.vertex;

				//position of vertices, transforming from object to camera space
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = v.uv;

				//get vertex normal in world space
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));

				//making the waves - only adjusting the Y axis (height)
				o.vertex.y += sin(0.5 * (v.vertex.x + _Time.w)) / 10;
				o.vertex.y += cos(0.5 * (v.vertex.z + _Time.w)) / 9;

				return o;
			}

			float2 random2d(float2 rand) //takes a vector2 to give an output of random vector2 between 0 and 1
			{
				return frac(sin(float2(dot(rand, float2(127.1, 311.7)), dot(rand, float2(269.5, 183.3))))*43758.5453);
			}

			fixed4 frag(v2f i) : SV_Target
			{

				fixed4 col = _SecondaryColor;

			//get world space light and view direction position
			float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
			float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

			float3 worldNormal = normalize(i.worldNormal);

			float3 reflectionVector = reflect(-lightDir, worldNormal);


			//find specular dot product
			float RdotV = max(0, dot(reflectionVector, viewDir));

			fixed4 specColor = _LightColor0 * pow(RdotV, _Shininess);


			//Voronoi pattern
			float2 grid = i.uv;

			grid *= _CellAmount; //Scaling cells

			//get values for the grid
			float2 iuv = floor(grid); //int values, no floating point
			float2 fuv = frac(grid); //only the fractional part

			float minDistToCell = 1;

			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					// Position of neighbour cell on the grid
					float2 neighbour = float2(x, y);

					// Random point in relation to current and neighbour position on grid
					float2 randomPoint = random2d(iuv + neighbour);

					// Move the point with time, get the actual position of the offset cell (adding neighbour)
					float2 cellPosition = 0.5 + _Speed * sin(_Time.z + _Offset * randomPoint) + neighbour;

					//Calculate distance from the point in relation to current cell to grid fractional point
					float2 difference = cellPosition - fuv;
					float dist = length(difference);

					// Keep the closer distance
					minDistToCell = min(minDistToCell, dist);
				}
			}

			col.rgb += pow(minDistToCell, _Ripples) + specColor;
			col.rgb *= _MainColor;
			return col;
		}
	ENDCG
	}
	}
}

