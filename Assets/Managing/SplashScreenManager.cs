using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{

    [SerializeField] private float autoLoadNextLevelAfter;

    private void Start()
    {
        Invoke("LoadNextLevel", autoLoadNextLevelAfter);
    }

    public void LoadLevel(string name)
    {
        Debug.Log("New Level load: " + name);
        Application.LoadLevel(name);
    }

    public void QuitRequest()
    {
        Debug.Log("Quit requested");
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}