using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsBase07 : MonoBehaviour
{
    private void Start()
    {
        // ConfortableOut();
        TupleEx();
    }

    // C# 7.0 부터 다시 주요한 변화를 이끌어내기 위해 함수형 언어에서나 제공하던 '패턴 매칭' 구문을 가능하게 했다는 점이다.
    // C# 7.0에 대응하는 닷넷 프레임워크 버전은 4.7이고 주요 개발 환경은 비주얼 스튜디오 2017이다. 하지만, C# 7.0 컴파일러가 출시되는 시점의
    // 닷넷 프레임워크 버전은 4.6.2였고, 이로 인해 C# 7.0의 새로운 두 가지 기능을 사용하는데 문제가 발생한다.

    // 첫 번째로는 '튜플'인데 4.6.2에는 제공하지 않지만, 4.7부터는 제공을 하기 때문에 없다면 System.ValueType을 직접 설치해주어야한다.
    // 두 번째는 async 메서드의 성능 향상을 위해 추가된 System.Threading.Tasks.ValueTask 타입인데, 닷넷 프레임워크 4.7에는 여전히 포함하고 있지 않아
    // 이 타입이 들어간 async 구문을 테스트하려면 반드시 NuGet 패키지 관리자로부터 ValueTask를 구현한 System.Threading.Tasks.Extension.dll을 다운로드해
    // 참조 추가 해야한다.

    // 1. 편리하게 out 사용
    public void ConfortableOut()
    {
        // out 매개변수가 정의된 대표적인 메서드가 바로 TryParse 인데.
        // 6.0 이전에는 out 매개변수가 정의된 메서드를 사용하려면 인스턴스를 미리 선언 했어야한다.
        int before;
        if(int.TryParse("5",out before))
        {
            Debug.Log(before);
        }

        if(int.TryParse("3",out int after)) // 이런식으로 사용 가능
        {
            Debug.Log(after);
        }

        if(int.TryParse("4",out var afterVar))  // 타입 추론을 컴파일러에게 맡기는 var 예약어도 사용 할 수 있음
        {                                       // out int afterVar로 처리됨

        }
    }

    // 2. 반환값 및 로컬 변수에 ref기능 추가
    public void RefEx()
    {
        // C# 6.0 이전까지는 ref 예약어를 오직 메서드의 매개변수로만 사용 할 수 있었지만,
        // 이제는 로컬 변수와 반환 값에 대해서도 참조 관계를 맻을 수 있게 개선됬다.
              
        int a = 5;
        ref int b = ref a;  // 이렇게 하면 a와 b가 같은 주소를 공유하고 있다.

        // 예를 들어 배열의 특정 요소의 값을 바꾸려면 배열 인스턴스 자체를 넘겨주거나 원하는 배열요소만을 바꿀 수 있는
        // 메소드를 만들었다고 가정하면

        IntList intList = new IntList();
        int[] list = intList.GetList();
        list[0] = 5;

        intList.Print();            // 리스트 값이 5,2로 변경된 것을 알 수 있음

        // 참조 return을 사용하면 원하는 요소의 참조만 반환 하는 것이 가능하다.
        ref int item = ref intList.GetFirstItem();
        item = 15;

        intList.Print();            // 15로 출력됨

        // 이런식으로도 표현 할 수 있음
        int val = intList.GetFirstItem() = 1;

        // 반환 및 로컬 변수에 사용 할 수 있는 ref 예약어는 두 가지 제약이 있는데, 모두 컴파일 오류가 나므로 걱정 ㄴㄴ
        // 1. 지역변수를 return ref 할 경우
        // public ref int Ref()
        // {
        //     int num = 2;
        //     return ref num;
        // }                    이런식으로 하면 안됨
        // 2. ref 예약어를 지정한 지역변수가 다시 다른 변수를 가리킬 경우
    }

    
    // 3. 튜플
    // 튜플이란 유한 개의 원소로 이뤄진 정렬된 리스트를 의미하는 자료구조임
    public void TupleEx()
    {
        IntResult result = ParseInt("15");
        Debug.Log($"result.num : {result.num}, result.parse : {result.parse}");

        // 위의 식을 살펴보면 2개 이상의 반환 값이 필요할 때마다 저런식으로 계속 클래스를 만들어야해서 굉장히 비효율 적이다.
        // 그래서 매번 정의되는 클래스를 없애기 위해 C# 3.0의 익명 타입과 C#4.0의 dynamic 예약어를 이용 할 수 있다.
        dynamic resultInDynamic = ParseIntInDynamic("24");
        Debug.Log($"result.num : {resultInDynamic.Number}, result.parse : {resultInDynamic.Parsed}");

        // 하지만 dynamic의 도입으로 정적 형식 검사를 할 수 없어 나중에 필드 이름을 바뀌어도
        // 컴파일 시 문제를 알아 낼 수 없다는 문제점이 발생한다.
        // 또 다른 방법으로는 닷넷 프레임워크 4.0의 BCL부터 제공되는 System.Tuple 제네릭 타입을 이용하는 것이다.
        Tuple<bool, int> resultInTuple = ParseIntInTuple("40");
        Debug.Log($"result.num : {resultInTuple.Item2}, result.parse : {resultInTuple.Item1}");

        // 위의식도 살짝 아쉬움? 이 남음..
        // 그래서 새로운 문법 형태가 나옴
        // 괄호를 이용해 다중 값을 처리할 수 있는 구문을 C# 7.0 수준에서 지원하도록 추가 한 것이다.
        (bool, int) resultNewTuple = ParseIntInNewTuple("59");
        Debug.Log($"result.num : {resultNewTuple.Item2}, result.parse : {resultNewTuple.Item1}");

        // 요런식으로도 표기가 가능하다.
        var resultNewTupleVersion2 = ParseIntInNewTupleVersion2("3");
        Debug.Log($"result.num : {resultNewTupleVersion2.num}, result.parse : {resultNewTupleVersion2.parsed}");

        // 모든 튜플 구문은 C# 7.0 컴파일러가 소스코드를 컴파일 하는 시점에 System.ValueTuple 제네릭 타입으로 변경해서 처리한다.
        // 참고로 닷넷 프레임워크 4에서 제공하는 System.Tuple은 클래스로 정의되어 있지만, 4.7에서 제공하는 System.ValueTuple은 struct로 되어있음
    }


    // TupleEx01. 일반 형태의 TryParse
    public class IntResult
    {
        public bool parse { get; set; }
        public int num { get; set; }
    }

    IntResult ParseInt(string inText)
    {
        IntResult result = new IntResult();

        try
        {
            result.num = Int32.Parse(inText);
            result.parse = true;
        }
        catch
        {
            result.parse = false;
        }

        return result;
    }

    // TupleEx02. dynamic 을 통한 TryParse
    dynamic ParseIntInDynamic(string inText)
    {
        int num = 0;
        try
        {
            num = Int32.Parse(inText);
            return new { Number = num, Parsed = true };
        }
        catch
        {
            return new { Number = num, Parsed = false };
        }

    }

    // TupleEx03.  System.Tuple 활용
    Tuple<bool, int> ParseIntInTuple(string inText)
    {
        int num = 0;
        bool result = false;

        try
        {
            num = Int32.Parse(inText);
            result = true;
        }
        catch
        {
        }

        return Tuple.Create(result, num);
    }

    // TupleEx04. 새로운 Tuple
    (bool, int) ParseIntInNewTuple(string inText)
    {
        int num = 0;
        bool result = false;

        try
        {
            num = Int32.Parse(inText);
            result = true;
        }
        catch
        {
        }

        return (result, num);
    }

    // 나중에 값을 받을 때 parsed, num으로 받을 수 있다.
    (bool parsed, int num) ParseIntInNewTupleVersion2(string inText)
    {
        int num = 0;
        bool result = false;

        try
        {
            num = Int32.Parse(inText);
            result = true;
        }
        catch
        {
        }

        return (result, num);
    }

    class IntList
    {
        int[] list = new int[2] { 1, 2 };
        public int[] GetList() => list;
        public ref int GetFirstItem() => ref list[0];  

        internal void Print()
        {
            Array.ForEach(list, e => Debug.Log(e));
        }
    }
}
