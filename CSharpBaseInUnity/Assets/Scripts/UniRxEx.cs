using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class UniRxEx : MonoBehaviour
{
    public enum EState
    {
        NONE,
        AT_START,
        AT_ATTENDANCE_END,
        AT_REWARD,
        AT_END,
    }

    public struct Message
    {
        public int id;
        public EState state;

        public Message(int inId, EState inState)
        {
            id = inId;
            state = inState;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MessageBrokerEx02();
        MessageBrokerEx02();
        MessageBrokerEx02();
        MessageBrokerEx02();
        MessageBrokerEx02();
        MessageBroker.Default.Publish<Message>(new Message { id = 15, state = EState.AT_END });
    }

    void MeesageBrokerEx01()
    {
        MessageBroker.Default.Receive<Message>().Subscribe(x => Debug.Log("첫번째 로그")).AddTo(this);

        MessageBroker.Default.Publish<Message>(new Message { id = 1, state = EState.AT_ATTENDANCE_END });

        MessageBroker.Default.Receive<Message>().Subscribe(x => Debug.Log("두번째 로그")).AddTo(this);

        MessageBroker.Default.Publish<Message>(new Message { id = 15, state = EState.AT_END });
    }

    IDisposable _dispose;
    int _count = 0;
    void MessageBrokerEx02()
    {
        _dispose?.Dispose();
        _dispose = MessageBroker.Default.Receive<Message>().Subscribe(x => Debug.Log($"첫번째 로그 호출 : {_count++}"));

    }
}


public class MessageBroker : IMessageBroker, IDisposable
{
    /// <summary>
    /// MessageBroker in Global scope.
    /// </summary>
    public static readonly IMessageBroker Default = new MessageBroker();

    bool isDisposed = false;
    readonly Dictionary<Type, object> notifiers = new Dictionary<Type, object>();

    public void Publish<T>(T message)
    {
        object notifier;
        lock (notifiers)
        {
            if (isDisposed) return;

            if (!notifiers.TryGetValue(typeof(T), out notifier))
            {
                return;
            }
        }
        ((ISubject<T>)notifier).OnNext(message);
    }

    public IObservable<T> Receive<T>()
    {
        object notifier;
        lock (notifiers)
        {
            if (isDisposed) throw new ObjectDisposedException("MessageBroker");

            if (!notifiers.TryGetValue(typeof(T), out notifier))
            {
                ISubject<T> n = new Subject<T>().Synchronize();
                notifier = n;
                notifiers.Add(typeof(T), notifier);
            }
        }

        return ((IObservable<T>)notifier).AsObservable();
    }

    public void Dispose()
    {
        lock (notifiers)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                notifiers.Clear();
            }
        }
    }
}