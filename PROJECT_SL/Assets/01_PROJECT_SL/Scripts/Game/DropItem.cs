using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class DropItem : MonoBehaviour, IInteractable
    {
        public string Message => $"[{itemName}]";
        //public ItemDataDTO.ItemData dropItemData;
        public Vector3 Position => this?this.transform.position:new Vector3();

        public string itemName;

        public string itemID;

        public int count = 1;

        public Sprite sprite;

        public bool isStackable;

        private void Awake()
        {
            if (GameDataModel.Singleton.GetItemData(itemID, out ItemDataDTO.ItemData itemData))
            {
                //dropItemData = itemData;
                itemName = itemData.ItemName;
                isStackable = itemData.IsStackable;
                ItemSO itemSO = itemData.GetItemSO();
                if (itemSO != null)
                {
                    sprite = itemSO.sprite;
                }
            }
        }
        public void Interact(CharacterBase actor)   // actor = Player = LinkedCharacter
        {
            actor.PickUp(this);
            // TODO : 아이템 인벤토리에 수납
            //Destroy(gameObject);
        }
    }
}
