Shader "Part12/Holo"
{
	//---------------------------
	// 홀로그램 만들어보기
	//---------------------------
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Rim ("Rim",Range(0.1,3)) = 1
	}
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert noambient alpha : fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		float _Rim;
		
        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
			float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            // o.Albedo = c.rgb;
			
			// 1. 홀로그램 테두리 강도 조절
			// float rim = saturate(dot(o.Normal, IN.viewDir));
			// rim = pow(1 - rim, 3)*_Rim;
			// o.Emission = float3(1, 0, 1);\
			// o.Alpha = rim;

			// 2. 삼각함수를 이용해서 반짝거리게 하기
			// float rim = saturate(dot(o.Normal, IN.viewDir));
			// rim = pow(1 - rim, 3)*(sin(_Time.y*2)*0.5 + 0.5);
			// o.Emission = float3(1, 0, 1);
			// o.Alpha = rim;

			// 3. 줄무니가 위로 올라가는 효과
			float3 rim = saturate(dot(o.Normal, IN.viewDir));
			rim = pow(1 - rim, 2);
			o.Emission = float3(0, 1, 0);
			o.Alpha = rim+pow(frac(IN.worldPos.g * 3 - _Time.y), 30);;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
