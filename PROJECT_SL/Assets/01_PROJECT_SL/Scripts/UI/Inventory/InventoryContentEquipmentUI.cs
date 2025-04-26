using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class InventoryContentEquipmentUI : MonoBehaviour
    {
        public NewInventoryUI inventoryUI;
        [field: SerializeField] public InfiniteScroll infiniteScroll;

        private void Awake()
        {
            inventoryUI = GetComponentInParent<NewInventoryUI>();
        }

        private void OnEnable()
        {
            RefreshEquipmentInventory();
            inventoryUI.OnEquipmentItemAdded += AddItemToEquipmentInventory;
            inventoryUI.OnEquipmentItemUpdated += UpdateItemAtEquipmentInventory;
        }

        private void OnDisable()
        {
            inventoryUI.OnEquipmentItemAdded -= AddItemToEquipmentInventory;
            inventoryUI.OnEquipmentItemUpdated -= UpdateItemAtEquipmentInventory;
        }

        public void RefreshEquipmentInventory()
        {
            infiniteScroll.ClearData();
            var equipmentItemDatas = inventoryUI.equipmentItems;

            for (int i = 0; i < equipmentItemDatas.Count; i++)
            {
                AddItem(equipmentItemDatas[i].itemID, equipmentItemDatas[i].count);
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
        public void UpdateItemAtEquipmentInventory(UserItemDTO checkItemDTO)
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

        private void AddItemToEquipmentInventory(UserItemDTO newItemDTO)
        {
            AddItem(newItemDTO.itemID, newItemDTO.count);
        }
    }
}
