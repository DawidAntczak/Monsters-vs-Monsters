using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkMushroom : MonoBehaviour
{
    private bool wasEaten = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(!wasEaten)
        {
            Wololo(collision.gameObject);
            wasEaten = true;
            Destroy(gameObject);
        }
    }

    private static void Wololo(GameObject obj)
    {
        obj.layer = Layers.defenderLayer;
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.eulerAngles = new Vector3(rectTransform.eulerAngles.x, rectTransform.eulerAngles.y - 180f, rectTransform.eulerAngles.z);
        obj.transform.SetParent(ObjectsOrganizer.Instance.DefenderParent);
    }
}
