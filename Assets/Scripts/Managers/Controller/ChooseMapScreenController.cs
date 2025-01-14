using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ChooseMapScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClickSelectMap()
    {
        GameManager.Instance.UIManager.ShowScreen(ESCREEN.IN_BATTLE);
        SceneManager.LoadScene("Map_01");
    }
}
