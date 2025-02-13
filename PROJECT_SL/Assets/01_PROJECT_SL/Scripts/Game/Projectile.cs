using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class Projectile : MonoBehaviour
    {
        public float damage;

        private void OnCollisionEnter(Collision collision)
        {
            // IDamage Interface 상속 확인 후 
            if (collision.rigidbody && collision.rigidbody.TryGetComponent(out IDamage damageInterface))
            {
                damageInterface.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
