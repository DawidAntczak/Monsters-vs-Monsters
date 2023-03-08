using System;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPanel : MonoBehaviour
{
    public static ErrorPanel Instance { get; private set; }

    [SerializeField] private Text _text;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
        gameObject.SetActive(false);
    }

    public void Show(string errorMessage)
    {
        gameObject.SetActive(true);
        _text.text = errorMessage;
    }

    public void Show(Exception exception)
    {
        gameObject.SetActive(true);
        _text.text = exception.Message;
    }

    public void Hide()
    {
        _text.text = string.Empty;
        gameObject.SetActive(false);
    }
}
