using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace cowsins
{
    public class DanmuPool
    {
        private GameObject prefab;
        private Queue<GameObject> pool = new Queue<GameObject>();

        public int CurrentCount;
        public DanmuPool(GameObject prefab)
        {
            this.prefab = prefab;
            CurrentCount = 0;
        }

        public GameObject Get()
        {
            CurrentCount++;
            if (pool.Count > 0)
                return pool.Dequeue();
            return GameObject.Instantiate(prefab);
        }

        public void ReturnToPool(GameObject danmuObj)
        {
            CurrentCount--;
            danmuObj.SetActive(false);
            pool.Enqueue(danmuObj);
        }
    }
    
    public class UITicktockPanel : MonoBehaviour
    {
        public NavMeshAgent NavMeshAgent;
        public GameObject prefab;
        private GameObject instantiatedObject;
        public GameObject PlayerCoordinates; 
        private Vector2 _leftBottom = new Vector3(-51, -28);
        private Vector2 _rightUp = new Vector3(51, 72);
        public Texture2D headIcon;

        public GameObject ControllerPanel;
        public GameObject SmokeExplore;

        public UIButtonCallFun CallFunBtn;

        public float KeyValue;
        public int KeyCount;

        public Text KillEnemyCount;
        public Text PassCount;
        public Image SucceedImg;

        public KeyUI KeyUI;
        public bool KeyRemove;
        public RawImage RawImage;
        
        public Text RestartGameText;
        
        private Dictionary<string, string> _danmuInfo = new Dictionary<string, string>()
        {
            ["1"] = "召唤怪物",
            ["2"] = "血量+10",
            ["3"] = "清空怪物",
            ["4"] = "怪物狂暴5s",
            ["5"] = "关闭手电筒",
            ["6"] = "装备手枪",
            ["7"] = "装备加特林",
            ["8"] = "生成人偶",
            ["9"] = "减少钥匙×1",
            ["["] = "生成突脸怪",
        };

        private Dictionary<CallFunction, UnityAction> _callFunctionDic = new Dictionary<CallFunction, UnityAction>();
        public GameObject textPrefab;
        public Transform danmuContainer;
        private DanmuPool danmuPool;
        
        public PlayerMovement Player;
        public static UITicktockPanel Instance { get; private set; }

        public void ShowSucceed()
        {
            SucceedImg.gameObject.SetActive(true);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
            danmuPool = new DanmuPool(textPrefab);
            ControllerPanel.SetActive(false);
            SucceedImg.gameObject.SetActive(false);
            RestartGameText.gameObject.SetActive(false);
            _callFunctionDic[CallFunction.CallEnemyDoll] = CallEnemyDoll;
            _callFunctionDic[CallFunction.CallEnemyGirl] = CallEnemyGirl;
            _callFunctionDic[CallFunction.CallEnemyFat] = CallEnemyFat;
            _callFunctionDic[CallFunction.CallEnemyRemote] = CallEnemyRemote;
            _callFunctionDic[CallFunction.CallEnemyBoss] = CallEnemyBoss;
            _callFunctionDic[CallFunction.CallLoseController] = CallLoseController;
            _callFunctionDic[CallFunction.ClearAllEnemy] = ClearAllEnemy;
            _callFunctionDic[CallFunction.CallPlayerNoDamage] = CallPlayerNoDamage;
            _callFunctionDic[CallFunction.CallSmokeExplore] = CallSmokeExplore;
            _callFunctionDic[CallFunction.CrazyAllEnemy] = CrazyAllEnemy;
            _callFunctionDic[CallFunction.RecoverHp] = RecoverHp;
            _callFunctionDic[CallFunction.CloseLight] = CloseLight;
            _callFunctionDic[CallFunction.RemoveKey] = RemoveKey;
            _callFunctionDic[CallFunction.CallTransferPlayer] = CallTransferPlayer;
            _callFunctionDic[CallFunction.EquipJiatelin] = EquipJiatelin;
            _callFunctionDic[CallFunction.DropWeapon] = DropGun;
            _callFunctionDic[CallFunction.ReduceBullet] = ReduceBullet;
            _callFunctionDic[CallFunction.IncreaseBullet] = IncreaseBullet;
            _callFunctionDic[CallFunction.RandomEnemy] = RandomEnemy;
            _callFunctionDic[CallFunction.BackgroundMusic] = ChangeBackgroundMusic;
            _callFunctionDic[CallFunction.CallEnemyExplosiveGhost] = CallEnemyExplosiveGhost;
            _callFunctionDic[CallFunction.TriggerRestartGame] = TriggerRestartGame;
            foreach (var info in GameDataInstance.Instance.TriggerFunctionSettingDic)
            {
                UIButtonCallFun btn = Instantiate(CallFunBtn);
                btn.Init(info.Key, info.Value.TriggerNum, _callFunctionDic[info.Value.FuncType]);
                btn.gameObject.SetActive(true);
                btn.gameObject.transform.SetParent(CallFunBtn.transform.parent);
            }
            InitHeadIcon();
        
        }

        private void InitHeadIcon()
        {
            string folderName = "头像";
            string exeFolderPath = Path.GetDirectoryName(Application.dataPath); // 获取 .exe 文件的目录路径

            string musicFolderPath = Path.Combine(exeFolderPath, folderName); // 构建音乐文件夹的完整路径
            string[] musicFiles = Directory.GetFiles(musicFolderPath, "*.png"); // 获取所有MP3文件

            // 输出所有找到的音乐文件路径（仅用于调试）
            foreach (var file in musicFiles)
            {
                Debug.LogError(file);
                StartCoroutine(PlayMusicFromFile("file://" + file));
            }
        }

        IEnumerator PlayMusicFromFile(string fileUrl)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(fileUrl))
            {
                yield return uwr.SendWebRequest();

                // 检查并处理可能发生的错误
                if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(uwr.error);
                }
                else
                {
                    // 获取下载的图片
                    var texture = DownloadHandlerTexture.GetContent(uwr);

                    // 如果是要修改3D对象的纹理
                    RawImage.texture = texture;

                    // 如果是UI的Image，则使用下面的代码片段：
                    // targetUIImage.sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }

        public void ShowDanmu(string text, Texture2D texture)
        {
            GameObject danmuObj = danmuPool.Get();
            danmuObj.transform.SetParent(danmuContainer, false);
            danmuObj.GetComponent<Text>().text = text;
            danmuObj.SetActive(true);
            StartCoroutine(FadeAndRise(danmuObj, texture));
        }

        IEnumerator FadeAndRise(GameObject danmuObj, Texture2D texture)
        {
            float duration = 2f;
            Text textComponent = danmuObj.GetComponent<Text>();
            RawImage image = danmuObj.GetComponentInChildren<RawImage>();
            if (texture != null)
            {
                image.texture = texture;
            }

            // 此处简化动画效果：直接使之线性上移消失
            float startY = danmuPool.CurrentCount * -50;
            float endY = startY + 200; // 上移的距离可以自行调整

            float startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                float t = (Time.time - startTime) / duration;
                danmuObj.transform.localPosition = new Vector3(danmuObj.transform.localPosition.x, Mathf.Lerp(startY, endY, t), 0);
                Color color =  new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1 - t);
                textComponent.color = color; // 逐渐透明
                image.color = color;
                yield return null;
            }

            danmuPool.ReturnToPool(danmuObj);
        }

        public void Interacting()
        {
            Player.InteractManager.PerformInteraction();
        }
        
        private int _currentTriggerCount = 0;
        public void LikeMessageTrigger(int total)
        {
            if (total - _currentTriggerCount >= GameDataInstance.Instance.GetLikeMessageInterval())
            {
                _currentTriggerCount += GameDataInstance.Instance.GetLikeMessageInterval();
                int count = _callFunctionDic.Count;
                int randomIndex = UnityEngine.Random.Range(0, count);
                int currentIndex = 0;
                foreach (var fUnityAction in _callFunctionDic)
                {
                    if (currentIndex == randomIndex)
                    { 
                        Debug.LogError(_currentTriggerCount);
                        Debug.LogError(fUnityAction.Key);
                        fUnityAction.Value();
                        break;
                    }

                    currentIndex++;
                }
            }
        }
        
        public void SendMessageClient(string str, Texture2D texture = null, int count = 1)
        {
           
            if (str.Contains("1"))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                ControllerPanel.SetActive(true);
            
                //ShowDanmu(_danmuInfo["1"], texture);
                //CallEnemy();
            }
            else if (str.Contains("2"))
            {
               
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                ControllerPanel.SetActive(false);
               // ShowDanmu(_danmuInfo["2"], texture);
                // RecoverHp();
            }
            // else if (str.Contains("3"))
            // {
            //     CallExplosiveGhost();
            //     ShowDanmu(_danmuInfo["1"], texture);
            // }
            // else if (str.Contains("5"))
            // {
            //     StartCoroutine(InvokeRepeatingMethod(10, CallSmokeExplore));
            //     //CallSmokeExplore();
            // }
            // else if (str.Contains("4"))
            // {
            //     CallEnemyGirl();
            //     ShowDanmu(_danmuInfo["1"], texture);
            // }


            FunctionInfo info = null;
            if (GameDataInstance.Instance.TriggerFunctionSettingDic.TryGetValue(str, out info))
            {
                if (info.TriggerMusic != null)
                {
                    SoundManager.Instance.PlaySound(info.TriggerMusic, 0, 0, false, 0);
                }

                if (info.FuncType == CallFunction.CallSmokeExplore)
                {
                    int num = count * info.TriggerNum;
                    StartCoroutine(InvokeRepeatingMethod(num, _callFunctionDic[info.FuncType]));
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < info.TriggerNum; j++)
                        {
                            _callFunctionDic[info.FuncType]();
                        }
                    }
                }
            }
        }
        
        private IEnumerator InvokeRepeatingMethod(int count, UnityAction ac)
        {
            int currentCount = 0;
            while (currentCount < count)
            {
                ac();
                currentCount++;
                yield return new WaitForSeconds(0.5f);
            }
        }


        public void RemoveKey()
        {
            KeyRemove = true;
            if (KeyValue > 0.15f)
            {
                KeyUI.StartFadeOut();
            }
        }

        public void CloseLight()
        {
            Player.CloseLight();
        }

        public void RecoverHp()
        {
            Player.RecoverHp(GameDataInstance.Instance.GetRecoverHp());
        }
        
        public void ChangeBackgroundMusic()
        {
            EnemyManager.Instance.ChangeBackGroundMusic();
        }
        public void RandomEnemy()
        {
            List<Action> enemyActions = new List<Action>
            {
                CallEnemyDoll,
                CallEnemyGirl,
                CallEnemyFat,
                CallEnemyRemote,
                CallEnemyBoss
            };
            int randomIndex = UnityEngine.Random.Range(0, enemyActions.Count);
            enemyActions[randomIndex]();
        }

        public void CallEnemy()
        {
            EnemyManager.Instance.CreateEnemy(EnemyType.Any);
        }
        
        public void CallEnemyDoll()
        {
            EnemyManager.Instance.CreateEnemy(EnemyType.Doll);
        }
        
        public void CallEnemyGirl()
        {
            EnemyManager.Instance.CreateEnemy(EnemyType.Girl);
        }
        public void CallEnemyExplosiveGhost()
        {
            EnemyManager.Instance.CreateEnemy(EnemyType.ExplosiveGhost);
        }
        
        private bool _triggerRestartGame = false;
        private float _restartTime = 10f;
        private int _currentRestartTimeInt = 0;
        private float _currentRestartTime = 0;
        public void TriggerRestartGame()
        {
            _restartTime = GameDataInstance.Instance.GetRestartGameTime();
            _triggerRestartGame = true;
            RestartGameText.gameObject.SetActive(true);
            RestartGameText.text = "游戏将在" + (_restartTime - _currentRestartTimeInt).ToString("F0") + "秒后重启";
        }

        public void ShowKillEnemyCount(int count)
        {
            KillEnemyCount.text = count.ToString();
        }

        public void ShowPassCount(int count)
        {
            PassCount.text = count.ToString();
        }

        public void CallTransferPlayer()
        {
            EnemyManager.Instance.CallTransferPlayer();
        }
        
        public void CallEnemyFat()
        {
            EnemyManager.Instance.CreateEnemy(EnemyType.FatWomen);
        }
        
        public void CallEnemyRemote()
        {
            EnemyManager.Instance.CreateEnemy(EnemyType.Remote);
        }

        public void CallExplosiveGhost()
        {
            EnemyManager.Instance.CreateEnemy(EnemyType.ExplosiveGhost);
        }

        public void ReduceBullet()
        {
            if (Player.GetComponent<WeaponController>().weapon == null) return;
            
            Player.GetComponent<WeaponController>().id.totalBullets -= 10;
            if (Player.GetComponent<WeaponController>().id.totalBullets < 0)
            {
                Player.GetComponent<WeaponController>().id.totalBullets = 0;
            }
        }

        public void IncreaseBullet()
        {
            if (Player.GetComponent<WeaponController>().weapon == null) return;

            Player.GetComponent<WeaponController>().id.totalBullets += 10;
        }

        public void CallEnemyBoss()
        {
            EnemyManager.Instance.CreateEnemy(EnemyType.Boss);
        }

        private Coroutine ct;
        public void EquipJiatelin()
        {
            Player.EquipJiatelin();
            if (ct != null)
            {
                StopCoroutine(ct);
            }
            ct = StartCoroutine(DropJiatelin());
        }

        private IEnumerator DropJiatelin()
        {
            yield return new WaitForSeconds(GameDataInstance.Instance.GetJiatelinTime());
            EquipGun();
        }

        public void EquipGun()
        {
            Player.EquipGun();
        }
        
        public void DropGun()
        {
            Player.InteractManager.DropWeapon();
        }
        
        public void OpenLight()
        {
            Player.OpenLight();
        }
        
        public void CallLoseController()
        {
            Player.LoseGontroller();
        }
        
        public void ClearAllEnemy()
        {
            EnemyManager.Instance.KillAllEnemy();
        }

        public void CallPlayerNoDamage()
        {
            CancelRestart();
            Player.GrantNoDamage();
        }

        public void CancelRestart()
        {
            _triggerRestartGame = false;
            RestartGameText.gameObject.SetActive(false);
        }

        public void CallSmokeExplore()
        {
            Vector3 pos = Player.transform.position;
            GameObject obj = Instantiate(SmokeExplore);
            obj.transform.position = pos;
        }
        public void CrazyAllEnemy()
        {
            EnemyManager.Instance.CrazyAllEnemy();
        }
        private void Update()
        {
            if (_triggerRestartGame == true)
            {
                _currentRestartTime += Time.deltaTime;
                RestartGameText.transform.localScale = new Vector3(0.3f *(1 + (0.5f *_currentRestartTime)), 0.3f *(1 + (0.5f *_currentRestartTime)), 0.3f *(1 + (0.5f *_currentRestartTime)));
                if (_currentRestartTime > 1)
                {
                    _currentRestartTime = 0;
                    _currentRestartTimeInt++;
                    if(_currentRestartTimeInt > _restartTime)
                    {
                        _currentRestartTime = 0;
                        _triggerRestartGame = false;
                        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                    }
                    UIController.instance.UpdateRestartGamePanel();
                    RestartGameText.text = "游戏将在" + (_restartTime - _currentRestartTimeInt).ToString("F0") + "秒后重启";
                }
            }
        }
    }
    
  
}
