using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public static bool isPaused = false;

    private Player player;
    private static GameObject pauseOverlay;
    private static GameObject deathOverlay;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        pauseOverlay = FindObjectOfType<Canvas>().gameObject.transform.Find("Pause Overlay").gameObject;
        pauseOverlay.SetActive(false);
        deathOverlay = FindObjectOfType<Canvas>().gameObject.transform.Find("Death Overlay").gameObject;
        deathOverlay.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && player.isAlive)
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ContinueGame();
            }
        }
    }

    public static void ContinueGame()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
        if (pauseOverlay)
        {
            pauseOverlay.SetActive(false);
        }
    }

    public static void PauseGame()
    {
        Time.timeScale = 0.0f;
        isPaused = true;
        if (pauseOverlay)
        {
            pauseOverlay.SetActive(true);
        }
    }

    //ДОБАВИТЬ ЗАДЕРЖКУ ПЕРЕД СМЕРТЬЮ
    public static void GameOver()
    {
        Time.timeScale = 0.0f;
        isPaused = true;
        deathOverlay.SetActive(true);
    }


}
