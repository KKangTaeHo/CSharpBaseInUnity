using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Chapter3 : MonoBehaviour
{
    public void Start()
    {
        Ex24();
    }

    //------------------------------------------------------------
    // 18. 반드시 필요한 제약 조건만 설정하라
    //------------------------------------------------------------

    public static bool AreEqual<T>(T left, T right) where T : IComparable<T> =>
        left.CompareTo(right) == 0;

    public class Ex18_Class : IComparable<Ex18_Class>
    {
        public bool isTrue;

        public int CompareTo(Ex18_Class other)
        {
            return 1;
        }
    }

    public void Ex18()
    {
        Ex18_Class e = new Ex18_Class();

        object b = 4;
        object c = b;
        Debug.Log(AreEqual<Ex18_Class>(e, e));
    }

    // [System.Object]
    // - 모든클래스의 조상
    // - 우리가 클래스를 생성할때, 상속하지 않더라도 C# 컴파일시 자동적으로 상속됨
    // - Equal 부터 시작해서 다양한 함수를 가지고 있음.





    //------------------------------------------------------------
    // 19. 런타임에 타입을 확인하여 최적의 알고리즘을 사용해라
    //------------------------------------------------------------

    // - 만약 어떤 알고리즘이 특정 타입에 대해 더 효율적으로 동작한다고 생각된다면, 그냥 그 타입을 이용하도록 코드를 작성하는게 좋음.
    // - 제네릭의 인스턴스화는 런타임의 타입을 고려하지 않으며, 컴파일 타입만을 고려한다.

    // ex) 특정 타입의 시퀀스를 역순으로 순회하기 위해서 다음과 같이 클래스를 만들었다고 가정한다면
    public sealed class ReverseEnumerable<T> : IEnumerable<T>
    {
        private class ReverseEnumerator : IEnumerator<T>
        {
            int currentIndex;
            IList<T> collection;

            public T Current => collection[currentIndex];

            object IEnumerator.Current => this.Current;

            public ReverseEnumerator(IList<T> srcCollection)
            {
                collection = srcCollection;
                currentIndex = collection.Count;
            }

            public void Dispose()
            {
                // - 원래 IEnumerator<T>는 IDisposable를 상속하고 있기 때문에 여기서 처리를 해줘야하지만
                //   지금은 그냥 패스
            }

            public bool MoveNext() => --currentIndex >= 0;

            public void Reset() => currentIndex = collection.Count;
        }

        IEnumerable<T> sourceSequence;
        IList<T> originalSequence;

        // 1. IEnumerable<T>를 통해 시퀀스를 받을 경우, GetEnumerator에서 값들을 복사를 해줘야한다.
        public ReverseEnumerable(IEnumerable<T> sequence) =>
            sourceSequence = sequence;

        // 2. 그래서 IEnumerable<T>말고 IList를 활용하면 말끔히 해결된다.
        public ReverseEnumerable(IList<T> sequence)
        {
            sourceSequence = sequence;
            originalSequence = sequence;
        }

        public IEnumerator<T> GetEnumerator()
        {

            if (originalSequence == null)
            {
                originalSequence = new List<T>();

                foreach (T item in sourceSequence)
                    originalSequence.Add(item);
            }

            return new ReverseEnumerator(originalSequence);
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            this.GetEnumerator();
    }

    // 순서가 이렇게 되어있네?
    // 1. IEnumerable
    // 2. ICollection
    // 3. 그 다음에 IList와 IDictionary를 받는듯
    public void Ex19()
    {
        List<int> listInt = new List<int>(new int[3] { 1, 2, 3 });
        IList iList = listInt;

        foreach (var val in iList)
        {
            if (val is int i)
            {
                Debug.Log(i);
            }
        }
    }


    //------------------------------------------------------------
    // 20. IComparable<T>와 IComparer<T>를 이용하여 객체의 선후 관계를 정의하라
    //------------------------------------------------------------

    // - 닷넷프레임워크에서는 선후관계 정리를 위해 IComparable<T>랑 IComparer<T> 2개 인터페이스를 제공함.
    // - IComparable 인터페이스는 CompareTo라는 하나의 메서드만 갖고 있음. 
    // - 현재 객체가 상대 각체보다 작으면 0보다 작은값을 같으면 0, 크면 0보다 큰값을 반환하도록 되어있음.
    // - 최신꺼는 IComparable<T>로 되어있고, 옛날꺼는 IComparable로 되어있음. (둘 차이는 단지 그것뿐임)

    // - 제네릭 버진이 아닌 IComparable 인터페이스는 왜 구현하냐면
    //   1. 하위 호환성때문에 구현(닷넷프레임워크 2.0)

    public struct Customer : IComparable<Customer>, IComparable
    {
        private readonly string name;

        public Customer(string name) =>
            this.name = name;

        public int CompareTo(Customer other) =>
            name.CompareTo(other.name);

        int IComparable.CompareTo(object obj)
        {
            if (!(obj is Customer))
                throw new ArgumentException();

            Customer otherCustomer = (Customer)obj;
            return this.CompareTo(otherCustomer);
        }
    }

    public struct Employee : IComparable<Employee>, IComparable
    {
        private readonly string name;

        public Employee(string name) =>
            this.name = name;

        public int CompareTo(Employee other) =>
            name.CompareTo(other.name);

        int IComparable.CompareTo(object obj)
        {
            if (!(obj is Employee))
                throw new ArgumentException();

            Employee otherCustomer = (Employee)obj;
            return this.CompareTo(otherCustomer);
        }
    }

    public void Ex20()
    {
        Customer c1 = new Customer("코딩");
        Customer c2 = new Customer("하자");
        Employee e1 = new Employee("코딩");

        if (c1.CompareTo(c2) != 0)
            Debug.Log("서로다름");

        // 이렇게하면 컴파일 에러는 발생하지 않지만 런타임에서 예외처리 발생(obj is Customer)
        if (((IComparable)c1).CompareTo(e1) == 0)
            Debug.Log("이름같다");
    }

    // - 연산자 오버로드를 사용하면 비교를 좀더 깔끔하게 처리 할 수 있다.
    public struct NewCustomer : IComparable<NewCustomer>, IComparable
    {
        private readonly string name;
        private readonly double revenue;

        public NewCustomer(string name, double revenue)
        {
            this.name = name;
            this.revenue = revenue;
        }

        public int CompareTo(NewCustomer other) =>
            name.CompareTo(other.name);

        int IComparable.CompareTo(object obj)
        {
            if (!(obj is NewCustomer))
                throw new ArgumentException();

            NewCustomer otherCustomer = (NewCustomer)obj;
            return this.CompareTo(otherCustomer);
        }

        // 관계연산자
        public static bool operator <(NewCustomer left, NewCustomer right) =>
            left.CompareTo(right) < 0;

        public static bool operator <=(NewCustomer left, NewCustomer right) =>
            left.CompareTo(right) <= 0;

        public static bool operator >(NewCustomer left, NewCustomer right) =>
            left.CompareTo(right) > 0;

        public static bool operator >=(NewCustomer left, NewCustomer right) =>
            left.CompareTo(right) >= 0;




        private class RevenueComparer : IComparer<NewCustomer>
        {
            public int Compare(NewCustomer x, NewCustomer y) =>
               x.revenue.CompareTo(y);
        }

        private static Lazy<RevenueComparer> revComp = new Lazy<RevenueComparer>(() => new RevenueComparer());

        public static IComparer<NewCustomer> RevenueCompare => revComp.Value;

        public static Comparison<NewCustomer> CompareByRevenue => (left, right) => left.revenue.CompareTo(right.revenue);


        // Lazy ?
        // Comparison ?
    }




    //------------------------------------------------------------
    // 21. 타입 매개변수가 IDisposable을 구현한 경우를 대비하여 제네릭 클래스를 작성하라
    //------------------------------------------------------------

    // Q. (제네릭형식)제약조건? >> where 의미함.
    // - 제약조건은 두가지 역활이있음.
    //   1. 런타임 오류가 발생할 가능성이 있는 부분을 컴파일 타임에서 대체 할 수 있음.
    //   2. 타입 매개변수로 사용 할 수 있는 타입을 명확히 규정하여 사용자에게도 도움을 준다.


    public interface IEngine
    {
        void DoWork();
    }

    public class EngineDriverOne<T> where T : IEngine, new()
    {
        public void GetThingsDone()
        {
            T driver = new T();

            // 이렇게하면 마지막에 Dispose도 호출된다.(IDisposable를 상속받은 경우엔)
            // Dispose가 없는 경우엔 그냥 Dispose가 없으니 자연스레 Dowork만 호출
            using (driver as IDisposable)
            {
                driver.DoWork();
            }
        }
    }

    public class MyEngine : IEngine, IDisposable
    {
        public void Dispose()
        {
            Debug.Log("Dispose 호출");
        }

        public void DoWork()
        {
            Debug.Log("DoWork 호출");
        }
    }

    public class MyEngineByNonDispose : IEngine
    {
        public void DoWork()
        {
            Debug.Log("DoWork 호출함");
        }
    }

    public void Ex21()
    {
        EngineDriverOne<MyEngineByNonDispose> myEngine = new EngineDriverOne<MyEngineByNonDispose>();

        myEngine.GetThingsDone();
    }




    //------------------------------------------------------------
    // 22. 공변성과 반공변성을 지원하라
    //------------------------------------------------------------

    // * 공변성과 반공변성 : 특정 타입의 객체를 다른 타입의 객체로 변환할 수 있는 성격
    // * 공변성 
    //      - X -> Y 가 가능할때 C<T> 가 C<X> -> C<Y> 로 가능하다면 공변
    //      - 자신과 자식으로만 형변환, out 키워드로 지정
    // * 반공변셩
    //      - X -> Y 가 가능할때 C<T> C<Y> -> C<X> 로 가능하다면 반공변
    //      - 자신과 부모로만 형변환, in 키워드로 지정

    public class Base : IComparable<Base>
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int CompareTo(Base other)
        {
            throw new NotImplementedException();
        }
    }

    public class SubA : Base { }

    public class SubB : Base { }

    // A. out이 없는 경우
    public interface ICovariant<T>
    {
        void DoSomething();
    }

    public class Sample<T> : ICovariant<T>
    {
        public void DoSomething()
        {
            throw new NotImplementedException();
        }
    }

    // B. out 있는 경우 (공변 제네릭 인터페이스)
    public interface ICovariantOut<out T>
    {
        void DoSomething();
    }

    public class SampleOut<T> : ICovariantOut<T>
    {
        public void DoSomething()
        {
            throw new NotImplementedException();
        }
    }

    // C. in 이 있는 경우 (반공변 제네릭 인터페이스)
    interface IContravariant<in A>
    {
        void DoSomething();
    }

    public class SampleIn<T> : IContravariant<T>
    {
        public void DoSomething()
        {
            throw new NotImplementedException();
        }
    }

    public void Ex22()
    {
        // C#에서는 배열이 공변적이기 때문에 아래와 같이 초기화가 가능하다.
        Base[] items = new SubA[5];
        var item = items[0];    // 이를 `공변적`이라고 함

        // items[0] = new SubB();  // 이렇게하면 애러발생한다.


        // 제네릭에서는 공변성을 지원하지 않음
        // - 즉 부모타입에 자식타입을 넣을 수 없음.
        // ICovariant<Base> sample = new Sample<SubA>();

        // out을 활용하면 공변성지원(확실한건 아니지만, 그냥 캐스팅으로 이해할란다)
        ICovariantOut<Base> sampleOut = new SampleOut<SubA>();
        // ICovariantOut<SubA> sampleOut_2 = new SampleOut<Base>(); -> 에러발생

        // in을 활용하면 반공변성 지원 
        // IContravariant<Base> sampleIn = new SampleIn<SubA>(); -> 에러발생 
        IContravariant<SubA> sampleIn_2 = new SampleIn<Base>();


        // 불변이라서 가능?
        IList<Base> baseList = new List<Base>() { null, null, };
        baseList[0] = new SubA();

        // * 정리
        // - 가변성을 두가지로 나눌 수 있는데, 공변성과 반공변성으로 나눔
        // - 인터페이스나 델리게이트에서 제네릭타입으로 받을 경우 처리되는 방식을 설명하는 내용인듯
        // - 원래 제너릭간의 가변성이 적용되지 않았는데, 4.0 이후부터 적용됨.(in out)
        // - IEumerable이 out의 형태가 사용되고 IComparable에서 in형태로 사용된다.
    }




    //------------------------------------------------------------
    // 24. 베이스 클래스나 인터페이스에 대해서 제네릭을 특화하지 말라
    //------------------------------------------------------------

    // ex) 아래 코드의 호출 순서는?
    public class Msg
    {
    }

    public interface IMessageWriter
    {
        void WriteMessage();
    }

    public class MyDerived : Msg, IMessageWriter
    {
        public void WriteMessage() => Debug.Log("MyDerived");
    }

    public class AnotherDerived : IMessageWriter
    {
        public void WriteMessage() => Debug.Log("AnotherDerived");
    }

    void WriteMessage(Msg m)
    {
        Debug.Log("WriteMeesage(Msg m)");
    }

    void WriteMessage<T>(T obj)
    {
        Debug.Log($"WriteMeesage(T obj) : {obj.ToString()}");
    }

    void WriteMessage(IMessageWriter obj)
    {
        Debug.Log($"WriteMeesage(IMessageWriter obj)");
        obj.WriteMessage();
    }

    public void Ex24()
    {
        // * 제네릭이 제일 우선적으로 처리되는 것이 포인트

        MyDerived d = new MyDerived();
        WriteMessage(d);                    // WriteMessage<T>

        WriteMessage((IMessageWriter)d);    // WriteMessage(IMessageWriter obj)

        WriteMessage((Msg)d);               // WriteMessage(Msg m)

        AnotherDerived a = new AnotherDerived();
        WriteMessage(a);                    // WriteMessage<T>

        WriteMessage((IMessageWriter)a);    // WriteMessage(IMessageWriter obj)



        // * 결론
        // - 오버로드의 여러 형태가 존재할 경우 베이스 클래스나 인터페이스(위의 Msg 나 IMessageWriter와) 매개변수로 할 경우
        //   우선순위에 있어 제네릭한테 밀려날 경우가 있으니 주의해야함
    }




    //------------------------------------------------------------
    // 25. 타입 매개변수로 인스턴스 필드를 만들필요가 없다면 제네릭 메서드를 정의하라
    //------------------------------------------------------------

    public static class Utils<T>
    {
        public static T Max(T left, T right) =>
            Comparer<T>.Default.Compare(left, right) < 0 ? right : left;

        public static T Min(T left, T right) =>
            Comparer<T>.Default.Compare(left, right) < 0 ? left : right;
    }

    // - 위처럼 사용하기 보다는, 아래처럼 제네릭메서드로 만드는게 좋음

    public static class NewUtils
    {
        public static T Max<T>(T left, T right) =>
             Comparer<T>.Default.Compare(left, right) < 0 ? right : left;

        public static double Max(double left, double right) =>
            Math.Max(left, right);

        public static T Min<T>(T left, T right) =>
                Comparer<T>.Default.Compare(left, right) < 0 ? left : right;

        public static double Min(double left, double right) =>
            Math.Min(left, right);
    }

    // - 선언된 매개변수가 존재할 경우 그 타입을 가고, 아닐경우 제네릭메서드를 사용하도록 만듬
    // - 이런식으로 쓰는게 더 효과적임

    // - 책에서는 다음의 두가지 경우에는 반드시 제네릭 클래스를 만들어야한다고 말함.
    // 1. 클래스 내에 타입 매개변수로 주어진 타입으로 내부상태를 유지하는 경우(컬렉션 같은거)
    // 2. 제네릭 인터페이스를 구현하는 경우




    //------------------------------------------------------------
    // 26. 제네릭 인터페이스와 논제네릭 인터페이스를 함께 구현하라
    //------------------------------------------------------------

    // - 제네릭이 만들어지기전 레거시코드 호환을 위해 될수 있으면 같이 콤보로 섞어서 사용해라





    //------------------------------------------------------------
    // 27. 인터페이스는 간략히 정의하고 기능의 확장은 확장 메서드를 사용하라
    //------------------------------------------------------------

    // - 인터페이스의 메서드를 늘리기보단, 확장메서드로 대체할 수 있는게 있다면 대체하는 것이 좋음
    // - 여러 클래스에서 반드시 구현해야하는 인터페이스를 정의하는 경우 인터페이스 내에 정의하는 멤버의 수를 최소한으로 하는게 좋음
    public void Ex27()
    {
        MyType t = new MyType();
        t.NextMarker(); // 확장메서드? 클래스 내부 메서드? 어떤게 우선순위일까?
                        // 정답은, '클래스 내부 메서드'가 우선순위로 호출됨

    }




    //------------------------------------------------------------
    // 28. 확장 메서드를 이용하여 구체화된 제네릭 타입을 개선하라
    //------------------------------------------------------------

    // System.Linq.Enumerable 클래스도 확장메서드를 잘 활용하여 선언한다?
}

public interface IFoo
{
    int Marker { get; set; }
}

public static class FooExtensions
{
    public static void NextMarker(this IFoo thing) =>
        thing.Marker += 1;
}

public class MyType : IFoo
{
    public int Marker { get; set; }
    public void NextMarker() => Marker += 5;
}
