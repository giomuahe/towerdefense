using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class EnemyManager
    {
        //test
        private static EnemyManager instance;   
        public static EnemyManager Instance
        {
            get 
            {
                if(instance == null)
                {
                    instance = new EnemyManager();
                }
                return instance;
            }
        }
        public List<Vector3> moveLocations;

        public void Init()
        {
            moveLocations = GameManager.Instance.MapManager.GetWaypoints();
        }
    }
}
