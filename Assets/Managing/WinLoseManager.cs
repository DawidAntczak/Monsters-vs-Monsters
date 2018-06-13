using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseManager : MonoBehaviour
{
    [SerializeField] protected GameObject winLabel;
    [SerializeField] protected string winScene = "03a Win";
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
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        while(winAudioSource.isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene(winScene);
    }

    public void Lose()
    {
        if (levelCompleted)
            { return; }

        SceneManager.LoadScene(loseScene);
    }
}
