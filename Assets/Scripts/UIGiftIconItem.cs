using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGiftIconItem : MonoBehaviour
{
    public Text TextDes;
    public Button Btn;
    public bool IsBackGroundMusic;
    private string _path;
    private string _key;
    
    public void InitInfo(string name, bool isBackGroundMusic, string path)
    {
        _path = path;
        TextDes.text = name;
        IsBackGroundMusic = isBackGroundMusic;
        Btn.onClick.AddListener(Onclick);
    }

    public void SetCanClick(bool canClick)
    {
        Btn.interactable = canClick;
    }

    private void Onclick()
    {
        RegisteCallFunctionMgr.Instance.GetGiftIcon(TextDes.text, _path);
    }

}
