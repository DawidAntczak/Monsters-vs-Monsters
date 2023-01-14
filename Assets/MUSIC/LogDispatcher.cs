using System;
using System.Collections.Concurrent;
using UnityEngine;

public class LogDispatcher : MonoBehaviour
{
    public static LogDispatcher Instance;

    private readonly ConcurrentQueue<string> _logs = new();
    private readonly ConcurrentQueue<Exception> _exceptions = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        while (_logs.TryDequeue(out var log))
        {
            Debug.Log(log);
        }
        while (_exceptions.TryDequeue(out var exception))
        {
            Debug.LogException(exception);
        }
    }

    public void Log(string log)
    {
        _logs.Enqueue(log);
    }

    public void Log(Exception exception)
    {
        _exceptions.Enqueue(exception);
    }
}
