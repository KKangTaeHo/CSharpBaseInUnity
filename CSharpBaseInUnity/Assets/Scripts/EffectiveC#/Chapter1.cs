using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Ex10_1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //----------------------------------------------
    // 9. 박싱과 언박싱을 최소화해라
    // 박싱 : 값 타입을 참조 타입으로 변경하는 과정
    //----------------------------------------------
    void Ex09_1()
    {
        int num1 = 5;
        int num2 = 6;
        Debug.Log($"{num1}, {num2.ToString()}");

        // 위와 같이 이루어 졌을 때 num1 은 사실
        // int num1 = 5;
        // object o = num1;
        // o.ToString() 이 이루어지면서 문자열로 사용되는거다(박싱)

        // 그래서 'num2.ToString()'을 하는 것이 성능에 더 좋다.(바로 문자열 인스턴스전달)
    }
    
    public struct Person
    {
        public string Name;
    }

    void Ex09_2()
    {
        var attendees = new List<Person>();
        Person p = new Person { Name = "Old Name" };
        attendees.Add(p);

        Person p2 = attendees[0];   // 컬렉션으로 부터 객체를 가져올 때, 복사본만 가지고 오므로 값을 변경할 수 없다.
        p2.Name = "New Name";


        Debug.Log(attendees[0].Name.ToString());    // Old Name

        // 결론
        // - 값타입은 system.object나 다른 인터페이스로 타입을 변경 할 수 있다.
        // - 그런데, 이런 작업은 암시적으로 이루어지기 때문에 실제로 어떤 부분에서 이루어지는지 찾기 힘들다.
        // - 그니깐 여러모로 쓰는게 안좋음
    }

    //------------------------------------------------------------
    // 10. 베이스 클래스가 업드레이드 된 경우에만 new한정자를 사용해라
    //------------------------------------------------------------
    public class MyClass
    {
        public virtual void Method() => Debug.Log("myClass Method");
        public void MagicMethod() => Debug.Log("myClass");
    }

    public class MyOtherClass :MyClass
    {
        public override void Method() => Debug.Log("myOtherClass Method");
        public new void MagicMethod() => Debug.Log("myOtherClass");
    }

    void Ex10_1()
    {
        object c = new MyOtherClass();

        MyClass c2 = c as MyOtherClass;
        c2.MagicMethod();
        c2.Method();

        // - 기본적으로 virtual 과 override 관계에서는 
        //   "자식의 객체를 부모형태로 할당 한 후 오버라이딩 된 메서드를 호출 하게 되면 자식에서 선언한 메서드가 나오는 것" 이 특징이였다 
        //   (c2.Method 호출시 override 된 메서드 호출)
        // - 하지만, new로 메서드를 선언 할 경우, 객체 생성된 타입에 상관 없이 할당된 타입을 우선적으로 반환한다
        //   (c2.MagicMethod 호출시 c2의 타입이 'MyClass' 인것을 고려하려 호출됨. (c 타입이 'MyOtherClass라도'))

        // 결론
        // - 베이스 클래스가 업그레이드 된 경우에만 사용.
        // Q. 그럼 베이스 클래스는 뭐임 >> 오버라이딩 메서드의 원래 메서드 (보통 base.메서드 이런 형태임)
        // Ex.)
        // class 자식 : 부모
        // {
        //      public new void Method()
        //      {
        //          base.Method() 
        //      }
        // }
        // 이런 형태로 쓸 경우, new 메서드를 쓰는게 좋다는 뜻. 부모 코드를 수정하지 않고 업그레이드 처리가 될 경우
        // - 진짜 신중하게 new 메서드를 사용해아함
    }
}
