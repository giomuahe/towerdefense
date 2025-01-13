using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Bullet
{
    public int BulletID;
    public GameObject BulletPrefab;
}

[CreateAssetMenu]
public class BulletConfig : ScriptableObject
{
    public List<Bullet> Bullets;
}

