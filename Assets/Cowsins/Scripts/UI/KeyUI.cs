using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public Image Key;
    private float fadeDuration = 1.2f;
    void Start()
    {

    }

    void Update()
    {
        
    }

    IEnumerator FadeOut()
    {
        Key.enabled = true;
        float elapsedTime = 0.0f;
        Color originalColor = Key.color;
        Key.enabled = true;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1.0f, 0.0f, t));
            Key.color = newColor;
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;
        Color originalColor = Key.color;
        Key.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0.0f, 1.0f, t));
            Key.color = newColor;
            yield return null;
        }
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }





}
