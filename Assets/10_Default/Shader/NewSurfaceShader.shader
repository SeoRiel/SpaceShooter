Shader "LimePapa/BasicShader"
{
	Properties									//
	{
		_Color("Color", Color) = (1, 1, 1, 1)   //
	}

	SubShader									// 
	{
		Pass									// 물체를 그릴 때, GPU에 넘겨줄 작업물들
		{
			Material							// 물체의 재질 설정
			{
				Diffuse[_Color]					// Diffuse 속성의 컬러 값 부여
			}
		Lighting On								// 라이트 활성화
		}
	}

	Fallback "Diffuse"							// SubShader 안의 내용이 그래픽 하드웨어에서 지원하지 않을 경우
												// 유니티가 해당 셰이더를 검색
}