using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class DropItem : MonoBehaviour, IInteractable
    {
        public string Message => $"[{itemName}]";
        public string itemName;

        public void Interact()
        {
            // TODO : 아이템 인벤토리에 수납

            Destroy(gameObject);
        }
    }
}
