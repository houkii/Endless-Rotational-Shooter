using System;
using UnityEngine;

[System.Serializable]
public class PlayerStats : ICloneable
{
    private int score;
    public int Score
    {
        get { return score; }
        set { score = value; OnScoreChanged?.Invoke(value); }
    }

    [SerializeField] private int health;
    public int Health
    {
        get { return health; }
        set { health = value; OnHealthChanged?.Invoke(value); }
    }

    // for UI updates
    public Action<int> OnScoreChanged;
    public Action<int> OnHealthChanged;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}