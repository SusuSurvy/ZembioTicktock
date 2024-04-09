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
    

    private Vector2 _leftBottom = new Vector3(-51,  -28);
    private Vector2 _rightUp = new Vector3(51,  72);
    // 对象池容器
    private Dictionary<string, Queue<ZombieEnemy>>  poolDic = new Dictionary<string, Queue<ZombieEnemy>>();

    private float _currentTime = 0;
    public PlayerMovement Player;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        foreach (var prefab in prefabs)
        {
            Queue<ZombieEnemy> queue = new Queue<ZombieEnemy>();
            poolDic[prefab.name] = queue;
        }
    }

    public void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > 3)
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
        return obj;
    }

    // 回收对象的方法
    public void Despawn(ZombieEnemy obj) {
        obj.OnDespawn();
        poolDic[obj.transform.name].Enqueue(obj);
    }
}