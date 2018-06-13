using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Text))]
public class SunCountDisplay : MonoBehaviour
{
    public enum Status { SUCCESS, FAILURE }

    public static SunCountDisplay Instance { get; private set; }
    [SerializeField] private int startSun = 50;
    private Text starsCounter;
    private int stars;

    private void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        stars = startSun;
        starsCounter = GetComponent<Text>();
        starsCounter.text = stars.ToString();
    }

    public void AddStars(int amount)
    {
        stars += amount;
        starsCounter.text = stars.ToString();
    }

    public Status UseStars(int amount)
    {
        if(stars >= amount)
        {
            stars -= amount;
            starsCounter.text = stars.ToString();
            return Status.SUCCESS;
        }

        return Status.FAILURE;
    }
}
