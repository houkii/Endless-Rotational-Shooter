using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // hide stats from outside world, only changes visible
    [SerializeField]
    private PlayerStats Stats;
    
    public PlayerStats.ScoreUpdatedDelegate OnScoreChanged
    {
        get { return Stats.OnScoreChanged; }
        set { Stats.OnScoreChanged = value; }
    }

    public PlayerStats.HealthChangedDelegate OnHealthChanged
    {
        get { return Stats.OnHealthChanged; }
        set { Stats.OnHealthChanged = value; }
    }

    private PlayerStats DefaultStats; // for reset

    public delegate void EnemyHitDelegate(Enemy enemy);
    public EnemyHitDelegate OnEnemyHit;

    public delegate void PlayerKilledDelegate();
    public PlayerKilledDelegate OnPlayerDead;

    public static PlayerController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DefaultStats = (PlayerStats)Stats.Clone();
        Enemy.OnEnemyKilled += AddScore;
        this.OnEnemyHit += HandleEnemyCollision;
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
            this.OnPlayerDead.Invoke();
        }
    }

    public void Restart()
    {
        // this could be better...
        Stats.Health = DefaultStats.Health;
        Stats.Score = DefaultStats.Score;
    }
}

[System.Serializable]
public class PlayerStats : ICloneable
{
    private int score;
    public int Score
    {
        get { return score; }
        set { score = value; OnScoreChanged?.Invoke(value); }
    }

    [SerializeField]
    private int health;
    public int Health
    {
        get { return health; }
        set { health = value; OnHealthChanged?.Invoke(value); }
    }

    // need those for UI updates
    public delegate void ScoreUpdatedDelegate(int value);
    public ScoreUpdatedDelegate OnScoreChanged;

    public delegate void HealthChangedDelegate(int value);
    public HealthChangedDelegate OnHealthChanged;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}