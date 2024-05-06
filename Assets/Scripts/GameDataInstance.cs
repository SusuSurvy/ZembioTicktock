using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionInfo
{
    public CallFunction FuncType;
    public int TriggerNum;
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
        [CallFunction.CallEnemyDoll] = "召唤人偶",
        [CallFunction.CallEnemyGirl] = "召唤突脸怪",
        [CallFunction.CallEnemyFat] = "召唤大型怪",
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
    };
    
    public Dictionary<string, CallFunction> CallFunctionSettingDic = new Dictionary<string, CallFunction>();
    
    public Dictionary<string, FunctionInfo> TriggerFunctionSettingDic = new Dictionary<string, FunctionInfo>();
    #endregion

    #region 游戏数据相关
    public Dictionary<string, int> GameLoadData = new Dictionary<string, int>()
    {
        ["UserHp"] = 100,
        ["GunMaxBullet"] = 200,
        ["JiatelinMaxBullet"] = 200,
        ["NoDameTime"] = 10,
        ["LoseControllerTime"] = 5,
        ["CreateEnemyTime"] = 10, 
        ["CrazyEnemyTime"] = 5, 
        ["DollHp"] = 1,
        ["GirlHp"] = 3,
        ["FatHp"] = 30,
        ["RemoteHp"] = 2,
        ["BossHp"] = 50,
        ["RecoverHp"] = 10,
    };

    public int GetUserMaxHP()
    {
        return GameLoadData["UserHp"];
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

    

    #endregion
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
