using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Reflection;

using System.Runtime.Remoting;
using ClassLibrary1;
public class ReflectionEx : MonoBehaviour
{
    private void Start()
    {
        ReflectionEx_04();
    }

    // 닷넷 응용 프로그램의 어셈블리 파일 안에는 메타데이터가 있다고 배웠다.
    // BCL에서 제공되는 리플렉션 관련 클래스를 이용하면 메타데이터 정보를 얻는 것이 가능하다.

    // 닷넷 응용 프로그램의 프로세스 구조는
    // 1. 운영체제 에서 EXE 프로세스로 실행되고 그 내부에는 CLR에 의해 "AppDomain(응용 프로그램 도메인)" 이라는 구획으로 나뉜다.
    // 2. AppDomain은 CLR이 구현한 내부적인 격리공간으로 AppDomain 간에 별로의 통신 방법을 설정하지 않는 한 서로의 영역을 침범 할 수 없다.
    // 3. AppDomain이 만들어지면 그 내부에 어셈블리들이 로드된다.

    private void ReflectionEx_01()
    {
        // 어셈블리는 '모듈'의 집합으로 구성되어있음
        // ex.1 현재 AppDomain에 로드된 어셈블리 목록
        AppDomain currDomain = AppDomain.CurrentDomain;
        Debug.Log($"현재 AppDomain.FriendlyName : {currDomain.FriendlyName}");        // Unity Child Domain 이라고 나옴
        foreach (Assembly asm in currDomain.GetAssemblies())
        {
            Debug.Log(" " + asm.FullName);

            foreach (Module module in asm.GetModules())
            {
                Debug.Log($"<color=#f0f0f0> 모듈 : {module.FullyQualifiedName}</color>");     // 어셈블리에 포함된 모듈

                foreach (Type type in module.GetTypes())
                {
                    Debug.Log($"<color=#00ff0f> 타입 : {type.FullName}</color>");
                }
            }

            // 어셈블리 레벨에서 타입을 열거 할 수도 있음.
            // 일반적으로 어셈블리 내에 모듈이 한개만 포함 돼 있는 경우가 대부분이므로 이게 사용하기 더 좋음
            foreach (Type type in asm.GetTypes())
            {
                Debug.Log($"<color=#1234aa>어셈블리에서의 타입 : {type.Name} </color>");
            }
        }
    }

    private void ReflectionEx_02()
    {
        // C# 코드가 빌드되어 어셈블리에 포함되는 경우 그에 대한 모든 정보를 조회 할 수 있는 기술을 '리플렉션' 이라고함!
        NewPerson newPerson = new NewPerson();
        // Assembly asm = Assembly.GetAssembly(typeof(NewPerson));
        Type type = newPerson.GetType();

        Debug.Log($"<color=#321200>타입 : {type.Name}</color>");

        // 클래스에 정의된 생성자 열거
        foreach (ConstructorInfo ctorInfo in type.GetConstructors())
        {
            Debug.Log($"<color=#110f22>생성자 : {ctorInfo.Name}</color>");
        }

        // 클래스에 정의된 이벤트 열거
        foreach (EventInfo eventInfo in type.GetEvents())
        {
            Debug.Log($"<color=#fcc01f> 이벤트 : {eventInfo.Name}</color>");
        }

        // 클래스에 정의된 필드 열거
        foreach (FieldInfo fieldInfo in type.GetFields())
        {
            Debug.Log($"<color=#44c3fa> 필드 : {fieldInfo.Name}</color>");
        }

        // 클래스에 정의된 메서드 열거
        foreach (MethodInfo method in type.GetMethods())
        {
            Debug.Log($"<color=#ffc1a0> 메소드 : {method.Name}</color>");
        }

        // 클래스에 정의된 프로퍼티 열거
        foreach (PropertyInfo property in type.GetProperties())
        {
            Debug.Log($"<color=#1231ff> 프로퍼티 : {property.Name}</color>");
        }
    }

    private void ReflectionEx_03()
    {
        // AppDomain은 EXE 프로세스 내에서 CLR에 의해 구현된 격리된 공간이라고 설명했다.
        // 원한다면 AppDomain을 별도로 생성하는것도 가능하다.
        AppDomain newAppDomain = AppDomain.CreateDomain("MyAppDomain");

        // AppDomain 내에 어셈블리를 로드하는 간단한 방법은 CreateInstanceFrom 메서드를 이용해
        // 어셈블리 파일의 경로와 최초 생성될 객체의 타입명을 지정하는 것이다.

        Debug.Log(Class1.GetStr());
        string dllPath = Application.dataPath + "/Plugins/ClassLibrary1.dll";

        ObjectHandle objhandle = newAppDomain.CreateInstanceFrom(dllPath, "ClassLibrary1.Class1");  // 이렇게 하면 해당 클래스가 생성된 것!

        // 닷넷 응용 프로그램의 경우, 어셈블리 자체를 해제하는 방법은 제공되지 않고
        // 반드시 그것이 속한 AppDomain을 해제하는 경우에만 해당 AppDomain에 속한 어셈블리가 모두 해제된다.
        AppDomain.Unload(newAppDomain);
    }

    private void ReflectionEx_04()
    {
        // public Static 메소드 부르기
        var method1 = typeof(NewPerson).GetMethod("PrintPerson");
        method1.Invoke(null, null);

        // 리플렉션을 활용하면 private도 부를 수 있다.
        var method2 = typeof(NewPerson).GetMethod("PrivatePerson",
                                                  BindingFlags.NonPublic|
                                                  BindingFlags.Static);
        method2.Invoke(null, null);

        // 인스턴스를 포함해서 이렇게 사용 할 수 도 있다.
        var method3 = typeof(NewPerson).GetMethod("PublicPerson");
        var instance3 = new NewPerson();
        var args3 = new object[] { "빌리" };

        method3.Invoke(instance3, args3);
    }
}

public class NewPerson
{
    public string Name { get; set; }    // 프로퍼티
    public int age;
    public string address;
    public float weight;

    public static void PrintPerson()
    {
        Debug.Log("PrintPerson 호출!");
    }
    
    public void PublicPerson(string name)
    {
        Debug.Log($"PrintPerson 호출! Name :{name}");
    }

    private static void PrivatePerson()
    {
        Debug.Log("PrivatePerson 호출!");
    }

    public NewPerson()
    {

    }

    public NewPerson(string inName, int inAge, string inAddress, float inWeight)
    {
        Name = inName;
        age = inAge;
        address = inAddress;
        weight = inWeight;
    }

    public void FindPerson()
    {

    }

    public bool IsFriend()
    {
        return false;
    }
}
