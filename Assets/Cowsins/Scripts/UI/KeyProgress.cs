using Assets.ProgressBars.Scripts;
using cowsins;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class KeyProgress : MonoBehaviour
{
    public GuiProgressBarUI guiProgressBar;
    public GameObject UI;
    public UITicktockPanel UIValue;
    private static int numberOfKeys = 0; 
    private float Keysvalue; 
    public GameObject keyObject;
    private float minDisplayCount = 2; 
    private float maxDisplayCount = 3; 
    void Start()
    {
        guiProgressBar.Value = 0.0F;
        UI.SetActive(false);
        numberOfKeys = keyObject.transform.childCount;
        Keysvalue = 1.0f / numberOfKeys;
    }


    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
                    float cancelDistance = 2.0f;
                    Debug.Log("Distance: " + distance);
                    Debug.Log("Cancel Distance: " + cancelDistance);

                    if (distance < cancelDistance)
                    {
                        GetComponent<Renderer>().enabled = false;
                        Debug.Log("ÕÒµ½Ô¿³×£º" + gameObject);
                        UIValue.KeyValue += Keysvalue;
                        StartCoroutine(DelayedProgressUpdate(UIValue.KeyValue));
                        numberOfKeys--;
                    }
                }
            }
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

        yield return new WaitForSeconds(3.0f);
        UI.SetActive(false);
        if (UIValue.KeyValue == 1.0f)
        {
            Destroy(keyObject);
        }
        GetComponent<Renderer>().enabled = true; 
        gameObject.SetActive(false);
        //DisplayRandomEntities();
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





}
