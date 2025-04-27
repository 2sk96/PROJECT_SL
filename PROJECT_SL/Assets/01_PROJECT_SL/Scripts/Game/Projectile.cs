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
            GameObject impact = null;
            if (collision.collider.material.name.Contains("Brick"))
            {
                impact = EffectManager.Instance.GetImpactEffect("Brick");
            }
            else if (collision.collider.material.name.Contains("Concrete"))
            {
                impact = EffectManager.Instance.GetImpactEffect("Concrete");
            }
            else if (collision.collider.material.name.Contains("Dirt"))
            {
                impact = EffectManager.Instance.GetImpactEffect("Dirt");
            }
            else if (collision.collider.material.name.Contains("Glass"))
            {
                impact = EffectManager.Instance.GetImpactEffect("Glass");
            }
            else if (collision.collider.material.name.Contains("Metal"))
            {
                impact = EffectManager.Instance.GetImpactEffect("Metal");
            }

            if (impact != null)
            {
                impact.transform.SetPositionAndRotation(collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
            }

            // IDamage Interface 상속 확인 후 
            //if (collision.rigidbody && collision.rigidbody.TryGetComponent(out IDamage damageInterface))
            //{
            //    damageInterface.TakeDamage(damage);
            //}
            // 캐릭터는 rigidbody 가 없어서 일단 수정
            if (collision.gameObject && collision.gameObject.TryGetComponent(out IDamage damageInterface))
            {
                damageInterface.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
