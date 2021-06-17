using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestEx : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co_Request03());
    }

    IEnumerator Co_Request01()
    {
        // 이렇게 나갈 경우, request가 완료 되기도 전에 GetContent가 호출되어
        // 'Can get content from an unfinished UntiyWebRequest Object' 가 호출된다.
        // - 반드시 isDone이 됬는지 확인해야한ㄷ.
        var request = UnityWebRequestTexture.GetTexture("http://www.my-server.com/myimage.png");
        request.SendWebRequest();

        Texture2D tex = null;
        tex = DownloadHandlerTexture.GetContent(request);

        yield return null;
    }

    IEnumerator Co_Request02()
    {
        // 올바른 리퀘스트 요청
        var request = UnityWebRequestTexture.GetTexture("http://www.my-server.com/myimage.png");
        yield return request.SendWebRequest();

        while (!request.isDone)
            yield return null;

        Texture2D tex = null;
        tex = DownloadHandlerTexture.GetContent(request);
    }

    IEnumerator Co_Request03()
    {
        // 잘못된 url이 들어갈 경우
        // 'Cannot resolve destination host' 에러가 난다
        var request = UnityWebRequestTexture.GetTexture("http://www.youtube");
        yield return request.SendWebRequest();

        while (!request.isDone)
            yield return null;

        Texture2D tex = null;
        tex = DownloadHandlerTexture.GetContent(request);
    }
}
