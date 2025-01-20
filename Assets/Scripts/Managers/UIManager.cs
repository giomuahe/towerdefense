using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public LobbyScreenController LobbyController;
    public ChooseMapScreenController ChoosemapController;
    public InBattleScreenController InBattleScreenController;
    public PopupController PopupController;

    private ESCREEN currentScreen = ESCREEN.NONE;

    /// <summary>
    /// Hàm khởi tạo
    /// </summary>
    public void Init()
    {
        currentScreen = ESCREEN.LOBBY;
        ShowScreen(currentScreen);
    }

    /// <summary>
    /// Ẩn toàn bộ UI Screen
    /// </summary>
    private void HideAllScreen()
    {
        LobbyController.gameObject.SetActive(false);
        ChoosemapController.gameObject.SetActive(false);
        InBattleScreenController.gameObject.SetActive(false);
        PopupController.HideAllMessage();
    }

    public ESCREEN GetCurrentScreen()
    {
        return currentScreen;
    }

    /// <summary>
    /// Show Screen chỉ định
    /// </summary>
    /// <param name="screen"></param>
    public void ShowScreen(ESCREEN screen)
    {
        switch (screen) { 
            case ESCREEN.LOBBY:
                HideAllScreen();
                LobbyController.gameObject.SetActive(true);
                break;
            case ESCREEN.CHOOSEMAP:
                HideAllScreen();
                ChoosemapController.gameObject.SetActive(true);
                break;
            case ESCREEN.IN_BATTLE:
                HideAllScreen();
                InBattleScreenController.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        currentScreen = screen;
    }

    /// <summary>
    /// Show popup
    /// </summary>
    /// <param name="popup"></param>
    /// <param name="header">Tittle popup</param>
    /// <param name="message">Thông tin</param>
    /// <param name="callbackOk">Callback OK</param>
    /// <param name="callbackCancel">callback Cancel</param>
    public void ShowPopup(EPOPUP popup, EMESSAGETYPE msgType, string header, string message, Action callbackOk = null, Action callbackCancel = null)
    {
        switch (popup) {
            case EPOPUP.CONFIRM_POPUP:
                PopupController.ShowConfirmPopup(msgType, message, callbackOk);
                break;
            case EPOPUP.COMMAND_POPUP:
                PopupController.ShowCommandPopup(message, header, callbackOk, callbackCancel);
                break;
            default:
                break;
        }
    }
}
