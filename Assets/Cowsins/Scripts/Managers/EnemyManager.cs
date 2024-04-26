using System;
using System.Collections.Generic;
using cowsins;
using UnityEngine;
using Random = System.Random;

public interface IPoolable {
    void OnSpawn();
    void OnDespawn();
}
public class EnemyManager : MonoBehaviour {
    public static EnemyManager Instance;
    // Prefab的引用
    public List<GameObject> prefabs;

    private List<ZombieEnemy> _enemysList = new List<ZombieEnemy>();
    private bool _needCreate = false;

    private Vector2 _leftBottom = new Vector3(-51,  -28);
    private Vector2 _rightUp = new Vector3(51,  72);
    // 对象池容器
    private Dictionary<string, Queue<ZombieEnemy>>  poolDic = new Dictionary<string, Queue<ZombieEnemy>>();

    private float _currentTime = 0;
    public PlayerMovement Player;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        Reset();
    }

    private void Reset()
    {
        poolDic.Clear();
        foreach (var prefab in prefabs)
        {
            Queue<ZombieEnemy> queue = new Queue<ZombieEnemy>();
            poolDic[prefab.name] = queue;
        }
        _enemysList.Clear();
        CreateEnemy();
    }

    public void Update()
    {
        if (!_needCreate)
        {
            return;
        }
        _currentTime += Time.deltaTime;
        if (_currentTime > 15)
        {
            CreateEnemy();
            _currentTime = 0;
        }
    }

    public void CreateEnemy()
    {
        ZombieEnemy enemy = Spawn();
        float randomX = UnityEngine.Random.Range(_leftBottom.x, _rightUp.x);
        float randomY = UnityEngine.Random.Range(_leftBottom.y, _rightUp.y);
        enemy.SetPosition(new Vector3(randomX, 0.65f, randomY));
    }
    
    // 生成对象的方法
    public ZombieEnemy Spawn() {
        ZombieEnemy obj;
        int random = UnityEngine.Random.Range(0, prefabs.Count);
        GameObject prefab = prefabs[random];
        if (poolDic[prefab.name].Count == 0) {
            obj = Instantiate(prefab).GetComponent<ZombieEnemy>();
            obj.playerMovement = Player;
            obj.name = prefab.name;
        } else {
            obj = poolDic[prefab.name].Dequeue();
        }
        obj.OnSpawn();
        _enemysList.Add(obj);
        return obj;
    }

    // 回收对象的方法
    public void Despawn(ZombieEnemy obj) {
        obj.OnDespawn();
        _enemysList.Remove(obj);
        poolDic[obj.transform.name].Enqueue(obj);
    }

    public void DestroyAllEnemy()
    {
        _needCreate = false;
        foreach (var enemy in _enemysList)
        {
            enemy.OnDespawn();
            poolDic[enemy.transform.name].Enqueue(enemy);
        }
        _enemysList.Clear();
    }

    public void KillAllEnemy()
    {
        foreach (var enemy in _enemysList)
        {
            enemy.Damage(99999);
        }
    }
    
    public void CrazyAllEnemy()
    {
        foreach (var enemy in _enemysList)
        {
            enemy.CrazyEnemy();
        }
    }
}