using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ReadScore : MonoBehaviour
{
    public TextMeshProUGUI highScoreText, levelText;
    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = "HIGH SCORE : " + ScoreHolder.score;
        levelText.text = "LEVEL : " + ScoreHolder.level;
    }

}
