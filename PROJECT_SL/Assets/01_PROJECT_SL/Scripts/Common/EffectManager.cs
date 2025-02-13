using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ProjectSL
{
    [System.Serializable]
    public class EffectData
    {
        public string key;
        public GameObject prefab;
        public float lifeTime;

        public int defaultSize;
        public int maxSize;

        public IObjectPool<GameObject> pool;

        public void Init()
        {
            pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, defaultSize, maxSize);
        }

        private GameObject CreatePooledItem()
        {
            GameObject poolGo = GameObject.Instantiate(prefab);
            return poolGo;
        }

        // 사용
        private void OnTakeFromPool(GameObject poolGo)
        {
            poolGo.SetActive(true);
        }

        // 반환
        private void OnReturnedToPool(GameObject poolGo)
        {
            poolGo.SetActive(false);
        }

        // 삭제
        private void OnDestroyPoolObject(GameObject poolGo)
        {
            GameObject.Destroy(poolGo);
        }
    }
    
    
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance { get; private set; } = null;
        

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public List<EffectData> effect_Impacts = new List<EffectData>();
        public List<EffectData> effect_Muzzle = new List<EffectData>();

        private void Start()
        {
            for (int i = 0; i < effect_Impacts.Count; i++)
            {
                effect_Impacts[i].Init();
            }
            for (int i = 0; i < effect_Muzzle.Count; i++)
            {
                effect_Muzzle[i].Init();
            }
        }

        public GameObject GetMuzzleEffect(string key)
        {
            var targetEffectData = effect_Muzzle.Find(x => x.key.Equals(key));
            if (targetEffectData != null)
            {
                var newEffect = targetEffectData.pool.Get();
                if (targetEffectData.lifeTime > 0f)
                {
                    StartCoroutine(DelayedRelease());
                    IEnumerator DelayedRelease()
                    {
                        yield return new WaitForSeconds(targetEffectData.lifeTime);
                        targetEffectData.pool.Release(newEffect.gameObject);
                    }
                }
                return newEffect;
            }

            return null;
        }

        public GameObject GetImpactEffect(string key)
        {
            var targetEffectData = effect_Impacts.Find(x => x.key.Equals(key));
            if (targetEffectData != null)
            {
                var newEffect = targetEffectData.pool.Get();
                StartCoroutine(DelayedRelease());
                IEnumerator DelayedRelease()
                {
                    yield return new WaitForSeconds(targetEffectData.lifeTime);
                    targetEffectData.pool.Release(newEffect.gameObject);
                }
                return newEffect;
            }

            return null;
        }
    }
}
