using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
   public DamageInfo damageInfo;
  
    [HideInInspector]
    public attackcore attackcore;
    public System.Action AttackEndCallback;

    public UnityEngine.Events.UnityEvent AttackEndEvent;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        attackcore=gameObject.root().GetComponent<attackcore>();

    }
//attacktagがないのはattackcoreのタグが適用されるため
    public void RadiusAddDammage(float radius)
    {
        var search = gameObject.RadiusSearch<Collider>(radius);
        foreach (var item in search)
        {
            ToAttackCore(item.gameObject);
        }
    }

    public void ToAttackCore(GameObject attacked)
    {
        var damadable = false;

        if (attackcore)
        {
            damadable = attackcore.attackon(gameObject,attacked,damageInfo);
        }
        else
        {
            damadable = attacked.Damage(damageInfo, false,gameObject);
        }

        if (damadable)
        {
            if (AttackEndCallback != null)
            {
                AttackEndCallback();
                AttackEndCallback = null;
            }
            if (AttackEndEvent!=null) AttackEndEvent.Invoke();
           
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other != gameObject)
        {
            ToAttackCore(other);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            ToAttackCore(other.gameObject);
        }
    }
}
