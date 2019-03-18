using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : SlidingEnemy
{
    public AnimationCurve HorizontalFlightCurve;

    // flying enemy should spawn at some height...
    public float SpawnHeight = 10.0f;

    protected override void Awake()
    {
        base.Awake();
        transform.position += new Vector3(0, SpawnHeight, 0);
    }

    protected override IEnumerator MoveTo(Vector3 targetPosition)
    {
        // run base movement coroutine
        StartCoroutine(base.MoveTo(targetPosition));

        float currentCurveTime = 0f;
        while(transform.position != Target)
        {
            // modulate movement function with animation curve
            transform.position += transform.right * HorizontalFlightCurve.Evaluate(currentCurveTime);

            currentCurveTime += Time.deltaTime / MovementTime;

            yield return null;
        }
    }
}
