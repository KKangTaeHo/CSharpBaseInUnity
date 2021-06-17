using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상팩토리패턴
// - 객체의 집합을 생성 할 때 유용(공통적으로 유사한 객체들을 한번에 만들 때)
// - 구체화된 클래스..? 일반화된 클래스...?
// - 비슷한 개체들 사이에 호환성을 유지할 때 사용
// - '서로 호환성이 있는 개체들을 생성하는 부분만 담당하는 개체를 제공함' >> 그니깐, 개체집단을 만들 때 사용
// - 1. 렌즈는 `ITake`라는 인터페이스를 상속받아 만들고, 카메라는 `Camera`추상클래스를 상속받아 제작한 한다
// - 2. 그 다음, 'IMakeCamera' 라는 인터페이스를 만들어, 카메라생성과 렌즈 생성을 정의하고, 그걸 상속받는 클래스를 만든다.
// >> 어떤 객체를 생성 할때, 반드시 필연적으로 묶여서 나와야 하는 경우가 있다.. 그럴 경우, 추상화된 인터페이스를 정의하고 그 인터페이스 속에서
//    연관 있는 객체들을 한방에 뽑을 때, 추상클래스를 사용한다.

// Step 1. 렌즈에 대한 정의
// 총 2가지로 이루어저 있음
public interface ITake
{
    void Take();    // 렌즈 기능의 추상적 약속
}

public class EvLans : ITake
{
    public void Take() => Debug.Log("부드럽다");
    public void AutoFocus() => Debug.Log("자동초점");
}

public class HoLans : ITake
{
    public void Take() => Debug.Log("자연스럽다.");
    public void ManualFocus() => Debug.Log("사용자의 명령대로 초점을 잡다.");
}

// Step 2. 카메라 정의
public abstract class Camera
{
    protected ITake Lens { get; set; }

    // 촬영
    public virtual bool TakeAPicture()
    {
        if (Lens == null)
            return false;

        Lens.Take();
        return true;
    }

    // 렌즈 장착
    public abstract bool PutInLens(ITake iTake);
    
    // 렌즈 탈착
    public ITake GetOutLens()
    {
        ITake re = Lens;
        Lens = null;
        return re;
    }

    public Camera()
    {
        Lens = null;
    }
}

public class EvCamera : Camera
{
    public override bool PutInLens(ITake iTake)
    {
        EvLans evlens = iTake as EvLans;
        if (evlens == null)
            return false;

        Lens = iTake;
        return true;
    }

    public override bool TakeAPicture()
    {
        if (Lens == null)
            return false;

        (Lens as EvLans).AutoFocus();
        return base.TakeAPicture();
    }
}

public class HoCamera : Camera
{
    public override bool PutInLens(ITake iTake)
    {
        HoLans evlens = iTake as HoLans;
        if (evlens == null)
            return false;

        Lens = iTake;
        return true;
    }

    public override bool TakeAPicture()
    {
        if (Lens == null)
            return false;

        (Lens as HoLans).ManualFocus();
        return base.TakeAPicture();
    }
}

// Step 3. 펙토리 구현
// - 펙토리에서는 카메라와 렌즈를 생산하는 기능을 약속하는 인터페이스를 정의함.
// - 그리고 IMakeCamera를 EvDayFactory, HoDayFactory 클래스를 정의한다.

public interface IMakeCamera
{
    ITake MakeLans();
    Camera MakeCamera();
}

public class EvDayFactory : IMakeCamera
{
    public Camera MakeCamera() => new EvCamera();
    public ITake MakeLans() => new EvLans();
}

public class HoDayFactory : IMakeCamera
{
    public Camera MakeCamera() => new HoCamera();
    public ITake MakeLans() => new HoLans();
}

// Step 4. 펙토리를 통한 객체 생성

public class AbstractFactory : MonoBehaviour
{
    IMakeCamera [] cameraFactory = {new EvDayFactory(), new HoDayFactory()};

    private void CreateCamera<T>() where T :IMakeCamera
    {
        foreach(IMakeCamera factory in cameraFactory)
        {
            if(factory is T)
            {
                factory.MakeCamera().TakeAPicture();
                factory.MakeLans().Take();
                return;
            }
        }
    }

    private void Start()
    {
        CreateCamera<HoDayFactory>();
    }
}
