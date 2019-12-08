using System;

// 파일 > 새로만들기 > '클래스 라이브러리' 로 만듬
// ClassLibrar1.dll은 해당프로젝트 폴더 > bin > Debug > netstandard2.0에 있음
// 유니티 Assets 내부에 넣어서 사용하면 됨.
namespace ClassLibrary1
{
    public class Class1
    {
        public static string GetStr()
        {
            return "Hello C# DLL!";
        }

        public Class1()
        {
            Console.WriteLine(typeof(Class1).FullName + "생성됨! (생성자)");
        }
    }
}
