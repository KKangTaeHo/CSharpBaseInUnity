using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEx : MonoBehaviour
{
    // 이벤트
    // 다음 조건을 만족하는 정형화된 콜백 패턴을 구현하려고 할 때 event 예약어를 사용하면 코드를 줄일 수 있다.
    // 1. 클래스에서 이벤트(콜백)를 제공한다.
    // 2. 외부에서 자유롭게 해당 이벤트(콜백)를 구독하거나 해지하는 것이 가능하다.
    // 3. 외부에서 구독/해지는 가능하지만 이벤트 발생은 오직 내부에서 가능하다.
    // 4. 이벤트(콜백)의 첫 번째 인자로는 이벤트를 발생시킨 타입의 인스턴스이다.
    // 5. 이벤트(콜백)의 두 번째 인자로는 해당 이벤트에 속한 의미 있는 값이 제공된다.

    // 이벤트는 GUI를 제공하는 응용 프로그램에서 매우 일반적으로 사용된다.

    // Start is called before the first frame update
    void Start()
    {

    }

    // 소수생성기를 구현한다고 했을 경우
    private void EventEx_01()
    {
        Ex01_PrimeGenerator gen = new Ex01_PrimeGenerator();

        // 콜백 메서드 추가

        Ex01_PrimeGenerator.PrimeDelegate callPrint = Ex01_PrintPrime;
        gen.AddDelegate(callPrint);

        Ex01_PrimeGenerator.PrimeDelegate callSum = Ex01_SumPrime;
        gen.AddDelegate(callSum);
    }

    private void EventEx_02()
    {
        Ex02_PrimeGenerator gen = new Ex02_PrimeGenerator();

        // event를 활용하면 위의 식을 이렇게 간단하게 표현 할 수 있다.
        gen.primeGenerated += Ex02_PrintPrime;
        gen.primeGenerated -= Ex02_SumPrime;

        // gen.primeGenerated(); 이벤트 호출은 오직 내부에서만 가능하다.
    }


    private int Ex01_Sum;

    /// <summary>
    /// 콜백으로 등록 될 메서드 1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="arg"></param>
    private void Ex01_SumPrime(object sender, Ex01_CallbackArg arg)
    {
        Ex01_Sum += (arg as Ex01_PrimeCallbackArg).prime;
    }

    /// <summary>
    /// 콜백으로 등록 될 메서드 2
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="arg"></param>
    private void Ex01_PrintPrime(object sender, Ex01_CallbackArg arg)
    {
        Debug.Log($"{(arg as Ex01_PrimeCallbackArg).prime}, ");
    }


    // 위의 식을 다음과 같이 간결하게 변경 할 수 있음
    private int Ex02_Sum;
    private void Ex02_SumPrime(object sender, EventArgs arg)
    {
        Ex02_Sum += (arg as Ex02_PrimeCallbackArg).prime;
    }

    private void Ex02_PrintPrime(object sender, EventArgs arg)
    {
        Debug.Log($"{(arg as Ex02_PrimeCallbackArg).prime}, ");
    }
}

class Ex01_CallbackArg { }  // 콜백의 값을 담는 클래스의 최상위 부모 클래스 역활

class Ex01_PrimeCallbackArg : Ex01_CallbackArg  // 콜백 값을 담는 클래스 정의
{
    public int prime;

    public Ex01_PrimeCallbackArg(int inPrime)
    {
        this.prime = inPrime;
    }
}


// 위의 식을 이렇게 간결하게 만들 수 있다.
// EventArg로 바로 상속 받을 수 있음
class Ex02_PrimeCallbackArg : System.EventArgs
{
    public int prime;

    public Ex02_PrimeCallbackArg(int inPrime)
    {
        this.prime = inPrime;
    }
}


// 소수 생성기
class Ex01_PrimeGenerator
{
    // 콜백을 위한 델리게이트 타입 정의
    public delegate void PrimeDelegate(object sender, Ex01_CallbackArg arg);

    // 콜백 메서드를 보관하는 델리게이트 인스턴스 필드
    PrimeDelegate callbacks;

    /// <summary>
    /// 콜백 메서드 추가
    /// </summary>
    /// <param name="inCallback"></param>
    public void AddDelegate(PrimeDelegate inCallback)
    {
        callbacks = Delegate.Combine(callbacks, inCallback) as PrimeDelegate;
    }

    /// <summary>
    /// 콜백 메서드 삭제
    /// </summary>
    /// <param name="inCallback"></param>
    public void RemoveDelegate(PrimeDelegate inCallback)
    {
        callbacks = Delegate.Remove(callbacks, inCallback) as PrimeDelegate;
    }

    public void Run(int inLimit)
    {
        for (int i = 2; i <= inLimit; i++)
        {
            if (IsPrime(i) && callbacks != null)
            {
                // 콜백을 발생시킨 측의 인스턴스와 발견된 소수를 콜백 메서드에 전달
                // 등록된 n개의 메서드가 실행됨
                callbacks(this, new Ex01_PrimeCallbackArg(i));
            }
        }
    }

    private bool IsPrime(int inCandidate)
    {
        if ((inCandidate & 1) == 0)
        {
            return inCandidate == 2;
        }

        for (int i = 3; i * i <= inCandidate; i += 2)
        {
            if ((inCandidate % i) == 0)
                return false;
        }

        return inCandidate != 1;
    }
}


class Ex02_PrimeGenerator
{
    // 위의 식을 event를 통해 다음과 같이 정의 할 수 있다.
    public event EventHandler primeGenerated;
    public void Run(int inLimit)
    {
        for (int i = 2; i <= inLimit; i++)
        {
            if (IsPrime(i) && primeGenerated != null)
            {
                // 콜백을 발생시킨 측의 인스턴스와 발견된 소수를 콜백 메서드에 전달
                // 등록된 n개의 메서드가 실행됨
                primeGenerated(this, new Ex02_PrimeCallbackArg(i));
            }
        }
    }

    private bool IsPrime(int inCandidate)
    {
        if ((inCandidate & 1) == 0)
        {
            return inCandidate == 2;
        }

        for (int i = 3; i * i <= inCandidate; i += 2)
        {
            if ((inCandidate % i) == 0)
                return false;
        }

        return inCandidate != 1;
    }
}
