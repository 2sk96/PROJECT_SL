using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class DropItem : MonoBehaviour, IInteractable
    {
        public string Message => $"[{itemName}]";
        public Vector3 Position => this?this.transform.position:new Vector3();

        public string itemName;

        public string itemID;

        public int count = 1;


        private void Awake()
        {
            itemName = GameDataModel.Singleton.GetItemNameFromItemData(itemID);
        }
        public void Interact(CharacterBase actor)   // actor = Player = LinkedCharacter
        {
            actor.PickUp(this);
            // TODO : 아이템 인벤토리에 수납
            //Destroy(gameObject);
        }
    }
}
