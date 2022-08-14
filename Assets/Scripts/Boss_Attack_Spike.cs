using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack_Spike : StateMachineBehaviour
{
    Boss boss;
    GameManager_Boss gameManager;

    const int LIMIT = 100;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        gameManager = boss.gameManager;
       

        bool rowOrCol;
        int index, amount = 1;

        if (boss.stage == 0) amount = 1;
        else if (boss.stage == 1) amount = 2;
        else if (boss.stage == 2) amount = 3;

        for (int it = 0; it < amount; it++)
        {
            var result = getRandomPos();
            rowOrCol = result.Key;
            index = result.Value;

            if (index == -1) continue;

            boss.attackSpikeRowOrCol(rowOrCol, index);
        }
    }

    public KeyValuePair<bool, int> getRandomPos()
    {
        bool rowOrCol = Random.Range(0, 2) == 1;
        int index, maxRange;

        if (rowOrCol) maxRange = gameManager.n;
        else maxRange = gameManager.m;

        bool check;
        int cnt = 0;
        do
        {
            check = true;
            index = Random.Range(0, maxRange);

            if (rowOrCol)
            {
                for (int x = 0; x < gameManager.m; x++)
                    if (gameManager.myMap[index, x].groundLayer == 'E')
                    {
                        check = false;
                        continue;
                    }
            }
            else
            {
                for (int x = 0; x < gameManager.n; x++)
                    if (gameManager.myMap[x, index].groundLayer == 'E')
                    {
                        check = false;
                        continue;
                    }
            }

            if (++cnt >= LIMIT) return new KeyValuePair<bool, int>(false, -1);

        } while (!check);

        return new KeyValuePair<bool, int>(rowOrCol, index);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

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
