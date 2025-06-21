using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectSL
{
    public class AICharacterController : MonoBehaviour
    {
        public CharacterBase LinkedCharacter => linkedCharacter;
        private CharacterBase linkedCharacter;
        private NavMeshAgent navAgent;

        public Transform DestinationPoint;

        private void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            linkedCharacter = GetComponent<CharacterBase>();
        }

        private void Start()
        {
            navAgent.SetDestination(DestinationPoint.position);
        }

        private void Update()
        {
            // 이동
            if (navAgent.hasPath)
            {
                Vector3 movementDirection = (navAgent.steeringTarget - transform.position).normalized;
                Vector2 input = new Vector2(movementDirection.x, movementDirection.z);

                linkedCharacter.Move(input, 0);
            }
            else
            {
                linkedCharacter.Move(Vector2.zero, 0);
            }
            
            //linkedCharacter.Move();
            //linkedCharacter.Rotate();
        }
    }
}
