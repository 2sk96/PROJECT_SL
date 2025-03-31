using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class RollStateMachine : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterBase character = animator.GetComponent<CharacterBase>();
            character.IsRolling = false;
        }
    }
}
