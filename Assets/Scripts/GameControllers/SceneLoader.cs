using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float timeToWait = 3f;


    private void Start()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0) StartCoroutine(StartMenu());
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options Scene");
    }

    public void LoadFirstLevel()
    {
        if (FindObjectOfType<ScoreController>())
        {
            DifficultyController.difficulty = DifficultyController.defaultDifficulty;
            ScoreController.score = 0;           
            //Destroy(FindObjectOfType<ModifiersController>());
            //Effects.ResetVars();
        }
        GamePause.ContinueGame();
        SceneManager.LoadScene("Level 0");
    }

    public static void LoadFLevel()
    {
        if (FindObjectOfType<ScoreController>())
        {
            DifficultyController.difficulty = DifficultyController.defaultDifficulty;
            ScoreController.score = 0;
        }
        GamePause.ContinueGame();
        SceneManager.LoadScene("Level 1");
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadManualNextScene()
    {
        if (FindObjectOfType<ScoreController>())
        {
            DifficultyController.difficulty = DifficultyController.defaultDifficulty;
            ScoreController.score = 0;
        }
        if(FindObjectOfType<Effects>()) Destroy(FindObjectOfType<Effects>());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void LoadNScene()
    {
        if (SceneManager.GetActiveScene().name == "Level 0")
        {

            if (FindObjectOfType<ScoreController>())
            {
                DifficultyController.difficulty = DifficultyController.defaultDifficulty;
                ScoreController.score = 0;
            }
            if (FindObjectOfType<Effects>()) Destroy(FindObjectOfType<Effects>());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Effects.instance = null;
        }
    }

    public void LoadLoseScreen()
    {
        SceneManager.LoadScene("Lose Scene");
    }

    IEnumerator StartMenu()
    {
        yield return new WaitForSeconds(timeToWait);
        LoadNextScene();
    }
}
