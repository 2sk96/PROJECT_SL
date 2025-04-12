using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class InventoryContentUI : MonoBehaviour
    {
        public Transform self;
        public GameObject UIPrefab;

        private void Start()
        {
            self = GetComponent<Transform>();
            GameObject ui = Instantiate(UIPrefab, self);
        }
    }
}
