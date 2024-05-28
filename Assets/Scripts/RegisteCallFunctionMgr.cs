using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using cowsins;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


using UnityEngine.Networking;
using UnityEngine.InputSystem;


public enum CallFunction
{
    CallEnemyDoll = 0,
    CallEnemyGirl = 1,
    CallEnemyFat = 2,
    CallEnemyRemote = 3,
    CallEnemyBoss = 4,
    CallLoseController = 5,
    CloseLight = 6,
    CallPlayerNoDamage = 7,
    CallSmokeExplore = 8,
    CrazyAllEnemy = 9,
    RecoverHp = 10,
    ClearAllEnemy = 11,
    RemoveKey = 12,
    CallTransferPlayer = 13,
    EquipJiatelin = 14,
    DropWeapon = 15,
    ReduceBullet = 16,
    IncreaseBullet = 17,
    RandomEnemy = 18,
    BackgroundMusic = 19,
    CallEnemyExplosiveGhost = 20,
    TriggerRestartGame = 21,
}



public class RegisteCallFunctionMgr : MonoBehaviour
{
    
    
    public static RegisteCallFunctionMgr Instance { get; private set; }
    public UIGiftDataItem GiftItem;
    public UIGiftIconItem ItemObj;
    private List<UIGiftDataItem> _items;
    private Dictionary<string, UIGiftIconItem> _giftIconsDic = new Dictionary<string, UIGiftIconItem>();
    public RectTransform Rect;

    public GameObject ChooseGiftWindow;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        Init();
        ChooseGiftWindow.SetActive(false);
    }

    private UnityAction<string, string > _callback = null;

    public void ChooseMusic(UnityAction<string, string> ac)
    {
        foreach (UIGiftIconItem item in musicItems)
        {
            item.gameObject.SetActive(!item.IsBackGroundMusic);
        }
        _callback = ac;
        ChooseGiftWindow.SetActive(true);
    }
    
    public void ChooseBackgroundMusic(UnityAction<string, string> ac)
    {
        foreach (UIGiftIconItem item in musicItems)
        {
            item.gameObject.SetActive(item.IsBackGroundMusic);
        }
        _callback = ac;
        ChooseGiftWindow.SetActive(true);
    }

    public void GetGiftIcon(string name, string path)
    {
        ChooseGiftWindow.SetActive(false);
        _callback(name, path);
    }

    public void SetFunctionSetting(string key, CallFunction value, int triggerNum, string musicName)
    {
        GameDataInstance.Instance.CallFunctionSettingDic[key] = value;
        FunctionInfo functionInfo = new FunctionInfo();
        functionInfo.FuncType = value;
        functionInfo.TriggerNum = triggerNum;
        functionInfo.MusicName = musicName;
        if (!string.IsNullOrEmpty(musicName))
        {
            if (key != GameDataInstance.CallFunctionDes[CallFunction.BackgroundMusic])
            {
                GameDataInstance.Instance.LoadMusic(functionInfo);
            }
            // 开始播放音乐
        }

        GameDataInstance.Instance.TriggerFunctionSettingDic[key] = functionInfo;
}
    
    private void Init()
    {
        _items = new List<UIGiftDataItem>();
    
        _giftIconsDic.Clear();
     
        foreach (var info in GameDataInstance.CallFunctionDes)
        {
            UIGiftDataItem item = GameObject.Instantiate(GiftItem);
           
            item.InitInfo(info.Key);
            string str = PlayerPrefs.GetString(info.Value, null);
            if (!string.IsNullOrEmpty(str))
            {
                item.SetName(str);
            }
            item.gameObject.transform.SetParent(this.transform);
            item.transform.localScale = Vector3.one;
            _items.Add(item);
        }

        InitMusic(folderName);
        InitMusic(backgroundMusicFolderName, true);


    }
    public string folderName = "音效";
    public string backgroundMusicFolderName = "背景音乐";
    private List<UIGiftIconItem> musicItems = new List<UIGiftIconItem>();
    private void InitMusic(string folderName, bool isBackgroundMusic = false)
    {
        string exeFolderPath = Path.GetDirectoryName(Application.dataPath); // 获取 .exe 文件的目录路径

        string musicFolderPath = Path.Combine(exeFolderPath, folderName); // 构建音乐文件夹的完整路径
        string[] musicFiles = Directory.GetFiles(musicFolderPath, "*.mp3"); // 获取所有MP3文件

        // 输出所有找到的音乐文件路径（仅用于调试）
        foreach (var file in musicFiles)
        {
            string musicFileName = Path.GetFileNameWithoutExtension(file); // 从文件路径中提取音乐文件的名称

            // 实例化音效对象并初始化信息
            UIGiftIconItem item = Instantiate(ItemObj);
            item.InitInfo(musicFileName, isBackgroundMusic, file); // 传递音效名称
            item.gameObject.SetActive(true);
            item.transform.SetParent(ItemObj.transform.parent);
            item.transform.localScale = Vector3.one;
            musicItems.Add(item);
            //Debug.Log(file);
        }
    }




    public bool CheckAllSettingComplete()
    {
        foreach (var item in _items)
        {
            item.SaveData();
        }
        foreach (var info in GameDataInstance.CallFunctionDes)
        {
            if (!GameDataInstance.Instance.CallFunctionSettingDic.ContainsValue(info.Key))
            {
                Debug.LogError(info.Value + "  还没有设置触发方式");
                return true;
            }
        }
        return true;
    }

    public void ChangeGiftIcon(string oldIcon, string newIcon)
    {
        if (_giftIconsDic.ContainsKey(oldIcon))
        {
            _giftIconsDic[oldIcon].SetCanClick(true);
        }
        if (_giftIconsDic.ContainsKey(newIcon))
        {
            _giftIconsDic[newIcon].SetCanClick(false);
        }
    }
}
