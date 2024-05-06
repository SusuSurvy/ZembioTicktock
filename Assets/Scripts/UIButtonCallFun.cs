using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButtonCallFun : MonoBehaviour
{
    public Text Text;

    public Button Btn;

    private int _triggerNum;
    private UnityAction _callback;
    public void Init(string key, int triggerNum, UnityAction ac)
    {
        _triggerNum = triggerNum;
        Text.text = key;
        _callback = ac;
        Btn.onClick.AddListener(CallFunction);
    }

    private void CallFunction()
    {
        for (int i = 0; i < _triggerNum; i++)
        {
            UITicktockPanel.Instance.SendMessage(Text.text);
        }
    }
}
