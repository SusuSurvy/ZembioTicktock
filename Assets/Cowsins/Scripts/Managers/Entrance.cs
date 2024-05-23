using System.Collections.Generic;
using System.Linq;
using cowsins;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class Entrance : MonoBehaviour
{
  //  public Button button;
   // public Configura configura;
    private Microphone _microphone;
    private ConnectDouYin _connectDouYin;
    private AudioSource _audioSource;
    private float _waitTime;
    private Dictionary<string, Texture2D> _headIconDic = new Dictionary<string, Texture2D>();

    public Text Text;
    public void Awake()
    {
      //  configura = JsonToos.ReadJson<Configura>("配置表");
        _audioSource = GetComponent<AudioSource>();
        //_microphone = GetComponent<Microphone>();
        _connectDouYin = GetComponent<ConnectDouYin>();
    }
    
    public void Start()
    {
      // _audioSource.clip = null;
      //  _audioSource.volume = configura.音量;
      //  button.onClick.AddListener(() => button.gameObject.SetActive(false));
        InitDouYinConnect();
    }
    
    public void Update()
    {
      //  if (button.gameObject.activeSelf) return;
        if (_waitTime > 0) _waitTime -= Time.deltaTime;
        else
        {
        //    _waitTime = configura.重置时间;
        }
    }
    private void InitDouYinConnect()
    {
        if (!_connectDouYin.enabled) return;
        _connectDouYin.OnGiftMessage = data =>
        {
            var giftName = data["giftName"];
            int count = 1;
            int.TryParse(data["giftCount"], out count);
            //Text.text = CantactString(data);
            var url = data["head_img"];
            UITicktockPanel.Instance.SendMessageClient(giftName, null, count);
        };
        _connectDouYin.OnLikeMessage = data =>
        {
            int count = 1;
            int.TryParse(data["total"], out count);
            //Text.text = CantactString(data);
            UITicktockPanel.Instance.LikeMessageTrigger(count);
        };
        _connectDouYin.OnEnterRoomMessage = data =>
        {
            Debug.LogError("进入房间");
            var url = data["head_img"];
        
            if (!_headIconDic.ContainsKey(url))
           {
             //  StartCoroutine(_connectDouYin.DownLoadHeadImage(url, teu =>
             //  {
             //      _headIconDic[url] = teu;
             //  }));
           }
            // else
            // {
            //     UITicktockPanel.Instance.SendMessageClient(giftName, _headIconDic[url]);
            // }
        };
        _connectDouYin.OnChatMessage = data =>
        {
            foreach (var VARIABLE in data)
            {
                Debug.LogError(VARIABLE.Value);
            }
            var content = data["content"];
           // Text.text = CantactString(data);
           
            var name = data["name"];
            if (name.Contains("用户")) return;
            var url = data["head_img"];
         //   if (_headIconDic.ContainsKey(url))
         //   {
                //Text.text = "显示头像";
               // UITicktockPanel.Instance.SendMessageClient(content, _headIconDic[url]);
          //  }
          //  else
          //  {
              //  StartCoroutine(_connectDouYin.DownLoadHeadImage(url, teu =>
              //  {
                //    _headIconDic[url] = teu;
               //     if (teu != null)
               //     {
                        //Text.text = "下载头像完成";  
                //    }
                //    else
                //    {
                       // Text.text = "头像为空";  
                //    }
                 //   UITicktockPanel.Instance.SendMessageClient(content, _headIconDic[url]);
                    //TODO
              //  }));
          //  }
           // if (!ReGexTools.IsChinese(content)) return;
          
        };
    }

    private string CantactString(Dictionary<string, string> dic)
    {
        string str = "";
        foreach (var VARIABLE in dic)
        {
            str += VARIABLE.Key + "__";
            str += VARIABLE.Value + ";";
        }

        return str;
    }
}