Shader "Part14/Toon"
{
	/*
		* 빌트인 셰이더
		- 내장된 셰이더
		- 크게 '스텐다드 셰이더', '레거시 셰이더' 이렇게 2가지로 나뉨

		* 랜더파이프라인
		- 씬에있는 콘텐츠들을 화면에 가져와 처리하는 과정
		- 렌더 파이프라인에서는 아래에 정의한 3가지 처리를 한다
			1. 컬링
			2. 랜더링
			3. 포스트 프로세싱
		- 유니티에서는 아래의 랜더파이프라인을 제공함.
			1. 빌트인 렌더 파이프라인
				A. 포워드 렌더링 패스
					- 실시간 광원을 랜더링하는데 구림
					- 프로젝트가 실시간 광원을 사용하지 않거나 조명의 정확도가 그리 중요하지 않을 때 사용
				B. 디퍼드 셰이더 랜더링 패스
					- 실시간 광원을 랜더링하는데 괜찮음
					- 하드웨어 GPU가 지원을 해야지 효과가 있음.
			2. 유니버셜 랜더 파이프라인
			3. 고해상도 랜더 파이프라인

		* 랜더링패스
			- 라이트와 셰이딩을 처리하는 경로
	*/


	//----------------------------
	// 2pass와 면뒤집기
	//----------------------------
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		cull front

		CGPROGRAM
		#pragma surface surf Nolight vertex:vert noshadow

		void vert(inout appdata_full v) {
			v.vertex.xyz = v.vertex.xyz + v.normal.xyz*0.05;
		}

		sampler2D _MainTex;

		struct Input
		{
			float4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{

		}

		float4 LightingNolight(SurfaceOutput s, float3 lightDir, float atten)
		{
			return float4(0, 0, 0, 1);
		}
		ENDCG

		cull back
		
		// 2pass
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
