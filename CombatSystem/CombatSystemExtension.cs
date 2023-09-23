using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

/// <summary>
/// GameObjectの拡張クラス
/// </summary>
[System.Serializable]
public class DamageInfo
{
    [Range(0,1000)]
   public int damagValue; 
   public float forceValue;
   public bool sequenceHit;
   public string attackableTag;
    public object Clone()                            // シャローコピーになります。
  {
    return MemberwiseClone();
  }
    public DamageInfo ShallowCopy()                          //シャローコピー
  {
    return (DamageInfo)this.Clone();
  }
}

public static class HPSystemExtension
{
    //万能ダメージクラス

    

    public static void ColliderDataInput(this GameObject a_collider, GameObject a_object, ref Vector3 a_vector,float Power=30)
    {
        a_vector.Set(
            a_object.transform.position.x - a_collider.transform.position.x,
            0f,
            a_object.transform.position.z - a_collider.transform.position.z
        );
        a_vector.Normalize();
        a_vector*=Power;
    }

     public static async void CrossFadeAnimation(this Animator animator,string name,int CrossFadeSmoothLevel,System.Func<bool> Check=null)
    {
            animator.GetComponent<attackcore>().allofftriger();
     
           // 遷移が終わるまで待機
            await animator.WaitForTransitionToEndAsync();

            if (Check==null||Check())
            {
                       // 遷移が終わった後にCrossFadeを行う
            animator.CrossFadeInFixedTime(name, Time.deltaTime * CrossFadeSmoothLevel);

            }
         
    }
  public static List<IMove> Stop(this GameObject g ){
var list= g.root().GetComponentsInChildren<IMove>();
    foreach (var item in list)
    {
        
item.Stop = true; 
    }
    return list.ToList();
  } 
   public static void Restart(this GameObject g ){

    foreach (var item in  g.root().GetComponentsInChildren<IMove>())
    {
        
item.Stop = false; 
    }
  }




      public static bool Damage(this GameObject attacked,
       DamageInfo damageInfo,
        bool crit = false,GameObject attacker=null
    )
    {

       return attacked.Damage(damageInfo.damagValue,damageInfo.attackableTag,crit,damageInfo.sequenceHit,attacker,damageInfo.forceValue);
    }

    public static bool Damage(
        this GameObject attacked,
        int damagevalue,
        string objtag,
        bool crit = false,
        bool sequencehit = false,
        GameObject attacker = null,
        float forcepower = 0
    )
    {
        var rootObj = attacked.root();
        bool damadable=false;
        if (rootObj.CompareTag(objtag))
        {
            if (rootObj.GetComponent<hpcore>() != null)
            {
                damadable = rootObj
                    .GetComponent<hpcore>()
                    .damage(damagevalue, crit, attacked.Collider(), sequencehit, attacker);
            }
            if (damadable && forcepower > 0)
            {
                if (attacked.GetComponent<ForceableObj>() != null)
                {
                    attacked.GetComponent<ForceableObj>().AddForce(attacker, forcepower);
                
                }
                if (attacked.GetComponent<DOForce>() != null)
                {
                    attacked.GetComponent<DOForce>().AddForce(attacker, forcepower);
                }
            }
            return damadable;
        }
        return false;
    }

    public static void PlayerAddForce(this GameObject attacked, Vector3 force)
    {
        attacked.root().GetComponent<ForceableObj>().AddForce(force);
    }

    public static void collset(this GameObject obj, int damagevalue, string AttackableTag = "Enemy")
    {
        if (obj.GetComponent<Collider>() == null)
        {
            var col = obj.AddComponent(typeof(BoxCollider)) as Collider;
            col.isTrigger = true;
            col.enabled = false;
        }

        var attack = obj.AddComponentIfnull<attack>() as attack;
        attack.damageInfo.damagValue = damagevalue;
        attack.damageInfo.attackableTag = AttackableTag;
    }

    public static void damageset(this GameObject obj, int damagevalue)
    {
        obj.GetComponentIfNotNull<attackcore>().basedamagevalue = damagevalue;
    }



    public static GameObject Getbodypart(this bodypart bodypart, GameObject obj)
    {
        switch (bodypart)
        {
            case bodypart.righthand:

                return obj.GetComponent<GetBodyPart>()
                    .GetRightHand();
                break;
            case bodypart.lefthand:

                return obj.GetComponent<GetBodyPart>()
                    .GetLeftHand();
                break;
            case bodypart.rightfoot:

                return obj.GetComponent<GetBodyPart>()
                    .GetRightFoot();
                break;
            case bodypart.leftfoot:
                return obj.GetComponent<GetBodyPart>()
                    .GetLeftFoot();
                break;
            case bodypart.weapons:
                return obj.GetComponent<GetBodyPart>()
                    .GetWeapon();      break;
            case bodypart.no:
                return obj;
                break;
            default:
                return null;
                break;
        }
    }
}
