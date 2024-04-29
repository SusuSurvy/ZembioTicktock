using Assets.ProgressBars.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class KeyProgress : MonoBehaviour
{
    public GuiProgressBarUI guiProgressBar;
    public GameObject guiUI;
    void Start()
    {
        guiProgressBar.Value = 0.0F;
        guiUI.SetActive(false);
    }

    
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) 
            {
                if (hit.collider.gameObject == gameObject)
                {
                    float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
                    float cancelDistance = 2.0f;
                    Debug.Log("Distance: " + distance);
                    Debug.Log("Cancel Distance: " + cancelDistance);

                    if (distance < cancelDistance)
                    {
                        GetComponent<Renderer>().enabled = false;

                        StartCoroutine(DelayedProgressUpdate(0.25f));
                    }
                }
            }
        }
    }

    IEnumerator DelayedProgressUpdate(float targetValue)
    {
        float Flag = 0;
        Flag += targetValue;
        while (guiProgressBar.Value < Flag)
        {
            guiUI.SetActive(true);
            guiProgressBar.Value += 0.010f;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3.0f);

        guiUI.SetActive(false);
        Destroy(gameObject); // ×îºóÏú»Ù gameObject
    }


}
