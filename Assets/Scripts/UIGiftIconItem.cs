using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGiftIconItem : MonoBehaviour
{
    public Text TextDes;
    public Image Image;
    public Button Btn;

    private string _key;
    public void InitInfo(Sprite sprite, bool canClick)
    {
        Image.sprite = sprite;
        TextDes.text = sprite.name;
        Btn.interactable = canClick;
        Btn.onClick.AddListener(Onclick);
    }

    public void SetCanClick(bool canClick)
    {
        Btn.interactable = canClick;
    }

    private void Onclick()
    {
        RegisteCallFunctionMgr.Instance.GetGiftIcon(Image.sprite);
    }

}
