
using System.Collections;
using cowsins;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    private float minIntensity = 0.1f;  // 光的最小强度
    private float maxIntensity = 1.5f;  // 光的最大强度
    private float flickerDuration = 0.1f;  // 闪烁的频率
    private float minWaitTime = 0.05f;  // 闪烁的最小间隔时间
    private float maxWaitTime = 0.1f;   // 闪烁的最大间隔时间
    private Light myLight;
    private float timer; // 计时器
    public AudioClip openClip;
    public AudioClip closeClip;
    void Start()
    {
        myLight = GetComponent<Light>();
      //  StartCoroutine(Flicker());
        // maxIntensity = 0.8f;
        // minIntensity = 0.5f;
        // myLight.spotAngle = 48;
    }

    public void OpenLight()
    {
        myLight.enabled = true;
        // maxIntensity = 0.8f;
        // minIntensity = 0.5f;
        // myLight.spotAngle = 48;
        SoundManager.Instance.PlaySound(openClip, 0, 0, false, 0);
    }

    public void CloseLight()
    {
        // maxIntensity = 0.3f;
        // minIntensity = 0.1f;
        // myLight.spotAngle = 13;
        myLight.enabled = false;
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