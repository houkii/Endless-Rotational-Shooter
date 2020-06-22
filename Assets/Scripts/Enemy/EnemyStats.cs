using UnityEngine;

[System.Serializable]
public struct EnemyStats
{
    // only hp is modifiable
    public int Health;

    [SerializeField] private int rewardPoints;
    public int RewardPoints
    {
        get { return rewardPoints; }
    }

    [SerializeField] private int damage;
    public int Damage
    {
        get { return damage; }
    }
}