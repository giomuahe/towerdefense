using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ChooseMapScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    string mapname = "Map_01";
    public void OnclickBack()
    {
        GameManager.Instance.UIManager.ShowScreen(ESCREEN.LOBBY);
    }
    public void OnClickSelectMap()
    {
        //SceneManager.LoadScene("Map_01");
        GameManager.Instance.UIManager.LoadScene(mapname, ESCREEN.IN_BATTLE);
    }
}
