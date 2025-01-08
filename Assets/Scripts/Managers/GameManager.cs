using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //UI
    public UIManager UIManager;

    public MapManager MapManager;

    public EnemyManager EnemyManager;

    public static GameManager Instance {  get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (UIManager)
            UIManager.Init();
        else
            Debug.LogError("NotFund UIManager");

       
        if (EnemyManager != null)
        {
            EnemyManager.Init();
        }
        else
        {
            Debug.LogError("NotFund UIManager");
        }    
    }

    #region UI
    /// UI -------------
    #region Lobby

    #endregion Lobby

    #region GamePlay

    #endregion GamePlay
    /// UI -------------
    #endregion UI

    #region Enemy
    public void NotifyOnHitTurret()
    {

    }
    #endregion
}
