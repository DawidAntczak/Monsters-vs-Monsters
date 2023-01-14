using UnityEngine;

public class ScenePlayerTrigger : MonoBehaviour
{
    void Start()
    {
        AutoMusicSystem.Instance?.StartPlaying();
    }
}
