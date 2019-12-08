using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// C# 2.0
public class CsBase02 : MonoBehaviour
{
    private void Start()
    {
        DefaultEx();
        // YieldEx();
        NullableEx();
    }

    // 박싱 vs 언방식
    // 값 형식 > 참조 형식 으로 변환 한 것을 '박싱' 그 반대를 '언박싱'이라고 함
    // 이런 변환과정은 object 타입과 System.ValueType을 상속 받는 값 형식을 섞어 쓰는 경우 발생함.
    private void BoxingUnBoxing()
    {
        int a = 10;         // 1. a는 지역변수로 스택메모리에 10이 들어감
        object obj = a;     // 2. obj도 지역변수이므로 스텍에 할당된다. 하지만 object가 참조형이기 때문에 힙에도 메모리가 할당되고
                            //    변수 a값이 들어간다(박싱)
        int b = (int)obj;   // 3. b도 지역변수이고 스택메모리에 b영역이 있고, 힙 메모리에 있는 값을 스택메모리에 복사함.

        // 스택의 값 복사만으로 끝날 수 있는 문제를 박싱으로 인해 관리 힙을 사용하게 되며, GC에 일을 시키게 만들었다.
        // 즉, 박싱이 빈번 할 수록 GC는 바빠지고 프로그램의 수행 성능은 그만큼 떨어짐!
    }

    // 1. default 예약어
    // 일반적으로 변수를 초기화하지 않은 경우 값 형식은 0, 참조형식은 null로 초기화된다.
    // 하지만, 제네릭의 형식 매개변수로 전달 된 경우에는 코드에서 미리 타입을 알 수 없기 때문에
    // 그에 대응되는 초기값도 설정 할 수 없다.
    private void DefaultEx()
    {
        // T형식에 따라 자동으로 컴파일러가 결정할 수 있도록 default 설정
        ArrayNoException<int> arr = new ArrayNoException<int>(10);
        arr[10] = 5;    // index가 10인데도 안터지는걸 볼 수 있다.
        Debug.Log(arr[10]);
    }

    // IEnumerable 인터페이스
    // GetEnumerator() 를 가지고 있으며, 이것은 '열거자'라고 하는 객체를 반환하도록 되어있다.
    // 이 객체는 IEnumerator 인터페이스를 구현한 객체를 말한다.
    // IEnumerable 인터페이스를 구현한 전형적인 예는 System.Array다. 그래서 아래와 같이 열거가 가능하다.
    private void IEnumeratorEx()
    {
        int[] arr = new int[] { 1, 2, 3, 4, 5 };

        IEnumerator enumerator = arr.GetEnumerator();

        while(enumerator.MoveNext())
        {
            Debug.Log(enumerator.Current + ", ");
        }

        // 이걸 줄이기 위해 나타난것이 foreach문이다.
        foreach(int i in arr)
        {
            Debug.Log(enumerator.Current + ", ");
        }
    }

    // 2. yield return / break
    // yield return 과 yield break 를 이용하면 기존의 IEnumerable, IEnumerator 인터페이스로 구현했던 열거기능을 더 쉽게 구현할 수 있음.
    private void YieldEx()
    {
        NaturelNumber number = new NaturelNumber();
        foreach(int n in number)
        {
            if (n > 1000) break;
            Debug.Log(n);
        }

        foreach(int n in YieldNaturalNumber.Next())
        {
            if (n > 1000) break;
            Debug.Log(n);
        }
    }

    // 3. 부분(partial) 클래스
    // partial 예약을 클래스에 적용하면 클래스의 소스코드를 2개 이상으로 나눌 수 있다.
    // 클래스 정의가 나뉜 코드는 한 파일에 있어도 되고 다른 파일로 나누는 것도 가능하지만 반드시 같은
    // 프로젝트에서 컴파일해야 한다.
    public partial class PartialEx { int num01; }
    public partial class PartialEx { int num02; }

    // 4. nullable 형식
    private void NullableEx()
    {
        // Nullable<T> 타입은 일반적으로 값 형식에 대해 null 표현이 가능하도록 만들어줌
        // 그러니깐, bool 같은건 null 이런 형태로 표현 할 수 없기 때문에 필요함.
        Nullable<int> intValue = 10;
        int target = intValue.Value;
        Debug.Log(target);

        Nullable<int> temp = null;
        Debug.Log(temp ?? 0);
        Debug.Log(temp.HasValue);
        //Debug.Log(temp.Value);

        temp = 3;
        Debug.Log(temp ?? 0);
        Debug.Log(temp.HasValue);
        Debug.Log(temp.Value);
    }

}

// Ex01 인덱스를 벗어나도 예외가 발생하지 않는 배열
public class ArrayNoException<T>
{
    int _size;
    T[] _items;

    public ArrayNoException(int size)
    {
        _size = size;
        _items = new T[size];
    }

    public T this[int index]
    {
        get
        {
            if (index >= _size)
                return default(T);  // Default 예약어

            return _items[index];
        }

        set
        {
            if (index >= _size)
                return;

            _items[index] = value;
        }
    }

}

// Ex02 IEnumerable 인터페이스를 이용한 자연수 표현
public class NaturelNumber : IEnumerable<int>
{
    public IEnumerator<int> GetEnumerator()
    {
        return new NaturalNumberEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new NaturalNumberEnumerator();
    }

    public class NaturalNumberEnumerator:IEnumerator<int>
    {
        public int Current { get; set; }

        object IEnumerator.Current { get { return Current; } }

        public void Dispose() { }

        public bool MoveNext()
        {
            Current++;
            return true;
        }

        public void Reset()
        {
            Current = 0;
        }
    }
}

// NaturelNumber 클래스와 동일한 기능(훨씬 간결)
public class YieldNaturalNumber
{
    // Next 메서드가 호출되면 yield return 에서 값이 반환 되면서 메서드의 실행을 중지한다.
    // 하지만 내부적으로는 yield return 이 실행된 코드의 위치를 기억해 뒀다가 다음에 다시한번 메서드가 호출되면
    // 처음부터 코드가 시작되지 않고, 마지막 yield return문이 호출됬던 코드의 다음 줄 부터 실행을 재개한다.
    public static IEnumerable<int>Next()
    {
        int _start = 0;
        while(true)
        {
            _start++;
            yield return _start;
        }
    }
}
