Shader "Part12/RimLight"
{
	//------------------------
	// Rim Light 만들기
	//------------------------
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_TexColor ("TexColor", Color) = (1,1,1,1)
		_RimColor ("RimColor", Color) = (1,1,1,1)
		_RimPower("RimPower", Range(1,10)) = 3
		_BumpMap("NormalMap", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _BumpMap;
		float4 _TexColor;
		float4 _RimColor;
		float _RimPower;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb*_TexColor.rgb;
			// 림 라이트 이전에 노멀을 해주는것이 포인트
			// o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			float rim = saturate(dot(o.Normal, IN.viewDir));
			o.Emission = pow(1 - rim, _RimPower)*_RimColor.rgb;
			o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
