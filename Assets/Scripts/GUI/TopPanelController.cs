using UnityEngine;
using UnityEngine.UI;

public class TopPanelController : MoveableUIElement
{
    [SerializeField] private Text SummaryText;
    [SerializeField] private Text StartGameText;

    public override void SetEnabled(bool enabled)
    {
        StartGameText.gameObject.SetActive(true);

        if (PlayerController.Instance.Score > 0)
            ShowSummary();
        else
            SummaryText.gameObject.SetActive(false);

        base.SetEnabled(enabled);
    }

    private void ShowSummary()
    {
        SummaryText.gameObject.SetActive(true);
        SummaryText.text = string.Format("You're dead!\nScore: {0}", PlayerController.Instance.Score); 
    }
}
