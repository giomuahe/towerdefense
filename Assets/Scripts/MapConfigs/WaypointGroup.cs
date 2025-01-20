using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

namespace MapConfigs
{
    [Serializable]
    public class WaypointsGroup
    {
        public int groupId;
        public string groupName;
        public List<Vector3> waypoints;
    }
}
