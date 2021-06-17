using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputEx : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            int cnt = SingletonManagerEx.Instance.Count;
            Debug.Log(cnt);
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        { 
            SceneManager.LoadScene(1);
        }
    }
}
