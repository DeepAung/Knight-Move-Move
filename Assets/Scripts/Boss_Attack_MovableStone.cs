using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack_MovableStone : StateMachineBehaviour
{
    Boss boss;
    GameManager_Boss gameManager;

    const int LIMIT = 1000;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        gameManager = boss.gameManager;

        int i, j, cnt = 0;
        do
        {
            i = Random.Range(0, gameManager.n);
            j = Random.Range(0, gameManager.m);

            if (++cnt >= LIMIT) return;
        } while (gameManager.myMap[i, j].topLayer != ' ');

        gameManager.myMap[i, j].topLayer = '!'; // pending



        boss.StartCoroutine( boss.attackMovableStone(i, j) );
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
