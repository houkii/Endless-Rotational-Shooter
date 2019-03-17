using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanelController : MoveableUIElement
{
    [SerializeField]
    private Text ScoreText;

    [SerializeField]
    private Slider HealthSlider;

    private int currentScore = 0;

    void Start()
    {
        PlayerController.Instance.OnScoreChanged += UpdateScore;
        PlayerController.Instance.OnHealthChanged += UpdateHPSlider;
    }

    private void UpdateScore(int value)
    {
        currentScore = value;
        ScoreText.text = string.Format("Score: {0}", value);
    }

    private void UpdateHPSlider(int value)
    {
        HealthSlider.value = value;
    }
}
