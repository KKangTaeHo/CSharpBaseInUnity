Shader "Part13/BlinnPhong"
{
	/*

		* 스펙큘러 표현방식에서 가장유명한 phong 방식
		- 내가 보는 방향으로 부터 반사된 방향에 조명이 있으면. 그부분의 하이라이트가 가장 밝다.
		

		* 블링퐁 공식
		1. 광원의 반사벡터를 구함
		2. 반사벡터와 시선(카메라)벡터의 내적을 구함
		3. 내적값의 차이에 따라 스펙큘러의 선명함을 구함.
		   (0은 선명, 90은 어두움)
		4. R= 2N*dot(L,N) - L
		   R : 결과
		   L : 조명
		   N : 노멀벡터

		* 유니티에서 블링퐁 공식(라이트한 Version)
		1. 카메라와 빛의 방향을 통해 하프벡터를 구함
		2. 하프벡터와 법선벡터를 내적하여 그 각도에 따라 스펙큘러를 구함


		* 레거시 셰이더 = 빌트 인 셰이더
		* 셰이더 퍼포먼스는 크게 2가지에 의해 좌우된다.
			1. 하나는 셰이더 자체
			2. 다른 하나는 프로젝트나 특정 카메라가 사용하는 렌더링 경로
			3. 렌더링경로에는 '디퍼드 셰이딩', '버텍스 리드' 등이 있다.
				- 렌더링경로는 조명과 셰이딩에 관한 작업을 함.
			4. 셰이더의 성능차이는 사용하는 텍스쳐수와 연산과정에 따라 다름.

	*/

	//----------------------------
	// 림라이트로 스펙큘러 효과 내기
	//----------------------------
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("BumpMan",2D) = "bump"{}

		_SpecCol ("SpecCol", Color)=(1,1,1,1)
		_SpecPower("SpecPower", Range(1,200)) = 100
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Test noambient alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _BumpMap;

		float4 _SpecCol;
		float _SpecPower;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldPos;
        };

		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
			fixed3 worldPos;
		};

		float4 LightingTest(SurfaceOutputCustom s, float3 lightDir, float3 viewDir, float atten)
		{
			// Lambert term
			float4 result;
			float3 lambert;
			float ndot1 = saturate(dot(s.Normal, lightDir));
			lambert = ndot1 * s.Albedo.rgb * atten * _LightColor0.rgb;
			
			// phong term
			float3 speculer;
			float3 H = normalize(lightDir + viewDir);
			float spec = saturate(dot(H, s.Normal));
			speculer = _SpecCol.rgb * pow(spec, _SpecPower);

			// Lim term
			float3 rimColor;
			float rim = saturate(dot(viewDir, s.Normal));
			float invrim = 1 - rim;
			rimColor = pow(invrim, 6) * float3(1, 0, 0);

			float holo = rimColor + pow(frac(5 * s.worldPos.g - _Time.y),10);
			
			result.rgb = lambert + speculer;
			// result.rgb = lambert + rimColor + pow(rim, 100);	// 이 방식으로도 스펙큘러처럼 만들 수 있다.
			result.a = holo;
			return result;
		}

        void surf (Input IN, inout SurfaceOutputCustom o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Albedo = c.rgb;
            o.Alpha = c.a;
			o.worldPos = IN.worldPos;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
