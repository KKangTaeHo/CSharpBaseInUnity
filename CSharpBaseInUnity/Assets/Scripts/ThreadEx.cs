using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;
public class ThreadEx : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Ex04_ThreadJoinInUnity();
        Ex05_Coroutine();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log($"num 결과 : {num}");
        }
    }

    private Thread _thread;
    private void ThreadEx_01()
    {
        _thread = new Thread(new ThreadStart(Ex01_InnerJoin));
        _thread.Start();

        Debug.Log("ThreadEx_01 끝");
    }

    private void Ex01_InnerJoin()
    {
        int num = 0;

        for (int i = 0; i < 1000000; i++)
            num++;

        _thread.Join();

        Debug.Log("Ex01_InnerJoin 종료");
    }

    private void ThreadEx_02()
    {
        // 루프가 짧을 경우에는 올바른값이 나오는데, 루프가 점점 길어질수록 예측할 수 없는 값이 나옴
        // 그 이유는, CPU(멀티이상의 코어)가 하나의 스레드를 짧은 시간 동안 실행한 후 그 스레드를 멈추고 다음 스레드를 선택해서 실행하는 과정을 반복하기 때문
        // 결과적으로 CPU는 동시에 작업을 수행하고 있지만, 각 코어마다 끊임없이 개별 스레드를 순간적으로 선택하면서 실행-중단-실행을 반복하고 있는거임
        // 이런 상황을 일반적으로 '공유 리소스에 대한 스레드 동기화(synchronization)가 되지 않았다'라고 함
        // 이 문제를 극복하기 위해서는 '동기화 처리'를 해야함.
        // 바로 그러한 목적으로 BCL에서 제공하는 클래스가 Moniter 이다.

        int num = 0;
        Thread t1 = new Thread(() =>
        {
            for (int i = 0; i < 100000; i++)
                num = num + 1;
        });

        Thread t2 = new Thread(() =>
        {
            for (int i = 0; i < 100000; i++)
                num = num + 1;
        });

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Debug.Log($"num 개수 : {num}");
    }


    public class Ex03_Class
    {
        public int Num { get; set; }
    }
    private void ThreadEx_03()
    {
        Ex03_Class c = new Ex03_Class();

        Thread t1 = new Thread(Ex03_Mointer);
        Thread t2 = new Thread(Ex03_Mointer);

        t1.Start(c);
        t2.Start(c);

        t1.Join();
        t2.Join();

        Debug.Log($"num 개수 : {c.Num}");
    }

    private void Ex03_Mointer(object inObj)
    {
        // 모니터를 통한 스레드 동기화
        // Enter/Exit는 반드시 이런 패턴으로 사용되어야 함. 
        // Enter와 Exit 코드사이에 위치한 모든 코드는 한 순간에 스레드 하나만 진입해서 실행할 수 있음 
        // 그리고 Enter와 Exit메서드의 인자로 전달하는 값은 반드시 참조형 타입의 인스턴스여야 함(일반 자료형 int, string 같은거 넣으면 크래시남)
        Ex03_Class c = inObj as Ex03_Class;

        for (int i = 0; i < 100000; i++)
        {
            Monitor.Enter(c);

            try
            {
                c.Num = c.Num + 1;
            }
            finally
            {
                Monitor.Exit(c);
            }

            //
            lock (c)
            {
                c.Num = c.Num + 1;
            }
        }
    }


    Thread subMainThread;
    private void Ex04_ThreadJoinInUnity()
    {
        // 유니티에서 Thread.Join 사용하기
        // Join 사용시 해당 메서드를 기점으로 wait가 걸림
        // 그래서 Start나 Update 내부에서 스레드를 걸고 Join을 써버리면
        // 해당 유니티의 메인스레드가 wait하는 상황이 발생함.
        // 그래서 올바르게 사용하기 위해서는 따로 부분 스레드를 생성하고 그 내부에서 Join을 사용해야한다;

        subMainThread = new Thread(() =>
        {
            Thread t1 = new Thread(() =>
            {
                Thread.Sleep(2000);
                Debug.Log("t1 호출");
            });

            Thread t2 = new Thread(() =>
            {
                Thread.Sleep(5000);
                Debug.Log("t2 호출");                
            });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Debug.Log("모두 끝!");
        });

        subMainThread.Start();
    }

    // 코루틴은 자원을 공유할 경우 올바르게 값이 나올까?
    // lock가 필요없이 값이 잘나옴
    // 그 이유는 코루틴은 비동기가 아니기 때문에!
    int num = 0;
    private void Ex05_Coroutine()
    {
        StartCoroutine(Co_ResouceShare(1234567891));
        StartCoroutine(Co_ResouceShare(1234));
        Debug.Log("코루틴끝!");

        // 코루틴이 비동기라고 생각하면 이 값은 "1234"가 먼저 노출되야 맞을 것이다
        // 하지만 그렇게 출력되지 않고 순차적으로 출력이 되는 이유는 코루틴은 비동기가 아니기 때문에 순차적으로 나오지 않는 것이다.
        // 구조는 똑같고 다른 메서드를 만들어서 실행하더라도 똑같은 값이 나오는 것을 볼 수 있다.

        StartCoroutine(Co_OneFrameYield(12345));
        StartCoroutine(Co_OneFrameYield(1234));     // 소요시간이 아마 20초 정도 될것이다.
        Debug.Log("먼저나옴");

        // 이제 호출해보면 예상하는 값이 나오는 것을 알 수 있다.
        // 그 이유는 for문안에 'yield return null' 해주므로써 다름 코루틴에게 양도를 했기 때문이다.
    }

    IEnumerator Co_ResouceShare(int inSec)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        for (int i = 0; i < inSec; i++)
            num++;

        stopwatch.Stop();

        UnityEngine.Debug.Log($"{inSec}코루틴 실행! num값 :{num}, 소요시간 : {stopwatch.Elapsed}");

        yield break;
    }

    IEnumerator Co_OneFrameYield(int inSec)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        for (int i = 0; i <inSec; i++)
        {
            num++;
            yield return null;
        }

        stopwatch.Stop();

        UnityEngine.Debug.Log($"{inSec}코루틴 실행! num값 :{num}, 소요시간 : {stopwatch.Elapsed}");
    }
}
