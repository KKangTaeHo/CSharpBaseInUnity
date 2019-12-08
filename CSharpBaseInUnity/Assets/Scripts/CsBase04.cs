using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsBase04 : MonoBehaviour
{
    private void Start()
    {
        DynamicEx();   
    }


    // C#4.0 으로 만든 응용 프로그램은 닷넷 프레임워크 4.0에서 실행됨
    // 닷넷 4.0은 새롭게 CLR4.0에서 운영된다. (이전의 닷넷은  CLR2.0을 기반으로 어셈블리가 추가되는 형태)

    // 프로그래밍 언어를 또다른 방식으로 구별하자면 컴파일시 Type Checking을 진행하는 Static Language와
    // 런타임시 Type을 판별하는 Dynamic Language로 구분 할 수 있다. 예를 들어, C#은 Static Language에 속하며,
    // Python, Ruby JavaScript 등은 Dynamic Language이다.

    // 하지만, C# 4.0 에서는 Dynamic Language의 요소를 추가 하므로써 .NET 4.0의 DLR을 사용하면서 다른 Dynamic Language를
    // 함께 사용하는 것이 가능해 졌다.

    // 1. Dynamic 예약어
    private void DynamicEx()
    {
        // var 예약어와 차이점은 var 예악어는 C# 컴파일러가 빌드 시점에 초깃값과 대응되는 타입으로 치환되는 반면
        // dynamic은 그렇게 하지 않는다. 즉, dynamic 변수는 컴파일 시점에 타입을 결정하지 않고, 해당 프로그램이 
        // 실행되는 시점에 타입을 결정한다.

        // var a = 5;
        // a = "test";     // 컴파일 오류 : System.int32로 결정되서 문자열을 받을 수 없음
        // Debug.Log(a);

        dynamic d = 5;
        d = "test";     // d는 형식이 결정되지 않았기 때문에 다시 문자열로 초기화 가능
        Debug.Log(d);   // dynamic을 사용할 경우, Missing compiler required member 'Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create' 이라는 에러가 뜰 때가 있다.
                        // 이럴 경우엔 Project Setting > Payer > Api Compatibility 를 .Net 4.0 이상으로 만들어 줘야한다.
    }
}
