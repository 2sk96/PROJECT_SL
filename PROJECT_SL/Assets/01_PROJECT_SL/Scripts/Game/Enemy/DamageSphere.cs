using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class DamageSphere : MonoBehaviour
    {
        public List<CharacterBase> insideCharacters = new List<CharacterBase>();

        public float damage = 10f;
        public float damageDelay = 1.0f;

        private float lastDamageTime;

        private void Update()
        {
            if (Time.time - lastDamageTime < damageDelay) return;

            lastDamageTime = Time.time;
            for (int i = 0; i < insideCharacters.Count; i++)
            {
                insideCharacters[i].TakeDamage(damage);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.TryGetComponent(out CharacterBase character))
            {
                insideCharacters.Add(character);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.TryGetComponent(out CharacterBase character))
            {
                insideCharacters.Remove(character);
            }
        }
    }
}
