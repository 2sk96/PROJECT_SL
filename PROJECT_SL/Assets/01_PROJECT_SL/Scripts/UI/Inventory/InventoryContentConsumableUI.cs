using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class InventoryContentConsumableUI : MonoBehaviour
    {
        public NewInventoryUI inventoryUI;
        [field: SerializeField] public InfiniteScroll infiniteScroll;

        private void Awake()
        {
            inventoryUI = GetComponentInParent<NewInventoryUI>();
        }

        private void OnEnable()
        {
            RefreshInventory();
            inventoryUI.OnConsumableItemAdded += AddItemToConsumableInventory;
            inventoryUI.OnConsumableItemUpdated += UpdateItemAtConsumableInventory;
        }

        private void OnDisable()
        {
            inventoryUI.OnConsumableItemAdded -= AddItemToConsumableInventory;
            inventoryUI.OnConsumableItemUpdated -= UpdateItemAtConsumableInventory;
        }

        public void RefreshInventory()
        {
            infiniteScroll.ClearData();
            var consumableItemDatas = inventoryUI.consumableItems;

            for (int i = 0; i < consumableItemDatas.Count; i++)
            {
                AddItem(consumableItemDatas[i].itemID, consumableItemDatas[i].count);
            }
        }

        public void AddItem(string itemID, int count)
        {
            var newInfiniteData = new InventoryUI_InfiniteScrollData();
            newInfiniteData.itemID = itemID;
            newInfiniteData.itemCount = count;

            infiniteScroll.InsertData(newInfiniteData);
        }

        public void RemoveItem(string itemID)
        {
            var allData = infiniteScroll.GetDataList();
            for (int i = 0; i < allData.Count; i++)
            {
                var castingData = allData[i] as InventoryUI_InfiniteScrollData;
                if (castingData.itemID.Equals(itemID))
                {
                    infiniteScroll.RemoveData(castingData);
                }
            }
        }
        public void UpdateItemAtConsumableInventory(UserItemDTO checkItemDTO)
        {
            List<InfiniteScrollData> currentInfiniteScrollData = infiniteScroll.GetDataList();
            for (int i = 0; i < currentInfiniteScrollData.Count; i++)
            {
                var data = currentInfiniteScrollData[i] as InventoryUI_InfiniteScrollData;
                if (data.itemID == checkItemDTO.itemID)
                {
                    data.itemCount = checkItemDTO.count;
                    infiniteScroll.UpdateData(data);
                }
            }
        }

        private void AddItemToConsumableInventory(UserItemDTO newItemDTO)
        {
            AddItem(newItemDTO.itemID, newItemDTO.count);
        }
    }
}
