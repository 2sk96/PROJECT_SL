using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class InventoryContentCraftingUI : MonoBehaviour
    {
        public NewInventoryUI inventoryUI;
        [field: SerializeField] public InfiniteScroll infiniteScroll;

        private void Awake()
        {
            inventoryUI = GetComponentInParent<NewInventoryUI>();
        }

        private void OnEnable()
        {
            RefreshCraftingInventory();
            inventoryUI.OnCraftingItemAdded += AddItemToCraftingInventory;
        }

        private void OnDisable()
        {
            inventoryUI.OnCraftingItemAdded -= AddItemToCraftingInventory;
        }

        public void RefreshCraftingInventory()
        {
            infiniteScroll.ClearData();
            var craftingItemDatas = inventoryUI.craftingItems;

            for (int i = 0; i < craftingItemDatas.Count; i++)
            {
                AddItem(craftingItemDatas[i].TargetItemID, craftingItemDatas[i].TargetItemCount);
            }
        }

        public void AddItem(string itemID, int count)
        {
            var newInfiniteData = new InventoryUI_CraftingInfiniteScrollData();
            newInfiniteData.targetItemID = itemID;
            newInfiniteData.targetItemCount = count;

            infiniteScroll.InsertData(newInfiniteData);
        }

        public void AddItemToCraftingInventory(CraftingDataDTO.CraftingData craftingData)
        {
            AddItem(craftingData.TargetItemID, craftingData.TargetItemCount);
        }
    }
}
