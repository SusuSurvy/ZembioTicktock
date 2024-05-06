using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGiftDataItem : MonoBehaviour
{
    public Text TextDes;
    public InputField InputField;
    private CallFunction _key;
    public InputField NumField;
    
    public void InitInfo(CallFunction type)
    {
        TextDes.text = GameDataInstance.CallFunctionDes[type];
        _key = type;
        NumField.gameObject.SetActive(CheckNeedNum());
        NumField.text = 1.ToString();
    }

    private string GetKeyNum(string key)
    {
        return key + "Num";
    }

    private bool CheckNeedNum()
    {
        if ((int) _key < 5)
        {
            return true;
        }

        return false;
    }

    public void SetName(string str)
    {
        InputField.text = str;
        int num = PlayerPrefs.GetInt(GetKeyNum(TextDes.text), 1);
        NumField.text = num.ToString();
    }

    public void SaveData()
    {
        if (string.IsNullOrEmpty(InputField.text))
        {
            return;
        }

        int num = int.Parse(NumField.text);
        RegisteCallFunctionMgr.Instance.SetFunctionSetting(InputField.text, _key, num);
        PlayerPrefs.SetString(TextDes.text, InputField.text);
        if (CheckNeedNum())
        {
            PlayerPrefs.SetInt(GetKeyNum(TextDes.text), num);
        }
    }

}
