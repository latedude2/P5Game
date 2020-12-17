Shader "Unlit/MovingLava"{
	Properties
	{
		_MainColor("Main Color", Color) = (1,1,1,0.3) //R,G,B,A - white
		_SecondaryColor("Secondary Color", Color) = (1,1,1,0.3) //R,G,B,A - white
		_CellAmount("Cell Amount", Range(2, 50)) = 0
		_Shininess("Shininess", Float) = 10
		_Speed("Cell Speed", Range(0,0.6)) = 0.3
		_Ripples("Ripple Thickness", Int) = 2
		_WaveAmplitude("Wave Amplitude", Range(0.01, 1)) = 0.1
		_WaveSpeed("Wave Speed", Range(1, 20)) = 1
	}
		SubShader
	{
		Tags { "Queue" = "Transparent"}

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
			float _WaveAmplitude;
			float _WaveSpeed;

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
				o.vertex.y += sin(0.5 * (v.vertex.x + _WaveSpeed * _Time.y)) * _WaveAmplitude;
				o.vertex.y += cos(0.5 * (v.vertex.z + _WaveSpeed * _Time.y)) * _WaveAmplitude;

				return o;
			}

			float2 random2d(float2 rand) //gives output of a random vector for each pixel
			{
				return frac(float2(dot(rand, float2(169.1562, 321.742)), dot(rand, float2(252.758, 179.323))))*48.5453;
			}

			fixed4 frag(v2f i) : SV_Target
			{

				//get world space light and view direction position
				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

				float3 worldNormal = normalize(i.worldNormal);
				float3 reflectionVector = reflect(-lightDir, worldNormal);

				float RdotV = max(0, dot(reflectionVector, viewDir));

				fixed4 specColor = pow(RdotV, _Shininess) * _LightColor0;

				//Voronoi pattern
				float2 grid = i.uv;
				grid *= _CellAmount; //scale grid cells

				float2 fuv = frac(grid) - 0.5; //generate grid cells
				float2 iuv = floor(grid); //assign unique IDs to grid cells

				float minDistToCell = 1;
				int moveOffset = 5;

				for (int z = -1; z <= 1; z++) {
					for (int x = -1; x <= 1; x++) {

						//Position of neighbouring(offset) point on the grid
						float2 neighbour = float2(x, z);

						//Generate random point in the other cell than current point
						float2 randomPoint = random2d(iuv + neighbour);

						//Move point, get position in relation to current cell by adding offset (neighbour)
						float2 pointPos = _Speed * sin(_Time.z + moveOffset * randomPoint) + neighbour;

						//Calculate distance from point to grid cell
						float dist = length(fuv - pointPos);

						//Find closest distance
						minDistToCell = min(minDistToCell, dist);
					}
				}

				fixed4 col = _SecondaryColor;

				col.rgb += pow(minDistToCell, _Ripples) + specColor;
				col.rgb *= _MainColor;

				return col;
			}
		ENDCG
		}
	}
}

