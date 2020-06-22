using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private Image HitMask;
    [SerializeField] private TopPanelController TopPanel;
    [SerializeField] private BottomPanelController BottomPanel;

    public void SetGUIState(bool isGameRunning)
    {
        if(isGameRunning)
        {
            TopPanel.SetEnabled(false);
            BottomPanel.SetEnabled(true);
        }
        else
        {
            TopPanel.SetEnabled(true);
            BottomPanel.SetEnabled(false);
        }
    }
}
