using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    private Image HitMask;

    [SerializeField]
    private RectTransform TopPanel;

    [SerializeField]
    private RectTransform BottomPanel;

    public void SetGUIState(bool isGameRunning)
    {
        if(isGameRunning)
        {
            TopPanel.gameObject.SetActive(false);
            BottomPanel.gameObject.SetActive(true);
        }
        else
        {
            TopPanel.gameObject.SetActive(true);
            BottomPanel.gameObject.SetActive(false);
        }
    }
}
