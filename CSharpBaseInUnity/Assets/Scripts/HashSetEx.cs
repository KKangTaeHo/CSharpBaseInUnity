using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashSetEx : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HashSet<int> hSet = new HashSet<int>();

        hSet.Add(5);
        hSet.Add(1);
        hSet.Add(4);
        hSet.Add(7);
        hSet.Add(4);
        hSet.Add(1);

        // - HashSet은 중복을 허용하지 않는다.
        // - 그리고 해당요소가 특정 순서로 정렬 되어있지 않다.
        string s = null;
        foreach(var v in hSet)
            s += $"{v}, ";

        Debug.Log(s);   // 5, 1, 4, 7
    }
}
