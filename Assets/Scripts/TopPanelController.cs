using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanelController : MonoBehaviour
{
    [SerializeField]
    private Text SummaryText;

    [SerializeField]
    private Text StartGameText;

    private void OnEnable()
    {
        StartGameText.gameObject.SetActive(true);

        if (PlayerController.Instance.Score > 0)
            ShowSummary();
        else
            SummaryText.gameObject.SetActive(false);
    }

    private void ShowSummary()
    {
        SummaryText.gameObject.SetActive(true);
        SummaryText.text = string.Format("You're dead!\nScore: {0}", PlayerController.Instance.Score); 
    }
}
