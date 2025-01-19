using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopupCommandController : MonoBehaviour
{
    public Text Tittle;
    public Text Message;
    public Button AcceptButton;
    public Button CancelButton;

    private EMESSAGETYPE messType;
    private Action onAcceptBtnPressed;
    private Action onCancelBtnPressed;

    private void OnEnable()
    {
        AcceptButton.onClick.AddListener(OnAcceptButtonClick);
        CancelButton.onClick.AddListener(OnCancelButtonClick);
    }

    private void OnDisable()
    {
        AcceptButton.onClick.RemoveAllListeners();
        CancelButton.onClick.RemoveAllListeners();
    }

    private void OnAcceptButtonClick()
    {
        // Gọi callback khi người dùng nhấn OK
        onAcceptBtnPressed?.Invoke();
        //Ẩn popup hiện tại
        this.gameObject.SetActive(false);
    }

    private void OnCancelButtonClick()
    {
        // Gọi callback khi người dùng nhấn Cancel
        onCancelBtnPressed?.Invoke();
        //Ẩn popup hiện tại
        this.gameObject.SetActive(false);
    }

    public void OnCloseButtonClick()
    {
        OnCancelButtonClick();
    }

    public void Show(string message, string tittleCustom = "", Action callbackOk = null, Action CallbackCancel = null)
    {
        string tittle = "";
        if (!string.IsNullOrEmpty(tittleCustom) && tittleCustom.Length <= 15)
        {
            tittle = tittleCustom;
        }
        else
        {
            tittle = "THÔNG BÁO";
        }

        onAcceptBtnPressed = callbackOk;
        onCancelBtnPressed = CallbackCancel;

        Tittle.text = tittle;
        Message.text = message;

        this.gameObject.SetActive(true);
    }

}
