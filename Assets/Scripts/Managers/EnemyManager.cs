using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class EnemyManager
    {
        public List<Vector3> moveLocations;
        public void Init()
        {
            moveLocations = GameManager.Instance.MapManager.GetWaypoints();
        }
    }
}
