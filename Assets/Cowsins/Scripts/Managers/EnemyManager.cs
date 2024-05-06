using System;
using System.Collections.Generic;
using cowsins;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public interface IPoolable {
    void OnSpawn();
    void OnDespawn();
}

public class EnemyManager : MonoBehaviour {
    public static EnemyManager Instance;
    // Prefab的引用
    public List<GameObject> prefabs;
    private const string KillCountKey = "KillCount";
    private const string PassCountKey = "PassCount";
    private const string DateKey = "LastRecordDate";
    private List<ZombieEnemy> _enemysList = new List<ZombieEnemy>();
    private List<Bullet> _bulletList = new List<Bullet>();
    private bool _needCreate = false;

    private Vector2 _leftBottom = new Vector3(-51,  -28);
    private Vector2 _rightUp = new Vector3(51,  72);
    // 对象池容器
    private Dictionary<string, Queue<EnemyHealth>>  poolDic = new Dictionary<string, Queue<EnemyHealth>>();

    private float _currentTime = 0;
    public PlayerMovement Player;

    public GameObject Bullet;

    public GameObject TransferObj;

    private List<Vector3> _transferList;

    private int _killEnemyCount = 0;
    private int _killDelta = 0;
    private float createEnemyTime;
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
        _needCreate = true;
        poolDic.Clear();
        createEnemyTime = GameDataInstance.Instance.GetCreateEnemyTime();
        foreach (var prefab in prefabs)
        {
            Queue<EnemyHealth> queue = new Queue<EnemyHealth>();
            poolDic[prefab.name] = queue;
        }
        Queue<EnemyHealth> bulletQueue = new Queue<EnemyHealth>();
        poolDic[Bullet.name] = bulletQueue;
        _bulletList.Clear();
        _enemysList.Clear();
        _transferList = new List<Vector3>();
        foreach (Transform item in TransferObj.transform)
        {
            _transferList.Add(item.position);
        }
        CheckDateAndResetIfNecessary();
        int currentKillCount = PlayerPrefs.GetInt(KillCountKey, 0);
        _killEnemyCount = currentKillCount;
        int passKillCount = PlayerPrefs.GetInt(PassCountKey, 0);
        UITicktockPanel.Instance.ShowKillEnemyCount(_killEnemyCount);
        UITicktockPanel.Instance.ShowPassCount(passKillCount);
    }

    public void CheckDateAndResetIfNecessary()
    {
        string lastRecordDate = PlayerPrefs.GetString(DateKey, "");
        string today = DateTime.Now.ToString("yyyyMMdd");

        if (!lastRecordDate.Equals(today))
        {
            PlayerPrefs.SetInt(PassCountKey, 0); // 重置击杀数
            PlayerPrefs.SetInt(KillCountKey, 0); // 重置击杀数
            PlayerPrefs.SetString(DateKey, today); // 更新记录日期
            PlayerPrefs.Save(); // 保存更改
        }
    }

    public void GameWin()
    {
        int passKillCount = PlayerPrefs.GetInt(PassCountKey, 0);
        passKillCount++;
        PlayerPrefs.SetInt(PassCountKey, passKillCount);
        UITicktockPanel.Instance.ShowSucceed();
        UITicktockPanel.Instance.ShowPassCount(passKillCount);
        SaveData();
        Invoke(nameof(ReloadGame), 1);
    }

    private void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(KillCountKey, _killEnemyCount); // 重置击杀数
        PlayerPrefs.Save(); // 保存更改
    }

    public Vector3 GetRandomTransferPos()
    {
        int count = _transferList.Count;
        int random = UnityEngine.Random.Range(0, count);
        return _transferList[random];
    }

    public void Update()
    {
        if (!_needCreate)
        {
            return;
        }
        _currentTime += Time.deltaTime;
        if (_currentTime > createEnemyTime)
        {
            CreateEnemy(EnemyType.Any);
            _currentTime = 0;
        }
    }

    Vector3 GetValidSpawnPositionAround(Vector3 characterPosition)
    {
        Vector3 spawnPosition = Vector3.zero;
        int maxAttempts = 10;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            // 在角色周围随机生成位置
            float radius = 5f; // 调整半径适合你的场景
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            float x = characterPosition.x + radius * Mathf.Cos(angle);
            float z = characterPosition.z + radius * Mathf.Sin(angle);
            spawnPosition = new Vector3(x, 0.5f, z);

            Collider[] colliders = Physics.OverlapSphere(spawnPosition, 0.5f); // 调整半径适合你的场景

            bool positionValid = true;
            foreach (Collider collider in colliders)
            {
                // 检查碰撞器是否是建筑物的碰撞器
                if (collider.gameObject.CompareTag("Building"))
                {
                    positionValid = false;
                    break;
                }
            }

            if (positionValid)
            {
                return spawnPosition;
            }

            attempts++;
        }

        return Vector3.zero;
    }

    public void CallTransferPlayer()
    {
        Vector3 pos = GetRandomTransferPos();
        Player.transform.position = pos;
    }
    public ZombieEnemy CreateEnemy(EnemyType enemyType, Vector3? bornPos = null)
    {
        ZombieEnemy enemy = Spawn(enemyType, bornPos);
        return enemy;
    }
    
    // 生成对象的方法

    public ZombieEnemy Spawn(EnemyType enemyType, Vector3? bornPos) {
        ZombieEnemy obj;
        int index = 0;
        switch (enemyType)
        {
            case EnemyType.Any:
                index = UnityEngine.Random.Range(0, prefabs.Count - 2); //不能随机生成人偶和boss
                switch (index)
                {
                    case 0:
                        enemyType = EnemyType.Girl;
                        break;
                    case 1:
                        enemyType = EnemyType.FatWomen;
                        break;
                    case 2:
                        enemyType = EnemyType.Remote;
                        break;
                    case 3:
                        enemyType = EnemyType.Boss;
                        break;
                    case 4:
                        enemyType = EnemyType.Doll;
                        break;
                }
                break;
            case EnemyType.Girl:
                index = 0;
                break;
            case EnemyType.FatWomen:
                index = 1;
                break;
            case EnemyType.Doll:
                index = 4;
                break;
            case  EnemyType.Remote:
                index = 2;
                break;
            case EnemyType.Boss:
                index = 3;
                break;
        }
      
        GameObject prefab = prefabs[index];
        if (poolDic[prefab.name].Count == 0) {
            obj = Instantiate(prefab).GetComponent<ZombieEnemy>();
            obj.playerMovement = Player;
            obj.name = prefab.name;
        } else {
            EnemyHealth enemyObj = poolDic[prefab.name].Dequeue();
            obj = enemyObj.GetComponent<ZombieEnemy>();
        }
        obj.EnemyType = enemyType;
        obj.OriginalBornPos = bornPos;
        obj.OnSpawn();
        _enemysList.Add(obj);
        return obj;
    }

    // 回收对象的方法
    public void Despawn(ZombieEnemy obj) {
        obj.OnDespawn();
        _killEnemyCount++;
        _killDelta++;
        if (_killDelta > 10)
        {
            SaveData();
            _killDelta = 0;
        }
        UITicktockPanel.Instance.ShowKillEnemyCount(_killEnemyCount);
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

    public void CreatBullet(Vector3 pos)
    {
        Bullet bt = SpawnBullet();
        bt.transform.position = pos;
        bt.SetTarget(Player);
    }

    public void DestroyBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        _bulletList.Remove(bullet);
        poolDic[bullet.name].Enqueue(bullet);
    }

    private Bullet SpawnBullet()
    {
        Bullet obj = null;
        if (poolDic[Bullet.name].Count == 0) {
            obj = Instantiate(Bullet).GetComponent<Bullet>();
            obj.name = Bullet.name;
        } else {
            EnemyHealth enemyObj = poolDic[Bullet.name].Dequeue();
            obj = enemyObj.GetComponent<Bullet>();
        }
        obj.gameObject.SetActive(true);
        _bulletList.Add(obj);
        return obj;
    }

}