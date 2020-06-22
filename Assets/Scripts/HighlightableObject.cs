using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(EventTrigger))]
public class HighlightableObject : MonoBehaviour
{
    public Material HighlightedMaterial;

    private Material DefaultMaterial;
    private EventTrigger eventTrigger;
    private Renderer renderer;

    private void Awake()
    {
        eventTrigger = GetComponent<EventTrigger>();
        renderer = GetComponent<Renderer>();
        DefaultMaterial = renderer.sharedMaterial;
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((eventData) => SwitchToHighlited());
        eventTrigger.triggers.Add(pointerEnterEntry);

        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((eventData) => SwitchToDefault());
        eventTrigger.triggers.Add(pointerExitEntry);
    }

    private void SwitchToHighlited()
    {
        renderer.sharedMaterial = HighlightedMaterial;
    }

    private void SwitchToDefault()
    {
        renderer.sharedMaterial = DefaultMaterial;
    }
}
