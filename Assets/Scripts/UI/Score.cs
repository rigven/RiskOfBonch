using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    [SerializeField] public bool isGameOverScreen = false;
    int score = 0;

    void Start()
    {
        scoreText = gameObject.transform.Find("Score Points Text").gameObject.GetComponent<TextMeshProUGUI>();
        score = ScoreController.score;
        if (isGameOverScreen)
            UpdateGameOverDisplay();
        else
            UpdateDisplay();
    }

    private void Update()
    {
        if (ScoreController.score != score)
        {
            score = ScoreController.score;
            if (isGameOverScreen)
                UpdateGameOverDisplay();
            else
                UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        scoreText.text = score.ToString();
    }

    private void UpdateGameOverDisplay()
    {
        scoreText.text = "Ваши очки: " + score.ToString();
    }

}
