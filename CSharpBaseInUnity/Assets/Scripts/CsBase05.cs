using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CsBase05 : MonoBehaviour
{

    [SerializeField]
    Image IMG_duelShock;
    // Start is called before the first frame update
    void Start()
    {
        // CallerInformation();
        // AsyncAndAwait();
        // AwaitAsync();
        // Debug.Log("안녕하세여");
        // TaskType();

        ParallelProcessing();
    }

    private void CallerInformation()
    {
        // 1. 호출자정보
        Ex01_Caller();
    }

    private void Ex01_Caller([CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber=0,
        [CallerMemberName] string memberName ="")
    {
        Debug.Log("호출된 메서드 이름 : " + memberName);
        Debug.Log("호출된 라인 번호 : " + lineNumber);
        Debug.Log("호출된 소스코드 파일명 : " + filePath);
    }

    private void AsyncAndAwait()
    {
        // 1. 디스크로부터 파일의 내용을 읽는 동기 방식의 Read 메서드는 명령어가 순차적으로 실행됬다.
        using(FileStream fs = new FileStream(Application.dataPath+ "/BaseResources/duelShock.png", FileMode.Open, FileAccess.Read,FileShare.ReadWrite))
        {
            byte[] buf = new byte[fs.Length];
            fs.Read(buf, 0, buf.Length);

            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(buf);
            IMG_duelShock.sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2(0.5f, 0.5f));
        }

        // 2. 비동기 버전의 BeginRead 메서드를 호출 했을 때는 Read 동작 이후의 코드를 별도로 분리해 Completed 같은 형식의 메서드에 담아 처리해야 하는 불편함이 있었다.
        FileStream fSteam = new FileStream(Application.dataPath + "/BaseResources/test.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        FileState fState = new FileState();
        fState.Buffer = new byte[fSteam.Length];
        fState.fs = fSteam;

        fSteam.BeginRead(fState.Buffer, 0, fState.Buffer.Length, ReadCompleted, fState);

        // 2번의 코드를 잘 보면 1번의 코드에서 ReadCompleted만 추가 된 것을 볼 수 있다.
        // 혹시 이 작업을 컴파일러가 알아서 해줄 수 는 없는가? > async/await 예약어의 탄생
    }

    private async void AwaitAsync()
    {
        // 1번 2번의 문제를 해결하기 위해 탄생
        using (FileStream fs = new FileStream(Application.dataPath + "/BaseResources/duelShock.png", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            byte[] buf = new byte[fs.Length];
            await fs.ReadAsync(buf, 0, buf.Length);

            int num = 0;
            for (int i = 0; i < 1000000; i++)
                num++;

            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(buf);
            IMG_duelShock.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            Debug.Log("이미지 로드");
        }
    }

    private void ReadCompleted(IAsyncResult ar)
    {
        FileState fState = ar.AsyncState as FileState;
        fState.fs.EndRead(ar);

        // 스레드가 Read 메서드를 완료하고 아래의 문자를 실행
        string str = Encoding.UTF8.GetString(fState.Buffer);  // 이렇게 하면 한글 깨진다.(해당 파일이 유니티에 있어서 그런가..?)
        Debug.Log(str);
    }


    private void TaskType()
    {
        // await fs.ReadAsync에서 ReadyAsync의 return형이 Tesk로 알수 있으며,
        // async 메서드의 반환값도 Tesk 임을 알 수있다.
        // Tesk는 await 비동기 처리와는 별도로 원래부터 닷넷 4.0부터 추가된 병렬 처리 라이브러리 이다.

        ThreadPool.QueueUserWorkItem(obj => { Debug.Log("스레드 풀 실행"); IMG_duelShock.sprite = null; });

        // .NET 4.0의 Task 타입을 이용해 별도의 스레드에서 작업을 수행
        Task task1 = new Task(() => { Debug.Log("Task 실행"); });
        task1.Start();
        task1.Wait();   // wait를 사용하면 해당 작업이 끝날 때까지 기다린다.

        // 굳이 객체를 생성 할 필요 없이 Action 델리게이트를 전달하자마자 곧바로 작업을 시작 하게 만들 수도 있음
        Task.Factory.StartNew(() => { Debug.Log("Task.Factory 실행"); });


        // Task<TResult>타입은 코드의 실행이 완료된 후 원한다면 반환값까지 처리할 수 있다.
        Task<int> task2 = new Task<int>(() =>
        {
            int num = 0;
            for(int i=0; i<12345; i++)
            {
                num++;
            }

            return num;
        });

        task2.Start();
        task2.Wait();
        Debug.Log($"num의 값 : {task2.Result}");

        // Task.Factory의 StartNew 메서드는 Task를 반환하는데, Task<TResult>를 반환하는 용도로 StartNew<TResult>도 함께 제공한다.
        Task<int> taskReturn = Task.Factory.StartNew<int>(() => 1);
        taskReturn.Wait();
        Debug.Log("Task.Factory.StartNew 값 : " + taskReturn.Result);
    }


    Thread _parellelMainThread; // 병렬처리 메인쓰레드
    private void ParallelProcessing()
    {
        _parellelMainThread = new Thread(() =>
        {
            // 1. 기본적으로 두개의 스레드를 동기화 시키는 방법은 다음과 같다.
            Thread t1 = new Thread(() =>
            {
                Thread.Sleep(3000);
                Debug.Log("3초 실행");
            });

            Thread t2 = new Thread(() =>
            {
                Thread.Sleep(5000);
                Debug.Log("5초 실행");
            });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Debug.Log("스레드 종료");

            // 2. Task를 활용하면 위의 식을 간결하게 수정 할 수 있다.
            var task1 = Task<int>.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                Debug.Log("Task 3초 끝!");
                return 3;
            });

            var task2 = Task<int>.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                Debug.Log("Task 5초 끝!");
                return 5;
            });

            Task.WaitAll(task1, task2);
            Debug.Log($"병렬 끝 :{task1.Result + task2.Result}");

            // 위의 식은 두개의 값이 return 되지 않을 경우 계속 대기 상태임을 알 수 있다.
            // 즉, 스레드를 실행하고 있는 메서드가 작업이 끝나기 전까지 다음상태로 넘어 갈 수 없다.
            // await를 활용하면 위의 식을 말끔하게 해결 할 수 있다.
            // 3. async/wait를 활용해서 비동기 메서드 만들기
            AsyncParellelTask();
            Debug.Log("바로 실행");
        });

        _parellelMainThread.Start();
    }

    private async void AsyncParellelTask()
    {
        var task3 = Task<int>.Factory.StartNew(() =>
        {
            Thread.Sleep(2000);
            Debug.Log("Task 2초 끝!");
            return 2;
        });

        var task4 = Task<int>.Factory.StartNew(() =>
        {
            Thread.Sleep(3000);
            Debug.Log("Task 3초 끝!");
            return 3;
        });

        await Task.WhenAll(task3, task4);
        Debug.Log($"비동기 Task 종료 : {task3.Result + task4.Result}");
    }

    class FileState
    {
        public byte[] Buffer;
        public FileStream fs;
    }
}
