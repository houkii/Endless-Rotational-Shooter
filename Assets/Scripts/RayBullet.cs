using System;
using System.Collections;
using UnityEngine;

public class RayBullet : MonoBehaviour
{
    [SerializeField] private LineRenderer Line;
    [SerializeField] private int shootableMask;
    [SerializeField] private float Range = 20f;
    [SerializeField] private int Damage = 1;
    [SerializeField] private float RayHeadMovementTime = 0.3f;
    [SerializeField] private float RayTailMovementTime = 0.4f;

    // if this is set to zero, ray tail will start to move when head achieves target
    [SerializeField] private float RayTailDelayTime = 0.1f;
    private float LifeTime;
    private Ray Ray;
    private RaycastHit Hit;
    private Action RayAchievedTargetAction = null;

    private void Awake()
    {
        // gun muzzle makes sure orientation is correct...
        Ray.origin = transform.position;
        Ray.direction = transform.forward;
        Line.SetPosition(0, transform.position);

        Vector3 targetRayPosition;
        this.GetRayTarget(out targetRayPosition);
        StartCoroutine(ShootRay(targetRayPosition, RayHeadMovementTime, RayTailMovementTime, RayTailDelayTime, RayAchievedTargetAction));

        LifeTime = RayHeadMovementTime + RayTailMovementTime;
        Destroy(this.gameObject, LifeTime);

        this.PlaySound();
    }

    private void GetRayTarget(out Vector3 target)
    {
        if (Physics.Raycast(Ray, out Hit, Range))
        {
            if (Hit.collider.gameObject.tag == "Enemy")
            {
                Enemy enemy = Hit.collider.gameObject.GetComponent<Enemy>();

                // enemy will be hit when head of the ray finishes its 'journey'
                RayAchievedTargetAction += () => {
                    // this check is needed because delegate could be called after object destruction 
                    if (enemy != null)
                    {
                        enemy.OnEnemyHit?.Invoke(this.Damage);
                    }
                };
            }

            // set line end at enemy pos
            target = Hit.point;
        }
        else
        {
            // set line end at max range
            target = Ray.origin + Ray.direction * Range;
        }
    }

    private IEnumerator ShootRay(Vector3 targetPosition, float headTime, float tailTime, float tailDelay = 0, Action onRayAchievedTarget = null)
    {
        Vector3 startPosition = Ray.origin;

        // if no tailDelay, delay rayTail movement until head achieves target
        if(tailDelay == 0)
        {
            // head of the ray travel
            yield return StartCoroutine(MoveLinePosition(Line, 1, startPosition, targetPosition, headTime));
            // target achieved by head action
            onRayAchievedTarget?.Invoke();
            // tail of the ray travel
            yield return StartCoroutine(MoveLinePosition(Line, 0, startPosition, targetPosition, tailTime));
        }
        else // otherwise wait desired time and trigger tail movement coroutine
        {
            StartCoroutine(MoveLinePosition(Line, 1, startPosition, targetPosition, headTime));

            // time tailDelay / headTime 'time conflict' solving - rest of the logic is the same as above
            if (tailDelay > headTime)
            {
                yield return new WaitForSeconds(headTime);
                onRayAchievedTarget?.Invoke();
                yield return new WaitForSeconds(tailDelay - headTime);
                StartCoroutine(MoveLinePosition(Line, 0, startPosition, targetPosition, tailTime));
            }
            else
            {
                yield return new WaitForSeconds(tailDelay);
                StartCoroutine(MoveLinePosition(Line, 0, startPosition, targetPosition, tailTime));
                yield return new WaitForSeconds(headTime - tailDelay);
                onRayAchievedTarget?.Invoke();
            }
        }
    }

    // basic lerp of linerenderer vertex position
    private IEnumerator MoveLinePosition(LineRenderer lineRenderer, int vertexIndex, Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        float currentTime = 0f;
        Vector3 currentPosition;

        while (currentTime <= duration)
        {
            var step = Mathf.Clamp01(currentTime / duration);
            currentPosition = Vector3.Lerp(startPosition, targetPosition, step);
            lineRenderer.SetPosition(vertexIndex, currentPosition);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private void PlaySound()
    {
        // play laser sound with slightly randomized pitch
        var AS = GetComponent<AudioSource>();
        AS.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        AS.Play();
    }
}
