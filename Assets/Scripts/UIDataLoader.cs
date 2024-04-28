using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Management;
using System.Management.Instrumentation;
using sysDia = System.Diagnostics;

public class UIDataLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public UIDataItem ItemObj;
    public static Dictionary<string, string> GameNameConst = new Dictionary<string, string>()
    {
        ["UserHp"] = "玩家血量",
        ["GunMaxBullet"] = "手枪最大子弹",
        ["JiatelinMaxBullet"] = "加特林最大子弹",
    };

    public Button EnterGameBtn;
    public static UIDataLoader Instance { get; private set; }

    private Dictionary<string, int> GameLoadData = new Dictionary<string, int>()
    {
        ["UserHp"] = 100,
        ["GunMaxBullet"] = 50,
        ["JiatelinMaxBullet"] = 100,
    };

    public int GetUserMaxHP()
    {
        return GameLoadData["UserHp"];
    }
    
    public int GetGunMaxBullet()
    {
        return GameLoadData["GunMaxBullet"];
    }
    
    public int GetJiatelinMaxBullet()
    {
        return GameLoadData["JiatelinMaxBullet"];
    }

    private List<UIDataItem> _items;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else
        {
            Instance = this;
        } 
        EnterGameBtn.onClick.AddListener(OnClick);
        _items = new List<UIDataItem>();
        foreach (var info in GameLoadData)
        {
            UIDataItem item = GameObject.Instantiate(ItemObj);
            item.InitInfo(info.Key, info.Value, GameNameConst[info.Key]);
            item.gameObject.transform.SetParent(this.transform);
            _items.Add(item);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveData(string key, int value)
    {
        GameLoadData[key] = value;
    }

    public void SaveDataAndEnterGame()
    {
        foreach (UIDataItem item in _items)
        {
            item.SaveData();
        }
       
        foreach (var info in GameLoadData)
        {
            PlayerPrefs.SetInt(info.Key, info.Value);
            PlayerPrefs.Save();
        }
    }

    private void OnClick()
    {
        GetCpuIDNew();
       // string cpuId = GetCpuID();
       // Debug.LogError(cpuId);
        // SaveDataAndEnterGame();
        // SceneManager.LoadScene(1);
    }
    
    public static string GetCpuID()
    {
        string cpuId = string.Empty;
        ManagementClass cimobject = new ManagementClass("Win32_Processor");
        ManagementObjectCollection moc = cimobject.GetInstances();


        foreach (ManagementObject mo in moc)
        {
            if(cpuId == "")
            {
                // 可能需要更具自己的需要调整属性名
                cpuId = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }
        }

        return cpuId;
    }

    public  void GetCpuIDNew()
    {
        string id = PlayerPrefs.GetString("xxxxx");
        if (string.IsNullOrEmpty(id))
        {
            Debug.Log("PlayerPrefs没有id,启动GetSerialNumber程序");
            string str = Application.streamingAssetsPath + "/GetSerialNumber.exe";
            Debug.LogError(str);
            sysDia.Process.Start(Application.streamingAssetsPath + "/GetSerialNumber.exe");
            StartCoroutine(IEConfigPwd());
          
        }
        else
        {
			
			
        }
    }
    
    private IEnumerator IEConfigPwd()
    {
        string pwdPath = "pwd.txt";
        Debug.Log("检测正在获取写入");
        //当有这个文档的时候
        yield return new WaitUntil(() => File.Exists(pwdPath));
 
        Debug.Log("写入成功");
        Debug.Log("开始读取");
        string pwdstr = File.ReadAllText(pwdPath);
        bool havaID=true;
        //这里自己做验证
        //。。。。。
        //。。。。。
        if (havaID)
        {
            PlayerPrefs.SetString("xxxxx", pwdstr);
            Debug.Log("获取成功！！！！！");
            File.Delete(pwdPath);
           
        }
        else
        {
            Debug.Log("设备类型不一致！！！！！");
        }
 
    }
}
