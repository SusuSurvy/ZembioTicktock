using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public Texture2D headIcon;
        private Dictionary<string, string> _danmuInfo = new Dictionary<string, string>()
        {
            ["1"] = "召唤怪物",
            ["2"] = "血量+10",
            ["3"] = "清空怪物",
            ["4"] = "怪物狂暴5s",
            ["5"] = "关闭手电筒",
            ["6"] = "装备手枪",
            ["7"] = "装备加特林",
        };
        public GameObject textPrefab;
        public Transform danmuContainer;
        private DanmuPool danmuPool;
        
        public PlayerMovement Player;
        public static UITicktockPanel Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
            danmuPool = new DanmuPool(textPrefab);
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

        public void SendMessage(string str, Texture2D texture = null)
        {
            if (str.Contains("1"))
            {
                ShowDanmu(_danmuInfo["1"], texture);
                CallEnemy();
            }
            else if (str.Contains("2"))
            {
                ShowDanmu(_danmuInfo["2"], texture);
                RecoverHp();
            }
            else if (str.Contains("3"))
            {
                ShowDanmu(_danmuInfo["3"], texture);
                ClearAllEnemy();
            }
            else if (str.Contains("4"))
            {
                ShowDanmu(_danmuInfo["4"], texture);
                CrazyAllEnemy();
            }
            else if (str.Contains("5"))
            {
                ShowDanmu(_danmuInfo["5"], texture);
                Player.CloseLight();
            }
            else if (str.Contains("6"))
            {
                ShowDanmu(_danmuInfo["6"], texture);
                EquipGun();
            }
            else if (str.Contains("7"))
            {
                ShowDanmu(_danmuInfo["7"], texture);
                EquipJiatelin();
            }
          
        }

        public void RecoverHp()
        {
            Player.RecoverHp(10);
        }
        
        public void CallEnemy()
        {
            EnemyManager.Instance.CreateEnemy();
        }
        
        public void EquipGun()
        {
            Player.EquipGun();
        }
        public void EquipJiatelin()
        {
            Player.EquipJiatelin();
        }
        
        public void OpenLight()
        {
            Player.OpenLight();
        }
        
        public void TransferPlayer()
        {
          
        }
        
        public void ClearAllEnemy()
        {
            EnemyManager.Instance.KillAllEnemy();
        }
        
        public void CrazyAllEnemy()
        {
            EnemyManager.Instance.CrazyAllEnemy();
        }
    }
}
