using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManagerEx
{
    // 상황 설명
    // 씬 전환이 일어났을 경우, 매니저는 어떻게 되는가?

    private static SingletonManagerEx _instance;
    public static SingletonManagerEx Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new SingletonManagerEx();
            }
            return _instance;
        }
    }

    private int _count;
    public int Count
    {
        get
        {
            _count += 1;
            return _count;
        }
    }
}