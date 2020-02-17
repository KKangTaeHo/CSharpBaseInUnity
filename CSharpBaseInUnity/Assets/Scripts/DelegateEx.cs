using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateEx : MonoBehaviour
{
    // 델리게이트 == 대리자.
    // '함수도 변수처럼 저장해 놓고 사용 할 수 없을까? or 함수들만 묶어서 호출 할 수 없을까?'
    delegate void Callback(int inNum);

    // 콜백 선언
    Callback _callback;

    void Start()
    {
        // 1. 콜백에 한가지만 넣을 수 있는 게 아니라, 여려개를 넣을 수 있다.
        _callback += SetAge;
        _callback += SetWeight;

        _callback(5);   // 실행하면 나이랑 몸무게가 '5'로 표시됨

        // 2. 물론 콜백을 뺄 수도 있다.
        _callback -= SetAge;

        // 3. 덮어 씌울 수 도 있다.
        _callback = SetAge;
    }

    public void SetAge(int inAge) => Debug.Log($"나이는 {inAge} 살임");
    public void SetWeight(int inWeight) => Debug.Log($"너의 몸무게는{inWeight} 임");
}
