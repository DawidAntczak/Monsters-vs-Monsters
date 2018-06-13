using UnityEngine;

public class Fence : MonoBehaviour
{
    private Animator animator;

	void Start ()
    {
        animator = GetComponent<Animator>();
	}

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Movable>())
        {
            animator.SetTrigger("is attacked trigger");
        }
    }

}
