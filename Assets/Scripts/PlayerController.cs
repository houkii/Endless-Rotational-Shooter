using System;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Action<Enemy> OnEnemyHit;
    public Action OnPlayerDead;

    public Action<int> OnScoreChanged
    {
        get { return Stats.OnScoreChanged; }
        set { Stats.OnScoreChanged = value; }
    }

    public Action<int> OnHealthChanged
    {
        get { return Stats.OnHealthChanged; }
        set { Stats.OnHealthChanged = value; }
    }

    public int Score { get { return Stats.Score; } }
    public AudioSource Audio;

    [SerializeField] private PlayerStats Stats;
    private PlayerStats DefaultStats; // for reset


    public override void Awake()
    {
        base.Awake();
        DefaultStats = (PlayerStats)Stats.Clone();
        Enemy.OnEnemyKilled += AddScore;
        this.OnEnemyHit += HandleEnemyCollision;
    }

    public void Restart()
    {
        Stats.Health = DefaultStats.Health;
        Stats.Score = DefaultStats.Score;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            OnEnemyHit?.Invoke(enemy);
        }
    }

    private void AddScore(Enemy enemy)
    {
        Stats.Score += enemy.RewardPoints;
    }

    private void HandleEnemyCollision(Enemy enemy)
    {
        Stats.Health -= enemy.Damage;
        enemy.Kill();

        if(Stats.Health <= 0)
        {
            Audio.Play();
            this.OnPlayerDead?.Invoke();
        }
    }
}
