using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MoveableUIElement : MonoBehaviour
{
    [SerializeField]
    private Vector2 EnabledPosition;

    [SerializeField]
    private Vector2 DisabledPosition;

    [SerializeField]
    private AnimationCurve MovementCurve;   // as an easing function

    [SerializeField]
    private float MovementTime = 0.5f;

    private RectTransform RT;
    private Coroutine MovementCoroutine = null;

    protected virtual void Awake()
    {
        RT = GetComponent<RectTransform>();
        RT.anchoredPosition = DisabledPosition;
    }

    public virtual void SetEnabled(bool enabled)
    {
        // no need to turn off an inactive object
        if (!gameObject.activeSelf && !enabled)
            return;

        Vector2 posToMove;
        AnimationCurve curveToUse;
        Action tweenCompletedAction = null;
        
        if (enabled)
        {
            gameObject.SetActive(true);
            posToMove = EnabledPosition;
            curveToUse = MovementCurve;
        }
        else
        {
            posToMove = DisabledPosition;
            curveToUse = reverseCurve(MovementCurve);
            tweenCompletedAction = () => { gameObject.SetActive(false); };
        }

        if(MovementCoroutine != null)
        {
            StopCoroutine(MovementCoroutine);
        }

        MovementCoroutine = StartCoroutine(MoveTo(posToMove, curveToUse, MovementTime, tweenCompletedAction));
    }

    // simple evaluation of animation curve through time to reach desired position
    private IEnumerator MoveTo(Vector2 position, AnimationCurve curve, float moveTime, Action onComplete = null)
    {
        float currentTime = 0;
        Vector2 dir = position - RT.anchoredPosition;
        Vector2 startPos = RT.anchoredPosition;

        while(currentTime <= moveTime)
        {
            float step = Mathf.Clamp01(currentTime / moveTime);
            RT.anchoredPosition = startPos + dir * curve.Evaluate(step);
            currentTime += Time.deltaTime;
            Debug.Log(step);
            yield return null;
        }

        onComplete?.Invoke();

        yield break;
    }

    // reversing animation curve for disabling tweens
    private AnimationCurve reverseCurve(AnimationCurve curve)
    {
        AnimationCurve reversedCurve = new AnimationCurve();
        foreach (Keyframe key in curve.keys)
        {
            Keyframe newKeyframe = key;
            newKeyframe.time = 1 - newKeyframe.time;
            newKeyframe.value = 1 - newKeyframe.value;
            reversedCurve.AddKey(newKeyframe);
        }
        return reversedCurve;
    }
}
