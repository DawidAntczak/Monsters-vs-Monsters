using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Text))]
public class SunCountDisplay : MonoBehaviour
{
    public enum Status { SUCCESS, FAILURE }

    public static SunCountDisplay Instance { get; private set; }
    [SerializeField] private int startSun = 50;
    private Text starsCounter;
    public int Stars { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        Stars = startSun;
        starsCounter = GetComponent<Text>();
        starsCounter.text = Stars.ToString();
    }

    public void AddStars(int amount)
    {
        Stars += amount;
        starsCounter.text = Stars.ToString();
    }

    public Status UseStars(int amount)
    {
        if(Stars >= amount)
        {
            Stars -= amount;
            starsCounter.text = Stars.ToString();
            return Status.SUCCESS;
        }

        return Status.FAILURE;
    }
}
