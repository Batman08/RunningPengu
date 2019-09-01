Shader "Bilal/BendWorld"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Curvature("Curvature", Float) = 0.001
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			CGPROGRAM
				uniform sampler2D _MainTex;
				uniform float _Curvature

				struct Input {
					float2 uv_MainTex;
				};

				void vert(inout appdata_full v)
				{
					float4 worldSpace = mul(unity_ObjectToWorld, v.vertex)
					worldSpace.xyz = _WorldSpaceCameraPos.xyz;
					worldSpace = float4(0f, (worldSpace.z * worldSpace.z) * -_Curvature, 0f, 0f);

					v.vertex += mul(unity_WorldToObject, worldSpace);
				}

				void surf(Input IN, inout SurfaceOutput o)
				{
					//prt 14 9:28 / 20:15
				}
			EndCG
		}
}
