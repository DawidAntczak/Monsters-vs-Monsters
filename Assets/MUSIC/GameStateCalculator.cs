using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateCalculator : MonoBehaviour
{
    public static GameStateCalculator Instance;

    private static Thread _mainThread = Thread.CurrentThread;

    private readonly ConcurrentQueue<GameState> _calculatedGameStates = new();
    private int _calculateGameStateRequests = 0;

    private Animator _spawningAnimator;

    private bool _isFirstCalculation = true;

    private GameState.Stage _stage = GameState.Stage.Preparing;


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
        if (arg1.name.Contains("LVL", System.StringComparison.InvariantCultureIgnoreCase) || arg1.name.Contains("Level", System.StringComparison.InvariantCultureIgnoreCase))
        {
            _stage = GameState.Stage.Preparing;
        }
        if (LevelConfig.Instance)
        {
            LevelConfig.Instance.WinLoseManager.PreWonEvent += WinLoseManager_WonEvent;
            LevelConfig.Instance.WinLoseManager.PreLostEvent += WinLoseManager_LostEvent;
            LevelConfig.Instance.GameStarted += LevelConfig_GameStarted;
        }
    }

    private void LevelConfig_GameStarted(object sender)
    {
        _stage = GameState.Stage.Playing;
    }

    private void WinLoseManager_WonEvent(object sender)
    {
        _isFirstCalculation = true;
        _stage = GameState.Stage.Won;
    }

    private void WinLoseManager_LostEvent(object sender)
    {
        _isFirstCalculation = true;
        _stage = GameState.Stage.Lost;
    }

    void Update()
    {
        if (_calculateGameStateRequests > 0)
        {
            var gameState = CalculateGameStateInternal();
            while (_calculateGameStateRequests > 0)
            {
                _calculatedGameStates.Enqueue(gameState);
                _calculateGameStateRequests--;
            }
        }
    }

    public GameState CalculateGameState()
    {
        if (Thread.CurrentThread == _mainThread)
        {
            return CalculateGameStateInternal();
        }

        _calculateGameStateRequests++;

        GameState gameState;
        while (!_calculatedGameStates.TryDequeue(out gameState)) ;

        return gameState;
    }

    private GameState CalculateGameStateInternal()
    {
        var isFirstCalculation = _isFirstCalculation;
        _isFirstCalculation = false;

        var gameState = _stage switch
        {
            GameState.Stage.Won =>
                new GameState { GameStage = GameState.Stage.Won, IsInitState = isFirstCalculation },
            GameState.Stage.Lost =>
                new GameState { GameStage = GameState.Stage.Lost, IsInitState = isFirstCalculation },
            GameState.Stage.Preparing =>
                new GameState { GameStage = GameState.Stage.Preparing, IsInitState = isFirstCalculation },
            GameState.Stage.Playing =>
                PrepareGameStateForPlayingStage()
        };

        Debug.Log(gameState);

        return gameState;
    }

    private GameState PrepareGameStateForPlayingStage()
    {
        var avgMouseSpeed = MouseTracker.Instance.GetAverageSpeed();
        var avgMouseClicks = MouseTracker.Instance.GetAverageClicks();

        var defendersCount = ObjectsOrganizer.Instance.DefenderParent.childCount;
        var sunflowerCount = ObjectsOrganizer.Instance.DefenderParent.GetComponentsInChildren<EvilSunflower>().Length;
        var enemiesCount = ObjectsOrganizer.Instance.AttackerParent.childCount;

        if (_spawningAnimator == null)
        {
            _spawningAnimator = FindObjectOfType<SpawningSystem>().GetComponent<Animator>();
        }
        var levelProgess = _spawningAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        return new GameState
        {
            IsInitState = false,
            GameStage = _stage,
            AverageMouseSpeed = avgMouseSpeed,
            AverageMouseClicks = avgMouseClicks,
            LevelProgess = levelProgess,
            DefendersCount = defendersCount,
            SunflowersCount = sunflowerCount,
            EnemiesCount = enemiesCount
        };
    }
}
