using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour {

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider difficultySlider;

    void Start ()
    {
        volumeSlider.value = PlayerPrefsManager.GetMasterVolume();
        difficultySlider.value = PlayerPrefsManager.GetDifficulty();
	}
	
    public void SaveAndExit()
    {
        PlayerPrefsManager.SetMasterVolume(volumeSlider.value);
        PlayerPrefsManager.SetDifficulty(difficultySlider.value);
        SceneManager.LoadScene("01a Start Menu");
    }

    public void SetDefaults()
    {
        volumeSlider.value = 0.5f;
        difficultySlider.value = 0f;
    }
}