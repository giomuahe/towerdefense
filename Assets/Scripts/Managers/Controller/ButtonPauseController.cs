using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPauseController : MonoBehaviour
{
    public Image ImageState;
    public Sprite pauseSprite; // Sprite khi pause
    public Sprite unPauseSprite; // Sprite khi unPause

    private void OnEnable()
    {
        InitUI(true);
    }

    private void OnDisable()
    {
    }

    public void OnClickButton()
    {
        bool isPause = GameManager.Instance.BattleManager.ChangePause();
        InitUI(isPause);
        if (isPause) 
            GameManager.Instance.UIManager.InBattleScreenController.OnClickPause(isPause);
    }

    public void InitUI(bool isPause)
    {
        if (isPause)
            ImageState.sprite = unPauseSprite;
        else
            ImageState.sprite = pauseSprite;
    }
}
