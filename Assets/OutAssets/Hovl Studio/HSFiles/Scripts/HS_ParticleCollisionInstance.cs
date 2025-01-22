/*This script created by using docs.unity3d.com/ScriptReference/MonoBehaviour.OnParticleCollision.html*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class HS_ParticleCollisionInstance : MonoBehaviour
{
    public GameObject[] EffectsOnCollision;
    public float DestroyTimeDelay = 8;
    public bool UseWorldSpacePosition;
    public float Offset = 0;
    public Vector3 rotationOffset = new Vector3(0,0,0);
    public bool useOnlyRotationOffset = true;
    public bool UseFirePointRotation;
    public bool DestoyMainEffect = false;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem ps;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        transform.position+= new Vector3(11,0,0);
    }
    
    void OnParticleCollision(GameObject other)
    {
        //if(other.CompareTag("Enemy")){
        //int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);     
        //for (int i = 0; i < numCollisionEvents; i++)
        //{
        //    foreach (var effect in EffectsOnCollision)
        //    {
        //        var instance = Instantiate(effect, collisionEvents[i].intersection + collisionEvents[i].normal * Offset, new Quaternion()) as GameObject;
        //        if (!UseWorldSpacePosition) instance.transform.parent = transform;
        //        if (UseFirePointRotation) { instance.transform.LookAt(transform.position); }
        //        else if (rotationOffset != Vector3.zero && useOnlyRotationOffset) { instance.transform.rotation = Quaternion.Euler(rotationOffset); }
        //        else
        //        {
        //            instance.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal);
        //            instance.transform.rotation *= Quaternion.Euler(rotationOffset);
        //        }
        //        Destroy(instance, DestroyTimeDelay);
        //    }
        //}
        //if (DestoyMainEffect == true)
        //{
        //    Destroy(gameObject, DestroyTimeDelay + 0.5f);
        //}
    //}
    }
    
}
