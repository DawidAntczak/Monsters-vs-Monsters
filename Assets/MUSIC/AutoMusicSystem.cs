using MusicInterface;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoMusicSystem : MonoBehaviour
{
    [SerializeField] private string _wsServerAddress = "ws://localhost:7890/listener";
    [SerializeField] private bool useDebugProperties = false;
    [SerializeField] private double[] _mode = new double[3];
    [SerializeField] private double[] _attackDensity = new double[6];
    [SerializeField] private double[] _avgPitchesPlayed = new double[3];
    [SerializeField] private double[] _entropy = new double[3];
    [SerializeField] private bool _reset = false;
    [SerializeField] private float _temperature = 1.5f;
    [SerializeField] private float _requestedTimeLength = 5f;
    [SerializeField] private bool _interpolate = true;
    [SerializeField][Range(0, 127)] private int _defaultVelocity = 127;
    [SerializeField][Range(-12, 12)] private int _keyAdjustmentInSemitones = 0;
    [SerializeField] [Range(0, 127)] private int _instrument = 1;
    
    private MusicGenerationModel _musicGenerationModel;
    private MusicPlayer _musicPlayer;

    private Task _musicPlayerBackgroundTask;
    private bool _playing = false;

    private GameStateToControlsConverter _gameStateToControlsConverter;

    public static AutoMusicSystem Instance { get; set; }

    public void StartPlaying(GameStateToControlsConverter gameStateToControlsConverter = null)
    {
        if (_playing)
            return;

        Debug.Log("Starting music player in background");
        _gameStateToControlsConverter = gameStateToControlsConverter == null ? new GameStateToControlsConverter(_interpolate) : gameStateToControlsConverter;
        _musicGenerationModel.Connect(Instance._musicPlayer.EnqueueAndRequestNext);
        _musicPlayerBackgroundTask = _musicPlayer.StartInBackground();
        _playing = true;
    }

    public void StopPlaying()
    {
        if (!_playing)
            return;

        Debug.Log("Stopping music player");
        _gameStateToControlsConverter = null;
        _musicPlayer.Stop();
        _musicGenerationModel.Disconnect();
        _musicPlayerBackgroundTask.GetAwaiter().GetResult();
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
        _musicGenerationModel = new MusicGenerationModel(_wsServerAddress, LogDispatcher.Instance.Log);
        _musicPlayer = new MusicPlayer(_musicGenerationModel, CollectControlData, CollectPlayingParams, LogDispatcher.Instance.Log, LogDispatcher.Instance.Log);
    }

    private ControlData CollectControlData()
    {
        if (useDebugProperties)
        {
            return CollectDebugControlData();
        }

        var gameState = GameStateCalculator.Instance.CalculateGameState();
        var controLData = _gameStateToControlsConverter.Convert(gameState, _temperature, _requestedTimeLength);
        return controLData;
    }

    private PlayingParams CollectPlayingParams()
    {
        return new PlayingParams
        {
            KeyAdjustmentInSemitones = _keyAdjustmentInSemitones,
            Velocity = _defaultVelocity,
            Instrument = _instrument
        };
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
            RequestedTimeLength = _requestedTimeLength
        };

        _reset = false;

        return inputData;
    }
}
