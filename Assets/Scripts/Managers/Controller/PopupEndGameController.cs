using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupEndGameController : MonoBehaviour
{
    // Start is called before the first frame update

    public bool IsWinGame;

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide() { 
        this.gameObject.SetActive(false); 
    }

    public void OnclickMenuButton()
    {
        GameManager.Instance.PlayerReturnMenu();
    }
}
