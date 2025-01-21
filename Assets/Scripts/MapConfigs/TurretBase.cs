using UnityEngine;

namespace MapConfigs
{
    public class TurretBase : MonoBehaviour
    {
        public int TurretBaseId {  get; private set; }
        public Vector3 Position { get; private set; }
        public GameObject Turret { get; set; }
        public TurretType TurretType { get; set; }

        public void Initialize(int id, Vector3 position)
        {
            TurretBaseId = id;
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
