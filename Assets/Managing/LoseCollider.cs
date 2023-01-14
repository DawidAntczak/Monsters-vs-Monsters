using UnityEngine;

public class LoseCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        LevelConfig.Instance.WinLoseManager.Lose();
    }
}
