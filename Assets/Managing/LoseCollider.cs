using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        print(collider.name + "\t" + collider.transform.position);
        Debug.Break();
        LevelConfig.Instance.WinLoseManager.Lose();
    }
}
