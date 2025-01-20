using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public PopupConfirmController confirmController;
    public PopupCommandController commandController;

    public void ShowConfirmPopup(EMESSAGETYPE msgType, string message, Action callback = null)
    {
        if (confirmController)
            confirmController.Show(msgType, message, callback);
        else
            Debug.LogError("CANT_SHOW_CONFIRM_POPUP");
    }

    public void ShowCommandPopup(string message, string tittleCustom = "", Action callbackOk = null, Action CallbackCancel = null)
    {
        if (commandController)
            commandController.Show(message, tittleCustom, callbackOk, CallbackCancel);
        else
            Debug.LogError("CANT_SHOW_COMMAND_POPUP");
    }

    public void HideAllMessage()
    {
        confirmController.OnClosePanel();
        commandController.OnCloseButtonClick();
    }
}
