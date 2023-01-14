using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseManager : MonoBehaviour
{

    public delegate void WonEventHandler(object sender);
    public delegate void LostEventHandler(object sender);

    public event WonEventHandler PreWonEvent;
    public event WonEventHandler WonEvent;
    public event LostEventHandler PreLostEvent;
    public event LostEventHandler LostEvent;

    [SerializeField] protected GameObject winLabel;
    [SerializeField] protected float timeToLoad = 5f;
    //[SerializeField] protected string winScene = "03a Win";
    [SerializeField] protected string loseScene = "03b Lose";

    protected AudioSource winAudioSource;
    protected bool levelCompleted = false;


    void Start()
    {
        winAudioSource = GetComponent<AudioSource>();
	}

    public void Win()
    {
        if(levelCompleted)
            { return; }

        levelCompleted = true;
        winAudioSource.Play();
        Instantiate(winLabel, LevelConfig.Instance.LevelCanvasTransform);

        PreWonEvent?.Invoke(this);
        WonEvent?.Invoke(this);

        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(timeToLoad);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Lose()
    {
        if (levelCompleted)
            { return; }

        PreLostEvent?.Invoke(this);
        LostEvent?.Invoke(this);

        SceneManager.LoadScene(loseScene);
    }
}
