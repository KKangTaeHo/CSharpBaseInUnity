using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsBase06 : MonoBehaviour
{

    private void Start()
    {
        Nameof_Operator(3, "안녕");
    }

    // C# 6.0 문법을 컴파일 하기 위해서는 c# 6.0이 필요하지만 일단 빌드된 결과물(EXE/DLL)을 실행 하기 위한 환경으로는
    // 굳이 닫넷 프레임 워크 4.6이 필요하지 않다. (그냥 알고 있으면 좋을 듯)

    // 1 . 자동 구현 속성 초기화
    // 생성자 없이 요런식으로 처리가 가능
    public class InitalizerForAuto_Properties
    {
        public string Name { get; set; }
        public string Adress { get; set; } = "서울";

        public InitalizerForAuto_Properties()
        {
            this.Name = "빌리";
        }
    }

    // 2. 람다식을 이용한 메서드, 속성 및 인덱서 정의
    public class LambdaExpression
    {
        int beforeNum1;
        int beforeNum2;

        public LambdaExpression(int num1, int num2)
        {
            this.beforeNum1 = num1;
            this.beforeNum2 = num2;
        }

        public int Sum(int num1, int num2)
        {
            return num1 + num2;
        }

        public void DebugLog()
        {
            Debug.Log(this);
        }

        // 기본적인 클래스 메서드를 다음과 같이 람다 형태로 변경 할 수 있음
        public int Substract(int num1, int num2) => num1 - num2;
        public void DebugLogError() => Debug.LogError(this);

        int sum => beforeNum1 + beforeNum2; // 이런식으로도 표현이 가능한데, get만 자동 정의되고, set기능은 제공되지 않는다.
    }

    // 3. null 조건 연산자
    public void Null_Coalescing_Operator()
    {
        List<int> list = null;

        Debug.Log(list?.Count); // list == null 이면 null 반환
                                // list != null 이면 list.count 반환

        Debug.Log(list != null ? new int?(list.Count) : null);  // 위의 코드는 다음과 같음

        // 예전에 for문을 돌렸을 때는 다음과 같이 처리를 했지만,
        if (list !=null)
        {
            for(int i=0; i<list.Count; i++)
            {
                // 내용
            }
        }

        // 지금은 이렇게 처리 할 수 있다.
        for(int? i =0; i<list?.Count;i++)
        {
            // 내용
        }

        // null 조건 연산자의 결과값이 null을 포함 할 수 있기 때문에 이를 저장 하기 위해서는 반드시 null 값을
        // 처리 할 수 있는 타입을 사용해야한다는 것이다.

        int? count = list?.Count;   // 이런식으로 사용되거나
        int cnt = list?.Count ?? 0; // 요런식으로 써야 한다.

        string[] lines = { "you", "and", "i" };
        string you = lines?[0];     // 요렇게도 표현 할 수 있음.
    }

    // 4. nameof 연산자
    public void Nameof_Operator(int inNum, string inStr)
    {
        Debug.Log($"inNum = {inNum}");
        Debug.Log($"{nameof(inNum)} = {inNum}");    // Nameof를 사용하면 해당 식별자의 이름을 바로 가지고 올 수 있다.
        Debug.Log(nameof(CsBase02));                // 클래스도 가지고 올 수 있음
    }
}
