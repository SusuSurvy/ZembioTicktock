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

    private UnityAction<string> _callback = null;

    public void ChooseMusic(UnityAction<string> ac)
    {
        _callback = ac;
        ChooseGiftWindow.SetActive(true);
    }

    public void GetGiftIcon(string sprite)
    {
        ChooseGiftWindow.SetActive(false);
        _callback(sprite);
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
            GameDataInstance.Instance.LoadMusic(functionInfo);
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

        InitMusic();

    }
    public string folderName = "音效"; 
    private void InitMusic()
    {
        string exeFolderPath = Path.GetDirectoryName(Application.dataPath); // 获取 .exe 文件的目录路径

        string musicFolderPath = Path.Combine(exeFolderPath, folderName); // 构建音乐文件夹的完整路径
        string[] musicFiles = Directory.GetFiles(musicFolderPath, "*.mp3"); // 获取所有MP3文件
        
        // 输出所有找到的音乐文件路径（仅用于调试）
        foreach (var file in musicFiles)
        {
            UIGiftIconItem item = Instantiate(ItemObj);
            item.InitInfo(file, true);
            item.gameObject.SetActive(true);
            item.transform.SetParent(ItemObj.transform.parent);
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
                return false;
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
