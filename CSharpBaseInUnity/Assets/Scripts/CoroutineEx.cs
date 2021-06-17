using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CoroutineEx : MonoBehaviour
{
    // Q. 난 코루틴을 어떻게 생각하고 있었나?
    // - 단일 쓰레드로 동작하는 유니티에서 비동기적 처리를 하기 위한 수단.

    private void Start()
    {
        StartCoroutine(Co_Init());
        StartCoroutine(Co_RequestConten());
    }

    IEnumerator Co_WatiTime(int inTime)
    {
        int num = 0;
        while(num<inTime)
        {
            Debug.Log($"{num}초");
            yield return new WaitForSeconds(1);
            num++;
        }
    }

    IEnumerator Co_Init()
    {
        yield return Co_Number();
        Debug.Log("첫번째 끝");
        yield return StartCoroutine(Co_Number());
        Debug.Log("두번째 끝");
    }

    // 실행을 해본 결과,, 별반 차이가 없었다..
    // 대체 왜 그런것일까?

    // 그 이유는, 코루틴과 열거형에 대핵서 재대하게 이해하지 않았기 때문에 발생한 이슈였다.

    IEnumerator Co_Number()
    {
        yield return 0;
        yield return 1;
        yield return 1;
    }

    IEnumerator Co_RequestConten()
    {
        var request = UnityWebRequest.Get("");
        // request.SendWebRequest();

        // while(!request.isDone)
        //     yield return null;

        yield return null;

        Texture2D tex = null;
        tex = DownloadHandlerTexture.GetContent(request);
    }
}
