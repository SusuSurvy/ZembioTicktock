using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP.WebSocket;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class ConnectDouYin: MonoBehaviour
{
    public Text DouyinText;
    private WebSocket webSocket;
    private string _url = "ws://127.0.0.1:9999";
    public Action<Dictionary<string,string>> OnChatMessage;
    public Action<Dictionary<string,string>> OnGiftMessage;
    public Action<Dictionary<string, string>> OnEnterRoomMessage;
    private void Start()
    {
        Debug.LogError("开启websocket");
        webSocket = new WebSocket(new Uri(_url));
        webSocket.OnOpen += OnOpen;
        webSocket.OnMessage += OnMessageReceived;
        webSocket.OnError += OnError;
        webSocket.OnClosed += OnClosed;
        Connect();
    }
    public void Connect()
    {
        try
        {
            webSocket.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine("链接失败");
        }
        Debug.LogError("发送连接");
    }
    void OnOpen(WebSocket ws)
    {
        DouyinText.text = "连接成功";
        Debug.LogError("连接成功"); 
    }
    void OnMessageReceived(WebSocket ws, string msg)
    {
        var msgArray = JsonConvert.DeserializeObject<Dictionary<string,string>>(msg);
        if (msgArray != null && msgArray.TryGetValue("type", out var type))
        {
            if (type == "ChatMessage")
            {
                OnChatMessage?.Invoke(msgArray);
            }
            if (type == "GiftMessage")
            {
                OnGiftMessage?.Invoke(msgArray);
            }
            if (type=="MemberMessage")
            {
                OnEnterRoomMessage?.Invoke(msgArray);
            }
        }
    }
    void OnClosed(WebSocket ws, UInt16 code, string message)
    {
        Debug.Log(message);
        webSocket.Close();
    }
    private void OnDestroy()
    {
        if (webSocket != null && webSocket.IsOpen)
        {
            Debug.Log("连接断开");
            webSocket.Close();
        }}
    void OnError(WebSocket ws, Exception ex)
    {
        DouyinText.text = "WebSocket出错:" + ex.Message;
        Debug.Log("WebSocket出错:"+ex.Message);
        webSocket.Close();
    }
    private bool _isDownLoad;
    public IEnumerator DownLoadHeadImage(string url,Action<Texture2D> complete)
    {
        if (!_isDownLoad)
        {
            _isDownLoad = true;
            var www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();
            var texture = DownloadHandlerTexture.GetContent(www);
            complete?.Invoke(texture);
            _isDownLoad = false;
        }
    }
}
