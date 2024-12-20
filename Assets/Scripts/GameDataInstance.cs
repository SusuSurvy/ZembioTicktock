using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FunctionInfo
{
    public CallFunction FuncType;
    public int TriggerNum;
    public AudioClip TriggerMusic;
    public string MusicName;
}

public class GameDataInstance : MonoBehaviour
{
    private static GameDataInstance _instance;
    public static GameDataInstance Instance {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "GameDataInstance";
                _instance = obj.AddComponent<GameDataInstance>();
            }
            return _instance;
        }
    }
    #region 礼物相关
    public static Dictionary<CallFunction, string> CallFunctionDes = new Dictionary<CallFunction, string>()
    {
       // [CallFunction.CallEnemyDoll] = "召唤人偶",
        [CallFunction.CallEnemyGirl] = "召唤突脸怪",
        [CallFunction.CallEnemyFat] = "召唤大型怪",
        [CallFunction.CallEnemyExplosiveGhost] = "召唤爆炸鬼",
        [CallFunction.CallEnemyRemote] = "召唤远程怪",
        [CallFunction.CallEnemyBoss] = "召唤Boss",
        [CallFunction.CallLoseController] = "失去控制",
        [CallFunction.ClearAllEnemy] = "清空敌人",
        [CallFunction.CallPlayerNoDamage] = "角色无敌",
        [CallFunction.CallSmokeExplore] = "生成烟雾弹",
        [CallFunction.CrazyAllEnemy] = "狂暴敌人",
        [CallFunction.RecoverHp] = "恢复血量",
        [CallFunction.CloseLight] = "关闭手电筒",
        [CallFunction.RemoveKey] = "失去钥匙",
        [CallFunction.CallTransferPlayer] = "随机传送",
        [CallFunction.EquipJiatelin] = "召唤神器",
        [CallFunction.DropWeapon] = "丢掉枪",
        [CallFunction.ReduceBullet] = "减少子弹",
        [CallFunction.IncreaseBullet] = "增加子弹",
        [CallFunction.RandomEnemy] = "随机怪物",
        [CallFunction.BackgroundMusic] = "更换音乐",
    
        [CallFunction.TriggerRestartGame] = "重新开始游戏",
    };
    
    public AudioClip BackgroundMusic;
    
    public float BgAudioVolume = 1;
    public float AudioVolume = 1;
    
    public Dictionary<string, CallFunction> CallFunctionSettingDic = new Dictionary<string, CallFunction>();
    
    public Dictionary<string, FunctionInfo> TriggerFunctionSettingDic = new Dictionary<string, FunctionInfo>();
    #endregion

    #region 游戏数据相关
    public Dictionary<string, int> GameLoadData = new Dictionary<string, int>()
    {
        ["UserHp"] = 100,
        ["GunMaxBullet"] = 8,
        ["JiatelinMaxBullet"] = 1000,
        ["NoDameTime"] = 10,
        ["LoseControllerTime"] = 5,
        ["CreateEnemyTime"] = 10, 
        ["CrazyEnemyTime"] = 5, 
        ["GirlHp"] = 3,
        ["FatHp"] = 30,
        ["BossHp"] = 50,
        ["RecoverHp"] = 10,
        ["JiatelinTime"] = 60,
        ["ExplosiveGhost"] = 1,
        ["LikeMessageInterval"] = 20,
        ["RestartGameTime"] = 30,
    };

    public int GetUserMaxHP()
    {
        return GameLoadData["UserHp"];
    }
    
    public int GetRestartGameTime()
    {
        return GameLoadData["RestartGameTime"];
    }
    
    public int GetLikeMessageInterval()
    {
        return GameLoadData["LikeMessageInterval"];
    }

    public int GetJiatelinTime()
    {
        return GameLoadData["JiatelinTime"];
    }

    public int GetNoDamageTime()
    {
        return GameLoadData["NoDameTime"];
    }
    
    public int GetLoseControllerTime()
    {
        return GameLoadData["LoseControllerTime"];
    }
    
    public int GetCreateEnemyTime()
    {
        return GameLoadData["CreateEnemyTime"];
    }
    public int GetCrazyEnemyTime()
    {
        return GameLoadData["CrazyEnemyTime"];
    }
    public int GetDollHp()
    {
        return GameLoadData["DollHp"];
    }
    public int GetGirlHp()
    {
        return GameLoadData["GirlHp"];
    }
    public int GetFatHp()
    {
        return GameLoadData["FatHp"];
    }
    public int GetRemoteHp()
    {
        return GameLoadData["RemoteHp"];
    }
    public int GetBossHp()
    {
        return GameLoadData["BossHp"];
    }
    
    public int GetRecoverHp()
    {
        return GameLoadData["RecoverHp"];
    }
    
    public int GetGunMaxBullet()
    {
        return GameLoadData["GunMaxBullet"];
    }
    
    public int GetJiatelinMaxBullet()
    {
        return GameLoadData["JiatelinMaxBullet"];
    }
    public int GetExplosiveGhostHp()
    {
        return GameLoadData["ExplosiveGhost"];
    }



    #endregion
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void LoadBackGroundMusic(string url)
    {
        StartCoroutine(PlayMusicFromFile("file://" + url));
    }

    IEnumerator PlayMusicFromFile(string fileUrl)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                BackgroundMusic = clip;
            }
            else
            {
                Debug.LogError("Music file load error: " + www.error);
            }
        }
    }

    public void LoadMusic(FunctionInfo functionInfo)
    {
        StartCoroutine(PlayMusicFromFile("file://" + functionInfo.MusicName, functionInfo));
    }

    IEnumerator PlayMusicFromFile(string fileUrl, FunctionInfo info)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                info.TriggerMusic = clip;
            }
            else
            {
                Debug.LogError("Music file load error: " + www.error);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
