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

    private void Start()
    {
        // Debug.Log(userDic["kim"].age);           // 1. userDic을 인스턴스 안했을 경우 'NullReferebceException' 뜸

        userDic = new Dictionary<string, User>();

        //  Debug.Log(userDic["kim"].name);         // 2. keyNotFoundException 예외가 든다. (딕셔너리 객체만 생성됬을 때)

        userDic.Add("kim", new User());

        Debug.Log(userDic["kim"].name.Split('.'));  // 3. name일 경우 'NullReferenceException' 뜬다.
    }
}

