Shader "Part11/CustomLight"
{
    Properties
    {
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Atten("Atten", Range(0.1,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Test

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		float _Atten;

        struct Input
        {
            float2 uv_MainTex;
        };

		float4 LightingTest(SurfaceOutput s, float3 lightDir, float atten)
		{
			// 1. Lambert 연산
			// float ndot1 = saturate(dot(s.Normal, lightDir));
			// return ndot1 + 0.5;
		
			// 2. Haft-Lambert 연산 (더 자연스러운 라이트)(실제 사용할 땐 제곱근을 이용하기도 함)
			// float ndot1 = saturate(dot(s.Normal, lightDir))*0.5 + 0.5;
			
			// 3. 조명, 색상, 감쇄를 연산
			// - 유니티 Legacy Shader, Bumped Diffuse와도 완전히 같은 쉐이더
			// _LightColor0.rgb : 조명의 색상이나 강도를 표현할 때 사용.(내장변수)
			float ndot1 = saturate(dot(s.Normal, lightDir));
			float4 final = (0, 0, 0, 0);
			final.rgb = ndot1 * s.Albedo *_LightColor0.rgb*(atten*_Atten);
			final.a = s.Alpha;
			return final;
		}

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
