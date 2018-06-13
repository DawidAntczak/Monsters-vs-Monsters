using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefsManager : MonoBehaviour
{

    const string MASTER_VOLUME_KEY = "master_volume";
    const string DIFFICULTY_KEY = "difficulty";
    const string LEVEL_KEY = "level_unlocked_";

    public static void SetMasterVolume(float volume)
    {
        if (volume >= 0f && volume <= 1f)
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Master volume out of range!");
        }
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }

    public static void UnlockLevel(int level)
    {
        if(level < Application.levelCount)
        {
            PlayerPrefs.SetInt(LEVEL_KEY + level.ToString(), 1);            //1 for true
        }
        else
        {
            Debug.LogError("Trying to unlock level out of build settings!");
        }
    }

    public static bool IsLevelUnlocked(int level)
    {
        if(level > SceneManager.sceneCountInBuildSettings - 2)
        {
            Debug.LogError("Level out of build settings!");
            return false;
        }
        int levelValue = PlayerPrefs.GetInt(LEVEL_KEY + level.ToString());
        return levelValue == 1;
    }

    public static void SetDifficulty(float difficulty)
    {
        if(difficulty < 1f || difficulty > 3f)
        {
            Debug.LogError("Trying to set difficulty out of range!");
        }
        else
        {
            PlayerPrefs.SetFloat(DIFFICULTY_KEY, difficulty);
        }
    }

    public static float GetDifficulty()
    {
        return PlayerPrefs.GetFloat(DIFFICULTY_KEY);
    }
}
