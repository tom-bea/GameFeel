using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The enemy to spawn.")]
    private GameObject m_EnemyPrefab;

    [SerializeField]
    [Tooltip("The area in which enemies can spawn. This is centered at the position of the spawner.")]
    private Vector2 m_SpawnArea;
    #endregion

    #region Private Variables
    private int p_NumberToSpawn;
    #endregion
    
    #region Spawner Methods
    private void SpawnEnemies()
    {
        for (int i = 0; i < p_NumberToSpawn; i++)
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Vector3 spawnLoc = transform.position;
        Vector2 offset = new Vector2(Random.Range(-m_SpawnArea.x / 2, m_SpawnArea.x / 2), 
            Random.Range(-m_SpawnArea.y / 2, m_SpawnArea.y / 2));
        spawnLoc.x += offset.x;
        spawnLoc.y += offset.y;
        Instantiate(m_EnemyPrefab, spawnLoc, Quaternion.identity);
    }
    #endregion

    #region Methods Used When Updating Slides
    public void SpawnEnemies(int newNum)
    {
        p_NumberToSpawn = newNum;

        SpawnEnemies();
    }
    #endregion
}
