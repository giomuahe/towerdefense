using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiUpgradeManager : MonoBehaviour
{
    public static UiUpgradeManager Instance; 
   public Transform parentPanel;
   public LoadPrefab loadPrefab=new LoadPrefab();
   public List<GameObject> listButton;
   public List<TurretType> listTurret;
   
   void Awake(){
        Instance=this;
    }
    void Start()
    {
        
    }

  
    void Update()
    {
        
    }
    public void SetListTurret(List<TurretType> turrets){
        listTurret=turrets;
    }
    List<GameObject> CreateListButton(List<TurretType> listUpgrade){
        foreach(TurretType turretType in listUpgrade){
            GameObject newButton= loadPrefab.LoadButtonUpgradeTurret(turretType);
            listButton.Add(newButton);
        }
        return listButton;
    }
    void CreateSelectTurretButtons(){
        if(listTurret.Count!=0){
            listButton=CreateListButton(listTurret);
            foreach(GameObject gameObject in listButton){
                CreateButton(gameObject, listButton.IndexOf(gameObject));
            }
        }
        else{
            return;
        }
    }
    void CreateButton(GameObject buttonPrefab, int index)
    {
       
        GameObject newButton = Instantiate(buttonPrefab, parentPanel);
       
        newButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -50 * index);
       
        
        Button buttonComponent = newButton.GetComponent<Button>();
        
    }
   
}
