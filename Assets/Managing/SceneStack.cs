using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStack : MonoBehaviour
{
    private List<int> _loadedScenes = new();

    public static SceneStack Instance { get; private set; }

    private void Awake()
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

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _loadedScenes.Add(arg0.buildIndex);
    }

    public int GetPreviousSceneIndex()
    {
        return _loadedScenes[_loadedScenes.Count - 2];
    }
}
