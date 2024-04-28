using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDataItem : MonoBehaviour
{
    public Text TextDes;

    public InputField InputField;

    private string _key;
    public void InitInfo(string key, int originalValue, string keyName)
    {
        _key = key;
        TextDes.text = keyName;
        if (PlayerPrefs.HasKey(key))
        {
            InputField.text = PlayerPrefs.GetInt(key).ToString();
        }
        else
        {
            InputField.text = originalValue.ToString();
        }
    }

    public void SaveData()
    {
        UIDataLoader.Instance.SaveData(_key, int.Parse(InputField.text));
    }

}
