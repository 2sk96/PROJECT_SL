using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class BaseLayerStateMachine : StateMachineBehaviour
    {
        private int locomotionHash;
        
        private void Awake()
        {
            locomotionHash = Animator.StringToHash("Locomotion");
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.tagHash != locomotionHash)
            {
                CharacterBase linkedCharacter = animator.GetComponentInParent<CharacterBase>();
                linkedCharacter.allowCharacterMovement = false;
            }
            else
            {
                CharacterBase linkedCharacter = animator.GetComponentInParent<CharacterBase>();
                linkedCharacter.allowCharacterMovement = true;
            }
        }
    }
}
