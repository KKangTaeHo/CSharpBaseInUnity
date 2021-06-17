Shader "MyShader/RimNormal"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_TexColor("TexColor",Color)=(1,1,1,1)
		_RimColor ("RimColor", Color) = (1,1,1,1)
		_RimPower ("RimPower",Range(1,20)) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf CustomRim

        sampler2D _MainTex;
		float4 _RimColor;
		float4 _TexColor;
		float _RimPower;

        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
		};

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			float rim = saturate(dot(o.Normal, IN.viewDir));
			
			o.Albedo = c.rgb*_TexColor.rgb;
			o.Emission = pow(1 - rim, _RimPower) * _RimColor.rgb;
			o.Alpha = c.a;

			/*float rim = saturate(dot(o.Normal, IN.viewDir));
			o.Emission = pow(1 - rim, _RimPower)*_RimColor.rgb;
			o.Alpha = c.a;*/
        }

		float4 LightingCustomRim(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
		{
			float ndotl = saturate(dot(s.Normal, lightDir));

			float4 result;
			result.rgb = ndotl * s.Albedo * _LightColor0.rgb * atten;
			result.a = s.Alpha;
			
			return result;
		}
		
        ENDCG
    }
    FallBack "Diffuse"
}
