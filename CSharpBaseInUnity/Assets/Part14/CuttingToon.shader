Shader "Part14/CuttingToon"
{
	//---------------------
	// 끊어지는 음영 만들기
	//---------------------
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("BumpMap", 2D)= "bump"{}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		cull front

		CGPROGRAM
		#pragma surface surf Nolight vertex:vert noshadow noambient


		struct Input
		{
			float4 color :COLOR;
		};
		
		void surf(Input IN, inout SurfaceOutput o)
		{
			
		}

		void vert(inout appdata_full v)
		{
			v.vertex.xyz = v.vertex.xyz + v.normal.xyz * 0.03;
		}

		float4 LightingNolight(SurfaceOutput s, float3 lightDir, float atten)
		{
			return float4(1,0,0,1);
		}

		ENDCG


		cull back

        CGPROGRAM
        #pragma surface surf Toon noambient

        sampler2D _MainTex;
		sampler2D _BumpMap;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
		};

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Alpha = c.a;
        }

		float4 LightingToon(SurfaceOutput s, float3 lightDir, float atten)
		{
			float4 result;
			float ndotl = dot(s.Normal, lightDir) *0.5 + 0.5;
			ndotl *= 5;
			ndotl = ceil(ndotl) / 5;
			
			result.rgb = s.Albedo * ndotl;
			result.a = s.Alpha;
			return result;
		}

        ENDCG
    }
    FallBack "Diffuse"
}
