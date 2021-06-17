using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortEx : MonoBehaviour
{
    public class User
    {
        public string name;
        public bool isA;
        public bool isB;
        public bool isC;

        public User(string name, bool isA = false, bool isB = false, bool isC = false)
        {
            this.name = name;
            this.isA = isA;
            this.isB = isB;
            this.isC = isC;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        List<User> users = new List<User>{
            new User("A",isB:true),
            new User("B",isC:true),
            new User("C",isA:true),
            new User("D",isC:true),
            new User("E",isB:true),
            new User("F",isA:true),
        };

        // 1. 변화하지 않고 그냥 [A, B, C, D, E, F]
        // users.Sort((x,y) => {

        //     if(x.isA && !y.isA)
        //         return 1;
        //     else
        //         return 0;
        // });

        // 2. isA = true인 경우가 앞으로 정렬됨 [C, F, A, B, D, E]
        // users.Sort((x,y) => {

        //     if(x.isA && !y.isA)
        //         return -1;
        //     else
        //         return 0;
        // });

        // 3. 2번과 똑같이 나옴 [C, F, A, B, D, E]
        // return 값이 -1 하면 앞으로 보내지는 것 같음
        // users.Sort((x,y) => {

        //     if(x.isA && !y.isA)
        //         return -1;
        //     else
        //         return 1;
        // });

        // 4. isA = true인 경우가 뒤로 가는데, 역전되서 들어감 (마치 F - A 순으로 정렬한것 처럼) [E, D, B, A, F, C]
        // users.Sort((x,y) => {

        //     if(x.isA && !y.isA)
        //         return 1;
        //     else
        //         return -1;
        // });

        // 5. isA = true인 경우가 뒤로 가는데, 역전되서 들어감 (마치 F - A 순으로 정렬한것 처럼) [E, D, B, A, F, C]
        // 소팅하면
        // [B A]
        //-------
        // [C A]
        //-------
        // [D C]
        // [D A]
        // [D B]
        //-------
        // [E C]
        // [E A]
        // [E B]
        // [E D]
        //-------
        // [F C]
        // [F A]
        // 이렇게 나옴
        // users.Sort((x,y) => {

        //     if(x.isA && !y.isA)
        //     {
        //         return 0;
        //     }
        //     else
        //     {
        //         return -1;
        //     }
        // });

        // 6. 반응 없다 [A, B, C, D, E, F]
        // users.Sort((x, y) =>{

        //     Debug.Log($"x.name :{x.name}, y.name :{y.name}");

        //     if (x.isA && !y.isA)
        //         return 0;
        //     else
        //         return 1;
        // });

        // 대체 안에 어떤 구조가 되어있길래
        // [B A] -> [B, A, C, D, E, F]
        //------
        // [C A] -> [B, C, A, D, E, F]
        // [C B] -> [C, B, A, D, E, F]
        //------
        // [D A] -> [C, B, D, A, E, F]
        // [D B]
        // [D C]
        //------
        // [E A]
        // [E B]
        // [E C]
        // [E D]
        //------
        // [F A]
        // [F B]
        // [F C]
        // [F D]
        // [F E]
        users.Sort((x,y)=>{
            string str =null;
            users.ForEach(o=>{
                str+=o.name+", ";
            });
            Debug.Log(str);
            return -1;
        });
    }
}
