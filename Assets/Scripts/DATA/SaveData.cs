using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DATA
{
    [Serializable]
    public class SaveData
    {
        public string Mapname { get; set; }
        public int CurrentWave { get; set; }
        public int CurrentHeart { get; set; }
        public long CurrentGold { get; set; }
        public Dictionary<int, TurretData> TurretInfo { get; set; }
    }

    public class TurretData
    {
        public float X {  get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public TurretType TurretType { get; set; }
    } 
}
