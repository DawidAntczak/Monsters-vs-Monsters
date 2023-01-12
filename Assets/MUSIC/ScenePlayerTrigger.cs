using UnityEngine;

public class ScenePlayerTrigger : MonoBehaviour
{
    void Start()
    {
        AutoMusicSystem.StartPlaying();
    }

    void OnDestroy()
    {
        AutoMusicSystem.StopPlaying();
    }
}
