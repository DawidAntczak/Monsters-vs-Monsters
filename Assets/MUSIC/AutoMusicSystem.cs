using MusicInterface;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class AutoMusicSystem : MonoBehaviour
{
    [SerializeField] private bool useDebugProperties = false;
    [SerializeField] private double[] _mode = new double[3];
    [SerializeField] private double[] _attackDensity = new double[6];
    [SerializeField] private double[] _avgPitchesPlayed = new double[3];
    [SerializeField] private double[] _entropy = new double[3];
    [SerializeField] private bool _reset = false;
    [SerializeField] private int _requestedEventCount = 100;

    private MusicReceiver _musicReceiver;
    private MusicPlayer _musicPlayer;

    private bool _playing = false;

    public static AutoMusicSystem Instance { get; set; }

    public void StartPlaying()
    {
        if (_playing)
            return;

        Debug.Log("Starting music player in background");
        Instance._musicReceiver.StartListening(Instance._musicPlayer.EnqueueAndRequestNext);
        Instance._musicPlayer.StartInBackground();
        _playing = true;
    }

    public void StopPlaying()
    {
        if (!_playing)
            return;

        Debug.Log("Stopping music player");
        Instance._musicPlayer.Stop();
        Instance._musicReceiver.StopListening();
        _playing = false;
    }

    void OnApplicationQuit()
    {
        StopPlaying();
    }

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
        if (LevelConfig.Instance)
        {
            LevelConfig.Instance.WinLoseManager.WonEvent += WinLoseManager_WonEvent;
            LevelConfig.Instance.WinLoseManager.LostEvent += WinLoseManager_LostEvent;
        }
    }

    private void WinLoseManager_WonEvent(object sender)
    {
        _musicPlayer.SkipToNext();
    }

    private void WinLoseManager_LostEvent(object sender)
    {
        _musicPlayer.SkipToNext();
    }

    void Start()
    {
        _musicReceiver = new MusicReceiver(new WsClient("ws://localhost:7890/listener"), LogDispatcher.Instance.Log);
        _musicPlayer = new MusicPlayer(_musicReceiver, CollectControlData, LogDispatcher.Instance.Log, LogDispatcher.Instance.Log);
    }

    private ControlData CollectControlData()
    {
        if (useDebugProperties)
        {
            return CollectDebugControlData();
        }

        var gameState = GameStateCalculator.Instance.CalculateGameState();
        var controlData = GameStateToControlsConverter.Convert(gameState);
        return controlData;
    }

    private ControlData CollectDebugControlData()
    {
        var inputData = new ControlData
        {
            Mode = Vector.FromArray(_mode),
            AttackDensity = Vector.FromArray(_attackDensity),
            AvgPitchesPlayed = Vector.FromArray(_avgPitchesPlayed),
            Entropy = Vector.FromArray(_entropy),
            Reset = _reset,
            RequestedEventCount = _requestedEventCount
        };

        _reset = false;

        return inputData;
    }
}
