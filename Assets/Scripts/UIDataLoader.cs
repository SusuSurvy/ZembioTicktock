using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Management;
using System.Management.Instrumentation;
using System.Text;
using sysDia = System.Diagnostics;

public class UIDataLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public UIDataItem ItemObj;
    public Text Text;
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
        Text.text = GetMotherboardID();
        SaveDataAndEnterGame();
         SceneManager.LoadScene(1);
        //StartCoroutine(IEConfigPwd());
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
         
          
        }
        else
        {
			
			
        }
    }
    
    public string GetMotherboardID()
    {
        string motherboardID = "";
        try
        {
            // 设置启动进程的参数
            sysDia.ProcessStartInfo procStartInfo = new sysDia.ProcessStartInfo("cmd", "/c wmic baseboard get SerialNumber")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // 启动进程以执行命令
            using (sysDia.Process process = sysDia.Process.Start(procStartInfo))
            {
                // 读取命令的标准输出
                motherboardID = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }

            //处理输出的字符串，提取主板序列号
            string[] lines = motherboardID.Split('\n');
            if (lines.Length >= 2)
            {
                motherboardID = lines[1].Trim(); // 通常序列号位于第二行
            }
            else
            {
                Debug.LogError("无法获取主板序列号。");
                motherboardID = "";
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("获取主板序列号时出错: " + e.Message);
        }

        return motherboardID;
    }


    string GetCPUSerialNumber()
    {
        var process = new sysDia.Process
        {
            StartInfo = new sysDia.ProcessStartInfo
            {
                FileName = "wmic",
                Arguments = "cpu get ProcessorId",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
 
        // 清理结果字符串并获取CPU序列号
        string[] lines = result.Split('\n');
        foreach (var line in lines)
        {
            if (line.Trim().StartsWith("ProcessorId"))
            {
                string[] parts = line.Split(' ');
                foreach (var part in parts)
                {
                    if (!string.IsNullOrEmpty(part) && part != "ProcessorId")
                    {
                        return part.Trim();
                    }
                }
            }
        }
        return "Unknown";
    }
    
    private IEnumerator IEConfigPwd(string arguments = null)
    {
        yield return new WaitForSeconds(2f);
       
    }
}
