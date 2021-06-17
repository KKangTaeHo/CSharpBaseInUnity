Shader "Part14/DiffuseWraping"
{
	//-----------------------
	// Diffuse Wraping 렌더
	//-----------------------
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RampTex ("RampTex", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Toon noambient

        sampler2D _MainTex;
		sampler2D _RampTex;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_RampTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

		float4 LightingToon(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
		{
			float ndotl = dot(s.Normal, lightDir)*0.5 + 0.5;

			float3 H = normalize(lightDir + viewDir);
			float spec = saturate(dot(s.Normal, H));

			float4 ramp = tex2D(_RampTex, float2(ndotl, spec));
			
			float4 result;
			result.rgb = s.Albedo * ramp.rgb +(ramp.rgb*0.1);
			result.a = s.Alpha;
			return result;
		}
        ENDCG
    }
    FallBack "Diffuse"
}
