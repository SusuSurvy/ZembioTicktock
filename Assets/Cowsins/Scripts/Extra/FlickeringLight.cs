using System.Collections;
using cowsins;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public float minIntensity = 0.1f;  // 光的最小强度
    public float maxIntensity = 1.5f;  // 光的最大强度
    public float flickerDuration = 0.1f;  // 闪烁的频率
    public float minWaitTime = 0.05f;  // 闪烁的最小间隔时间
    public float maxWaitTime = 0.1f;   // 闪烁的最大间隔时间
    private Light myLight;
    private float timer; // 计时器
    public AudioClip openClip;
    public AudioClip closeClip;
    void Start()
    {
        myLight = GetComponent<Light>();
        StartCoroutine(Flicker());
        maxIntensity = 1.6f;
        myLight.spotAngle = 30;
    }

    public void OpenLight()
    {
        maxIntensity = 1.6f;
        myLight.spotAngle = 30;
        SoundManager.Instance.PlaySound(openClip, 0, 0, false, 0);
    }

    public void CloseLight()
    {
        maxIntensity = 0.3f;
        myLight.spotAngle = 13;
        SoundManager.Instance.PlaySound(closeClip, 0, 0, false, 0);
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // 使用Mathf.PingPong根据时间创建一个往返值，模拟光强的忽明忽暗
            myLight.intensity = Random.Range(minIntensity, maxIntensity);
            //float lightIntensity = Mathf.PingPong(Time.time, maxIntensity - minIntensity) + minIntensity;
            //myLight.intensity = lightIntensity;
            // 随机等待一段时间后再次执行，创建不规律的闪烁效果
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        }
    }
}