using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField]
    private float spawnDelay = 2.0f;
    [SerializeField]
    private float centerRadius = 26.0f;
    [SerializeField]
    private float annulusWidth = 6.0f;

    private List<Enemy> aliveEnemies = new List<Enemy>();

    private void Awake()
    {
        Enemy.OnEnemyKilled += RemoveEnemy;
    }

    public void StartSpawner()
    {
        StartCoroutine(Spawn(spawnDelay));
    }

    public void StopSpawner()
    {
        StopAllCoroutines();
        this.ClearEnemies();
    }

    public void Restart()
    {
        this.StopSpawner();
        this.StartSpawner();
    }

    private IEnumerator Spawn(float spawnDelay)
    {
        while(true)
        {
            this.SpawnRandomEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnRandomEnemy()
    {
        float randomAngle = Random.value * 2 * Mathf.PI;
        float posX = Mathf.Sin(randomAngle);
        float posZ = Mathf.Cos(randomAngle);
        Vector3 spawnPosition = new Vector3(posX, 0, posZ) * centerRadius;
        Vector3 annulusPosition = Vector3.Scale(Random.insideUnitSphere, new Vector3(1, 0, 1)) * annulusWidth;
        spawnPosition += annulusPosition;
        int index = Random.Range(0, enemyPrefabs.Count);
        var spawnedEnemy = Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity);
        aliveEnemies.Add(spawnedEnemy.GetComponent<Enemy>());
    }

    private void RemoveEnemy(Enemy enemy)
    {
        aliveEnemies.Remove(enemy);
    }

    private void ClearEnemies()
    {
        for(int i = aliveEnemies.Count-1; i >= 0; i--)
        {
            aliveEnemies[i].Kill();
        }
    }
}
