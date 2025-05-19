using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectSL
{
    public class NewInventoryUI : UIBase
    {
        public override bool IsCursorVisible => true;

        public List<UserItemDTO> consumableItems = new List<UserItemDTO>();
        public List<UserItemDTO> equipmentItems = new List<UserItemDTO>();
        public List<CraftingDataDTO.CraftingData> craftingItems = new List<CraftingDataDTO.CraftingData>();

        public event System.Action<UserItemDTO> OnConsumableItemAdded;
        public event System.Action<UserItemDTO> OnConsumableItemUpdated;

        public event System.Action<UserItemDTO> OnEquipmentItemAdded;
        public event System.Action<UserItemDTO> OnEquipmentItemUpdated;

        public event System.Action<CraftingDataDTO.CraftingData> OnCraftingItemAdded;



        public override void Show()
        {
            base.Show();
            RefreshInventory();
            UserDataModel.Singleton.OnItemAdded += AddItemToInventory;
            UserDataModel.Singleton.OnItemUpdated += UpdateItemAtInventory;
        }

        public override void Hide()
        {
            base.Hide();
            UserDataModel.Singleton.OnItemAdded -= AddItemToInventory;
            UserDataModel.Singleton.OnItemUpdated -= UpdateItemAtInventory;
        }

        

        public void RefreshInventory()
        {
            // UserData 의 PlayerInventoryData 를 바탕으로 Consumable/Equipment 리스트 정리
            var inventoryItemDatas = UserDataModel.Singleton.PlayerInventoryData.Items;
            for (int i = 0; i < inventoryItemDatas.Count; i++)
            {
                InventoryType itemInventoryType = GameDataModel.Singleton.GetItemInventoryCategory(inventoryItemDatas[i].itemID);
                switch(itemInventoryType)
                {
                    case InventoryType.Consumable:
                        // 해당 아이템이 consumableItems 리스트에 있는지 체크 (dtoID 체크)
                        UserItemDTO checkConsumableItem = consumableItems.Find(x => x.dtoID == inventoryItemDatas[i].dtoID);
                        // 없으면 아이템 추가
                        if (checkConsumableItem == null)
                        {
                            consumableItems.Add(inventoryItemDatas[i]);
                        }
                        else
                        {
                            // 있으면 카운트 체크, 다르면 카운트 업데이트
                            if (!(checkConsumableItem.count == inventoryItemDatas[i].count))
                            {
                                checkConsumableItem.count = inventoryItemDatas[i].count;
                            }
                        }
                        break;
                    case InventoryType.Equipment:
                        // 해당 아이템이 consumableItems 리스트에 있는지 체크 (dtoID 체크)
                        UserItemDTO checkEquipmentItem = equipmentItems.Find(x => x.dtoID == inventoryItemDatas[i].dtoID);
                        // 없으면 아이템 추가
                        if (checkEquipmentItem == null)
                        {
                            equipmentItems.Add(inventoryItemDatas[i]);
                        }
                        else
                        {
                            // 있으면 카운트 체크, 다르면 카운트 업데이트
                            if (!(checkEquipmentItem.count == inventoryItemDatas[i].count))
                            {
                                checkEquipmentItem.count = inventoryItemDatas[i].count;
                            }
                        }
                        break;
                }
            }

            // GameData 의 CraftingData 를 바탕으로 CraftingUI 에 필요한 craftingItems 리스트 정리
            var craftingItemDatas = GameDataModel.Singleton.CraftingData.CraftingDatas;
            for (int i = 0; i < craftingItemDatas.Count; i++)
            {
                var checkCraftingItem = craftingItems.Find(x => x.TargetItemID == craftingItemDatas[i].TargetItemID);
                if (checkCraftingItem == null)
                {
                    var newCraftingItem = craftingItemDatas[i];
                    craftingItems.Add(newCraftingItem);
                    OnCraftingItemAdded?.Invoke(newCraftingItem);
                }

            }
        }

        public void RemoveItem(string itemID)
        {
            //var allData = infiniteScroll.GetDataList();
            //for (int i = 0; i < allData.Count; i++)
            //{
            //    var castingData = allData[i] as InventoryUI_InfiniteScrollData;
            //    if (castingData.itemID.Equals(itemID))
            //    {
            //        infiniteScroll.RemoveData(castingData);
            //    }
            //}
        }

        public void UpdateItemAtInventory(UserItemDTO checkItemDTO)
        {
            InventoryType itemInventoryType = GameDataModel.Singleton.GetItemInventoryCategory(checkItemDTO.itemID);
            switch (itemInventoryType)
            {
                case InventoryType.Consumable:
                    UserItemDTO consumableItemData = consumableItems.Find(x => x.dtoID == checkItemDTO.dtoID);
                    if (consumableItemData != null)
                    {
                        consumableItemData.count = checkItemDTO.count;
                    }
                    OnConsumableItemUpdated?.Invoke(checkItemDTO);
                    break;
                case InventoryType.Equipment:
                    UserItemDTO equipmentItemData = equipmentItems.Find(x => x.dtoID == checkItemDTO.dtoID);
                    if (equipmentItemData != null)
                    {
                        equipmentItemData.count = checkItemDTO.count;
                    }
                    OnEquipmentItemUpdated?.Invoke(checkItemDTO);
                    break;
            }
        }

        // 1. 인벤토리가 열려있는 상태에서
        // 2. 아이템 추가가 발생했을 때 호출
        private void AddItemToInventory(UserItemDTO newItemDTO)
        {
            InventoryType itemInventoryType = GameDataModel.Singleton.GetItemInventoryCategory(newItemDTO.itemID);
            Debug.Log(itemInventoryType.ToString());
            switch (itemInventoryType)
            {
                case InventoryType.Consumable:
                    consumableItems.Add(newItemDTO);
                    OnConsumableItemAdded?.Invoke(newItemDTO);
                    // ConsumableData 리스트에 추가
                    // InventoryConsumableUI 쪽에서 실행될 이벤트 Invoke
                    break;
                case InventoryType.Equipment:
                    equipmentItems.Add(newItemDTO);
                    OnEquipmentItemAdded?.Invoke(newItemDTO);
                    // Equipment 리스트에 추가
                    // EquipmentInventoryUI 쪽에서 실행될 이벤트 Invoke
                    break;
            }
        }
    }
}
