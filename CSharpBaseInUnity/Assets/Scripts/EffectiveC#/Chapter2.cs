using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.IO;
using System.Runtime.InteropServices;

public class Chapter2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var val = new Ex16_Sub("생성자 초기화");
    }

    //------------------------------------------------------------
    // 11. 닷넷 리소스 관리에 대한 이해
    //------------------------------------------------------------

    // 가비지 수집기는 리소스를 제거 + 콤팩트 작업도 같이함.
    // ※ 콤팩트 작업 : 사용중인 객체들을 한쪽으로 차곡차곡 옮겨서 조각난 가용메모리를 단일 형태의 큰메모리로 만드는 과정
    // ※ 도달가능하다 = 사용중이다.

    // 1. 비관리 리소스(데이터베이스 연결, GDI+객체, COM객체, 시스템객체)는 여전히 개발자가 관리해줘야함.
    // 2. 닷넷프레임워크는 비관리 리소스의 생명주기를 처리하기위해 finalizer(소멸자)랑 IDisposable 인터페이스를 제공함.
    // 3. finalizer를 가지고 있는 객체는 가비지로 간주된 후에도 꽤 긴시간 동안 메모리를 점유하고 있는데, 
    //    그 이유는 finalizer가 언제쯤 호출될지는 아무도 모르기 때문이다.
    // 4. 가비지 수집기에는 세대라는 개념이 있어서 finalizer가 있는 객체는 가비지로 수집된 그 새대에 처리가 되는것이 아니라, 
    //    그 이후 세대에 처리되기 때문에 오랫동안 메모리를 점유하고 있는 것
    // 5. 근데,, 문제는 finalizer가 비관리 리소스들을 해제하는 유일한 방법이라는거.
    // 6. 그래서 닷넷환경에서 비관리리소스들을 관리하기 위해 IDisaposible 인터페이스를 제공함..



    //------------------------------------------------------------
    // 12. 할당 구문보다 멤버 초기화 구문이 좋다.
    //------------------------------------------------------------

    public class Ex12
    {
        // 이렇게 맴베어서 미리 객체를 할당하는게 더 좋음
        public List<int> myList = new List<int>();
    }




    //------------------------------------------------------------
    // 13. 정적 클래스 멤버를 올바르게 초기화해라
    //------------------------------------------------------------

    // 정적생성자 : 타입 내에 정의된 모든메서드, 변수, 속성에 최초로 접근하기전에 자동으로 호출되는 특이한 메서드
    // 싱글톤 만들때 좋다.
    public class Ex13
    {
        // 기본적으로 이렇게 싱글톤을 만들거나
        private static readonly Ex13 _instance = new Ex13();
        public static Ex13 Instance { get => _instance; }

        // 이런식으로 초기화도 가능하다.
        static Ex13()
        {
            _instance = new Ex13();
        }
    }




    //------------------------------------------------------------
    // 14. 초기화 코드가 중복되는 것을 최소화해라 (생성자 여러개 만들지마라)
    //------------------------------------------------------------

    public class Ex14
    {
        private string _name;
        private int _age;

        public Ex14() :
            this("", 5)
        {

        }

        public Ex14(string name = "", int age = 0) =>
            (_name, _age) = (name, age);

        // 상황에 맞게 생성자를 만드는 것이 아니라, 위와 같이 생성자를 만들어 그때 그때 필요에 따라 생성자를 사용하도록 함.
        // Q. 그럼 기본 생성자 (Ex14())는 없어도 되지 않나...?
        // - new() 제약조건을 명시한 제너릭 클래스는 매개변수가 없는 생성자를 필요로 하기 때문에, 그럴경우를 대비해서
        // - 또, 리플렉션을 사용할 경우에도 매개변수가 없는 생성자가 필요함.

        // * 특정타입으로 첫번째 인스턴스를 생성할 때 수행되는 과정
        // 1. 정적변수 저장공간 0 수행
        // 2. 정적변수 초기화 구문 수행
        // 3. 베이스 클래스의 정적 생성자 수행
        // 4. 정적 생성자 수행
        // 5. 인스턴스 변수의 저장공간 0 수행
        // 6. 인스턴스 변수의 초기화 구문 수행
        // 7. 적절한 베이스 인스턴스 클래스 수행
        // 8. 인스턴스 생성자수행
    }




    //------------------------------------------------------------
    // 15. 불필요한 객체를 만들지마라
    //------------------------------------------------------------

    // - 가비지 수집기가 많이 돌아가면 성능에 많은 문제가 발생한다.
    // - 그렇기 때문에, 가비지 수집기가 최대한 활동하지 않도록 코드를 짜야한다.
    // 1. 자주 사용되는 지역변수는 맴버변수로
    // 2. '종속절 삽입'을 활용하여 사용되는 객체는 한번 생성하면 다음에 재사용하도록 하는 것이다.
    //     ex) static 활용 (싱글톤처럼)
    // 3. 변경불가능한 타입을 작성하는 경우, 이에 대응되는 변경가능한 빌더 클래스를 사용하는 것

    public class Ex15
    {
        public void Print()
        {
            string msg = "Hello, ";
            msg += "my name is";
            msg += "bitcoin";

            // - 위의 코드는 기존의 코드를 뒤에 붙이는 걸로 보이지만,
            //   사실상 지우고 새로운 객체를 만드는 행동이다. (비 효율적)
            // - 그렇기 때문에 저렇게 사용하는 것보다 'StringBuilder'를 활용해 수정이 가능한 문자열을 만든 후, 최종적으로 수정불가능한 문자열을 반환하도록한다.

            StringBuilder sb = new StringBuilder("Hello, ");
            sb.Append("my name is");
            sb.Append("bitcoin");
        }
    }





    //------------------------------------------------------------
    // 16. 생성자 내에서는 절대로 가상함수를 호출하지마라
    //------------------------------------------------------------

    public class Ex16
    {
        public Ex16() => Print();

        public virtual void Print() =>
            Debug.Log("Ex16 Print");
    }

    public class Ex16_Sub : Ex16
    {
        private readonly string msg = "인스턴스 변수";

        public Ex16_Sub(string msg) =>
            this.msg = msg;

        public override void Print() =>
            Debug.Log(msg);
    }

    // - Ex16_Sub를 객체 생성을 하면 'this.msg = msg' 를 통해 값을 넣었을 데도 불구하고
    //   '인스턴스 변수' 가 호출되는 것을 알 수 있음
    // - 파생클래스의 생성자를 통해 정의를 했더라도, 실제적으로 파생 클래스의 생성자 본문은 아직 수행조차 하지 않았을 수도 있기 때문..
    // - 이런식으로는 사용 안하는게 좋음




    //------------------------------------------------------------
    // 17. 표준 Dispose 패턴을 구현하라
    //------------------------------------------------------------

    // - 가장먼저 알아두어야하는 것은 '비관리 리소스를 포함한 클래스는 반드시 finalizer를 구현' 해야한다는거
    // 
    // public interface IDisposable
    // {
    //      void Dispose();
    // }
    //
    // * Dispose의 역활
    // 1. 모든 비관리 리소스를 정리한다.
    // 2. 모든 관리 리소스를 정리한다.
    // 3. 이미 정리된 객체에 대하여 추가로 정리 작업이 요청될 경우 예외발생
    // 4. finalizer 호출 회피. 이를 위해 GC.SupperessFinalize(this) 호출함.

    // - 위에것만 쓸 경우에는 약간? 부족한 부분이 있음.
    // - 인터페이스를 가지고 있는 클래스가 상속을 할 경우 약간 문제 생김.(베이스클레스도 같이 관리해야하니깐)
    // - 또, finalizer나 Dispose는 동일한 기능을 하는데 두개다 호출될수도 있기 때문(3번이슈)

    // - 위의 문제를 해결하기 위해 '가상헬퍼(virtual)'를 만들어준다.
    public class Ex17 : IDisposable
    {
        private bool alreadyDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (alreadyDisposed)
                return;

            if (isDisposing)
            {
                // 여기서 관리 리소스를 정리한다.(어떻게 정리하는거임? null 해주면 되나?)
            }

            // 여기서 비관리 리소스를 정리한다.
            alreadyDisposed = true;
        }
    }

    public class Ex17_Sub : Ex17
    {
        private bool disposed = false;

        protected override void Dispose(bool isDisposing)
        {
            if (disposed)
                return;

            if (isDisposing)
            {
                // 여기서 관리 리소스를 정리한다.
            }

            // 여기서 비관리 리소스 정리

            // 베이스 클래스가 자신의 리소스릴 정리 할수 있도록 해주어야한다.
            // 베이스 클래스는 GC.SupperssFinalize()를 호출해야한다.
            base.Dispose(isDisposing);

            disposed = true;
        }
    }

    // - Dispose로 관리 되는 객체들 중에 일부분이 먼저 정리가 되었을 때, Dispose를 호출할 경우 문제가 발생할 수도 있다.
    // - 그래서 Dispose를 반복해서 호출 할때도 문제가 발생하지 않도록 만드는 것이 포인트

    // - 위의 예제에서는 finailzer가 없는 이유는, 클래스가 비관리 리소스를 직접 포함하고 있지 않기때문이라고.. (뭔말?)
    // Q. 그럼 호출 여부와 상관없이 finalizer를 만들면? >> 성능에 상당히 문제가 발생
    // - 무조건 Dispose나 finalizer에서는 리소스 정리작업만 해야함



    // Finalizer 기초
    // - 닷넷 메모리 관리는 CLR의 가비 컬렉터에 의해 수행됨. 즉 메모리할당과 해제가 CLR에서 처리됨.
    // - 근데, 어떤 객체가 파일이나 데이터베이스 연결, 그리고 CLR에 의해 관리되지 않는 메모리 등의 시스템 자원을 사용할 겨우
    //   CLR에 의해 자동적으로 해제되지 않음. 따라서 이런건 개발자가 해제해 주어야함.
    // - 그래서 CLR에서는 'Finalizer'를 제공함.

    // Finalizer 사용시 주의사항
    // - Finalizer 메서드는 관리되지 않는 자원을 해제하기 유용하나 별로 권장되는 패턴은 아님
    // 1. 기존의 객체보다 FInalizer가 선언된 객체는 더 오랫동안 관리되는 힙에 남음
    // 2. Finalizer를 호출하는 스레드가 Finalizer 스레드이기 때문에 특정 스레드에 선호도를 같는 객체일 경우 문제가 발생함.(뭔말인지 모르겟음)
    // 3. Finalizer 메서드 호출 순서는 불규칙적임.. 아래코드를 보자


    public class DangerousType
    {
        private StreamWriter _stream;

        public DangerousType() =>
            _stream = new StreamWriter("test.txt");

        ~DangerousType() =>
            _stream.Close();
    }

    // - 위와 같이 코드를 짜면 문제가 발생함.
    // - DangerousType이 가비지콜렉션 대상이 된다면, 내부에 있는 StreamWriter도 같이 가비지콜렉션의 대상이 됨.
    // - 그래서 두 객체의 Finalizer가 Finalizer 큐에 들어감.
    // - 여기까지는 좋은데.. 위의 3번에서 말한것처럼 Finalizer메서드 호출은 불규칙적이기 때문에
    //   _stream의 finalizer가 호출 된 후, DangerousType이 호출되면 뻑이 날수 있음.

    // - 단순히 IDisposable 인터페이스를 구현 하는 것 만으로 시스템 자원을 안전하게 반납하거나 효율적으로 사용하는 건 아님
    // - IDisposable 인터페이스는 CLR에게는 그냥 인터페이스 중 하나.
    // - 따라서 개발자가 반드시 Dispose 메서드를 호출해줘야하며, 이것은 하나의 코딩패턴임.

    public class SafeType : IDisposable
    {
        private StreamWriter _stream;
        private IntPtr _unmanagedResources;

        public SafeType()
        {
            _stream = new StreamWriter("test.txt");
            _unmanagedResources = Marshal.AllocHGlobal(1024);
        }

        ~SafeType()
        {
            Dispose(false);
        }

        // - 이 Dispose를 호출하면 가비지컬렉터가 Finalizer를 호출하지 않도록 함
        // - Dispose가 명시적으로 호출되지 않고 Finalize 메서드가 호출 된 경우에는
        //   자원을 해제하지 않음. 그 이유는 SafeType 클래스가 사용하는 자원(_stream)이 관리되는 자원이기 때문
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                _stream.Dispose(); 
            }

            // 비관리 리소스
            Marshal.FreeHGlobal(_unmanagedResources);
        }
    }

    // - 위의 핵심은 Dispose 호출을 통해서만  _stream 데이터를 Dispose 하도록한다.
    // - Finalizer에 의해 Dispose 메서드가 호출되는 경우(Dispose(false)),
    //   Finalizer 스레드에 의해 _stream 객체의 Finalizer가 이미 호출 되었거나, 앞으로 호출 될 수 있기 때문에
    //   _stream의 Dispose를 호출

    
    // * 내생각
    // - 관리되는 지원이 되면, 가비지컬렉터에 의해 언제 자원이 해제될지 모르니 Dispose를 통해 빨리 해제해서 메모리를 관리하자..?
}