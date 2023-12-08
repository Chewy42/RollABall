using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLevel : MonoBehaviour
{
    [SerializeField] protected float _enemy_spawnrate = 0f;
    [SerializeField] protected int _max_bosses = 0;
    [SerializeField] protected int _level = 0;
    
    [SerializeField] protected List<float> _boss_spawn_times = new List<float>();
    [Header("Level Enemy Prefabs")]
    [SerializeField] protected List<GameObject> prefabs = new List<GameObject>();

    public float GetEnemySpawnRate() { return _enemy_spawnrate; }
    public int GetMaxBosses() { return _max_bosses; }
    public int GetLevel() { return _level; }
    public List<float> GetBossSpawnTimes() { return _boss_spawn_times; }
    public List<GameObject> GetPrefabs() { return prefabs; }
}
