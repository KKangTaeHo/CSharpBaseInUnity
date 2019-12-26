using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SerializationEx : MonoBehaviour
{
    // 직렬화 - 일련의 바이트 배열로 변환하는 작업
    // 역직렬화 - 그 바이트로부터 원래의 데이터를 복원하는 작업 (이름은 그럴듯해 보이지만 별거아님)

    // 1. MemoryStream
    // MemoryStream은 Stream 추상 클래스를 상속 받는 타입이다. 여기서 Stream 타입은 일련의 바이트를 일관성있게 다루는 공통 기반을 제공한다.
    // 'Stream'은 프로그래밍에서 사용될 때, 일반적으로 '바이트 데이터의 흐름'을 위미한다.
    // MemoryStream타입은 이름 그대로 '메모리에 바이트 데이터를 순서대로 읽고 쓰는 작업을 수행하는 클래스'임

    private void Start()
    {
        MemoryStreamEx();
    }

    private void MemoryStreamEx()
    {
        byte[] shortBytes = BitConverter.GetBytes((short)12345);
        byte[] intBytes = BitConverter.GetBytes(654321);

        MemoryStream ms = new MemoryStream();
        ms.Write(shortBytes, 0, shortBytes.Length);  // 12345 출력
        ms.Write(intBytes, 0, intBytes.Length);      // 654321  출력

        ms.Position = 0;    // 데이터를 읽고 쓸 포지션을 다시 0으로 맞춤

        // 데이터를 다시 역직렬화
        byte[] outByte = new byte[2];
        ms.Read(outByte, 0, 2);
        int shortResult = BitConverter.ToInt16(outByte, 0);
        Debug.Log(shortResult);

        outByte = new byte[4];
        ms.Read(outByte, 0, 4);
        shortResult = BitConverter.ToInt32(outByte, 0);
        Debug.Log(shortResult);

        ms.Position = 0;

        // 위의 역직렬화방식을 ToArray를 통해 해결 할 수 있음
        byte[] buf = ms.ToArray();
        Debug.Log(BitConverter.ToInt16(buf, 0));
        Debug.Log(BitConverter.ToInt32(buf, 2));    // Stream으로 읽지 않을 경우엔 Position 기능이 없으므로 직접 해주어야한다.
    }
}
