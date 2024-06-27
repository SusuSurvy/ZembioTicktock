using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Management;
using System.Management.Instrumentation;
using System.Runtime.Serialization.Formatters.Binary;
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
        ["NoDameTime"] = "玩家无敌时间",
        ["LoseControllerTime"] = "失去控制时间",
        ["CreateEnemyTime"] = "生成怪物时间间隔", 
        ["CrazyEnemyTime"] = "怪物狂暴时间", 
       // ["DollHp"] = "玩偶血量",
        ["GirlHp"] = "突脸怪血量",
        ["FatHp"] = "大型怪血量",
        //["RemoteHp"] = "远程怪血量",
        ["BossHp"] = "Boss血量",
        ["ExplosiveGhost"] = "爆炸鬼血量",
        ["RecoverHp"] = "玩家恢复血量",
        ["JiatelinTime"] = "加特林持续时间",
        ["LikeMessageInterval"] = "点赞触发效果间隔",
        ["RestartGameTime"] = "游戏重启时间",
    };

    public Button EnterGameBtn;
    public static UIDataLoader Instance { get; private set; }

    public InputField InputField;

    void LaunchEXE(string path)
    {
        if (IsProcessRunning("WssBarrageServer"))
        {
            UnityEngine.Debug.Log("Process is already running: WssBarrageServer" );
            return;
        }
        try
        {
            Debug.LogError(path);
            sysDia.Process process = new sysDia.Process();
            process.StartInfo.FileName = path;
            process.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
            process.Start();
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("Failed to launch exe: " + ex.Message);
        }
    }
    
    bool IsProcessRunning(string processName)
    {
        sysDia.Process[] processes = sysDia.Process.GetProcessesByName(processName);
        return processes.Length > 0;
    }
    private List<UIDataItem> _items;
    private void Awake()
    {
        string folderName = "抖音监听器";
        string exeFolderPath = Path.GetDirectoryName(Application.dataPath); // 获取 .exe 文件的目录路径

        string musicFolderPath = Path.Combine(exeFolderPath, folderName); // 构建音乐文件夹的完整路径
        string[] musicFiles = Directory.GetFiles(musicFolderPath, "*.exe"); // 获取所有MP3文件

        // 输出所有找到的音乐文件路径（仅用于调试）
        foreach (var file in musicFiles)
        {
            LaunchEXE("file://" + file);
        }
        if (Instance != null && Instance != this) Destroy(this);
        else
        {
            Instance = this;
        } 
        EnterGameBtn.onClick.AddListener(LoginIn);
        _items = new List<UIDataItem>();
        foreach (var info in GameDataInstance.Instance.GameLoadData)
        {
            UIDataItem item = GameObject.Instantiate(ItemObj);
            item.InitInfo(info.Key, info.Value, GameNameConst[info.Key]);
            item.gameObject.transform.SetParent(this.transform);
            item.transform.localScale = Vector3.one;
            
            _items.Add(item);
        }

        InputField.text = GetMotherboardID();
        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveData(string key, int value)
    {
        GameDataInstance.Instance.GameLoadData[key] = value;
    }

    public void SaveDataAndEnterGame()
    {
        foreach (UIDataItem item in _items)
        {
            item.SaveData();
        }
       
        foreach (var info in GameDataInstance.Instance.GameLoadData)
        {
            PlayerPrefs.SetInt(info.Key, info.Value);
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene(1);
        
    }

    private void OnClick()
    {
        //GerServerKeycode();
        // Text.text = GetMotherboardID();
        // SaveDataAndEnterGame();
        //  SceneManager.LoadScene(1);
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
    
    public string GetMotherboardID()
    {
        return SystemInfo.deviceUniqueIdentifier.ToString();
        // if (InputField.text != "")
        // {
        //     return InputField.text;
        // }
        // string motherboardID = "";
        // try
        // {
        //     // 设置启动进程的参数
        //     sysDia.ProcessStartInfo procStartInfo = new sysDia.ProcessStartInfo("cmd", "/c wmic baseboard get SerialNumber")
        //     {
        //         RedirectStandardOutput = true,
        //         UseShellExecute = false,
        //         CreateNoWindow = true
        //     };
        //
        //     // 启动进程以执行命令
        //     using (sysDia.Process process = sysDia.Process.Start(procStartInfo))
        //     {
        //         // 读取命令的标准输出
        //         motherboardID = process.StandardOutput.ReadToEnd();
        //         process.WaitForExit();
        //     }
        //
        //     //处理输出的字符串，提取主板序列号
        //     string[] lines = motherboardID.Split('\n');
        //     if (lines.Length >= 2)
        //     {
        //         motherboardID = lines[1].Trim(); // 通常序列号位于第二行
        //     }
        //     else
        //     {
        //         Debug.LogError("无法获取主板序列号。");
        //         motherboardID = "";
        //     }
        // }
        // catch (System.Exception e)
        // {
        //     Debug.LogError("获取主板序列号时出错: " + e.Message);
        // }
        //
        // return motherboardID;
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

    public void PushKeycode()
    {
        string keyCode = GetMotherboardID();
        
    }
    public long GetCurrentUnixTimestampSeconds()
    {
        DateTime now = DateTime.UtcNow;
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        long timestamp = (long)(now - epochStart).TotalSeconds;
        return timestamp;
    }
    
    public int CalculateDaysSince(long registrationTimestamp)
    {
        DateTime now = DateTime.UtcNow;
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime registrationDate = epochStart.AddSeconds(registrationTimestamp);

        TimeSpan elapsed = now - registrationDate;
        return elapsed.Days;
    }
    
    private void EnterGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoginIn()
    {
        string keycode = GetMotherboardID();
        string ossUrl = string.Format("fengrenyuan/{0}/{1}", keycode, "keycodeData.bytes");
        string cacheUrl = Application.persistentDataPath + "/keycodeData.bytes";
        Oss.GetObject(ossUrl, cacheUrl, downloadUrl =>
        {
            if (downloadUrl != null)
            {
              
                byte[] bytes = LoadFromFile(downloadUrl);
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream(bytes);
                ObjsData dynamicObjData = formatter.Deserialize(stream) as ObjsData;
                int lastDay = dynamicObjData.PlayDay - CalculateDaysSince(dynamicObjData.SignInTime);
                if (lastDay < 0)
                {
                    Text.text = "您的卡密已到期，请重新注册登录" ;
                }
                else
                {
                    if (!RegisteCallFunctionMgr.Instance.CheckAllSettingComplete())
                    {
                        Text.text = "礼物配置未完成，请先完成配置" ;
                        return;
                    }
                    Text.text = "您还可以玩" + lastDay + "天，即将进入游戏" ;
                    SaveDataAndEnterGame();
                    
                }

            
            }
            else
            {
                Text.text = "未查询到您的账号信息，请联系管理员注册";
            }
        });
    }
    
    public static byte[] LoadFromFile (string path) {
        using (var stream = new FileStream(path, FileMode.Open)) {
            var bytes = new byte[(int)stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            return bytes;
        }
    }


    private byte[] SerializeToByteArray(ObjsData obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }
}
