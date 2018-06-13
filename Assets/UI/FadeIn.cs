using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{

    [SerializeField] private float fadeInTime;
    private Image fadePanel;
    private Color currentColor = Color.black;

	void Start ()
    {
        fadePanel = GetComponent<Image>();
	}
	
	void Update ()
    {
		if(Time.timeSinceLevelLoad < fadeInTime)
        {
            currentColor.a -= (Time.deltaTime / fadeInTime);
            fadePanel.color = currentColor;
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
}
