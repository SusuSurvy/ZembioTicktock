using Assets.ProgressBars.Scripts;
using cowsins;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class KeyProgress : MonoBehaviour
{
    private static int numberOfKeys = 0;
    private float Keysvalue;
    public GuiProgressBarUI guiProgressBar;
    public GameObject UI;
    public UITicktockPanel UIValue;
    public UITicktockPanel Player;
    public Text KeyText;
    public GameObject keyObject;
    private float minDisplayCount = 1; 
    private float maxDisplayCount = 3; 
    void Start()
    {
        guiProgressBar.Value = 0.0F;
        numberOfKeys = 7;
        //UI.SetActive(false);
       // numberOfKeys = keyObject.transform.childCount;
        Keysvalue = 1.0f / numberOfKeys;
    }


    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (var item in hits)
            {
                if (item.collider.gameObject == this.gameObject)
                {
                    float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
                    float cancelDistance = 2.0f;
                    Debug.Log("Distance: " + distance);
                    Debug.Log("Cancel Distance: " + cancelDistance);

                    if (distance < cancelDistance)
                    {
                        GetComponent<Renderer>().enabled = false;
                        Debug.Log("找到钥匙：" + gameObject);
                        UIValue.KeyValue += Keysvalue;
                        Player.KeyCount++;
                        StartCoroutine(DelayedProgressUpdate(UIValue.KeyValue));
                        numberOfKeys--;
                    }
                }
            }
        }

        if(Player.KeyRemove == true && Player.KeyCount>0)
        {
            RemoveKey();
            DisplayRandomEntities();
            Player.KeyRemove = false;
        }

    }

    IEnumerator DelayedProgressUpdate(float targetValue)
    {
        while (guiProgressBar.Value < targetValue)
        {
            UI.SetActive(true);
            guiProgressBar.Value += 0.010f;
            yield return new WaitForSeconds(0.05f);
        }
        KeyText.text = "钥匙收集" + Player.KeyCount + "/7";
        if (UIValue.KeyValue == 1.0f)
        {
            EnemyManager.Instance.GameWin();
        }
        yield return new WaitForSeconds(3.0f);
       // UI.SetActive(false);
        GetComponent<Renderer>().enabled = true; 
        gameObject.SetActive(false);
    }

    void DisplayRandomEntities()
    {
        int displayCount = 0; 

        foreach (Transform child in keyObject.transform)
        {
            if (child.gameObject.activeSelf)
            {
                displayCount++; 
            }
        }
        Debug.Log(displayCount);

        if (displayCount <= minDisplayCount)
        {
            int randomDisplayCount = Random.Range((int)minDisplayCount, (int)maxDisplayCount + 1);
            for (int i = 0; i < randomDisplayCount; i++)
            {
                int randomIndex = Random.Range(0, keyObject.transform.childCount);
                keyObject.transform.GetChild(randomIndex).gameObject.SetActive(true);
            }
        }
    }

    IEnumerator RemoveKeyCoroutine()
    {
        Player.KeyCount--;
        UIValue.KeyValue -= Keysvalue;
        while (guiProgressBar.Value > UIValue.KeyValue)
        {
            UI.SetActive(true);
            guiProgressBar.Value -= 0.010f;
            yield return new WaitForSeconds(0.05f);
        }
        KeyText.text = "钥匙收集" + Player.KeyCount + "/7";
        yield return new WaitForSeconds(3.0f);
       // UI.SetActive(false);
    }

    public void RemoveKey()
    {
        StartCoroutine(RemoveKeyCoroutine());
    }



}
