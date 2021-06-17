using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionEx : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var popup = new Popup();

        string str = null;

        // popup.Show(null,1);

        popup.Show(()=> 
        {
            // 콜백 내부에서 NullException 호출시
            // - <Start>b__0 () 이런식으로 에러뜬다.
            str.Split('.');
        }, 1);

        Init(popup);
    }

    void Init(Popup p)
    {
        p.callback();
    }
}

public class Popup
{
    public Action callback;

    public void Show(Action inCallback, int age)
    {
       callback = inCallback;
    }
}
