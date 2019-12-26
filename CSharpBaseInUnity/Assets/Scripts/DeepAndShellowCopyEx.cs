using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepAndShellowCopyEx : MonoBehaviour
{
    // 깊은복사 vs 얕은복사
    // 객체가 가진 값 형식과 참조 형식의 복제 방식에 따라 깊은복사와 얕은복사로 나뉨

    // 핵심은 '클래스 내부의 맴버가 일반 자료형이 아닌 객체일 경우' 어떤식으로 달라지는지가 핵심이다.

    private void Start()
    {
        ShallowCopyEx();
        DeepCopyEx();
    }

    // 1. 얕은 복사
    // 걍 '단순복사' 라고도 함
    // 객체가 참조타입의 맴버를 가지고 있을 경우 '참조값만 복사'된다.
    private void ShallowCopyEx()
    {
        PS4 myPs4 = new PS4();
        myPs4.name = "내 플스";
        myPs4.age = 2017;
        myPs4.game = new Game();
        myPs4.game.titleName = "Last of us";

        // 얕은 복사
        // 해당 객체 내부의 맴버는 참조값(해당 데이터가 있는 위치? 라고 생각하면 좋을듯)만 복사된다.
        // 즉, 맴버 객체의 데이터가 공유됨
        PS4 youPs4 = myPs4.ShallowCopy() as PS4;
        youPs4.name = "너의 플스";
        youPs4.age = 2019;
        youPs4.game.titleName = "loco loco";

        // game.titleName 이 동일한 값이 나오는 것을 알 수 있음
        Debug.Log($"{nameof(myPs4.name)} : {myPs4.name}, {nameof(myPs4.age)} : {myPs4.age}, {nameof(myPs4.game.titleName)} : {myPs4.game.titleName}");
        Debug.Log($"{nameof(youPs4.name)} : {youPs4.name}, {nameof(youPs4.age)} : {youPs4.age}, {nameof(youPs4.game.titleName)} : {youPs4.game.titleName}");
    }

    // 2. 깊은 복사
    // '전체복사' 라고 불림
    // 객체가 참조타입의 멤버를 포함할 경우 참조값의 복사가 아닌 참조된 객체 자체가 복사되는 것

    // ICloneable 인터페이스
    // 닷넷 프레임워크에서는 깊은복사를 위해 ICloneable 인터페이스를 제공함.
    private void DeepCopyEx()
    {
        PS4 myPs4 = new PS4();
        myPs4.name = "내 플스";
        myPs4.age = 2017;
        myPs4.game = new Game();
        myPs4.game.titleName = "Last of us";

        PS4 youPs4 = (PS4)myPs4.Clone();
        youPs4.name = "너의 플스";
        youPs4.age = 2019;
        youPs4.game.titleName = "loco loco";

        // game.titleName 이 동일한 값이 나오는 것을 알 수 있음
        Debug.Log($"{nameof(myPs4.name)} : {myPs4.name}, {nameof(myPs4.age)} : {myPs4.age}, {nameof(myPs4.game.titleName)} : {myPs4.game.titleName}");
        Debug.Log($"{nameof(youPs4.name)} : {youPs4.name}, {nameof(youPs4.age)} : {youPs4.age}, {nameof(youPs4.game.titleName)} : {youPs4.game.titleName}");
    }
}

public class PS4 : ICloneable
{
    public string name { get; set; }
    public int age { get; set; }
    public Game game { get; set; }

    public object ShallowCopy() => MemberwiseClone();   // 앝은복사 Clone
    public object Clone()
    {
        PS4 newPs4 = new PS4();
        newPs4.name = name;
        newPs4.age = age;
        newPs4.game = new Game();
        newPs4.game.titleName = game.titleName;
        return newPs4;
    }
}

public class Game
{
    public string titleName { get; set; }
}
