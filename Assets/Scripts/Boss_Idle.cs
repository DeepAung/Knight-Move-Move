using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Idle : StateMachineBehaviour
{
    Boss boss;
    float movableStoneTime, spikeTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movableStoneTime -= Time.deltaTime;
        spikeTime -= Time.deltaTime;

        if (movableStoneTime <= 0)
        {

            animator.SetTrigger("Attack_MovableStone");

            movableStoneTime = 2f;
        }

        //if (boss.stage == 0) return;

        if (spikeTime <= 0)
        {
            animator.SetTrigger("Attack_Spike");

            if (boss.stage == 0) spikeTime = 2.5f;
            else if (boss.stage == 1) spikeTime = 1.5f;
            else if (boss.stage == 2) spikeTime = 1f;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
