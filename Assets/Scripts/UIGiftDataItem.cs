using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIGiftDataItem : MonoBehaviour
{
    public Text TextDes;
    public InputField InputField;
    private CallFunction _key;
    public InputField NumField;
    public Button Btn;
    public Text MusicName;
    private string _path;
    public void InitInfo(CallFunction type)
    {
        TextDes.text = GameDataInstance.CallFunctionDes[type];
        _key = type;
        NumField.gameObject.SetActive(CheckNeedNum());
        NumField.text = 1.ToString();
        
        if (_key == CallFunction.BackgroundMusic)
        {
            Btn.onClick.AddListener(() => { RegisteCallFunctionMgr.Instance.ChooseBackgroundMusic(ChooseMusic); });
        }
        else
        {
            Btn.onClick.AddListener(() => { RegisteCallFunctionMgr.Instance.ChooseMusic(ChooseMusic); });
        }
    }

    private void ChooseMusic(string str, string path)
    {
        MusicName.text = str;
        _path = path;
    }
    
    private string GetKeyNum(string key)
    {
        return key + "Num";
    }
    private string GetMusicName(string key)
    {
        return key + "Music";
    }

    private bool CheckNeedNum()
    {
        if ((int) _key <= 5)
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
        _path = PlayerPrefs.GetString(GetMusicName(TextDes.text), "");
        if (!string.IsNullOrEmpty(_path))
        {
            MusicName.text = Path.GetFileNameWithoutExtension(_path); // 从文件路径中提取音乐文件的名称
        }
        else
        {
            MusicName.text = ("");  
        }
    }

    public void SaveData()
    {
        if (string.IsNullOrEmpty(InputField.text))
        {
            return;
        }

        int num = int.Parse(NumField.text);
        RegisteCallFunctionMgr.Instance.SetFunctionSetting(InputField.text, _key, num, _path);
        if (_key == CallFunction.BackgroundMusic)
        {
            GameDataInstance.Instance.LoadBackGroundMusic(_path);
        }

        PlayerPrefs.SetString(TextDes.text, InputField.text);
        if (CheckNeedNum())
        {
            PlayerPrefs.SetInt(GetKeyNum(TextDes.text), num);
        }

        if (!string.IsNullOrEmpty(MusicName.text))
        {
            PlayerPrefs.SetString(GetMusicName(TextDes.text), _path);
        }
    }

}
