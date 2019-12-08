using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefAndOutEx : MonoBehaviour
{
    private void Start()
    {
        RefEx();
    }

    // 1. 깊은 복사와 얕은 복사
    public void DeepAndShallow()
    {
        // 값 형식과 참조 형식의 결정적인 차이점은 인스턴스의 대입이 일어날 때 뚜렷해진다.
        // 아래에 sturct와 class 를 사용한다고 치면
        Vec v1;
        v1.x = 5;
        v1.y = 6;

        Vec v2 = v1;    // 값 형식 대입.
                        // 메모리 자체가 복사됨(깊은 복사)

        Pt p1 = new Pt();
        p1.x = 6;
        p1.y = 7;

        Pt p2 = p1;     // 참조 형식 대입.
                        // new가 생성된 힙 메모리를 가리킴(얕은 복사)

        // 자세히 보면 둘 간의 동작 방식에는 공통점이 하나 있는데, 다름 아닌 '변수의 스텍값은 여전히 복사 된다는 것이다.!
        // (vec는 값을 복사하고, pt는 값이 있는 주소를 복사하고)
        // 이렇게 '변수의 스택값'이 복사되는 상황을 특별히 '메서드의 인자 전달과 관련해 [값에 의한 호출]' 이라고 함.
    }

    // 2. ref 예약어
    public void RefEx()
    {
        Vec v1;
        v1.x = 1;
        v1.y = 23;

        Debug.Log($"x값 : {v1.x}, y값 : {v1.y}");
        Change(v1);
        Debug.Log($"변경 후 x값 : {v1.x}, y값 : {v1.y}");         // 이렇게 하면 값이 변경되지 않은 것을 알 수 있음.

        Debug.Log("ref Change 호출");
        RefChange(ref v1);
        Debug.Log($"변경 후 x값 : {v1.x}, y값 : {v1.y}");

        // ref 예약어를 통해 '얕은복사' 효과를 냄. 
        
        // 하지만 얕은 복사와 ref 예약어는 분명하게 동작 방식에 차이가 있다.
        // 기존의 얕은 복사와 깊은 복사는 변수의 스택 값이 복사되어 전달 됬지만, ref를 사용할 경우
        // 해당 매개변수 자체(여기서는 ref Vec inVec)가 주소값을 바로 전달되어 같은 메모리의 주소를 바라본다.

        
    }
    
    public struct Vec
    {
        public int x;
        public int y;
    }

    public class Pt
    {
        public int x;
        public int y;
    }

    public void Change(Vec inVec)
    {
        int swap = inVec.x;
        inVec.x = inVec.y;
        inVec.y = swap;
    }

    public void RefChange(ref Vec inVec)
    {
        int swap = inVec.x;
        inVec.x = inVec.y;
        inVec.y = swap;
    }
}
