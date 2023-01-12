using MusicInterface;
using UnityEngine;
using UnityEngine.Assertions;

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

    private static AutoMusicSystem Instance { get; set; }

    public static void StartPlaying()
    {
        Debug.Log("Starting music player in background");
        Instance._musicReceiver.StartListening(Instance._musicPlayer.EnqueueAndRequest);
        Instance._musicPlayer.StartInBackground();
    }

    public static void StopPlaying()
    {
        Debug.Log("Stopping music player");
        Instance._musicPlayer.Stop();
        Instance._musicReceiver.StopListening();
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
    }

    void Start()
    {
        _musicReceiver = new MusicReceiver(new WsClient("ws://localhost:7890/listener"));
        _musicPlayer = new MusicPlayer(_musicReceiver, CollectControlData);
    }

    private ControlData CollectControlData()
    {
        if (useDebugProperties)
        {
            return CollectDebugControlData();
        }

        var gameStateCalculator = FindObjectOfType<GameStateCalculator>();
        Assert.IsNotNull(gameStateCalculator);

        var gameState = gameStateCalculator.CalculateGameState();
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
