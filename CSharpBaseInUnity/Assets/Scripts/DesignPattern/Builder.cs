using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 빌더 패턴
// - 목적에 따라 개체를 생성하는 과정의 약속과 구체적인 알고리즘 구현을 분리하여 개체를 생성하는 패턴(뭔말..?)
//   >> 생성과정 & 알고리즘을 분리해서 만드는 패턴이라는 뜻인듯...
// - 빌더 패턴은 `추상 클래스`로 개체를 생성하는 과정을 약속함.
//   >> `생성하는 과정?` 이게 뭔뜻인지 확실히 알아야함..
// - 빌더를 기반으로 파생한 클래스에서는 `생성 과정`들에 대해 구체적으로 구현함.

// - 핵심은!! 생성과 표현의 분리
// - 

// - 빌더 패턴도 새로운 객체를 만들어서 반환하는 패턴이긴 하지만 동작방식은 추상 팩토리패턴과 다르다

// 디자인
// - 카메라, 사진, 사진빌더 로 빌더페턴에 대한 예를들음
// - 1. Camera는 디렉터 역활로
// - 2. Picture는 카메라를 통해 사용자가 얻고자 하는 개체임
// - 3. PictureBuilder는 '빛을 모아서 피사체를 상으로 맺히는 기능과 이미지를 수정하는 기능'을 구현하고, 추상클래스롤 제작

// Step 1. 사진 정의
// - 피사체와 이에 대한 이미지를 갖는 형식으로 정의

public class Picture
{
    public static readonly Picture EmptyPicture;

    static Picture() => EmptyPicture = new Picture(string.Empty, string.Empty);    // 정적 생성자

    public string Subject { get; private set; }
    public string Image { get; private set; }

    public Picture(string inSubject, string inImage)
    {
        Subject = inSubject;
        Image = inImage;
    }
}


// Step 2. 사진 빌더
// - 빛을 모아 피사체를 이미지로 맺히는 역할, 수정작업을 수행하는 역할을 구현
public abstract class PictureBuilder
{
    public Picture Picture { get; protected set; }

    public abstract void SetSubject(string inSubject);    // 상을 맺히는 기능
    public abstract void Change();                      // 이미지 수정
}

// - 상속 받은 두개의 빌더에는 자신의 고유의 기능들을 구체적으로 구현해야함.

public class SmoothBuilder : PictureBuilder
{
    const string sharp = "Sharp";
    const string smooth = "Smooth";

    string subject;

    public override void Change()
    {
        string image = subject.Replace(sharp, smooth);
        Picture = new Picture(subject, image);
    }

    public override void SetSubject(string inSubject)
    {
        subject = inSubject;
        Picture = new Picture(subject, inSubject);
    }
}

public class REPreventBuilder : PictureBuilder
{
    const string red_eye = "RedEye";
    const string normal_eye = "NomalEye";
    const int max_length = 20;
    string subject;

    public override void Change()
    {
        string image = subject.Replace(red_eye, normal_eye);
        Picture = new Picture(subject, image);
    }

    public override void SetSubject(string inSubject)
    {
        if (inSubject.Length > max_length)
            inSubject = inSubject.Substring(0, max_length);

        subject = inSubject;
    }
}

// Step 3. 빌더 카메라 생성
// - 셔터를 누르면 사진이 생성되도록 구현
// - 피사체가 낮인지 밤인지에 따라 빌더를 다르게 사용하도록 구현
public class BuilderCamera
{
    PictureBuilder[] builders = new PictureBuilder[2];
    public Picture MyPicture
    {
        get;
        private set;
    }
    public BuilderCamera()
    {
        builders[0] = new REPreventBuilder();
        builders[1] = new SmoothBuilder();
        MyPicture = Picture.EmptyPicture;
    }
    public void PressAShutter(string origin, bool night)
    {
        PictureBuilder pb = null;
        if (night) //밤일 때
        {
            pb = builders[0];
        }
        else//밤이 아닐 때
        {
            pb = builders[1];
        }

        pb.SetSubject(origin); //상을 맺힌다.
        pb.Change(); //이미지를 수정한다.
        MyPicture = pb.Picture;
    }
}

public class Builder : MonoBehaviour
{
    private void Start()
    {
        BuilderCamera camera = new BuilderCamera();
        camera.PressAShutter("길 마당 RedEyeSharpBody 집 산 ", true);
        Debug.Log(camera.MyPicture.Image);
        camera.PressAShutter("길 마당 RedEyeSharpBody 집 산", false);        // 뭔가 같은 메서드를 사용하나, 뒤의 매개변수에 따라 구체적으로 정의해 놓았던 빌더에 의해 값이 변경
        Debug.Log(camera.MyPicture.Image);
    }
}

// 위에있는 내용으로는 빌더패턴이 뭔지 하나도 몰라서 다른곳에서 좀 찾아봤다.
// 빌더 패턴
// - 인스턴스를 생성 할 때, 생성자만을 통해서 생성할때 발생하는 어려움을 극복하기 위해서 고안된 패턴인것 같다.
// - 생성자의 매개변수를 통해 데이터를 넘길 때 1,2개일 경우엔 괜찮지만.. 그것들이 무수히 많아지면 굉장히 읽기 어려워지는 문제점이 발생한다.
// 이런 것을 빌더패턴이라고 한다는데,,, 뭐가 맞는건지 모르겠다(요건 다른의미에서 빌더패턴인듯)
// - 불필요한 생성자를 만들 필요없이 계속해서 만드는 패턴이라는데,,, 잘모르겟다
// - 대부분의 사람들이 `생성과 표현`의 분리라고 하면서 생성자를 유동적으로 사용하는 패턴의 형태로 설명을 하는데..
//   위의 예제가 왜 빌더패턴인지 모르겠다..
// - 아!! 자바의 빌더패턴과 GoF의 빌더패턴의 의미가 다르네,,, 지금 위에껀 자바의 빌더패턴
