using UnityEngine;

namespace MapConfigs
{
    public class TurretBase : MonoBehaviour
    {
        public Vector3 Position { get; private set; }
        public GameObject Turret { get; set; }
        public TurretType TurretType { get; set; }

        public void Initialize(Vector3 position)
        {
            Position = position;
            Turret = null; // No turret placed as default
            TurretType = TurretType.Base;
        }

        public TurretType GetTurretType()
        {
            return TurretType;
        }
    }
    
}
