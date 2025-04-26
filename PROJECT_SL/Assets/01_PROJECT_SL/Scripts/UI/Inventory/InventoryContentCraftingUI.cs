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
                AddItem(craftingItemDatas[i].itemID, craftingItemDatas[i].count);
            }
        }

        public void AddItem(string itemID, int count)
        {
            var newInfiniteData = new InventoryUI_InfiniteScrollData();
            newInfiniteData.itemID = itemID;
            newInfiniteData.itemCount = count;

            infiniteScroll.InsertData(newInfiniteData);
        }

        public void AddItemToCraftingInventory(UserItemDTO newItemDTO)
        {
            AddItem(newItemDTO.itemID, newItemDTO.count);
        }
    }
}
