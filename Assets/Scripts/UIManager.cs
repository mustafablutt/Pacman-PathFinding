using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI scoreText, levelText, lifeText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateUI()
    {
        scoreText.text = "SCORE : " + GameManager.instance.ReadScore();
        levelText.text = "LEVEL : " + GameManager.instance.ReadLevel();
        lifeText.text = "LİFE : " + GameManager.instance.ReadLife();

    }

}
