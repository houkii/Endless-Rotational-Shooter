using System.Collections;
using UnityEngine;

public class SlidingEnemy : Enemy
{
    public float MovementTime = 5.0f;

    protected override IEnumerator MoveTo(Vector3 targetPosition)
    {
        float currentTime = 0f;
        Vector3 startPosition = transform.position;

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.Clamp01(currentTime / this.MovementTime));
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
