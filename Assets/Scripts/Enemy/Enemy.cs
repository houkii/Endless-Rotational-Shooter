using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public static Action<Enemy> OnEnemyKilled;
    public Action<int> OnEnemyHit;
    public int Health { get { return Stats.Health; } }
    public int Damage { get { return Stats.Damage; } }
    public int RewardPoints { get { return Health > 0 ? 0 : Stats.RewardPoints; } } // prevent adding points for enemies killed in "different way"
    public float SpawnTweenTime = 1.0f;

    [SerializeField] private EnemyStats Stats;
    [SerializeField] private GameObject HitParticleSys;
    protected Vector3 Target;

    protected virtual void Awake()
    {
        Target = Vector3.Scale(PlayerController.Instance.transform.position, new Vector3(1,0,1));
        OnEnemyHit += Hit;
        RunBehaviour();
    }

    private void RunBehaviour()
    {
        StartCoroutine(RunBehaviourCoroutine());
    }

    private IEnumerator RunBehaviourCoroutine()
    {
        yield return StartCoroutine(SpawnTween(this.SpawnTweenTime));
        yield return StartCoroutine(RotateTowards(this.Target));
        yield return StartCoroutine(MoveTo(this.Target));
        yield break;
    }

    // basic enemy "behaviour"...
    protected abstract IEnumerator MoveTo(Vector3 targetPosition);

    // entity's spawn "animation" - simple scale change
    protected virtual IEnumerator SpawnTween(float tweenTime)
    {
        float time = 0f;
        Vector3 defaultScale = transform.localScale;
        transform.localScale = Vector3.zero;
        while(transform.localScale != defaultScale)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, defaultScale, time / tweenTime);
            time += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    protected virtual IEnumerator RotateTowards(Vector3 targetPosition, float rotationTime = 0.5f)
    {
        float currentTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion destRotation = Quaternion.LookRotation(targetPosition - transform.position, transform.up);

        while (currentTime < rotationTime)
        {
            transform.rotation = Quaternion.Lerp(startRotation, destRotation, Mathf.Clamp01(currentTime / rotationTime));
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public void Kill(bool instantly = false)
    {
        if(!instantly)
        {
            // sound, death anim triggering...
        }

        OnEnemyKilled?.Invoke(this);
        Destroy(this.gameObject);
    }

    private void Hit(int damage)
    {
        this.Stats.Health -= damage;
        var hitPS = Instantiate(HitParticleSys, transform.position, Quaternion.identity);
        Destroy(hitPS, .5f);

        if (this.Health <= 0)
            this.Kill();
    }
}

