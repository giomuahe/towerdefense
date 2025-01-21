using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreenController : MonoBehaviour
{
    public Button NewgameBtn;
    public Button SettingBtn;
    public Button AboutUsBtn;
    public Button QuitGameBtn;

    public SettingPopup SettingPopup;
    public AboutUsPopup AboutUsPopup;


    private void OnEnable()
    {
        if (NewgameBtn)
            NewgameBtn.onClick.AddListener(OnClickNewgame);
        if (SettingBtn)
            SettingBtn.onClick.AddListener(OnClickSetting);
        if (AboutUsBtn)
            AboutUsBtn.onClick.AddListener(OnClickAboutUs);
        if (QuitGameBtn)
            QuitGameBtn.onClick.AddListener(OnClickQuitApp);

        SettingPopup.Close();
        AboutUsPopup.Close();
    }

    private void OnDisable()
    {
        if (NewgameBtn)
            NewgameBtn.onClick.RemoveAllListeners();
        if (SettingBtn)
            SettingBtn.onClick.RemoveAllListeners();
        if (AboutUsBtn)
            AboutUsBtn.onClick.RemoveAllListeners();
        if (QuitGameBtn)
            QuitGameBtn.onClick.RemoveAllListeners();
    }

    private void OnClickNewgame()
    {
        GameManager.Instance.UIManager.ShowScreen(ESCREEN.CHOOSEMAP);
    }

    private void OnClickSetting()
    {
        SettingPopup.Open();
    }

    private void OnClickAboutUs()
    {
        AboutUsPopup.Open();
    }

    private void OnClickQuitApp()
    {
        print("click quit app");
        //SAVE DATA ?
        Application.Quit();
    }
}
