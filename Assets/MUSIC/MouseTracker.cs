using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseTracker : MonoBehaviour
{
    public static MouseTracker Instance;

    [SerializeField] private float _timeWindow = 5f;

    private readonly LinkedList<(float, Vector2)> _mousePositionsInTime = new();
    private readonly LinkedList<float> _clicks = new();

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

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        _mousePositionsInTime.Clear();
    }

    void Update()
    {
        _mousePositionsInTime.AddLast((Time.time, GetMouseScreenPosition()));
        while (_mousePositionsInTime.Any() && _mousePositionsInTime.First.Value.Item1 < Time.time - _timeWindow)
        {
            _mousePositionsInTime.RemoveFirst();
        }

        if (Input.GetMouseButtonDown(0))
        {
            _clicks.AddLast(Time.time);
        }
        while (_clicks.Any() && _clicks.First.Value < Time.time - _timeWindow)
        {
            _clicks.RemoveFirst();
        }
    }

    public float GetAverageSpeed()
    {
        if (!_mousePositionsInTime.Any())
            return 0f;

        var wToHFactor = Screen.width / Screen.height;

        var distanceSum = 0f;
        var timeSum = 0f;

        var lastPositionInTime = _mousePositionsInTime.First.Value;
        foreach (var positionInTime in _mousePositionsInTime)
        {
            distanceSum += new Vector2(
                (positionInTime.Item2.x - lastPositionInTime.Item2.x) * wToHFactor,
                (positionInTime.Item2.y - lastPositionInTime.Item2.y) / wToHFactor
                ).magnitude;
            timeSum += positionInTime.Item1 - lastPositionInTime.Item1;
            lastPositionInTime = positionInTime;
        }

        return timeSum > 0 ? distanceSum / timeSum : 0f;
    }

    public float GetAverageClicks()
    {
        if (!_clicks.Any())
            return 0f;

        var timeSum = _clicks.Last.Value - _clicks.First.Value;
        return timeSum > 0 ? _clicks.Count() / timeSum : 0f;
    }


    private Vector2 GetMouseScreenPosition()
    {
        return Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }
}
