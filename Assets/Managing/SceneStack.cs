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
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    public int GetPreviousSceneIndex()
    {
        return _loadedScenes[_loadedScenes.Count - 2];
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        _loadedScenes.Add(arg1.buildIndex);
    }
}
