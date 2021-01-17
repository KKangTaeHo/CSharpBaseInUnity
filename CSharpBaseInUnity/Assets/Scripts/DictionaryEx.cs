using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryEx : MonoBehaviour
{
    public class User
    {
        public string name;
        public int age;
    }

    Dictionary<string, User> userDic;

    void Start()
    {
        // 1. 인스턴스 생성 안하고 호출시 'NullReferenceException' 호출
        Debug.Log(userDic["kim"]);

        // 2. 딕셔너리 인스턴스 생성하고  데이터 선언 없이 호출 시 'KeyNotFoundException' 호출
        userDic = new Dictionary<string, User>();
        Debug.Log(userDic["kim"]);

        // 3. 선언 후, 데이터가 null 인 경우 메서드 호출시 'NullRefeenceException' 호출
        userDic.Add("kim", new User());
        Debug.Log(userDic["kim"].name.Split('-'));
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
}
