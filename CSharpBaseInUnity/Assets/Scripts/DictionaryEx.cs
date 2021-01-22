using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DictionaryEx : MonoBehaviour
{
    public class User
    {
        public string name;
        public int age;

        public void DebugInfo(Action inCallback)
        {
            inCallback();
        }

        public void DebugError(Action<bool> inCallback)
        {
            inCallback(true);
        }
    }

    Dictionary<string, User> userDic;
    Action _inCallback;

    void Start()
    {
        // 1. 인스턴스 생성 안하고 호출시 'NullReferenceException' 호출
        // Debug.Log(userDic["kim"]);

        // 2. 딕셔너리 인스턴스 생성하고  데이터 선언 없이 호출 시 'KeyNotFoundException' 호출
        // userDic = new Dictionary<string, User>();
        // Debug.Log(userDic["kim"]);

        // 3. 선언 후, 데이터가 null 인 경우 메서드 호출시 'NullRefeenceException' 호출
        // userDic.Add("kim", new User());
        // Debug.Log(userDic["kim"].name.Split('-'));

        // 4. 콜백 내부에서 null 인 경우, '<Start>b__2_0()' 이런식으로 람다가 만들어진 메서드를 보여줌
        // var user = new User();
        // user.DebugInfo(()=>
        // {
        //     string name = null;
        //     name.Split('.');
        // });

        // 5. 이거 에러 터질 땐, 'DictionaryEx+<>c.<Ex02>b__4_0 ()' 라고 뜸
        // Ex02(true);

        // 6.'DictionaryEx+<>c.<Ex03>b__5_0 (System.Boolean isSuccess)' 라고 뜸
        // Ex03();

        // 7. 콜백 안에서 콜백 호출시 터칠 때 'DictionaryEx+<>c__DisplayClass6_0.<Ex04>b__0 ()' 라고 뜸
        // Ex04();

        // 8. 전역으로 처리된 _callback = null을 코루틴에서 실행 할 경우
        //    'DictionaryEx+<Co_Ex05>d__8.MoveNext ()' 라고 뜬다
        // StartCoroutine(Co_Ex05());

        // 9. 코루틴안에서 호출된 _inCallback 내부에 NullException이 뜰 경우
        _inCallback =()=>{
            string name = null;
            name.Split('.');
        };

        StartCoroutine(Co_Ex05());
    }


    void Ex01()
    {
        // Dictionary(HashMap)와 Hashtable의 차이점
        // 1. 해시테이블
        // - Non-Generic
        // - Key와 Value가 모두 Object를 입력받는다.
        // - 박싱/언박싱 을 사용함.
        // 2. 딕셔너리
        // - Generic
        // - Key와 Value가 모두 타입을 받는다.
        // - 박싱/ 언박싱이 안일어난다.
    }

    void Ex02(bool isSuccess)
    {
        var user = new User();
        user.DebugInfo(delegate(){
            GameObject obj = null;
            Debug.Log(obj.transform);
        });
    }

    void Ex03()
    {
        var user = new User();
        user.DebugError(isSuccess => {
            GameObject obj = null;
            Debug.Log(obj.transform);
        });
    }

    void Ex04()
    {
        Action callback = null;
        var user = new User();
        user.DebugInfo(()=>{
            callback();
        });
    }

    IEnumerator Co_Ex05()
    {
        _inCallback();
        yield return null;
    }
}
