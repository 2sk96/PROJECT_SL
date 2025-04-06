using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class RollStateMachine : StateMachineBehaviour
    {
        public string upperArmLayerName = "UpperArm Layer";
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            int index = animator.GetLayerIndex(upperArmLayerName);
            animator.SetLayerWeight(index, 1);
            
            CharacterBase character = animator.GetComponent<CharacterBase>();
            character.IsRolling = false;
            character.fixDirection = false;
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            int index = animator.GetLayerIndex(upperArmLayerName);
            animator.SetLayerWeight(index, 0);
        }
    }
}
