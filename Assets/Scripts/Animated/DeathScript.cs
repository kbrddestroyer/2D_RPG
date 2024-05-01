using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAlive", false);
    }
}
