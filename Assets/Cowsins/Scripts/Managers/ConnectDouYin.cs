﻿using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP.WebSocket;
using System.Collections.Generic;
using System.Collections;
using cowsins;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class ConnectDouYin: MonoBehaviour
{
    public Text DouyinText;
    private WebSocket webSocket;
    private string _url = "ws://127.0.0.1:8888";
    public Action<Dictionary<string,string>> OnChatMessage;
    public Action<GiftMsg> OnGiftMessage;
    public Action<Dictionary<string, string>> OnEnterRoomMessage;
    public Action<LikeMsg> OnLikeMessage;
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
        // string url = "http://p3-webcast.douyinpic.com/img/webcast/small_DefAvatar.png~tplv-obj.image";
        // StartCoroutine(DownLoadHeadImage(url, teu =>
        // {
        //     UITicktockPanel.Instance.SendMessage("1", teu);
        //     Debug.LogError("下载完成");
        // }));
    }
    void OnMessageReceived(WebSocket ws, string msg)
    {
        BarrageMsgPack msgArray = JsonConvert.DeserializeObject<BarrageMsgPack>(msg);
        if (msgArray != null)
        {
            PackMsgType type = msgArray.Type;
          
            if (type == PackMsgType.礼物消息)
            {
                GiftMsg giftMsg = JsonConvert.DeserializeObject<GiftMsg>(msgArray.Data);
                var giftName = giftMsg.GiftName;
                int count = (int)giftMsg.GiftCount;
                UITicktockPanel.Instance.SendMessageClient(giftName, null, count);
            }
            else if (type == PackMsgType.点赞消息)
            {
                LikeMsg giftMsg = JsonConvert.DeserializeObject<LikeMsg>(msgArray.Data);
                int count = giftMsg.Total;
                //Text.text = CantactString(data);
                UITicktockPanel.Instance.LikeMessageTrigger(count);
            }
        }
    }
    void OnClosed(WebSocket ws, UInt16 code, string message)
    {
        webSocket.Close();
    }
    private void OnDestroy()
    {
        if (webSocket != null)
        {
            webSocket.OnOpen -= OnOpen;
            webSocket.OnMessage -= OnMessageReceived;
            webSocket.OnError -= OnError;
            webSocket.OnClosed -= OnClosed;
            Debug.LogError("关闭回调");
            if (webSocket.IsOpen)
            {
                Debug.Log("连接断开");
                webSocket.Close();
            }
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
        DouyinText.text = "开始下载";
        if (!_isDownLoad)
        {
            DouyinText.text = url;
            _isDownLoad = true;
            var www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                DouyinText.text = "error:" + www.error;
            } else {
                // 请求成功，处理Texture
                var texture = DownloadHandlerTexture.GetContent(www);
                DouyinText.text = "下载完成";
                complete?.Invoke(texture);
            }
            _isDownLoad = false;
        }
    }
}
