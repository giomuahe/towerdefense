using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupConfirmController : MonoBehaviour
{
    public Text Tittle;
    public Text Message;
    public Button OkButton;

    private Action onOkBtnPressed;

    private void OnEnable()
    {
        OkButton.onClick.AddListener(OnOkClicked);
    }

    private void OnDisable()
    {
        OkButton.onClick.RemoveAllListeners();
    }

    public void Show(EMESSAGETYPE msgType, string message, Action callback = null)
    {
        switch (msgType)
        {
            case EMESSAGETYPE.MESSAGE:
                Tittle.text = "<color=white>THÔNG BÁO</color>";
                break;
            case EMESSAGETYPE.ERROR:
                Tittle.text = "<color=red>LỖI !!!</color>";
                break;
            case EMESSAGETYPE.WARNING:
                Tittle.text = "<color=white>CHÚ Ý !</color>";
                break;
        }
        onOkBtnPressed = callback;
        Message.text = message;
        this.gameObject.SetActive(true);
    }

    private void OnOkClicked()
    {
        // Gọi callback khi người dùng nhấn OK
        onOkBtnPressed?.Invoke();

        // Ẩn cửa sổ thông báo đi
        gameObject.SetActive(false);
    }

    public void OnClosePanel()
    {
        OnOkClicked();
    }
}
