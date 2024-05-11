using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowCtr : MonoBehaviour
{
    public Button BtnGiftSetting;

    public Button BtnGameDataSetting;

    public GameObject UGiftSettingPanel;

    public GameObject UIDataPanel;

    public Button BtnSetting;

    public GameObject SettingPanel;
    // Start is called before the first frame updatein
    void Start()
    {
        UGiftSettingPanel.gameObject.SetActive(true);
        UIDataPanel.gameObject.SetActive(false);
        BtnGiftSetting.interactable = false;
        BtnGameDataSetting.interactable = true;
        BtnGiftSetting.onClick.AddListener(() =>
        {
            BtnGiftSetting.interactable = false;
            BtnGameDataSetting.interactable = true;
            UGiftSettingPanel.gameObject.SetActive(true);
            UIDataPanel.gameObject.SetActive(false);
        });
        
        BtnGameDataSetting.onClick.AddListener(() =>
        {
            BtnGiftSetting.interactable = true;
            BtnGameDataSetting.interactable = false;
            UGiftSettingPanel.gameObject.SetActive(false);
            UIDataPanel.gameObject.SetActive(true);
        });
        BtnSetting.onClick.AddListener(() =>
        {
            SettingPanel.gameObject.SetActive(!SettingPanel.gameObject.activeSelf);
        });
        SettingPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
