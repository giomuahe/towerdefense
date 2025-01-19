using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPauseController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnclickContinue()
    {
        bool isPause = GameManager.Instance.BattleManager.ChangePause();
        GameManager.Instance.UIManager.InBattleScreenController.ButtonPause.InitUI(isPause);
        this.gameObject.SetActive(false);
    }

    public void OnclickRestart()
    {
        GameManager.Instance.OnPlayerRestartBattle();
        this.gameObject.SetActive(false);
    }

    public void OnclickGoToMenu()
    {
        GameManager.Instance.PlayerReturnMenu();
    }
}
