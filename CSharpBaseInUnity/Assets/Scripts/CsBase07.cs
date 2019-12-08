using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsBase07 : MonoBehaviour
{
    private void Start()
    {
        ConfortableOut();
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

        if(int.TryParse("3",out int after))
        {
            Debug.Log(after);
        }
    }
}
