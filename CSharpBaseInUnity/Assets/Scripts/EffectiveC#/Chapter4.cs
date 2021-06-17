using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Chapter4 : MonoBehaviour
{

    private void Start()
    {
        Ex33();
    }

    //------------------------------------------------------------
    // 29. 컬렉션을 반환하기보다 이터레이터를 반환하는 것이 낫다
    //------------------------------------------------------------

    // 이터레이더 메서드 
    // - yield return 이 있는 Enumerable 메서드
    // - 호출 할때마다 순차적으로 값을 return 함

    public IEnumerable<int> GetNumber()
    {
        for (int i = 0; i < 10; i++)
            yield return i;
    }

    public void Ex29()
    {
        foreach (int i in GetNumber())
            Debug.Log(i);

        Debug.Log(GetNumber());  // 이거는 값이 안나오고 그냥 메서드 이름만나옴
    }





    //------------------------------------------------------------
    // 30. 루프보다는 쿼리구문이 낫다
    //------------------------------------------------------------

    public void Ex30()
    {
        // 쿼리문으로 루프돌릴 수 있음
        var foo = (from n in Enumerable.Range(0, 100)
                   select n * n).ToArray();

        // 익스텐션으로 아름답게 처리도 가능
        foo.ForAll(x => { });

        // 2중 for문 형태로 돌리는것도 가능하다
        // 근데, 기본 for문보다 더 좋은건, 기타 linq 쿼리문을 사용할 수 있다는것
        var loop = from x in Enumerable.Range(0, 100)
                   from y in Enumerable.Range(0, 100)
                   where x + y < 100
                   select Tuple.Create(x, y);

        // 위의 내용을 매서드 호출구문으로 변경하면
        var method = Enumerable.Range(0, 100).
                    SelectMany(x => Enumerable.Range(0, 100), (x, y) => Tuple.Create(x, y)).    // SelectMany는 뭐라고 설명해야하나?
                    Where(o => o.Item1 + o.Item2 < 100);


        List<string> nameList = new List<string> { "KIN", "ANNA", "JOHN", "TOM" };

        var selectMany = nameList.SelectMany(x => Enumerable.Range(0,100),(x,y)=>x+y);          // x는 컬렉선 아이템(KIN,ANNA 같은거), y는 앞에 정의된 것들(Range(0,100))

        foreach (var val in selectMany)
            Debug.Log(val);
    }





    //------------------------------------------------------------
    // 31. 시퀀스에 사용할 수 있는 조합 가능한 API를 작성하라
    //------------------------------------------------------------

    // - yield를 사용하여 메서드를 작성하면 내가 원하는만큼 루프를 돌릴때 편리함.
    // - 또, 현재수행 상태를 보전할 수 있어서 원하는 지점에 중단하고 다시 실행시킬 수 있는 장점이 있음.

    // - 아래의 코드는 호출 될때마다 하나씩 아이템을 반환하도록 되어있다.
    public IEnumerable<T> Unique<T>(IEnumerable<T> sequence)
    {
        var uniqueVals = new HashSet<T>();

        foreach(T item in sequence)
        {
            if(!uniqueVals.Contains(item))
            {
                uniqueVals.Add(item);
                yield return item;
            }
        }
    }




    //------------------------------------------------------------
    // 33. 필요한 시점에 필요한 요소를 생성해라
    //------------------------------------------------------------

    // - 컬렉션을 초기화 생성시 어떤식으로 처리해야 효율적인지에 대한 설명임
    // - 'BindingList<int>' 라고해서 생성자의 매개변수로 주어진 리스트들을 복사할때 어떤식으로 처리하면 좋을까?에 대한 내용
    // - 이때도 Enumerable<T>를 활용해서 데이터를 초기화 하는 것이 좋음.

    private IList<int> CreateSequenceByList(int numberOfElements, int startAt, int stepBy)
    {
        var collection = new List<int>(numberOfElements);

        for (int i = 0; i < numberOfElements; i++)
            collection.Add(startAt + i * stepBy);

        return collection;
    }

    private IEnumerable<int> CreateSequence(int numberOfElements, int startAt, int stepBy)
    {
        for (var i = 0; i < numberOfElements; i++)
            yield return startAt + i * stepBy;
    }

    public void Ex33()
    {
        // - 두개의 코드는 똑같은 작업을 함.
        // - 책에서는 두번째 IEnumerable를 활용해서 처리하는 것이 좋다고 함.
        // - 이유는 보긴했는데 가슴으로 이해가 안되서.. Pass
        List<int> numberListBy = new List<int>(CreateSequenceByList(100, 0, 5).ToList());

        Debug.Log(numberListBy.Count());

        List<int> numberList = new List<int>(CreateSequence(100,0,5).ToList());

        Debug.Log(numberList.Count());  // 100개 나오네?
    }




    //------------------------------------------------------------
    // 34. 함수를 매개변수로 사용하여 결합도를 낮춰라
    //------------------------------------------------------------

    // - 함수를 매개변수로 사용하면 컴포넌트를 사용하는 측과 컴포넌트를 구현하는 측의 코드를 잘 분리할 수 있음
    // - 인터페이스를 사용하면? 결합이 강해짐, 델리게이트를 사용하면..? 결합도가 낮춰짐
    // - 콜백을 처리했을 경우, null일 경우도 항상 고려하여 메서드를 작성해야함.
    public IEnumerable<T> CreateSequence<T>(int numberOfElements, Func<T> generator)
    {
        for (var i = 0; i < numberOfElements; i++)
            yield return generator();
    }

    public void Ex34()
    {
        // - Ex33 예제로 만들어진 메서드에 델리게이트만 추가하면
        //   메서드 선언시에 공식을 직접 만들어서 전달 할 수 있음
        int startAt = 0;
        // List<int> numberListBy = new List<int>(CreateSequence<int>(100, () => startAt++);
    }
}

public static class Ex30_Extensions
{
    public static void ForAll<T>(this IEnumerable<T> sequence, Action<T> action)
    {
        foreach (T item in sequence)
            action(item);
    }
}