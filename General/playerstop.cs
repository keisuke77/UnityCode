using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerstop : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      var list= animator.gameObject.Stop();
      Debug.Log(list);

    } 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        animator.gameObject.Restart();
    }
}