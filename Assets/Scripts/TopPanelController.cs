using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanelController : MonoBehaviour
{
    [SerializeField]
    private Text ScoreText;

    [SerializeField]
    private Text SummaryText;

    [SerializeField]
    private Text StartGameText;

    [SerializeField]
    private Slider HealthSlider;

    [SerializeField]
    private Image HitMask;

    private int currentScore = 0;

    private void Start()
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

    private void ShowSummary()
    {
        SummaryText.gameObject.SetActive(true);
        SummaryText.text = string.Format("You're dead!\nScore: {0}", currentScore);
        StartGameText.gameObject.SetActive(true);
    }

    // so lazy
    public void SetGUIState(bool isGameRunning)
    {
        if(isGameRunning)
        {
            HealthSlider.gameObject.SetActive(true);
            ScoreText.gameObject.SetActive(true);
            SummaryText.gameObject.SetActive(false);
            StartGameText.gameObject.SetActive(false);
        }
        else
        {
            HealthSlider.gameObject.SetActive(false);
            ScoreText.gameObject.SetActive(false);
            StartGameText.gameObject.SetActive(true);
            if(currentScore > 0)
                this.ShowSummary();
        }
    }
}
