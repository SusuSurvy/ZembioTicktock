using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
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
            foreach (var info in GameDataInstance.Instance.TriggerFunctionSettingDic)
            {
                UIButtonCallFun btn = Instantiate(CallFunBtn);
                btn.Init(info.Key, info.Value.TriggerNum, _callFunctionDic[info.Value.FuncType]);
                btn.gameObject.SetActive(true);
                btn.gameObject.transform.SetParent(CallFunBtn.transform.parent);
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
            FunctionInfo info = null;
            if (GameDataInstance.Instance.TriggerFunctionSettingDic.TryGetValue(str, out info))
            {
                if (info.TriggerMusic != null)
                {
                    SoundManager.Instance.PlaySound(info.TriggerMusic, 0, 0, false, 0);
                }

                for (int i = 0; i < count; i++)
                {
                    _callFunctionDic[info.FuncType]();
                    ShowDanmu(GameDataInstance.CallFunctionDes[info.FuncType], texture);
                }
            }
        }

        public void RemoveKey()
        {
            if (KeyValue > 0.15f)
            {
                KeyUI.StartFadeOut();
                KeyRemove = true;
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
        
        public void CallEnemyBoss()
        {
            EnemyManager.Instance.CreateEnemy(EnemyType.Boss);
        }
        public void EquipJiatelin()
        {
            Player.EquipJiatelin();
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
            Player.GrantNoDamage();
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
    }
}
