using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class DotweenMove : StateMachineBehaviour
{Sequence Sequence;
   public DoTweenSeri DoTweenSeri;
   public DiretionEnum dir;
   public float speed;
   public float MoveAmount;
 public LayerMask groundMask;
    public RaycastHit hit;
     public EffectAndParticle HitEffect;
    private Vector3 lastPosition;
    public float RayLength = 0.3f;
    public float CastSize=0.5f;

public string HitBool;

void Kill(int num){
   if (Sequence!=null)
   {
 Sequence?.Kill();
   }
   
}
float time;
     override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      animator.GetComponent<hpcore>().OnHpChanged+=Kill;
      
      if (dir!=DiretionEnum.No)
       {animator.gameObject.transform.DOLocalMove(animator.gameObject.transform.GetDirection(dir)*MoveAmount,speed).SetRelative(true);
    
       }
       Sequence= DoTweenSeri.Play(animator.gameObject.transform);
       Sequence.OnUpdate(()=>{
time+=Time.deltaTime;
      Vector3 currentPosition = animator.transform.position;
        Vector3 direction = (currentPosition - lastPosition).normalized;
        lastPosition = currentPosition;

        if (Physics.SphereCast(animator.transform.position,CastSize, direction, out hit, RayLength, groundMask))
        { Kill(0);
            animator.SetBool(HitBool,true);
                 if (HitEffect!=null&&time>0.2f)
            {
                  HitEffect.Execute(hit.point);
            }
        }
       
       });
       
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {time=0;
        hpcore hpComponent = animator.GetComponent<hpcore>();

    if (hpComponent != null)
    {
        hpComponent.OnHpChanged -= Kill;
    }
        Kill(0);
    }


}
