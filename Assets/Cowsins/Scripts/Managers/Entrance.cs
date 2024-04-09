using System.Linq;
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
            var gift = $"感谢{data["name"]}";
            var url = data["head_img"];
            StartCoroutine(_connectDouYin.DownLoadHeadImage(url, teu =>
            {
                //TODO
            }));
        };
        _connectDouYin.OnEnterRoomMessage = data =>
        {
            var url = data["head_img"];
            var name = $"{data["name"]}进入房间，生成丧尸";
            Text.text = name;
            EnemyManager.Instance.CreateEnemy();
            StartCoroutine(_connectDouYin.DownLoadHeadImage(url, teu =>
            {
                Text.text = name;
                //TODO
            }));
        };
        _connectDouYin.OnChatMessage = data =>
        {
            var content = data["content"];
            var name = data["name"];
            if (name.Contains("用户")) return;
            var url = data["head_img"];
           // if (!ReGexTools.IsChinese(content)) return;
            StartCoroutine(_connectDouYin.DownLoadHeadImage(url, teu =>
            {
                //TODO
            }));
        };
    }
}