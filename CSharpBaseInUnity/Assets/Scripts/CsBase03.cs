using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CsBase03 : MonoBehaviour
{
    private void Start()
    {
        // LambdaMethod();
        LinqEx();
    }

    private void NoType()
    {
        // 1. 익명타입
        var  v = new { count = 10, title = "server" };
    }

    private void LambdaMethod()
    {
        // foreach문을 다음과 같이 처리가 가능하다.
        List<int> list = new List<int>() { 2, 3, 1, 8, 7 };
        list.ForEach(i => { Debug.Log(i); });

        // 특정조건을 만족하는 요소만 반환하는 작업을 할 때, 다음과 같이 사용이 가능하다.
        List<int> evenList = list.FindAll(i => i % 2 == 0);
        evenList.ForEach(i => { Debug.Log(i); });

        // Where은 FindAll과 비슷하지만, 반환형이 List가 아닌 IEnumerable로 반환한다.
        IEnumerable<int> enumList = list.Where(o => o % 2 == 0);
        Debug.Log("Where");
        foreach(int i in enumList)
        {
            Debug.Log(i);
        }
        
        // 사실 위의 코드는 아래와 같이 표현이 가능하다.
        IEnumerator enumerator = enumList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            Debug.Log(enumerator.Current + ", ");
        }

        // FindAll과 Where의 차이를 알아야하는데,
        // FindAll의 경우 메서드 실행이 완료되는 순간 람다 메서드가 컬렉션의 모든 요소를 대상으로 실행되어 조건을 만족하는 목록을 반환하는 반면
        // Where의 경우 메서드가 실행됬을 때는 어떤 코드도 실행되지 않은 상태이고 이후 열거자를 통해 요소를 순회 했을 때에 비로소 람다 메서드가 하나씩 실행된다는 차이가 있다. 이를 가르켜 '지연된 평가'라고 하며
        // IEnumerable<T>를 반환 하는 모든 메서드가 이러한 방식으로 동작함.

        // 그래서, 지연된 평가의 장점은 최초 메서드 호출로 인한 직접적인 성능 손실이 발생하지 않고 실제 데이터가 필요한 순간에만 코드가 CPU에 의해 실행된다는 점이다..(뭔말..?)
        // 예를 들어 소수 1만개를 반환하는 메서드를 구현한다고 가정했을 때. List<T>를 반환하는 FindAll 방식으로 구하면 데이터가 설령 500개만 필요해도 1만개의 소수를 구할 때까지  CPU가 실행되지만, 지연평가를 사용해
        // 결과가 반환되는 경우 500개의 데이터만 반환받고 끝낼 수 있는 것이다.
    }

    private void LinqEx()
    {
        List<Person> people = new List<Person>
        {
            new Person{name="빌리",age="29",address = "korea"},
            new Person{name="재현",age="31",address = "tibet"},
            new Person{name="은정",age="27",address = "sudan"},
            new Person{name="박강",age="30",address = "japan"},
            // new Person{name="태호",age="30",address = "korea"},
            // new Person{name="태호",age="20",address = "korea"}
        };

        List<MainLanguage> languages = new List<MainLanguage>
        {
            new MainLanguage{name="빌리",language="Delphi"},
            new MainLanguage{name="재현",language="C++"},
            new MainLanguage{name="은정",language="JavaScript"},
            new MainLanguage{name="박강",language="Python"},
            new MainLanguage{name="태호",language="C#"},
            new MainLanguage{name="지숙",language="Java"},
        };

        // Linq에서 Group 사용
        var addressGroup = from Person in people
                           group Person by Person.address;
        foreach(var itemGroup in addressGroup)
        {
            Debug.Log($"<color=#ff0000>주소 : {itemGroup.Key}</color>");
            foreach(var item in itemGroup)
            {
                Debug.Log($"Name : {item.name}");
            }
        }

        // Linq에서 Join 사용
        var nameToLangList = from Person in people
                             join MainLanguage in languages on Person.name equals MainLanguage.name
                             select new { name = Person.name, age = Person.age, language = MainLanguage.language };

        // SingleOrDefault : 싱글에 해당하는 요소가 없는 경우 기본값을 반환
        // 동일한 값이 2개일 경우에는 예외처리됨
        Person p = people.SingleOrDefault(o => o.name == "태호") ?? null;
        Debug.Log(p?.name);
    }
}

public class Person
{
    public string name { get; set; }
    public string age { get; set; }
    public string address { get; set;}

    public Person()
    {

    }
}

public class MainLanguage
{
    public string name { get; set; }
    public string language { get; set; }
}
