using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefAndOutEx : MonoBehaviour
{
    private void Start()
    {
        RefEx_02();
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
    public void RefEx_01()
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

    public void RefEx_02()
    {
        Pt pt = null;
        ChangePt(pt);
        Debug.Log($"pt.x값 : {pt?.x}");  // null 로 나옴
        RefChangePt(ref pt);
        Debug.Log($"pt.x값 : {pt?.x}");  // 메서드 내부에서 해당 객체를 잘 받아서 값이 잘나옴
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

    public void ChangePt(Pt inPt)
    {
        // 얕은 복사 형태로 전달된 참조 값
        // 매개변수(inPt)에 주소가 생겨서 해당 값을 인자로 들어간 값을 호출하더라도 null 이 뜬다.
        inPt = new Pt();
        inPt.x = 1;
        inPt.y = 2;
    }

    public void RefChangePt(ref Pt inPt)
    {
        // ref를 사용하면 매개변수로 들어오는 인자(여기서는 pt)와 inPt가 같은곳으로 바라보기 때문에
        // 값이 잘 전달 되는 것을 알 수 있다.
        inPt = new Pt();
        inPt.x = 1;
        inPt.y = 2;
    }


    // 3. out 예약어
    public void OutEx()
    {
        // 참조에 의한 호출을 가능케 하는 또 하나의 예약어가 out임. 하지만 out은 ref랑 비교했을 때 몇가지 차이점이 있음
        // 1. out으로 지정된 인자에 넘길 변수는 초기화 하지 않아도 됨. 초기화 돼 있더라도 out 인자를 받는 메서드에서는 그 값을 사용 할 수 없음
        // 2. out으로 지정된 인자를 받는 메서드는 반드시 변수에 값을 너허어서 반환해야 한다

        // 즉, out 예약어는 ref예약어의 기능 가운데 몇가지를 강제로 제한하므로써 개발자가 좀 더 특별한 용도로 사용하게 끔 만든것
        int num;
        if(Devide(1,2, out num))
        {
            Debug.Log(num);
        }
    }

    bool Devide(int n1, int n2, out int result)
    {
        if(n2 == 0)
        {
            result = 0;     // 메서드가 끝나기 전에 반드시 할당을 해주어야 컴파일 애러가 안걸린다.
            return false;
        }
        else
        {
            result = n1 / n2;
            return true;
        }
    }
}
