using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelText : MonoBehaviour
{
    void Start()
    {
        transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = (SceneManager.GetActiveScene().buildIndex - 3).ToString();
    }
}
